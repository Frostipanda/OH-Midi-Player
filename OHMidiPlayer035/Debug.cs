using System;
using System.Windows.Forms;

namespace OHMidiPlayer035
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
        }

        public void AppendDebugText(string text)
        {
            debugWin.AppendText($"{text}{Environment.NewLine}");
        }
    }
}
