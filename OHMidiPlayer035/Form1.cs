using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Midi;
using WindowsInput;
using WindowsInput.Native;

namespace OHMidiPlayer035
{
    public partial class Form1 : Form
    {
        
        private string scrollingText = string.Empty;
        private int scrollPosition = 0;
        private MidiIn midiIn;
        private System.Windows.Forms.Timer scrollTimer = new System.Windows.Forms.Timer();
        private DebugForm debugForm;
        private const int HOTKEY_ID_SHIFT_F4 = 5; // Unique ID for Shift+F4
        private const string SettingsFileName = "settings.ini"; // Name of the settings file

        private int totalTracks = 0;
        private int currentTrackIndex = 0;
        private int ignoreKeysClickCount = 0;
        private MidiFile currentMidiFile;
        public int CurrentLayoutIndex { get; set; } = 0; // 0 for QWERTY, 1 for AZERTY

        private CancellationTokenSource cancellationTokenSource;
        private bool isPlaying = false;

        private System.Windows.Forms.Timer processCheckTimer; // Explicitly using System.Windows.Forms.Timer
        private InputSimulator inputSimulator = new InputSimulator();

        private const int MOD_NONE = 0x0000;
        private const int WM_HOTKEY = 0x0312;
        private const int MOD_CONTROL = 0x0002;
        private const int MOD_SHIFT = 0x0004;
        private const int MOD_WIN = 0x0008;
        private VirtualKeys virtualKeys;

        private TimeSpan songDuration; // To store the total duration of the song
        private TimeSpan elapsedTime; // To store the elapsed time during playback
        private bool ignoreOHChecked = false; // Track the state of the ignoreOH checkbox

        private List<string> midiFilePaths = new List<string>(); // Original list of MIDI file paths
        private List<string> filteredMidiFilePaths = new List<string>(); // Filtered list for displaying

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        private const int HOTKEY_ID_F5 = 1;
        private const int HOTKEY_ID_F6 = 2;
        private const int HOTKEY_ID_SHIFT_F6 = 3;
        private const int HOTKEY_ID_CONTROL_F6 = 4;
        private List<VirtualKeyCode> lastPressedModifiers = new List<VirtualKeyCode>();

        public Form1()
        {
            InitializeComponent();
            selectLib.Click += SelectLib_Click;
            midiLibrary.SelectedIndexChanged += MidiLibrary_SelectedIndexChanged;
            nextTrack.Click += NextTrack_Click;
            prevTrack.Click += PrevTrack_Click;
            speedSlider.Scroll += speedSlider_Scroll;

            scrollTimer.Interval = 100;
            scrollTimer.Tick += ScrollTimer_Tick;
            songTimer.Interval = 1000; // Update every second
            songTimer.Tick += SongTimer_Tick;

            searchBox.TextChanged += SearchBox_TextChanged;

            // Initialize the process check timer
            processCheckTimer = new System.Windows.Forms.Timer(); // Explicitly using System.Windows.Forms.Timer
            processCheckTimer.Interval = 1000; // Check every second
            processCheckTimer.Tick += ProcessCheckTimer_Tick;
            ApplySettings();

            virtualKeys = new VirtualKeys(this); // Initialize VirtualKeys

            RegisterHotKeys();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            ShiftButton.Tag = "On";
            ControlButton.Tag = "On";
            loop.Tag = "Off";
            loopind.Image = Properties.Resources.off;
            CheckMidiDevice();
            Keyboardind.Image = Properties.Resources.off;
            this.TopMost = Properties.Settings.Default.AlwaysOnTop;
            CurrentLayoutIndex = Properties.Settings.Default.CurrentLayoutIndex;
            UpdateKeyMapping(); // Apply the current layout
            ApplySettings();

            ShiftButton.Enabled = true;
            ControlButton.Enabled = true;

            refreshLib.Click += refreshLib_Click;


            // Retrieve the values from settings and parse them as integers
            int modHold = int.Parse(Properties.Settings.Default["modHold"].ToString());
            int minNoteDelay = int.Parse(Properties.Settings.Default["minNoteDelay"].ToString());

            speedSlider.Value = 100;
            speedLabel.Text = "Speed: 100%";

            UpdateButtonState(ShiftButton);
            UpdateButtonState(ControlButton);

            trackind.Image = Properties.Resources.good;
            trackind.Tag = "good";

            ignoreOHChecked = Properties.Settings.Default.IgnoreOH; // Load the ignoreOH state
            UpdateForIgnoreOH(ignoreOHChecked); // Apply initial settings

            // Load the last used path from settings.ini
            string lastUsedPath = LoadLastUsedPath();
            if (!string.IsNullOrEmpty(lastUsedPath) && Directory.Exists(lastUsedPath))
            {
                LoadMidiFilesFromPath(lastUsedPath);
            }

            processCheckTimer.Start();
        }



        private void ProcessCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!ignoreOHChecked)
            {
                CheckOnceHumanProcess(); // Check if ONCE_HUMAN.exe is running and update the picture box

                // Check if ONCE_HUMAN.exe is the active window
                if (isPlaying && !IsOnceHumanFocused())
                {
                    StopPlayback();
                    MessageBox.Show("Playback Stopped Because User Clicked Away From Once Human", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    virtualKeys.ResetAllKeys(); // Reset all key highlights
                }
            }
        }

        public void UpdateForIgnoreOH(bool ignoreOH)
        {
            ignoreOHChecked = ignoreOH;
            if (ignoreOHChecked)
            {
                OHind.Image = Properties.Resources.off; // Set the image to off.png
                processCheckTimer.Stop(); // Stop checking for the ONCE_HUMAN.exe process
            }
            else
            {
                processCheckTimer.Start(); // Resume checking for the ONCE_HUMAN.exe process
            }
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchBox.Text.ToLower();

            // Filter the list based on the search text
            filteredMidiFilePaths = midiFilePaths
                .Where(path => Path.GetFileName(path).ToLower().Contains(searchText))
                .ToList();

            // Update the ListBox with the filtered items
            UpdateMidiLibraryListBox();
        }

        private void UpdateMidiLibraryListBox()
        {
            midiLibrary.Items.Clear();

            foreach (string filePath in filteredMidiFilePaths)
            {
                midiLibrary.Items.Add(Path.GetFileName(filePath));
            }

            // Optionally, select the first item in the filtered list
            if (midiLibrary.Items.Count > 0)
            {
                midiLibrary.SelectedIndex = 0;
            }
        }

        private Dictionary<VirtualKeyCode, bool> keyStates = new Dictionary<VirtualKeyCode, bool>();
        

        private void RegisterHotKeys()
        {
            RegisterHotKey(this.Handle, HOTKEY_ID_F5, MOD_NONE, (int)Keys.F5);
            RegisterHotKey(this.Handle, HOTKEY_ID_F6, MOD_NONE, (int)Keys.F6);
            RegisterHotKey(this.Handle, HOTKEY_ID_SHIFT_F6, MOD_SHIFT, (int)Keys.F6);
            RegisterHotKey(this.Handle, HOTKEY_ID_CONTROL_F6, MOD_CONTROL, (int)Keys.F6);
            RegisterHotKey(this.Handle, HOTKEY_ID_SHIFT_F4, MOD_SHIFT, (int)Keys.F4); // Register Shift+F4
        }

        private void LogDebug(string message)
        {
            Debug.WriteLine(message); // Still send to output window

            if (debugForm != null && !debugForm.IsDisposed)
            {
                debugForm.AppendDebugText(message); // Send to debug window
            }
        }



        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY)
            {
                int id = m.WParam.ToInt32();

                if (id == HOTKEY_ID_F5)
                {
                    // Check if we need to ignore ONCE_HUMAN checks
                    if (ignoreOHChecked || (IsOnceHumanRunning() && IsOnceHumanFocused()))
                    {
                        StartPlayback();
                    }
                    else
                    {
                        MessageBox.Show("Please Click F5 While Inside Once Human", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (id == HOTKEY_ID_F6 || id == HOTKEY_ID_SHIFT_F6 || id == HOTKEY_ID_CONTROL_F6)
                {
                    StopPlayback(); // Stop playback when F6, Shift+F6, or Control+F6 is pressed
                }
                else if (id == HOTKEY_ID_SHIFT_F4) // Handle Shift+F4
                {
                    if (debugForm == null || debugForm.IsDisposed)
                    {
                        debugForm = new DebugForm();
                    }

                    debugForm.Show();
                    debugForm.BringToFront();
                }
            }
        }





        private bool IsOnceHumanFocused()
        {
            IntPtr hwnd = GetForegroundWindow();
            GetWindowThreadProcessId(hwnd, out uint processId);
            Process process = Process.GetProcessById((int)processId);
            return process.ProcessName.Equals("ONCE_HUMAN", StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateButtonState(Button button)
        {
            if (button.Tag as string == "Off")
            {
                button.Enabled = false; // Disable the button
                button.BackColor = Color.DarkGray; // Change background color to dark grey
                button.ForeColor = Color.LightGray; // Optionally change text color for better visibility
            }
            else
            {
                button.Enabled = true; // Enable the button
                button.BackColor = SystemColors.Control; // Restore default color
                button.ForeColor = SystemColors.ControlText; // Restore default text color
            }
        }

        // MIDI KEYBOARD COMMANDS FOR KEYBOARD INPUT //

        private void CheckMidiDevice()
        {
            int deviceCount = MidiIn.NumberOfDevices;
            if (deviceCount > 0)
            {

                Keyboardind.Image = Properties.Resources.good;


                midiIn = new MidiIn(0);
                midiIn.MessageReceived += MidiIn_MessageReceived;
                midiIn.Start();
            }
            else
            {

                Keyboardind.Image = Properties.Resources.off;
            }
        }

        private Dictionary<int, List<VirtualKeyCode>> activeNotes = new Dictionary<int, List<VirtualKeyCode>>();

        private async void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            if (e.MidiEvent is NoteEvent noteEvent)
            {
                int midiKey = noteEvent.NoteNumber;

                // Check if the key is mapped
                if (MidiKeyMap.ContainsKey(midiKey))
                {
                    var keyCombination = MidiKeyMap.MidiToKey[midiKey];

                    if (noteEvent.CommandCode == MidiCommandCode.NoteOn && noteEvent.Velocity > 0)
                    {
                        // Handle NoteOn event
                        if (!activeNotes.ContainsKey(midiKey))
                        {
                            activeNotes[midiKey] = keyCombination;
                            await PressKeyAsync(keyCombination); // Press the keys
                        }
                    }
                    else if (noteEvent.CommandCode == MidiCommandCode.NoteOff ||
                             (noteEvent.CommandCode == MidiCommandCode.NoteOn && noteEvent.Velocity == 0))
                    {
                        // Handle NoteOff event (or NoteOn with velocity 0)
                        if (activeNotes.ContainsKey(midiKey))
                        {
                            await ReleaseKeyAsync(keyCombination); // Release the keys
                            activeNotes.Remove(midiKey);
                        }
                    }
                }
            }
        }







        private string LoadLastUsedPath()
        {
            string filePath = Path.Combine(Application.StartupPath, SettingsFileName);
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            return null;
        }

        private void SaveLastUsedPath(string path)
        {
            string filePath = Path.Combine(Application.StartupPath, SettingsFileName);
            File.WriteAllText(filePath, path);
        }

        private void LoadMidiFilesFromPath(string path)
        {
            string[] files = Directory.GetFiles(path, "*.mid");
            midiFilePaths.Clear(); // Clear the original list
            filteredMidiFilePaths.Clear(); // Clear the filtered list
            midiLibrary.Items.Clear(); // Clear the ListBox

            foreach (string file in files)
            {
                midiFilePaths.Add(file);
                filteredMidiFilePaths.Add(file); // Also add to the filtered list
            }

            // Display the files in the ListBox
            UpdateMidiLibraryListBox();
        }

        private void SelectLib_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;

                    // Load MIDI files from the selected path
                    LoadMidiFilesFromPath(selectedPath);

                    // Save the selected path to settings.ini
                    SaveLastUsedPath(selectedPath);
                }
            }
        }

        private void refreshLib_Click(object sender, EventArgs e)
        {
            // Load the last used path from settings.ini
            string lastUsedPath = LoadLastUsedPath();
            if (!string.IsNullOrEmpty(lastUsedPath) && Directory.Exists(lastUsedPath))
            {
                // Reload the MIDI files from the last used path
                LoadMidiFilesFromPath(lastUsedPath);
            }
            else
            {
                MessageBox.Show("No valid path found or directory doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void MidiLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (midiLibrary.SelectedIndex >= 0)
            {
                // Determine which list to use based on whether the search box is empty
                List<string> currentList = string.IsNullOrEmpty(searchBox.Text) ? midiFilePaths : filteredMidiFilePaths;

                // Get the selected file from the appropriate list
                string selectedFileName = Path.GetFileName(currentList[midiLibrary.SelectedIndex]);
                currentSong.Text = selectedFileName;
                StartScrollingText(selectedFileName);

                // Load the MIDI file based on the appropriate list
                LoadMidiFile(currentList[midiLibrary.SelectedIndex]);

                // Calculate and display the song duration
                if (currentMidiFile != null)
                {
                    long totalTimeInSeconds = (long)(currentMidiFile.Events[0].Last().AbsoluteTime * currentMidiFile.DeltaTicksPerQuarterNote / 500000.0);
                    songDuration = TimeSpan.FromSeconds(totalTimeInSeconds);
                    songTime.Text = "00:00";  // Initialize songTime to 00:00
                    songTotal.Text = songDuration.ToString(@"mm\:ss");  // Display total duration in songTotal label
                }
            }
        }



        private void SongTimer_Tick(object sender, EventArgs e)
        {
            if (elapsedTime < songDuration)
            {
                elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
                songTime.Text = $"{elapsedTime.ToString(@"mm\:ss")}";  // Update elapsed time with correct format
            }
            else
            {
                songTimer.Stop(); // Stop the timer when the song is done
            }
        }









        private void LoadMidiFile(string filePath)
        {
            try
            {
                currentMidiFile = new MidiFile(filePath, false);
                totalTracks = currentMidiFile.Tracks;
                currentTrackIndex = 0;
                UpdateTrackInfo();

                // Calculate and display the song duration using the new method
                songDuration = GetMidiFileDuration(currentMidiFile);
                songTime.Text = "00:00";  // Reset the elapsed time to 00:00
                songTotal.Text = $"{songDuration.ToString(@"mm\:ss")}";  // Display the total duration with correct format
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading MIDI file: " + ex.Message);
            }
        }









        private void UpdateTrackInfo()
        {
            if (currentMidiFile != null)
            {
                trackInfo.Text = $"Track {currentTrackIndex + 1}/{totalTracks}";
            }
        }

        private void NextTrack_Click(object sender, EventArgs e)
        {
            if (currentMidiFile != null && currentTrackIndex < totalTracks - 1)
            {
                currentTrackIndex++;
                UpdateTrackInfo();
            }
        }

        private void PrevTrack_Click(object sender, EventArgs e)
        {
            if (currentMidiFile != null && currentTrackIndex > 0)
            {
                currentTrackIndex--;
                UpdateTrackInfo();
            }
        }

        private void loop_Click(object sender, EventArgs e)
        {
            if (loop.Tag as string == "Off")
            {
                loop.Tag = "On";
                loopind.Image = Properties.Resources.good; // Change image to "good.png"
            }
            else
            {
                loop.Tag = "Off";
                loopind.Image = Properties.Resources.off; // Change image to "off.png"
            }
        }


        private void StartScrollingText(string text)
        {
            scrollTimer.Stop();
            scrollingText = text;
            scrollPosition = 0;

            if (TextRenderer.MeasureText(scrollingText, currentSong.Font).Width > currentSong.Width)
            {
                scrollTimer.Start();
            }
            else
            {
                currentSong.Text = scrollingText;
            }
        }

        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            if (scrollPosition < scrollingText.Length)
            {
                currentSong.Text = scrollingText.Substring(scrollPosition);
                scrollPosition++;
            }
            else
            {
                scrollTimer.Stop();
                scrollPosition = 0;
                System.Windows.Forms.Timer pauseTimer = new System.Windows.Forms.Timer(); // Explicitly using System.Windows.Forms.Timer
                pauseTimer.Interval = 2000;
                pauseTimer.Tick += (s, args) =>
                {
                    pauseTimer.Stop();
                    scrollTimer.Start();
                };
                pauseTimer.Start();
            }
        }

        private void speedSlider_Scroll(object sender, EventArgs e)
        {
            speedLabel.Text = $"Speed: {speedSlider.Value}%";

            // If playing, adjust tempo immediately
            if (isPlaying)
            {
                int tempo = GetInitialTempo(currentMidiFile);
                tempo = (int)(tempo * (100.0 / speedSlider.Value)); // Adjust tempo based on new slider value
                Debug.WriteLine($"Adjusted Tempo: {tempo} microseconds per quarter note");
            }
        }


        // Method to start playback
        private void StartPlayback()
        {
            if (isPlaying)
            {
                MessageBox.Show("Playback is already in progress.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (currentMidiFile == null)
            {
                MessageBox.Show("Please select a MIDI file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Only check ONCE_HUMAN if ignoreOHChecked is false
            if (!ignoreOHChecked)
            {
                if (!IsOnceHumanRunning() || !IsOnceHumanFocused())
                {
                    MessageBox.Show("Please click F5 while inside Once Human", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            cancellationTokenSource = new CancellationTokenSource();
            isPlaying = true;

            // Reset and start the song timer
            elapsedTime = TimeSpan.Zero;
            songTimer.Start();

            Task.Run(() => PlayMidi(cancellationTokenSource.Token));
        }






        // Method to stop playback and reset
        // Method to stop playback and reset
        private void StopPlayback()
        {
            cancellationTokenSource?.Cancel();
            isPlaying = false;

            // Stop the song timer and reset the elapsed time
            songTimer.Stop();
            songTime.Text = "0:00";

            // If you want to ensure all modifiers are released, call ReleaseModifiers with an empty list or last used keys
            ReleaseModifiers(new List<VirtualKeyCode>());
        }




        public void UpdateKeyMapping()
        {
            if (CurrentLayoutIndex == 0)
            {
                MidiKeyMap.SetLayout("QWERTY");
            }
            else if (CurrentLayoutIndex == 1)
            {
                MidiKeyMap.SetLayout("AZERTY");
            }
        }

        private void ResetUI()
        {
            currentMidiFile = null;
            midiLibrary.ClearSelected();
            currentSong.Text = string.Empty;
            trackInfo.Text = string.Empty;
        }

        private void ProcessMidiEvent(MidiEvent midiEvent, ref int lastTime, int tempo, int ticksPerQuarterNote)
        {
            int delay = CalculateDelay((int)midiEvent.AbsoluteTime, lastTime, tempo, ticksPerQuarterNote);

            if (delay > 0)
            {
                PreciseSleep(delay);

            }

            lastTime = (int)midiEvent.AbsoluteTime;

            if (midiEvent is NoteOnEvent noteOnEvent && noteOnEvent.Velocity > 0)
            {
                // Send key press based on the note using the MidiKeyMap
                if (MidiKeyMap.MidiToKey.ContainsKey(noteOnEvent.NoteNumber))
                {
                    var keys = MidiKeyMap.MidiToKey[noteOnEvent.NoteNumber];

                    foreach (var key in keys)
                    {
                        inputSimulator.Keyboard.KeyDown(key);
                    }

                    PreciseSleep(50); // Small delay for key press

                    foreach (var key in keys)
                    {
                        inputSimulator.Keyboard.KeyUp(key);
                    }
                }
            }
        }

        private void ignorekeys_Click(object sender, EventArgs e)
        {
            ignoreKeysClickCount++;

            switch (ignoreKeysClickCount % 4)
            {
                case 1:
                    ShiftButton.Tag = "Off";
                    ControlButton.Tag = "On";
                    break;

                case 2:
                    ShiftButton.Tag = "On";
                    ControlButton.Tag = "Off";
                    break;

                case 3:
                    ShiftButton.Tag = "Off";
                    ControlButton.Tag = "Off";
                    break;

                case 0:
                    ShiftButton.Tag = "On";
                    ControlButton.Tag = "On";
                    break;
            }

            // Update button states
            UpdateButtonState(ShiftButton);
            UpdateButtonState(ControlButton);
        }

        private void Coffee_Click(object sender, EventArgs e)
        {
            // The URL of the webpage you want to open
            string url = "https://buymeacoffee.com/ohmidiplayer";

            // Open the webpage in the default browser
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // UseShellExecute is required to open URLs on .NET Core/.NET 5+ or later
            });
        }


        private async Task PlayMidi(CancellationToken token)
        {
            try
            {
                if (currentMidiFile == null)
                {
                    MessageBox.Show("No MIDI file loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                do
                {
                    int ticksPerQuarterNote = currentMidiFile.DeltaTicksPerQuarterNote;
                    int tempo = GetInitialTempo(currentMidiFile);
                    tempo = (int)(tempo * (100.0 / speedSlider.Value)); // Adjust tempo based on speed slider
                    int lastTime = 0;

                    List<(MidiEvent midiEvent, int absoluteTime)> allEvents = CollectMidiEvents(currentMidiFile, ticksPerQuarterNote, tempo);

                    foreach (var (midiEvent, absoluteTime) in allEvents)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return;
                        }

                        // Update tempo dynamically based on current position
                        tempo = GetCurrentTempo(absoluteTime, currentMidiFile);
                        tempo = (int)(tempo * (100.0 / speedSlider.Value)); // Adjust for speed slider

                        // Calculate the delay before the next event
                        int delay = CalculateDelay(absoluteTime, lastTime, tempo, ticksPerQuarterNote);

                        // Ensure a minimum delay of 1.5ms between events
                        delay = Math.Max(delay, 2);

                        await Task.Delay(delay); // Apply delay before handling keys

                        lastTime = absoluteTime;

                        if (midiEvent is NoteOnEvent noteOn)
                        {
                            if (MidiKeyMap.MidiToKey.TryGetValue(noteOn.NoteNumber, out var keys))
                            {
                                if (noteOn.CommandCode == MidiCommandCode.NoteOn && noteOn.Velocity > 0)
                                {
                                    // Debug: Log note pressed
                                    LogDebug($"Note Pressed (PlayMidi): {noteOn.NoteNumber} -> {string.Join(", ", keys)}");

                                    // Handle NoteOn event
                                    await PressKeyAsync(keys);
                                }
                                else if (noteOn.CommandCode == MidiCommandCode.NoteOn && noteOn.Velocity == 0)
                                {
                                    // Debug: Log note released
                                    LogDebug($"Note Released (PlayMidi): {noteOn.NoteNumber} -> {string.Join(", ", keys)}");

                                    // Handle NoteOff event (NoteOn with velocity 0)
                                    await ReleaseKeyAsync(keys);
                                }
                            }
                        }
                    }
                } while (loop.Tag as string == "On" && !token.IsCancellationRequested); // Loop if enabled and not canceled
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Playback error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isPlaying = false;
                // Ensure all keys are released after playback ends
                foreach (var key in keyStates.Where(k => k.Value).Select(k => k.Key).ToList())
                {
                    await ReleaseKeyAsync(new List<VirtualKeyCode> { key });
                }
            }
        }









        private void PreciseSleep(int milliseconds)
        {
            if (milliseconds > 10)
            {
                Thread.Sleep(milliseconds - 10); // Sleep for most of the duration
            }
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedMilliseconds < milliseconds)
            {
                // Busy-wait loop to fine-tune the remaining duration
            }
        }





        public void RefreshSettings()
        {
            this.TopMost = Properties.Settings.Default.AlwaysOnTop;
            CurrentLayoutIndex = Properties.Settings.Default.CurrentLayoutIndex;
            UpdateKeyMapping();
        }




        private int GetInitialTempo(MidiFile midiFile)
        {
            foreach (var track in midiFile.Events)
            {
                var tempoEvent = track.OfType<TempoEvent>().FirstOrDefault();
                if (tempoEvent != null)
                {
                    int tempo = tempoEvent.MicrosecondsPerQuarterNote;
                    LogDebug($"Initial Tempo: {tempo} microseconds per quarter note");
                    return tempo;
                }
            }

            return 500000; // Default tempo if no tempo event is found
        }



        private TimeSpan GetMidiFileDuration(MidiFile midiFile)
        {
            long lastAbsoluteTime = 0;

            foreach (var track in midiFile.Events)
            {
                if (track.Count > 0)
                {
                    var lastEvent = track.Last();
                    if (lastEvent.AbsoluteTime > lastAbsoluteTime)
                    {
                        lastAbsoluteTime = lastEvent.AbsoluteTime;
                    }
                }
            }

            // Calculate duration in microseconds
            var ticksPerQuarterNote = midiFile.DeltaTicksPerQuarterNote;
            var microsecondsPerQuarterNote = GetInitialTempo(midiFile);
            var totalMicroseconds = (lastAbsoluteTime * microsecondsPerQuarterNote) / ticksPerQuarterNote;

            return TimeSpan.FromMilliseconds(totalMicroseconds / 1000.0);
        }



        private List<(MidiEvent midiEvent, int absoluteTime)> CollectMidiEvents(MidiFile midiFile, int ticksPerQuarterNote, int tempo)
        {
            var allEvents = new List<(MidiEvent, int)>();
            int[] absoluteTimes = new int[midiFile.Tracks];

            for (int trackIndex = 0; trackIndex < midiFile.Events.Tracks; trackIndex++)
            {
                foreach (MidiEvent midiEvent in midiFile.Events[trackIndex])
                {
                    allEvents.Add((midiEvent, absoluteTimes[trackIndex]));
                    absoluteTimes[trackIndex] += midiEvent.DeltaTime;
                }
            }

            allEvents.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            return allEvents;
        }

        private int CalculateDelay(int absoluteTime, int lastTime, int tempo, int ticksPerQuarterNote)
        {
            int deltaTime = absoluteTime - lastTime;
            int delay = (int)((deltaTime * (tempo / 1000.0)) / ticksPerQuarterNote);

            LogDebug($"Absolute Time: {absoluteTime}, Delay: {delay}ms, Tempo: {tempo}");

            return delay;
        }

        private int GetCurrentTempo(int absoluteTime, MidiFile midiFile)
        {
            int currentTempo = 500000; // Default tempo
            foreach (var track in midiFile.Events)
            {
                foreach (var midiEvent in track)
                {
                    if (midiEvent.AbsoluteTime > absoluteTime)
                        break;

                    if (midiEvent is TempoEvent tempoEvent)
                        currentTempo = tempoEvent.MicrosecondsPerQuarterNote;
                }
            }
            return currentTempo;
        }



        private void HandleModifiers(List<VirtualKeyCode> keys)
        {
            foreach (var key in keys)
            {
                if ((key == VirtualKeyCode.LCONTROL || key == VirtualKeyCode.RCONTROL ||
                     key == VirtualKeyCode.LSHIFT || key == VirtualKeyCode.RSHIFT) &&
                    (!keyStates.ContainsKey(key) || !keyStates[key]))
                {
                    inputSimulator.Keyboard.KeyDown(key);
                    keyStates[key] = true; // Mark the modifier as pressed
                    LogDebug($"Modifier Down: {key} at {DateTime.Now}");

                    // Add a small delay after pressing the modifier to ensure it is active
                    Thread.Sleep(10);
                }
            }
        }

        private async Task PressKeyAsync(List<VirtualKeyCode> keys)
        {
            int modHold = int.Parse(Properties.Settings.Default["modHold"].ToString());
            int minNoteDelay = int.Parse(Properties.Settings.Default["minNoteDelay"].ToString());

            var modifiers = keys.Where(k => IsModifierKey(k)).ToList();
            var mainKeys = keys.Except(modifiers).ToList();

            // Press all new modifiers first
            foreach (var modifier in modifiers)
            {
                if (!keyStates.ContainsKey(modifier) || !keyStates[modifier])
                {
                    inputSimulator.Keyboard.KeyDown(modifier);
                    keyStates[modifier] = true;
                    LogDebug($"Modifier Down: {modifier}");  // Log when a modifier is pressed
                    virtualKeys.HighlightKey(modifier);  // Highlight the modifier key
                    await Task.Delay(modHold); // Small delay to ensure the modifier is registered
                }
            }

            // Press all main keys, only if they haven't been pressed yet
            foreach (var key in mainKeys)
            {
                if (!keyStates.ContainsKey(key) || !keyStates[key])
                {
                    inputSimulator.Keyboard.KeyDown(key);
                    keyStates[key] = true;
                    LogDebug($"Key Down: {key}");  // Log when a key is pressed
                    virtualKeys.HighlightKey(key);  // Highlight the main key
                    await Task.Delay(minNoteDelay); // Delay to simulate key press time
                }
            }

            // Release the main keys after pressing them
            foreach (var key in mainKeys)
            {
                if (keyStates.ContainsKey(key) && keyStates[key])
                {
                    inputSimulator.Keyboard.KeyUp(key);
                    keyStates[key] = false;
                    LogDebug($"Key Up: {key}");  // Log when a key is released
                    virtualKeys.ResetKey(key);  // Reset the main key color
                    await Task.Delay(1); // Ensure proper release
                }
            }

            // Release the modifiers after releasing the main keys
            foreach (var modifier in modifiers)
            {
                if (keyStates.ContainsKey(modifier) && keyStates[modifier])
                {
                    inputSimulator.Keyboard.KeyUp(modifier);
                    keyStates[modifier] = false;
                    LogDebug($"Modifier Up: {modifier}");  // Log when a modifier is released
                    virtualKeys.ResetKey(modifier);  // Reset the modifier key color
                    await Task.Delay(1); // Small delay to ensure proper timing
                }
            }

            // Update the list of last pressed modifiers
            lastPressedModifiers = modifiers;
        }



        private async Task ReleaseKeyAsync(List<VirtualKeyCode> keys)
        {
            var modifiers = keys.Where(k => IsModifierKey(k)).ToList();
            var mainKeys = keys.Except(modifiers).ToList();

            // Release all main keys first, only if they are currently pressed
            foreach (var key in mainKeys)
            {
                if (keyStates.ContainsKey(key) && keyStates[key])
                {
                    inputSimulator.Keyboard.KeyUp(key);
                    keyStates[key] = false;
                    virtualKeys.ResetKey(key);  // Reset the main key color
                    await Task.Delay(1); // Ensure proper release
                }
            }

            // Release all modifiers last, only if they are currently pressed
            foreach (var modifier in modifiers)
            {
                if (keyStates.ContainsKey(modifier) && keyStates[modifier])
                {
                    inputSimulator.Keyboard.KeyUp(modifier);
                    keyStates[modifier] = false;
                    virtualKeys.ResetKey(modifier);  // Reset the modifier key color
                    await Task.Delay(1); // Small delay to ensure proper timing
                }
            }
        }


        private bool IsModifierKey(VirtualKeyCode key)
        {
            return key == VirtualKeyCode.LCONTROL || key == VirtualKeyCode.RCONTROL ||
                   key == VirtualKeyCode.LSHIFT || key == VirtualKeyCode.RSHIFT ||
                   key == VirtualKeyCode.LMENU || key == VirtualKeyCode.RMENU;
        }


















        private void ReleaseModifiers(List<VirtualKeyCode> keys)
        {
            foreach (var key in keys)
            {
                if ((key == VirtualKeyCode.LCONTROL || key == VirtualKeyCode.RCONTROL ||
                     key == VirtualKeyCode.LSHIFT || key == VirtualKeyCode.RSHIFT) &&
                    keyStates.ContainsKey(key) && keyStates[key])
                {
                    Thread.Sleep(10); // Delay before releasing the modifier to ensure proper timing with the main key
                    inputSimulator.Keyboard.KeyUp(key);
                    keyStates[key] = false; // Mark the modifier as released
                    LogDebug($"Modifier Up: {key} at {DateTime.Now}");
                }
            }
        }


        // Method to check if Once Human is running
        private bool IsOnceHumanRunning()
        {
            return Process.GetProcessesByName("ONCE_HUMAN").Any();
        }

        // Method to update OHind picture box based on the process
        private void CheckOnceHumanProcess()
        {
            if (IsOnceHumanRunning())
            {
                OHind.Image = Properties.Resources.good;
            }
            else
            {
                OHind.Image = Properties.Resources.error;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID_F5);
            UnregisterHotKey(this.Handle, HOTKEY_ID_F6);
            base.OnFormClosed(e);
        }

        private void playall_Click(object sender, EventArgs e)
        {
            // Check the tag to determine the current image
            if (trackind.Tag as string == "good")
            {
                // Set to "off.png", indicating only the selected track will play
                trackind.Image = Properties.Resources.off;
                trackind.Tag = "off"; // Update the tag to "off"
            }
            else
            {
                // Set to "good.png", indicating all tracks will play
                trackind.Image = Properties.Resources.good;
                trackind.Tag = "good"; // Update the tag to "good"
            }
        }

        private void Discord_Click_1(object sender, EventArgs e)
        {
            // The URL of the webpage you want to open
            string url = "https://discord.gg/bSeZ8EDYAj";

            // Open the webpage in the default browser
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // UseShellExecute is required to open URLs on .NET Core/.NET 5+ or later
            });
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            using (SettingsForm settingsForm = new SettingsForm())
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // Reapply settings after the settings form is closed
                    ApplySettings();
                }
            }
        }

        private void ApplySettings()
        {
            this.TopMost = Properties.Settings.Default.AlwaysOnTop;
            CurrentLayoutIndex = Properties.Settings.Default.CurrentLayoutIndex;
            UpdateKeyMapping();  // Apply the key mapping based on the current layout
        }
    }
}