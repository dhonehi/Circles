using System;
using System.Drawing;
using System.Windows.Forms;

namespace Circles {
    public partial class Form1 : Form {
        private readonly CirclesController _circlesController;
        private bool _isInitialized = false;

        public Form1() {
            InitializeComponent();
            _circlesController = new CirclesController(
                ClientSize.Height - menuStrip1.Height,
                ClientSize.Width);
        }

        private void TimerTick(object sender, EventArgs e) {
            _circlesController.Move(smartToolStripMenuItem.Checked);

            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) {
            if (_isInitialized) {
                _circlesController.RepaintAll(e.Graphics);
            }

            _isInitialized = true;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e) {
            _timer.Stop();
            _circlesController.AddOrRemoveCircle(e);
            pictureBox1.Invalidate();
            _timer.Start();
        }

        private void smartToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            defaultToolStripMenuItem.Checked = !smartToolStripMenuItem.Checked;
        }

        private void defaultToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            smartToolStripMenuItem.Checked = !defaultToolStripMenuItem.Checked;
        }
    }
}