using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;

namespace NM_asm_highlight
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
        private readonly ITextBuffer _buffer;
        private bool _disposed;
        private readonly IDictionary<NM_TokenTypes, ImageSource> _icons;

        public NMCompletionSource(ITextBuffer buffer)
        {
            _buffer = buffer;
            _icons = new Dictionary<NM_TokenTypes, ImageSource>();
            loadIcons();
        }

        private string GetExplanation(string key)
        {
            var explanation = Dictionary_QuickInfo.GetDescription(key.ToLower());
            if (explanation.Length > 0)
            {
                return explanation;
            }
            return null;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (_disposed)
                throw new ObjectDisposedException("NMCompletionSource");

            var completions = new List<Completion>();
            var completionWithIcons = new List<Tuple<string, ImageSource>> ();

            foreach (var el in Dictionary_asm.Keywords)
            {
                completionWithIcons.Add(Tuple.Create(el, _icons[NM_TokenTypes.NM_Keyword]));
            }

            foreach (var el in Dictionary_asm.Directives)
            {
                completionWithIcons.Add(Tuple.Create(el, _icons[NM_TokenTypes.NM_directive]));
            }

            foreach (var el in Dictionary_asm.Labels)
            {
                completionWithIcons.Add(Tuple.Create(el, _icons[NM_TokenTypes.NM_label]));
            }

            foreach (var el in Dictionary_asm.Data_registers)
            {
                completionWithIcons.Add(Tuple.Create(el, _icons[NM_TokenTypes.NM_data_registers]));
            }

            completionWithIcons.Sort();
            foreach(var el in completionWithIcons)
            {
                completions.Add(new Completion(el.Item1, el.Item1, GetExplanation(el.Item1), el.Item2, ""));
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

        /// <summary>
        /// Get the path where this visual studio extension is installed.
        /// </summary>
        private string getInstallPath()
        {
            try
            {
                string fullPath = Assembly.GetExecutingAssembly().Location;
                string filenameDll = "NM_asm_Highlight.dll";
                return fullPath.Substring(0, fullPath.Length - filenameDll.Length);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private ImageSource bitmapFromUri(Uri bitmapUri)
        {
            var bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.UriSource = bitmapUri;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }
            catch (Exception)
            {
                
            }
            return bitmap;
        }

        private void loadIcons()
        {
            Uri uri;
            string extension_path = getInstallPath();
            try
            {
                uri = new Uri(extension_path + "images/Method_16x.png");
                _icons[NM_TokenTypes.NM_Keyword] = bitmapFromUri(uri);
            }
            catch (FileNotFoundException) { }
            try
            {
                uri = new Uri(extension_path + "images/Partition_16x.png");
                _icons[NM_TokenTypes.NM_directive] = bitmapFromUri(uri);
            }
            catch (FileNotFoundException) { }
            try
            {
                uri = new Uri(extension_path + "images/Component_16x.png");
                _icons[NM_TokenTypes.NM_label] = bitmapFromUri(uri);
            }
            catch (FileNotFoundException) { }
            try
            {
                uri = new Uri(extension_path + "images/Structure_16x.png");
                _icons[NM_TokenTypes.NM_data_registers] = bitmapFromUri(uri);
            }
            catch (FileNotFoundException) { }
        }
    }
}

