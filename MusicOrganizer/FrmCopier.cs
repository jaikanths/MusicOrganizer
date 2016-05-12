using MusicFileService;
using System;
using System.Windows.Forms;

namespace MusicOrganizer
{
    public partial class FrmCopier : Form
    {
        FolderBrowserDialog _dialog = new FolderBrowserDialog();
        public FrmCopier()
        {
            InitializeComponent();
            lblDirectory.Text = "Click start to iterate";
            lblSong.Text = "No Song to Display";
            lblExtensions.Text = "Extensions will be displayed here";
            EnableStart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            if (_dialog.ShowDialog(this) == DialogResult.OK)
            {
                lblSource.Text = _dialog.SelectedPath;
            }
            EnableStart();
        }

        private void destination_Click(object sender, EventArgs e)
        {
            _dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            if (_dialog.ShowDialog(this) == DialogResult.OK)
            {
                lblDestination.Text = _dialog.SelectedPath;
            }
            EnableStart();
        }

        private void EnableStart()
        {
            btnStart.Enabled = !string.IsNullOrWhiteSpace(lblSource.Text) && !string.IsNullOrWhiteSpace(lblDestination.Text);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            var copier = new MusicFileCopier(lblSource.Text, lblDestination.Text);
            var statusDisplay = new CopyStatusDisplay { Album = lblDirectory, Count = lblFileCount, Extensions = lblExtensions, Song = lblSong, Title = lblTitle, ShowStatus = ((Master)MdiParent).ShowStatus };
            var statusUpdater = new MusicFileCopyStatusUpdater(statusDisplay);
            copier.Subscribe(statusUpdater);
            copier.Copy();
        }
    }
}
