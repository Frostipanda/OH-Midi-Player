using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OHMidiPlayer035.Properties;

namespace OHMidiPlayer035
{
    public partial class SettingsForm : Form
    {
        private const string SettingsFileName = "settings.ini"; // Name of the settings file

        public SettingsForm()
        {
            InitializeComponent();

            // Load settings from the ini file when the form is initialized
            LoadSettingsFromIni();

            // Bind the listbox to the CurrentLayoutIndex setting
            layoutselec.DataBindings.Add("SelectedIndex", Properties.Settings.Default, "CurrentLayoutIndex", false, DataSourceUpdateMode.OnPropertyChanged);

            // Bind the checkbox to the ignoreOH setting
            ignoreOH.DataBindings.Add("Checked", Properties.Settings.Default, "IgnoreOH", false, DataSourceUpdateMode.OnPropertyChanged);
            ignoreOH.CheckedChanged += IgnoreOH_CheckedChanged;

            // Bind the text boxes to the delay settings
            minNoteDelay.DataBindings.Add("Text", Properties.Settings.Default, "minNoteDelay", false, DataSourceUpdateMode.OnPropertyChanged);
            modHold.DataBindings.Add("Text", Properties.Settings.Default, "modHold", false, DataSourceUpdateMode.OnPropertyChanged);

            // Bind the checkbox to the AlwaysOnTop setting
            alwaysOnTop.DataBindings.Add("Checked", Properties.Settings.Default, "AlwaysOnTop", false, DataSourceUpdateMode.OnPropertyChanged);
            alwaysOnTop.CheckedChanged += AlwaysOnTop_CheckedChanged;

        }

        private void LoadSettingsFromIni()
        {
            string path = Path.Combine(Application.StartupPath, "settings.ini");
            if (File.Exists(path))
            {
                Dictionary<string, string> dictionary = (from line in File.ReadAllLines(path)
                                                         where line.Contains('=') && line.Split('=').Length == 2
                                                         select line.Split('=')).ToDictionary((string[] parts) => parts[0].Trim(), (string[] parts) => parts[1].Trim());
                if (dictionary.ContainsKey("AlwaysOnTop"))
                {
                    Settings.Default.AlwaysOnTop = bool.Parse(dictionary["AlwaysOnTop"]);
                }
                if (dictionary.ContainsKey("CurrentLayoutIndex"))
                {
                    Settings.Default.CurrentLayoutIndex = int.Parse(dictionary["CurrentLayoutIndex"]);
                }
                if (dictionary.ContainsKey("IgnoreOH"))
                {
                    Settings.Default.IgnoreOH = bool.Parse(dictionary["IgnoreOH"]);
                }
                if (dictionary.ContainsKey("minNoteDelay"))
                {
                    Settings.Default["minNoteDelay"] = int.Parse(dictionary["minNoteDelay"]);
                }
                if (dictionary.ContainsKey("modHold"))
                {
                    Settings.Default["modHold"] = int.Parse(dictionary["modHold"]);
                }
            }
        }

        private void AlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AlwaysOnTop = alwaysOnTop.Checked;
            if (Application.OpenForms["Form1"] is Form1 form)
            {
                form.UpdateAlwaysOnTop(alwaysOnTop.Checked);
            }
        }

        private void SaveSettingsToIni()
        {
            string path = Path.Combine(Application.StartupPath, "settings.ini");
            List<string> contents = new List<string>
        {
            $"AlwaysOnTop={Settings.Default.AlwaysOnTop}",
            $"CurrentLayoutIndex={Settings.Default.CurrentLayoutIndex}",
            $"IgnoreOH={Settings.Default.IgnoreOH}",
            string.Format("minNoteDelay={0}", Settings.Default["minNoteDelay"]),
            string.Format("modHold={0}", Settings.Default["modHold"])
        };
            File.WriteAllLines(path, contents);
        }

        private void IgnoreOH_CheckedChanged(object sender, EventArgs e)
        {
            // Access the main form and update the OHind picture and stop checking the process
            if (Application.OpenForms["Form1"] is Form1 mainForm)
            {
                mainForm.UpdateForIgnoreOH(ignoreOH.Checked);
            }
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save settings when the form is closing
            Properties.Settings.Default.Save();

            // Also save settings to ini file
            SaveSettingsToIni();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
