using System;
using System.Windows.Forms;

namespace MusicFileService
{
    public class CopyStatusDisplay
    {
        public Label Album { get; set; }
        public Label Song { get; set; }
        public Label Extensions { get; set; }
        public Label Count { get; set; }
        public Label Title { get; set; }
        public Action<string> ShowStatus { get; set; }

    }
}
