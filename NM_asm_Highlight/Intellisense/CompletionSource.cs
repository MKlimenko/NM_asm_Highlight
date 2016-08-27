using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using System.Windows.Media.Imaging;

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
        private ITextBuffer _buffer;
        private bool _disposed = false;
        private readonly IDictionary<NM_TokenTypes, System.Windows.Media.ImageSource> _icons;

        public NMCompletionSource(ITextBuffer buffer)
        {
            _buffer = buffer;
            this._icons = new Dictionary<NM_TokenTypes, System.Windows.Media.ImageSource>();
            this.loadIcons();
        }
        
        private string GetImageFullPath(string filename)
        {
            return System.IO.Path.Combine(
                    //Get the location of your package dll
                    System.Reflection.Assembly.GetExecutingAssembly().Location,
                    //reference your 'images' folder
                    "/images/",
                    filename
                 );
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (_disposed)
                throw new ObjectDisposedException("NMCompletionSource");

            List<Completion> completions = new List<Completion>();

            foreach (var el in NM_asm_highlight.Dictionary_asm.Keywords)
            {
               completions.Add(new Completion(el, el, null, _icons[NM_TokenTypes.NM_Keyword], ""));
            }

            foreach (var el in NM_asm_highlight.Dictionary_asm.Directives)
            {
                completions.Add(new Completion(el, el, null, _icons[NM_TokenTypes.NM_directive], ""));
            }

            foreach (var el in NM_asm_highlight.Dictionary_asm.Labels)
            {
                completions.Add(new Completion(el, el, null, _icons[NM_TokenTypes.NM_label], ""));
            }

            foreach (var el in NM_asm_highlight.Dictionary_asm.Data_registers)
            {
                completions.Add(new Completion(el, el, null, _icons[NM_TokenTypes.NM_data_registers], ""));
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
                string fullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string filenameDll = "NM_asm_Highlight.dll";
                return fullPath.Substring(0, fullPath.Length - filenameDll.Length);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static System.Windows.Media.ImageSource bitmapFromUri(Uri bitmapUri)
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
            Uri uri = null;
            string extension_path = getInstallPath();
            try
            {
                uri = new Uri(extension_path + "images/Method_16x.png");
                this._icons[NM_TokenTypes.NM_Keyword] = bitmapFromUri(uri);
            }
            catch (System.IO.FileNotFoundException) { }
            try
            {
                uri = new Uri(extension_path + "images/Partition_16x.png");
                this._icons[NM_TokenTypes.NM_directive] = bitmapFromUri(uri);
            }
            catch (System.IO.FileNotFoundException) { }
            try
            {
                uri = new Uri(extension_path + "images/Component_16x.png");
                this._icons[NM_TokenTypes.NM_label] = bitmapFromUri(uri);
            }
            catch (System.IO.FileNotFoundException) { }
            try
            {
                uri = new Uri(extension_path + "images/Structure_16x.png");
                this._icons[NM_TokenTypes.NM_data_registers] = bitmapFromUri(uri);
            }
            catch (System.IO.FileNotFoundException) { }
        }
    }
}

