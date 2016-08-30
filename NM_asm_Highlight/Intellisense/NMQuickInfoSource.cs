using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Language.Intellisense;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using System.Reflection;

namespace NM_asm_highlight
{
    /// <summary>
    /// Factory for quick info sources
    /// </summary>
    [Export(typeof(IQuickInfoSourceProvider))]
    [ContentType("nm_asm")]
    [Name("NM_QuickInfo")]
    class NMQuickInfoSourceProvider : IQuickInfoSourceProvider
    {

        [Import]
        IBufferTagAggregatorFactoryService aggService = null;

        public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer)
        {
            return new NMQuickInfoSource(textBuffer, aggService.CreateTagAggregator<NM_TokenTag>(textBuffer));
        }
    }

    /// <summary>
    /// Provides QuickInfo information to be displayed in a text buffer
    /// </summary>
    class NMQuickInfoSource : IQuickInfoSource
    {
        private readonly ITagAggregator<NM_TokenTag> _aggregator;
        private readonly ITextBuffer _buffer;
        private bool _disposed;

        public NMQuickInfoSource(ITextBuffer buffer, ITagAggregator<NM_TokenTag> aggregator)
        {
            _aggregator = aggregator;
            _buffer = buffer;
        }

        /// <summary>
        /// Determine which pieces of Quickinfo content should be displayed
        /// </summary>
        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> quickInfoContent, out ITrackingSpan applicableToSpan)
        {
            applicableToSpan = null;

            if (_disposed)
                throw new ObjectDisposedException("QuickInfoSource");

            var triggerPoint = (SnapshotPoint) session.GetTriggerPoint(_buffer.CurrentSnapshot);

            if (triggerPoint == null)
                return;

            foreach (IMappingTagSpan<NM_TokenTag> curTag in _aggregator.GetTags(new SnapshotSpan(triggerPoint, triggerPoint)))
            {
                if(curTag.Tag.type == NM_TokenTypes.NM_data_registers)
                {
                    var tagSpan = curTag.Span.GetSpans(_buffer).First();
                    string key = tagSpan.GetText().ToLower();
                    var explanation = Dictionary_QuickInfo.GetDescription(key);
                    if (explanation.Length > 0)
                    {
                        applicableToSpan = _buffer.CurrentSnapshot.CreateTrackingSpan(tagSpan, SpanTrackingMode.EdgeExclusive);
                        quickInfoContent.Add(explanation);
                    }

                }
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}

