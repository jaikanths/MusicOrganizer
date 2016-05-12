using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicFileService
{
    public class MusicFileCopier : IObservable<FileInfo>, IDisposable
    {
        private string _destination;
        private string _source;
        private IObserver<FileInfo> _observer;
        private List<string> _validExtensions;

        public MusicFileCopier(string source, string destination)
        {
            _source = source;
            _destination = destination;
        }

        public IDisposable Subscribe(IObserver<FileInfo> observer)
        {
            _observer = observer;
            return this;
        }

        public void Copy()
        {
            _validExtensions = (Properties.Settings.Default.ValidExtensions == null) ? ".mp3, .wav, m4a, .wma".Split(',').ToList() : Properties.Settings.Default.ValidExtensions.Split(',').ToList();

            GetSongs(_source);

            var albums = Directory.EnumerateDirectories(_source).ToList();

            foreach (var album in albums)            
                GetSongs(album);

            _observer.OnCompleted();
        }

        private void GetSongs(string album)
        {
            var songs = Directory.EnumerateFiles(album).ToList();

            foreach (var song in songs)
            {
                var info = new FileInfo(song);
                Copy(info);
                _observer.OnNext(info);
            }
        }

        private void Copy(FileInfo info)
        {
            if (Properties.Settings.Default.TestMode)
                return;

            if (!_validExtensions.Contains(info.Extension.ToLowerInvariant()))
                return;

            var destName = $"{_destination}\\{info.Name}";
            if (File.Exists(destName))
                destName = $"{_destination}\\{Path.GetFileNameWithoutExtension(info.Name)}{DateTime.Now.Ticks.ToString()}.{info.Extension}";

            File.Copy(info.FullName, destName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _observer = null;
        }
    }
}