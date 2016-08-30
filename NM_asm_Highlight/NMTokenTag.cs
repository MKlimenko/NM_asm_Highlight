namespace NM_asm_highlight
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(ITaggerProvider))]
    [ContentType("nm_asm")]
    [TagType(typeof(NM_TokenTag))]
    internal sealed class NM_TokenTagProvider : ITaggerProvider
    {

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new NM_TokenTagger() as ITagger<T>;
        }
    }

    public class NM_TokenTag : ITag 
    {
        public NM_TokenTypes type { get; private set; }

        public NM_TokenTag(NM_TokenTypes type)
        {
            this.type = type;
        }
    }

    internal sealed class NM_TokenTagger : ITagger<NM_TokenTag>
    {
        readonly IDictionary<string, NM_TokenTypes> _NM_Types;
        
        internal NM_TokenTagger()
        {
            _NM_Types = new Dictionary<string, NM_TokenTypes>();
            
            foreach (var el in Dictionary_asm.Keywords)
            {
                _NM_Types[el] = NM_TokenTypes.NM_Keyword;
            }

            foreach (var el in Dictionary_asm.Directives)
            {
                _NM_Types[el] = NM_TokenTypes.NM_directive;
            }

            foreach (var el in Dictionary_asm.Labels)
            {
                _NM_Types[el] = NM_TokenTypes.NM_label;
            }

            foreach (var el in Dictionary_asm.Data_registers)
            {
                _NM_Types[el] = NM_TokenTypes.NM_data_registers;
            }
            _NM_Types["Comment"] = NM_TokenTypes.NM_comment;
            _NM_Types["Quote"] = NM_TokenTypes.NM_quote;
            _NM_Types["Number"] = NM_TokenTypes.NM_number;
            _NM_Types["Custom_label"] = NM_TokenTypes.NM_custom_label;
            _NM_Types["Macro"] = NM_TokenTypes.NM_Macro;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged
        {
            add { }
            remove { }
        }

        public IEnumerable<ITagSpan<NM_TokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (SnapshotSpan curSpan in spans)
            {
                ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
                int curLoc = containingLine.Start.Position;
                int start_span;
                int temp_finish;
                var containing_edited = Dictionary_asm.RemoveAux(containingLine.GetText(), out start_span, out temp_finish);
                string[] tokens = containing_edited.Split(' ');

                Dictionary_asm.AddCustomLine(containingLine.GetText());
                foreach (string nm_Token in tokens)
                {
                    var temp_token = nm_Token;
                    int macro_size;
                    if (_NM_Types.ContainsKey(temp_token))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc + start_span, nm_Token.Length));
                        if (tokenSpan.IntersectsWith(curSpan))
                            yield return new TagSpan<NM_TokenTag>(tokenSpan,
                                                                  new NM_TokenTag(_NM_Types[temp_token]));
                    }
                    else if (Dictionary_asm.IsQuoted(temp_token))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc + start_span, nm_Token.Length));
                        if (tokenSpan.IntersectsWith(curSpan))
                            yield return new TagSpan<NM_TokenTag>(tokenSpan,
                                                                  new NM_TokenTag(_NM_Types["Quote"]));
                    }
                    else if (Dictionary_asm.IsNumber(temp_token))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc + start_span, nm_Token.Length));
                        if (tokenSpan.IntersectsWith(curSpan))
                            yield return new TagSpan<NM_TokenTag>(tokenSpan,
                                                                  new NM_TokenTag(_NM_Types["Number"]));
                    }
                    else if (Dictionary_asm.IsCustomLabel(containingLine.GetText()) || Dictionary_asm.CustomLabel.Contains(nm_Token.ToLower()))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc + start_span, nm_Token.Length));
                        if (tokenSpan.IntersectsWith(curSpan))
                            yield return new TagSpan<NM_TokenTag>(tokenSpan,
                                                                  new NM_TokenTag(_NM_Types["Custom_label"]));
                    }
                    else if (Dictionary_asm.IsMacro(nm_Token, out macro_size))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc + start_span, macro_size));
                        if (tokenSpan.IntersectsWith(curSpan))
                            yield return new TagSpan<NM_TokenTag>(tokenSpan,
                                                                  new NM_TokenTag(_NM_Types["Macro"]));
                    }

                    else if (Dictionary_asm.IsComment(nm_Token))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc + start_span, containingLine.Length - containingLine.GetText().IndexOf(nm_Token)));
                        if (tokenSpan.IntersectsWith(curSpan))
                            yield return new TagSpan<NM_TokenTag>(tokenSpan,
                                                                  new NM_TokenTag(_NM_Types["Comment"]));
                        break;
                    }
                    //add an extra char location because of the space
                    curLoc += nm_Token.Length + 1;
                }
            }
        }
    }
}
