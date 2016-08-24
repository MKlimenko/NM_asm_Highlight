namespace NM_asm_Language
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Tagging;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(ITaggerProvider))]
    [ContentType("nm_asm")]
    [TagType(typeof(NM_TokenTag))]
    internal sealed class NM_TokenTagProvider : ITaggerProvider
    {

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return new NM_TokenTagger(buffer) as ITagger<T>;
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

        ITextBuffer _buffer;
        IDictionary<string, NM_TokenTypes> _NM_Types;
        
        internal NM_TokenTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
            _NM_Types = new Dictionary<string, NM_TokenTypes>();
            
            foreach (var el in NM_asm_Language.Dictionary_asm.Keywords)
            {
                _NM_Types[el] = NM_TokenTypes.NM_Keyword;
            }

            foreach (var el in NM_asm_Language.Dictionary_asm.Directives)
            {
                _NM_Types[el] = NM_TokenTypes.NM_directive;
            }

            foreach (var el in NM_asm_Language.Dictionary_asm.Labels)
            {
                _NM_Types[el] = NM_TokenTypes.NM_label;
            }

            foreach (var el in NM_asm_Language.Dictionary_asm.Data_registers)
            {
                _NM_Types[el] = NM_TokenTypes.NM_data_registers;
            }
            _NM_Types["Comment"] = NM_TokenTypes.NM_comment;
            _NM_Types["Quote"] = NM_TokenTypes.NM_quote;
            _NM_Types["Number"] = NM_TokenTypes.NM_number;
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
                int temp_start = 0;
                int temp_finish = 0;
                var containing_edited = Dictionary_asm.RemoveAux(containingLine.GetText().ToLower(), ref temp_start, ref temp_finish);
                string[] tokens = containing_edited.Split(' ');

                foreach (string nm_Token in tokens)
                {
                    //int start = 0;
                    // int finish = 0;
                    // string temp_token = Dictionary_asm.RemoveAux(nm_Token, ref start, ref finish);
                    var temp_token = nm_Token;
                    if (_NM_Types.ContainsKey(temp_token))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc + temp_start, nm_Token.Length));
                        if( tokenSpan.IntersectsWith(curSpan) ) 
                            yield return new TagSpan<NM_TokenTag>(tokenSpan, 
                                                                  new NM_TokenTag(_NM_Types[temp_token]));
                    }
                    //!!!
                    // Prev address : C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe
                    //!!!
                    else if (Dictionary_asm.IsQuoted(temp_token))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc + temp_start, nm_Token.Length));
                        if (tokenSpan.IntersectsWith(curSpan))
                            yield return new TagSpan<NM_TokenTag>(tokenSpan,
                                                                  new NM_TokenTag(_NM_Types["Quote"]));
                    }
                    else if (Dictionary_asm.IsNumber(temp_token))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc + temp_start, nm_Token.Length));
                        if (tokenSpan.IntersectsWith(curSpan))
                            yield return new TagSpan<NM_TokenTag>(tokenSpan,
                                                                  new NM_TokenTag(_NM_Types["Number"]));
                    }
                    else if (Dictionary_asm.IsComment(nm_Token))
                    {
                        var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc + temp_start, containingLine.Length));
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
