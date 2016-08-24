using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace NM_asm_Language
{
    [Export(typeof(ICompletionSourceProvider))]
    [ContentType("nm_asm")]
    [Name("nmCompletion")]
    class NMCompletionSourceProvider : ICompletionSourceProvider
    {
        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new NMCompletionSource(textBuffer);
        }
    }

    class NMCompletionSource : ICompletionSource
    {
        private ITextBuffer _buffer;
        private bool _disposed = false;
        
        public NMCompletionSource(ITextBuffer buffer)
        {
            _buffer = buffer;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (_disposed)
                throw new ObjectDisposedException("NMCompletionSource");

            List<Completion> completions = new List<Completion>()
           /* {
                new Completion("Ook!"),
                new Completion("Ook."),
                new Completion("Ook?")
            }*/;
            foreach (var el in NM_asm_Language.Dictionary_asm.Keywords)
            {
                completions.Add(new Completion(el));
            }

            foreach (var el in NM_asm_Language.Dictionary_asm.Directives)
            {
                completions.Add(new Completion(el));
            }

            foreach (var el in NM_asm_Language.Dictionary_asm.Labels)
            {
                completions.Add(new Completion(el));
            }

            foreach (var el in NM_asm_Language.Dictionary_asm.Data_registers)
            {
                completions.Add(new Completion(el));
            }


            ITextSnapshot snapshot = _buffer.CurrentSnapshot;
            var triggerPoint = (SnapshotPoint)session.GetTriggerPoint(snapshot);

            if (triggerPoint == null)
                return;

            var line = triggerPoint.GetContainingLine();
            SnapshotPoint start = triggerPoint;

            while (start > line.Start && !char.IsWhiteSpace((start - 1).GetChar()))
            {
                start -= 1;
            }

            var applicableTo = snapshot.CreateTrackingSpan(new SnapshotSpan(start, triggerPoint), SpanTrackingMode.EdgeInclusive);

            completionSets.Add(new CompletionSet("All", "All", applicableTo, completions, Enumerable.Empty<Completion>()));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}

