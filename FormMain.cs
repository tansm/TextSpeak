using System.Windows.Forms;
using System.Speech.Synthesis;
using System;

namespace EnglishReader {
    public partial class FormMain : Form {
        SpeechSynthesizer speaker = new SpeechSynthesizer();
        private bool _breakIt;

        public FormMain() {
            InitializeComponent();
            
            speaker.SpeakCompleted += speaker_SpeakCompleted;
            speaker.SpeakStarted += speaker_SpeakStarted;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            speaker.SpeakAsyncCancelAll();
            base.OnClosing(e);
        }
        
        protected override void OnClosed(System.EventArgs e) {
            base.OnClosed(e);

            speaker.SpeakCompleted -= speaker_SpeakCompleted;
            speaker.SpeakStarted -= speaker_SpeakStarted;

            this.speaker.Dispose();
        }

        void speaker_SpeakStarted(object sender, SpeakStartedEventArgs e) {
            labState.Text = "开始朗读...";
            mnuStart.Enabled = false;
            mnuStop.Enabled = true;
        }

        void speaker_SpeakCompleted(object sender, SpeakCompletedEventArgs e) {
            labState.Text = "完毕.";
            mnuStart.Enabled = true;
            mnuStop.Enabled = false;

            if (this._breakIt) {
                this._breakIt = false;
            }
            else {
                if (this.mnuLoop.Checked) {
                    Speech();
                }
            }
        }

        public void Speech() {    
            speaker.SpeakAsync(txtMain.Text);
        }

        private void mnuStart_Click(object sender, System.EventArgs e) {
            this.Speech();
        }

        private void mnuStop_Click(object sender, System.EventArgs e) {
            this._breakIt = true;
            speaker.SpeakAsyncCancelAll();
        }

        private void mnuExit_Click(object sender, System.EventArgs e) {
            this.Close();
        }

        private void mnuNew_Click(object sender, System.EventArgs e) {
            CheckFile();
            this.txtMain.Text = null;
            this._fileName = null;
            _txtChanged = false;
        }

        private bool _txtChanged;
        private string _fileName;

        private void CheckFile() {
            if (_txtChanged) {
                var result = MessageBox.Show("当前文本已经改变，是否保存?", "保存", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes) {
                    this.Save();
                }
                else if (result == DialogResult.Cancel) {
                    throw new OperationCanceledException();
                }//no
            }
        }

        private void Save() {
            if (string.IsNullOrEmpty(this._fileName)) {
                SaveAs();
            }
            System.IO.File.WriteAllText(this._fileName, txtMain.Text);
            this._txtChanged = false;
        }

        private void SaveAs() {
            var result = saveFileDialog.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK) {
                this._fileName = saveFileDialog.FileName;
                this.Save();
            }
            else {
                throw new OperationCanceledException();
            }
        }

        private void txtMain_TextChanged(object sender, EventArgs e) {
            _txtChanged = true;
        }

        private void mnuOpen_Click(object sender, EventArgs e) {
            CheckFile();
            var result = openFileDialog.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK) {
                this._fileName = openFileDialog.FileName;
                this.txtMain.Text = System.IO.File.ReadAllText(this._fileName);
                this._txtChanged = false;
            }
        }

        private void mnuSave_Click(object sender, EventArgs e) {
            this.Save();
        }

        private void mnuSaveAs_Click(object sender, EventArgs e) {
            this.SaveAs();
        }

    }
}
