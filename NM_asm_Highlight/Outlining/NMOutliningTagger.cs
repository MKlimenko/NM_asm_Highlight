using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace NM_asm_highlight
{
    internal sealed class NMOutliningTagger : ITagger<IOutliningRegionTag>
    {
        class PartialRegion
        {
            public int StartLine { get; set; }
            public int StartOffset { get; set; }
            public int Level { get; set; }
            public string LabelName { get; set; }
            public PartialRegion PartialParent { get; set; }
            public string ellipsis = "...";    //the characters that are displayed when the region is collapsed 
            //public string hover_text = ""; //the contents of the tooltip for the collapsed span  
            public string end_text = "";
            public bool collapsed;
        }

        class Region : PartialRegion
        {
            public int EndLine { get; set; }
        }

        readonly ITextBuffer _buffer;
        ITextSnapshot _snapshot;
        List<Region> _regions;

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public NMOutliningTagger(ITextBuffer buffer)
        {
            this._buffer = buffer;
            _snapshot = buffer.CurrentSnapshot;
            _regions = new List<Region>();
            ReParse();
            this._buffer.Changed += BufferChanged; //use the same mechanism for the theme change
        }

        public IEnumerable<ITagSpan<IOutliningRegionTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0)
                yield break;
            var currentRegions = _regions;
            var currentSnapshot = _snapshot;
            var entire = new SnapshotSpan(spans[0].Start, spans[spans.Count - 1].End).TranslateTo(currentSnapshot, SpanTrackingMode.EdgeExclusive);
            int startLineNumber = entire.Start.GetContainingLine().LineNumber;
            int endLineNumber = entire.End.GetContainingLine().LineNumber;

            foreach (var region in currentRegions)
            {
                if (region.StartLine > endLineNumber || region.EndLine < startLineNumber)
                {
                    continue;
                }
                var startLine = currentSnapshot.GetLineFromLineNumber(region.StartLine);
                var endLine = currentSnapshot.GetLineFromLineNumber(region.EndLine);

                //the region starts at the beginning of the "[", and goes until the *end* of the line that contains the "]".
                yield return new TagSpan<IOutliningRegionTag>(
                    new SnapshotSpan(startLine.Start + region.StartOffset, endLine.End),
                    new OutliningRegionTag(region.collapsed, false, region.ellipsis, region.ellipsis));
            }
        }

        void BufferChanged(object sender, TextContentChangedEventArgs e)
        {
            // If this isn't the most up-to-date version of the _buffer, then ignore it for now 
            // (we'll eventually get another change event). 
            if (e.After != _buffer.CurrentSnapshot) return;
            ReParse();
        }

        void ReParse()
        {
            ITextSnapshot newSnapshot = _buffer.CurrentSnapshot;
            List<Region> newRegions = new List<Region>();

            //keep the current (deepest) partial region, which will have 
            // references to any parent partial _regions.
            PartialRegion currentRegion = null;
            bool comment_started = false;

            foreach (var line in newSnapshot.Lines)
            {
                int regionStart = -1;
                string linetext = line.GetText();

                string ellipsis_ = "";
                string end_text_ = "";
                string LabelName_ = "";
                bool collapsed_ = false;


                string current_line = linetext.Replace('\t', ' ');
                current_line = current_line.TrimStart();

                bool regionFinished_comment = false;

                #region comment section
                int comment_start = current_line.IndexOf("//", StringComparison.Ordinal);
                if (comment_start == 0 && !comment_started)
                {
                    regionStart = comment_start;
                    ellipsis_ = linetext.Substring(comment_start + 2, linetext.Length - comment_start - 2).Trim();
                    comment_started = true;
                    if (ellipsis_ == "")
                        ellipsis_ = "...";
                }
                if(comment_start != 0 && comment_started)
                {
                    regionFinished_comment = true;
                }
                #endregion

                #region labels
                bool regionFinished_label = false;
                var label_values = current_line.Split(' ');
                if (label_values.Length >= 2)
                {
                    bool labelFound = Dictionary_asm.Labels.Contains(label_values[0]);
                    if (labelFound && label_values[0] != "global" && label_values[0] != "local")
                    {
                        if (label_values[0] != "end")
                        {
                            var a = 5;
                            regionStart = current_line.IndexOf(label_values[0], StringComparison.Ordinal);
                            ellipsis_ = label_values[1].Replace('\"', ' ').Trim();
                            LabelName_ = ellipsis_;
                        }
                        else
                        {
                            regionFinished_label = true;
                            LabelName_ = label_values[1].Replace('\"', ' ').Trim();
                        }
                    }
                }
                #endregion

                #region closing the region
                if (regionFinished_comment || regionFinished_label)
                {
                    CloseRegion(newRegions, ref currentRegion, ref comment_started, line, linetext, LabelName_, regionFinished_comment);
                    if(regionFinished_comment && regionFinished_label)
                    {
                        CloseRegion(newRegions, ref currentRegion, ref comment_started, line, linetext, LabelName_);
                    }
                }
                #endregion

                #region opening the new region
                if (regionStart > -1)
                {
                    currentRegion = OpenRegion(newRegions, currentRegion, line, regionStart, ellipsis_, end_text_, LabelName_, collapsed_);
                }
                #endregion
            }
            if (comment_started)
            {
                CloseRegion(newRegions, ref currentRegion, ref comment_started, newSnapshot.Lines.Last(), newSnapshot.Lines.Last().GetText(), null);
            }


            //determine the changed span, and send a changed event with the new spans
            List<Span> oldSpans =
                new List<Span>(_regions.Select(r => AsSnapshotSpan(r, _snapshot)
                    .TranslateTo(newSnapshot, SpanTrackingMode.EdgeExclusive)
                    .Span));
            List<Span> newSpans =
                    new List<Span>(newRegions.Select(r => AsSnapshotSpan(r, newSnapshot).Span));

            NormalizedSpanCollection oldSpanCollection = new NormalizedSpanCollection(oldSpans);
            NormalizedSpanCollection newSpanCollection = new NormalizedSpanCollection(newSpans);

            //the changed _regions are _regions that appear in one set or the other, but not both.
            NormalizedSpanCollection removed =
            NormalizedSpanCollection.Difference(oldSpanCollection, newSpanCollection);

            int changeStart = int.MaxValue;
            int changeEnd = -1;

            if (removed.Count > 0)
            {
                changeStart = removed[0].Start;
                changeEnd = removed[removed.Count - 1].End;
            }

            if (newSpans.Count > 0)
            {
                changeStart = Math.Min(changeStart, newSpans[0].Start);
                changeEnd = Math.Max(changeEnd, newSpans[newSpans.Count - 1].End);
            }

            _snapshot = newSnapshot;
            _regions = newRegions;

            if (changeStart <= changeEnd)
            {
                if (TagsChanged != null)
                    TagsChanged(this, new SnapshotSpanEventArgs(
                        new SnapshotSpan(_snapshot, Span.FromBounds(changeStart, changeEnd))));
            }
        }

        private static PartialRegion OpenRegion(List<Region> newRegions, PartialRegion currentRegion, ITextSnapshotLine line, int regionStart, string ellipsis_, string end_text_, string LabelName_, bool collapsed_)
        {
            int currentLevel = (currentRegion != null) ? currentRegion.Level : 1;
            int newLevel = currentLevel + 1;

            if (currentLevel == newLevel && currentRegion != null)
            {
                newRegions.Add(new Region
                {
                    Level = currentRegion.Level,
                    StartLine = currentRegion.StartLine,
                    StartOffset = currentRegion.StartOffset,
                    EndLine = line.LineNumber,
                    ellipsis = ellipsis_,
                    LabelName = LabelName_,
                    end_text = end_text_,
                    collapsed = collapsed_
                });
                currentRegion = new PartialRegion
                {
                    Level = newLevel,
                    StartLine = line.LineNumber,
                    StartOffset = regionStart,
                    PartialParent = currentRegion.PartialParent,
                    ellipsis = ellipsis_,
                    LabelName = LabelName_,
                    end_text = end_text_,
                    collapsed = collapsed_
                };
            }
            else
            {
                currentRegion = new PartialRegion
                {
                    Level = newLevel,
                    StartLine = line.LineNumber,
                    StartOffset = regionStart,
                    PartialParent = currentRegion,
                    ellipsis = ellipsis_,
                    LabelName = LabelName_,
                    end_text = end_text_,
                    collapsed = collapsed_
                };
            }

            return currentRegion;
        }

        private static void CloseRegion(List<Region> newRegions, ref PartialRegion currentRegion, ref bool comment_started, ITextSnapshotLine line, string linetext, 
            string LabelName_, bool closing_comment = false)
        {
            int currentLevel = (currentRegion != null) ? currentRegion.Level : 1;
            var currRegionStart = (currentRegion == null) ? -1 :
                                    linetext.IndexOf(currentRegion.end_text, StringComparison.Ordinal);
            if (currRegionStart > -1)
            {
                int closingLevel = currentLevel; //! -1
                comment_started = false;

                if (currentRegion != null && currentLevel == closingLevel)
                {
                    if ((line.LineNumber - currentRegion.StartLine) > 1) //check if comment is only one line
                    {
                        newRegions.Add(new Region
                        {
                            Level = currentLevel,
                            StartLine = currentRegion.StartLine,
                            StartOffset = currentRegion.StartOffset,
                            EndLine = line.LineNumber - (closing_comment ? 1 : 0),
                            ellipsis = currentRegion.ellipsis,
                            LabelName = currentRegion.LabelName,
                            collapsed = currentRegion.collapsed
                        });

                        var tt = newRegions;

                    }
                    currentRegion = currentRegion.PartialParent;
                }
            }
        }

        static SnapshotSpan AsSnapshotSpan(Region region, ITextSnapshot snapshot)
        {
            var startLine = snapshot.GetLineFromLineNumber(region.StartLine);
            var endLine = (region.StartLine == region.EndLine) ?
                startLine : snapshot.GetLineFromLineNumber(region.EndLine);
            return new SnapshotSpan(startLine.Start + region.StartOffset, endLine.End);
        }
    }
}
