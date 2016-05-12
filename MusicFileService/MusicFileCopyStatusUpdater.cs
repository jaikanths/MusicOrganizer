using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TagLib;

namespace MusicFileService
{
    public class MusicFileCopyStatusUpdater : IObserver<FileInfo>
    {
        private CopyStatusDisplay _display;
        private int count = 0;
        private List<string> _extensions = new List<string>();
        private List<string> _validExtensions = null;

        public MusicFileCopyStatusUpdater(CopyStatusDisplay display)
        {
            _display = display;
        }
        
        public void OnNext(FileInfo info)
        {
            if (IsMetaFile(info))
                return;

            count = count + 1;
            _display.Song.Text = info.Name;
            _display.Album.Text = info.DirectoryName.Split('\\').ToList().Last();
            _display.Count.Text = $"Reading File #: {count}";

            var mp3 = TagLib.File.Create(info.FullName);
            _display.Title.Text = mp3.Tag.Title;

            Application.DoEvents();
            Thread.Sleep(100);
        }

        private bool IsMetaFile(FileInfo info)
        {
            if (_validExtensions == null)
                _validExtensions = (Properties.Settings.Default.ValidExtensions == null) ? ".mp3, .wav, m4a, .wma".Split(',').ToList() : Properties.Settings.Default.ValidExtensions.Split(',').ToList();

            if (!_extensions.Contains(info.Extension))
                _extensions.Add(info.Extension);

            return !_validExtensions.Contains(info.Extension.ToLowerInvariant());
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            _display.ShowStatus("Done");
            _display.Extensions.Text = string.Join(", ", _extensions.OrderBy(q => q));
        }
    }
}
