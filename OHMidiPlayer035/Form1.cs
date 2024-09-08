using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using OHMidiPlayer035.Properties;
using WindowsInput;
using WindowsInput.Native;

namespace OHMidiPlayer035
{
    public partial class Form1 : Form
    {
        
        private string scrollingText = string.Empty;
        private int scrollPosition = 0;
        private System.Windows.Forms.Timer scrollTimer = new System.Windows.Forms.Timer();
        private DebugForm debugForm;
        private const int HOTKEY_ID_SHIFT_F4 = 5; // Unique ID for Shift+F4
        private const string SettingsFileName = "settings.ini"; // Name of the settings file
        private List<string> trackNames = new List<string>();

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
            base.TopMost = Settings.Default.AlwaysOnTop;

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

        private List<(MidiEvent midiEvent, long absoluteTime, int tempo, int trackIndex)> BuildPlaybackQueue()
        {
            List<(MidiEvent, long, int, int)> list = new List<(MidiEvent, long, int, int)>();
            if (currentMidiFile == null)
            {
                MessageBox.Show("No MIDI file loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return list;
            }
            int ticksPerQuarterNote = ((TicksPerQuarterNoteTimeDivision)currentMidiFile.TimeDivision).TicksPerQuarterNote;
            int initialTempo = GetInitialTempo(currentMidiFile);
            int initialTempo2 = (int)((double)initialTempo * (100.0 / (double)speedSlider.Value));
            List<(MidiEvent, long, int, int)> list2 = CollectMidiEventsWithTrack(currentMidiFile, ticksPerQuarterNote, initialTempo2);
            if (trackind.Tag as string == "off")
            {
                list2 = list2.Where<(MidiEvent, long, int, int)>(((MidiEvent midiEvent, long absoluteTime, int tempo, int trackIndex) e) => e.trackIndex == currentTrackIndex).ToList();
            }
            if (ignoreKeysClickCount > 0)
            {
                list2 = FilterIgnoredKeysWithLong(list2);
            }
            list.AddRange(list2);
            return list;
        }

        private List<(MidiEvent midiEvent, long absoluteTime, int tempo, int trackIndex)> FilterIgnoredKeysWithLong(List<(MidiEvent midiEvent, long absoluteTime, int tempo, int trackIndex)> allEvents)
        {
            List<(MidiEvent, long, int, int)> list = new List<(MidiEvent, long, int, int)>();
            foreach (var (midiEvent, item, item2, item3) in allEvents)
            {
                if (midiEvent is NoteOnEvent noteOnEvent)
                {
                    if (!MidiKeyMap.MidiToKey.TryGetValue((byte)noteOnEvent.NoteNumber, out var value))
                    {
                        continue;
                    }
                    List<VirtualKeyCode> list2 = new List<VirtualKeyCode>(value);
                    if (ignoreKeysClickCount == 1)
                    {
                        list2.RemoveAll((VirtualKeyCode k) => k == VirtualKeyCode.LSHIFT);
                    }
                    else if (ignoreKeysClickCount == 2)
                    {
                        list2.RemoveAll((VirtualKeyCode k) => k == VirtualKeyCode.LCONTROL);
                    }
                    else if (ignoreKeysClickCount == 3)
                    {
                        list2.RemoveAll((VirtualKeyCode k) => k == VirtualKeyCode.LSHIFT || k == VirtualKeyCode.LCONTROL);
                    }
                    if (list2.Count == 0)
                    {
                        continue;
                    }
                }
                list.Add((midiEvent, item, item2, item3));
            }
            return list;
        }

        private void SafeUpdateDebugWindow(string message)
        {
            if (debugForm.InvokeRequired)
            {
                debugForm.Invoke((Action)delegate
                {
                    SafeUpdateDebugWindow(message);
                });
            }
            else
            {
                debugForm.AppendDebugText(message);
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
            List<InputDevice> list = InputDevice.GetAll().ToList();
            if (list.Count > 0)
            {
                Keyboardind.Image = Resources.good;
                InputDevice inputDevice = list[0];
                inputDevice.EventReceived += MidiIn_MessageReceived;
                inputDevice.StartEventsListening();
            }
            else
            {
                Keyboardind.Image = Resources.off;
            }
        }


        private async void MidiIn_MessageReceived(object sender, MidiEventReceivedEventArgs e)
        {
            MidiEvent @event = e.Event;
            if (!(@event is NoteEvent noteEvent))
            {
                return;
            }
            int midiKey = (byte)noteEvent.NoteNumber;
            if (!MidiKeyMap.ContainsKey(midiKey))
            {
                return;
            }
            List<VirtualKeyCode> keyCombination = MidiKeyMap.MidiToKey[midiKey];
            if (noteEvent is NoteOnEvent noteOnEvent)
            {
                if ((byte)noteOnEvent.Velocity > 0)
                {
                    if (!activeNotes.ContainsKey(midiKey))
                    {
                        activeNotes[midiKey] = keyCombination;
                        await PressKeyAsync(keyCombination);
                    }
                }
                else if (activeNotes.ContainsKey(midiKey))
                {
                    await ReleaseKeyAsync(keyCombination);
                    activeNotes.Remove(midiKey);
                }
            }
            else if (noteEvent is NoteOffEvent && activeNotes.ContainsKey(midiKey))
            {
                await ReleaseKeyAsync(keyCombination);
                activeNotes.Remove(midiKey);
            }
        }


        private Dictionary<int, List<VirtualKeyCode>> activeNotes = new Dictionary<int, List<VirtualKeyCode>>();

       




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
            midiFilePaths.Clear();
            filteredMidiFilePaths.Clear();
            midiLibrary.Items.Clear();
            string[] array = files;
            foreach (string item in array)
            {
                midiFilePaths.Add(item);
                filteredMidiFilePaths.Add(item);
            }
            UpdateMidiLibraryListBox();
        }

        private void SelectLib_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    LoadMidiFilesFromPath(selectedPath);
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
            if (midiLibrary.SelectedIndex < 0)
            {
                return;
            }
            List<string> list = (string.IsNullOrEmpty(searchBox.Text) ? midiFilePaths : filteredMidiFilePaths);
            string fileName = Path.GetFileName(list[midiLibrary.SelectedIndex]);
            currentSong.Text = fileName;
            StartScrollingText(fileName);
            LoadMidiFile(list[midiLibrary.SelectedIndex]);
            if (currentMidiFile != null)
            {
                TempoMap tempoMap = currentMidiFile.GetTempoMap();
                long timeSpan = (from trackChunk in currentMidiFile.GetTrackChunks()
                                 select trackChunk.GetDuration<MidiTimeSpan>(tempoMap)).Max();
                MetricTimeSpan metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>((ITimeSpan)new MidiTimeSpan(timeSpan), tempoMap);
                songDuration = TimeSpan.FromSeconds(metricTimeSpan.TotalSeconds);
                songTime.Text = "00:00";
                songTotal.Text = songDuration.ToString("mm\\:ss");
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
                currentMidiFile = MidiFile.Read(filePath);
                totalTracks = currentMidiFile.GetTrackChunks().Count();
                currentTrackIndex = 0;
                LoadTrackNames();
                UpdateTrackInfo();
                songDuration = GetMidiFileDuration(currentMidiFile);
                songTime.Text = "00:00";
                songTotal.Text = songDuration.ToString("mm\\:ss") ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading MIDI file: " + ex.Message);
            }
        }



        private void LoadTrackNames()
        {
            trackNames.Clear();
            foreach (TrackChunk trackChunk in currentMidiFile.GetTrackChunks())
            {
                string item = "Unnamed Track";
                SequenceTrackNameEvent sequenceTrackNameEvent = trackChunk.Events.OfType<SequenceTrackNameEvent>().FirstOrDefault();
                if (sequenceTrackNameEvent != null)
                {
                    item = sequenceTrackNameEvent.Text;
                }
                trackNames.Add(item);
            }
        }









        private void UpdateTrackInfo()
        {
            if (currentMidiFile != null && trackNames.Count > currentTrackIndex)
            {
                string arg = trackNames[currentTrackIndex];
                trackInfo.Text = $"Track {currentTrackIndex + 1}/{totalTracks} {arg}";
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
                loopind.Image = Resources.good;
            }
            else
            {
                loop.Tag = "Off";
                loopind.Image = Resources.off;
            }
        }


        private void StartScrollingText(string text)
        {
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


        private async Task HandleKeyPressAsync(List<VirtualKeyCode> mainKeys, List<VirtualKeyCode> modifiers, bool shouldHoldModifier, List<VirtualKeyCode> currentModifiers, int modHold)
        {
            foreach (VirtualKeyCode currentModifier in currentModifiers.Except(modifiers))
            {
                if (keyStates.ContainsKey(currentModifier) && keyStates[currentModifier])
                {
                    inputSimulator.Keyboard.KeyUp(currentModifier);
                    keyStates[currentModifier] = false;
                    virtualKeys.ResetKey(currentModifier);
                }
            }
            foreach (VirtualKeyCode modifier in modifiers.Except(currentModifiers))
            {
                if (!keyStates.ContainsKey(modifier) || !keyStates[modifier])
                {
                    inputSimulator.Keyboard.KeyDown(modifier);
                    keyStates[modifier] = true;
                    virtualKeys.HighlightKey(modifier);
                    await Task.Delay(modHold);
                }
            }
            foreach (VirtualKeyCode key in mainKeys)
            {
                if (!keyStates.ContainsKey(key) || !keyStates[key])
                {
                    inputSimulator.Keyboard.KeyDown(key);
                    keyStates[key] = true;
                    virtualKeys.HighlightKey(key);
                    await Task.Delay(10);
                }
            }
            foreach (VirtualKeyCode key in mainKeys)
            {
                if (keyStates.ContainsKey(key) && keyStates[key])
                {
                    await Task.Delay(50);
                    inputSimulator.Keyboard.KeyUp(key);
                    keyStates[key] = false;
                    virtualKeys.ResetKey(key);
                }
            }
            if (!shouldHoldModifier)
            {
                foreach (VirtualKeyCode modifier in modifiers)
                {
                    if (keyStates.ContainsKey(modifier) && keyStates[modifier])
                    {
                        inputSimulator.Keyboard.KeyUp(modifier);
                        keyStates[modifier] = false;
                        virtualKeys.ResetKey(modifier);
                    }
                }
            }
            currentModifiers.Clear();
            if (shouldHoldModifier)
            {
                currentModifiers.AddRange(modifiers);
            }
        }

        private int CalculateAdjustedTempo(int initialTempo)
        {
            double num = (double)speedSlider.Value / 100.0 * 1.4;
            return (int)((double)initialTempo / num);
        }

        private async Task PlayFromQueue(List<(MidiEvent midiEvent, long absoluteTime, int tempo, int trackIndex)> playbackQueue, CancellationToken token)
        {
            try
            {
                try
                {
                    List<VirtualKeyCode> currentModifiers = new List<VirtualKeyCode>();
                    int modHold = int.Parse(Settings.Default["modHold"].ToString());
                    int minNoteDelay = int.Parse(Settings.Default["minNoteDelay"].ToString());
                    int ticksPerQuarterNote = ((TicksPerQuarterNoteTimeDivision)currentMidiFile.TimeDivision).TicksPerQuarterNote;
                    int i = 0;
                    while (true)
                    {
                        if (i < playbackQueue.Count)
                        {
                            (MidiEvent midiEvent, long absoluteTime, int tempo, int trackIndex) currentEvent = playbackQueue[i];
                            var (midiEvent, absoluteTime, _, _) = currentEvent;
                            if (token.IsCancellationRequested)
                            {
                                return;
                            }
                            int adjustedTempo = CalculateAdjustedTempo(currentEvent.tempo);
                            List<VirtualKeyCode> modifiers;
                            List<(MidiEvent midiEvent, List<VirtualKeyCode> mainKeys, List<VirtualKeyCode> modifiers)> simultaneousNotes = new List<(MidiEvent, List<VirtualKeyCode>, List<VirtualKeyCode>)> { (midiEvent, GetKeysFromMidiEvent(midiEvent, out modifiers), modifiers) };
                            while (i + 1 < playbackQueue.Count && playbackQueue[i + 1].absoluteTime == absoluteTime)
                            {
                                i++;
                                (MidiEvent midiEvent, long absoluteTime, int tempo, int trackIndex) nextEvent = playbackQueue[i];
                                simultaneousNotes.Add((nextEvent.midiEvent, GetKeysFromMidiEvent(nextEvent.midiEvent, out modifiers), modifiers));
                            }
                            foreach (IGrouping<string, (MidiEvent, List<VirtualKeyCode>, List<VirtualKeyCode>)> group in from note in simultaneousNotes
                                                                                                                         group note by string.Join(",", note.modifiers.Select((VirtualKeyCode m) => m.ToString())))
                            {
                                List<VirtualKeyCode> combinedMainKeys = group.SelectMany<(MidiEvent, List<VirtualKeyCode>, List<VirtualKeyCode>), VirtualKeyCode>(((MidiEvent midiEvent, List<VirtualKeyCode> mainKeys, List<VirtualKeyCode> modifiers) note) => note.mainKeys).Distinct().ToList();
                                List<VirtualKeyCode> combinedModifiers = group.SelectMany<(MidiEvent, List<VirtualKeyCode>, List<VirtualKeyCode>), VirtualKeyCode>(((MidiEvent midiEvent, List<VirtualKeyCode> mainKeys, List<VirtualKeyCode> modifiers) note) => note.modifiers).Distinct().ToList();
                                bool shouldHoldModifier = ShouldHoldModifier(combinedModifiers, playbackQueue, i);
                                await HandleKeyPressAsync(combinedMainKeys, combinedModifiers, shouldHoldModifier, currentModifiers, modHold);
                            }
                            if (i + 1 < playbackQueue.Count)
                            {
                                long nextNoteTime = playbackQueue[i + 1].absoluteTime;
                                int delay = CalculateDelay(nextNoteTime - absoluteTime, adjustedTempo, ticksPerQuarterNote);
                                delay = Math.Max(delay, minNoteDelay);
                                if (delay > 0)
                                {
                                    await Task.Delay(delay);
                                }
                            }
                            modifiers = null;
                            i++;
                            continue;
                        }
                        if (loop.Tag as string == "On" && !token.IsCancellationRequested)
                        {
                            await PlayFromQueue(playbackQueue, token);
                        }
                        break;
                    }
                    goto end_IL_0033;
                }
                catch (Exception ex2)
                {
                    Exception ex = ex2;
                    MessageBox.Show("Playback error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    goto end_IL_0033;
                }
            end_IL_0033:;
            }
            finally
            {
                isPlaying = false;
                foreach (VirtualKeyCode key in (from k in keyStates
                                                where k.Value
                                                select k.Key).ToList())
                {
                    await ReleaseKeyAsync(new List<VirtualKeyCode> { key });
                }
            }
        }

        private List<VirtualKeyCode> GetKeysFromMidiEvent(MidiEvent midiEvent, out List<VirtualKeyCode> modifiers)
        {
            modifiers = new List<VirtualKeyCode>();
            List<VirtualKeyCode> result = new List<VirtualKeyCode>();
            if (midiEvent is NoteOnEvent noteOnEvent && MidiKeyMap.MidiToKey.TryGetValue((byte)noteOnEvent.NoteNumber, out var value))
            {
                modifiers = value.Where(IsModifierKey).ToList();
                result = value.Except(modifiers).ToList();
                if (ShiftButton.Tag as string == "Off")
                {
                    modifiers.RemoveAll((VirtualKeyCode m) => m == VirtualKeyCode.LSHIFT || m == VirtualKeyCode.RSHIFT);
                }
                if (ControlButton.Tag as string == "Off")
                {
                    modifiers.RemoveAll((VirtualKeyCode m) => m == VirtualKeyCode.LCONTROL || m == VirtualKeyCode.RCONTROL);
                }
            }
            return result;
        }

        private bool ShouldHoldModifier(List<VirtualKeyCode> modifiers, List<(MidiEvent midiEvent, long absoluteTime, int tempo, int trackIndex)> playbackQueue, int currentIndex)
        {
            for (int i = currentIndex + 1; i < playbackQueue.Count; i++)
            {
                if (playbackQueue[i].midiEvent is NoteOnEvent noteOnEvent && MidiKeyMap.MidiToKey.TryGetValue((byte)noteOnEvent.NoteNumber, out var value))
                {
                    List<VirtualKeyCode> first = value.Where(IsModifierKey).ToList();
                    if (first.Intersect(modifiers).Any())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void StartPlayback()
        {
            if (isPlaying)
            {
                MessageBox.Show("Playback is already in progress.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (currentMidiFile == null)
            {
                MessageBox.Show("Please select a MIDI file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (CurrentLayoutIndex == 0)
            {
                MidiKeyMap.SetLayout("QWERTY");
            }
            else if (CurrentLayoutIndex == 1)
            {
                MidiKeyMap.SetLayout("AZERTY");
            }
            List<(MidiEvent midiEvent, long absoluteTime, int tempo, int trackIndex)> playbackQueue = BuildPlaybackQueue();
            TempoMap tempoMap = currentMidiFile.GetTempoMap();
            cancellationTokenSource = new CancellationTokenSource();
            isPlaying = true;
            elapsedTime = TimeSpan.Zero;
            songTimer.Start();
            Task.Run(() => PlayFromQueue(playbackQueue, cancellationTokenSource.Token));
        }

        private void StopPlayback()
        {
            cancellationTokenSource?.Cancel();
            isPlaying = false;
            songTimer.Stop();
            songTime.Text = "0:00";
            ReleaseModifiers(new List<VirtualKeyCode>());
            GC.Collect();
            GC.WaitForPendingFinalizers();
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






        private int CalculateDelay(long absoluteTimeDelta, int tempo, int ticksPerQuarterNote)
        {
            int num = (int)((double)absoluteTimeDelta * ((double)tempo / 1000.0) / (double)ticksPerQuarterNote);
            double num2 = (double)speedSlider.Value / 100.0;
            num = (int)((double)num / num2);
            return Math.Max(num, 10);
        }


        public void UpdateAlwaysOnTop(bool alwaysOnTop)
        {
            this.TopMost = alwaysOnTop;
        }







        private int GetInitialTempo(MidiFile midiFile)
        {
            foreach (TrackChunk trackChunk in midiFile.GetTrackChunks())
            {
                SetTempoEvent setTempoEvent = trackChunk.Events.OfType<SetTempoEvent>().FirstOrDefault();
                if (setTempoEvent != null)
                {
                    int num = (int)setTempoEvent.MicrosecondsPerQuarterNote;
                    LogDebug($"Initial Tempo: {num} microseconds per quarter note");
                    return num;
                }
            }
            return 500000;
        }



        private TimeSpan GetMidiFileDuration(MidiFile midiFile)
        {
            TempoMap tempoMap = midiFile.GetTempoMap();
            MidiTimeSpan time = (from trackChunk in midiFile.GetTrackChunks()
                                 select trackChunk.GetDuration<MidiTimeSpan>(tempoMap)).Max();
            MetricTimeSpan metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>((ITimeSpan)time, tempoMap);
            return TimeSpan.FromSeconds(metricTimeSpan.TotalSeconds);
        }

        private List<(MidiEvent midiEvent, long absoluteTime, int tempo, int trackIndex)> CollectMidiEventsWithTrack(MidiFile midiFile, int ticksPerQuarterNote, int initialTempo)
        {
            List<(MidiEvent, long, int, int)> list = new List<(MidiEvent, long, int, int)>();
            long[] array = new long[midiFile.GetTrackChunks().Count()];
            int item = initialTempo;
            List<TrackChunk> list2 = midiFile.GetTrackChunks().ToList();
            for (int i = 0; i < list2.Count; i++)
            {
                foreach (TimedEvent timedEvent in list2[i].GetTimedEvents())
                {
                    MidiEvent @event = timedEvent.Event;
                    if (@event is SetTempoEvent setTempoEvent)
                    {
                        item = (int)setTempoEvent.MicrosecondsPerQuarterNote;
                    }
                    list.Add((@event, timedEvent.Time, item, i));
                }
            }
            list.Sort(((MidiEvent, long, int, int) x, (MidiEvent, long, int, int) y) => x.Item2.CompareTo(y.Item2));
            return list;
        }











        private async Task PressKeyAsync(List<VirtualKeyCode> keys)
        {
            List<VirtualKeyCode> modifiers = keys.Where((VirtualKeyCode k) => IsModifierKey(k)).ToList();
            List<VirtualKeyCode> mainKeys = keys.Except(modifiers).ToList();
            foreach (VirtualKeyCode modifier in modifiers)
            {
                if (!keyStates.ContainsKey(modifier) || !keyStates[modifier])
                {
                    inputSimulator.Keyboard.KeyDown(modifier);
                    keyStates[modifier] = true;
                    virtualKeys.HighlightKey(modifier);
                    await Task.Delay(10);
                }
            }
            foreach (VirtualKeyCode key in mainKeys)
            {
                if (!keyStates.ContainsKey(key) || !keyStates[key])
                {
                    inputSimulator.Keyboard.KeyDown(key);
                    keyStates[key] = true;
                    virtualKeys.HighlightKey(key);
                    await Task.Delay(10);
                }
            }
        }



        private async Task ReleaseKeyAsync(List<VirtualKeyCode> keys)
        {
            List<VirtualKeyCode> modifiers = keys.Where((VirtualKeyCode k) => IsModifierKey(k)).ToList();
            List<VirtualKeyCode> mainKeys = keys.Except(modifiers).ToList();
            foreach (VirtualKeyCode key in mainKeys)
            {
                if (keyStates.ContainsKey(key) && keyStates[key])
                {
                    inputSimulator.Keyboard.KeyUp(key);
                    keyStates[key] = false;
                    virtualKeys.ResetKey(key);
                    await Task.Delay(10);
                }
            }
            foreach (VirtualKeyCode modifier in modifiers)
            {
                if (keyStates.ContainsKey(modifier) && keyStates[modifier])
                {
                    inputSimulator.Keyboard.KeyUp(modifier);
                    keyStates[modifier] = false;
                    virtualKeys.ResetKey(modifier);
                    await Task.Delay(10);
                    activeNotes.Clear();
                    GC.Collect();
                }
            }
        }


        private bool IsModifierKey(VirtualKeyCode key)
        {
            return key == VirtualKeyCode.LCONTROL || key == VirtualKeyCode.RCONTROL || key == VirtualKeyCode.LSHIFT || key == VirtualKeyCode.RSHIFT || key == VirtualKeyCode.LMENU || key == VirtualKeyCode.RMENU;
        }


















        private void ReleaseModifiers(List<VirtualKeyCode> keys)
        {
            foreach (VirtualKeyCode key in keys)
            {
                if ((key == VirtualKeyCode.LCONTROL || key == VirtualKeyCode.RCONTROL || key == VirtualKeyCode.LSHIFT || key == VirtualKeyCode.RSHIFT) && keyStates.ContainsKey(key) && keyStates[key])
                {
                    Thread.Sleep(0);
                    inputSimulator.Keyboard.KeyUp(key);
                    keyStates[key] = false;
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



        private void playall_Click(object sender, EventArgs e)
        {
            if (trackind.Tag as string == "good")
            {
                trackind.Image = Resources.off;
                trackind.Tag = "off";
            }
            else
            {
                trackind.Image = Resources.good;
                trackind.Tag = "good";
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
                    ApplySettings();
                }
            }
        }

        private void ApplySettings()
        {
            base.TopMost = Settings.Default.AlwaysOnTop;
            CurrentLayoutIndex = Settings.Default.CurrentLayoutIndex;
            UpdateKeyMapping();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
        }
    }
}