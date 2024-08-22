using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
        }

        private void LoadSettingsFromIni()
        {
            string filePath = Path.Combine(Application.StartupPath, SettingsFileName);
            if (File.Exists(filePath))
            {
                var settings = File.ReadAllLines(filePath)
                    .Where(line => line.Contains('=') && line.Split('=').Length == 2)
                    .Select(line => line.Split('='))
                    .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());

                // Update application settings based on the ini file
                if (settings.ContainsKey("AlwaysOnTop"))
                    Properties.Settings.Default.AlwaysOnTop = bool.Parse(settings["AlwaysOnTop"]);
                if (settings.ContainsKey("CurrentLayoutIndex"))
                    Properties.Settings.Default.CurrentLayoutIndex = int.Parse(settings["CurrentLayoutIndex"]);
                if (settings.ContainsKey("IgnoreOH"))
                    Properties.Settings.Default.IgnoreOH = bool.Parse(settings["IgnoreOH"]);
                if (settings.ContainsKey("minNoteDelay"))
                    Properties.Settings.Default["minNoteDelay"] = int.Parse(settings["minNoteDelay"]);
                if (settings.ContainsKey("modHold"))
                    Properties.Settings.Default["modHold"] = int.Parse(settings["modHold"]);
            }
        }


        private void SaveSettingsToIni()
        {
            string filePath = Path.Combine(Application.StartupPath, SettingsFileName);
            var lines = new List<string>
            {
                $"AlwaysOnTop={Properties.Settings.Default.AlwaysOnTop}",
                $"CurrentLayoutIndex={Properties.Settings.Default.CurrentLayoutIndex}",
                $"IgnoreOH={Properties.Settings.Default.IgnoreOH}",
                $"minNoteDelay={Properties.Settings.Default["minNoteDelay"]}",
                $"modHold={Properties.Settings.Default["modHold"]}"
            };
            File.WriteAllLines(filePath, lines);
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
    }
}
