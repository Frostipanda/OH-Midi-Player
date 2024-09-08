using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OHMidiPlayer035
{
    static class Program
    {
        private static readonly string VersionUrl = "https://raw.githubusercontent.com/Frostipanda/Once-Human-Midi-Player/main/version.txt";
        private static readonly string CurrentVersion = "0.36"; // Your current version

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Show the splash screen and then load the main form
            ShowSplashScreen();
            Application.Run(new Form1());
        }

        private static void ShowSplashScreen()
        {
            using (SplashScreen splash = new SplashScreen())
            {
                splash.StartPosition = FormStartPosition.CenterScreen;
                splash.Show();
                Application.DoEvents(); // Process all windows messages

                Task.Delay(3000).Wait(); // Wait asynchronously for 3 seconds

                // Perform version check after the splash screen
                var latestVersion = CheckVersionAsync().Result;
                if (latestVersion != null && IsNewVersionAvailable(latestVersion))
                {
                    splash.Close(); // Close the splash screen before showing the update dialog
                    ShowUpdateDialog();
                }
            }
        }

        private static async Task<string> CheckVersionAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string version = await client.GetStringAsync(VersionUrl);
                    return version.Trim(); // Trim any extra whitespace/newline characters
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error checking version: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        private static bool IsNewVersionAvailable(string latestVersion)
        {
            return string.Compare(latestVersion, CurrentVersion) > 0;
        }

        private static void ShowUpdateDialog()
        {
            DialogResult result = MessageBox.Show("A new version is available. Would you like to update?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://discord.gg/yD2UGNxsdS",
                    UseShellExecute = true
                });
                Application.Exit(); // Close the application after opening the link
            }
            else
            {
                Application.Exit(); // Close the application if the user does not want to update
            }
        }
    }
}
