namespace OHMidiPlayer035
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.trackind = new System.Windows.Forms.PictureBox();
            this.Keyboardind = new System.Windows.Forms.PictureBox();
            this.OHind = new System.Windows.Forms.PictureBox();
            this.U = new System.Windows.Forms.Button();
            this.Y = new System.Windows.Forms.Button();
            this.T = new System.Windows.Forms.Button();
            this.R = new System.Windows.Forms.Button();
            this.E = new System.Windows.Forms.Button();
            this.W = new System.Windows.Forms.Button();
            this.Q = new System.Windows.Forms.Button();
            this.TWO = new System.Windows.Forms.Button();
            this.THREE = new System.Windows.Forms.Button();
            this.FIVE = new System.Windows.Forms.Button();
            this.SIX = new System.Windows.Forms.Button();
            this.SEVEN = new System.Windows.Forms.Button();
            this.ShiftButton = new System.Windows.Forms.Button();
            this.ControlButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.ignorekeys = new System.Windows.Forms.Button();
            this.loop = new System.Windows.Forms.Button();
            this.playall = new System.Windows.Forms.Button();
            this.nextTrack = new System.Windows.Forms.Button();
            this.prevTrack = new System.Windows.Forms.Button();
            this.midiLibrary = new System.Windows.Forms.ListBox();
            this.Info = new System.Windows.Forms.GroupBox();
            this.songTime = new System.Windows.Forms.Label();
            this.songTotal = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.speedLabel = new System.Windows.Forms.Label();
            this.trackInfo = new System.Windows.Forms.Label();
            this.currentSong = new System.Windows.Forms.Label();
            this.selectLib = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.loopind = new System.Windows.Forms.PictureBox();
            this.speedSlider = new System.Windows.Forms.HScrollBar();
            this.Discord = new System.Windows.Forms.Button();
            this.Coffee = new System.Windows.Forms.Button();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.refreshLib = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.trackind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Keyboardind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OHind)).BeginInit();
            this.Info.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loopind)).BeginInit();
            this.SuspendLayout();
            // 
            // trackind
            // 
            this.trackind.BackColor = System.Drawing.Color.Transparent;
            this.trackind.Image = global::OHMidiPlayer035.Properties.Resources.good;
            this.trackind.Location = new System.Drawing.Point(833, 176);
            this.trackind.Name = "trackind";
            this.trackind.Size = new System.Drawing.Size(19, 21);
            this.trackind.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.trackind.TabIndex = 17;
            this.trackind.TabStop = false;
            // 
            // Keyboardind
            // 
            this.Keyboardind.BackColor = System.Drawing.Color.Transparent;
            this.Keyboardind.Image = global::OHMidiPlayer035.Properties.Resources.off;
            this.Keyboardind.Location = new System.Drawing.Point(833, 116);
            this.Keyboardind.Name = "Keyboardind";
            this.Keyboardind.Size = new System.Drawing.Size(19, 21);
            this.Keyboardind.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Keyboardind.TabIndex = 18;
            this.Keyboardind.TabStop = false;
            // 
            // OHind
            // 
            this.OHind.BackColor = System.Drawing.Color.Transparent;
            this.OHind.Image = global::OHMidiPlayer035.Properties.Resources.error;
            this.OHind.Location = new System.Drawing.Point(833, 51);
            this.OHind.Name = "OHind";
            this.OHind.Size = new System.Drawing.Size(19, 21);
            this.OHind.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.OHind.TabIndex = 19;
            this.OHind.TabStop = false;
            // 
            // U
            // 
            this.U.Location = new System.Drawing.Point(833, 290);
            this.U.Name = "U";
            this.U.Size = new System.Drawing.Size(69, 258);
            this.U.TabIndex = 26;
            this.U.Text = "U";
            this.U.UseVisualStyleBackColor = true;
            // 
            // Y
            // 
            this.Y.Location = new System.Drawing.Point(758, 290);
            this.Y.Name = "Y";
            this.Y.Size = new System.Drawing.Size(69, 258);
            this.Y.TabIndex = 27;
            this.Y.Text = "Y";
            this.Y.UseVisualStyleBackColor = true;
            // 
            // T
            // 
            this.T.Location = new System.Drawing.Point(683, 290);
            this.T.Name = "T";
            this.T.Size = new System.Drawing.Size(69, 258);
            this.T.TabIndex = 28;
            this.T.Text = "T";
            this.T.UseVisualStyleBackColor = true;
            // 
            // R
            // 
            this.R.Location = new System.Drawing.Point(608, 290);
            this.R.Name = "R";
            this.R.Size = new System.Drawing.Size(69, 258);
            this.R.TabIndex = 29;
            this.R.Text = "R";
            this.R.UseVisualStyleBackColor = true;
            // 
            // E
            // 
            this.E.Location = new System.Drawing.Point(533, 290);
            this.E.Name = "E";
            this.E.Size = new System.Drawing.Size(69, 258);
            this.E.TabIndex = 30;
            this.E.Text = "E";
            this.E.UseVisualStyleBackColor = true;
            // 
            // W
            // 
            this.W.Location = new System.Drawing.Point(458, 290);
            this.W.Name = "W";
            this.W.Size = new System.Drawing.Size(69, 258);
            this.W.TabIndex = 31;
            this.W.Text = "W";
            this.W.UseVisualStyleBackColor = true;
            // 
            // Q
            // 
            this.Q.Location = new System.Drawing.Point(383, 290);
            this.Q.Name = "Q";
            this.Q.Size = new System.Drawing.Size(69, 258);
            this.Q.TabIndex = 32;
            this.Q.Text = "Q";
            this.Q.UseVisualStyleBackColor = true;
            // 
            // TWO
            // 
            this.TWO.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TWO.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TWO.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TWO.Location = new System.Drawing.Point(434, 290);
            this.TWO.Name = "TWO";
            this.TWO.Size = new System.Drawing.Size(42, 119);
            this.TWO.TabIndex = 33;
            this.TWO.Text = "2";
            this.TWO.UseVisualStyleBackColor = false;
            // 
            // THREE
            // 
            this.THREE.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.THREE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.THREE.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.THREE.Location = new System.Drawing.Point(508, 290);
            this.THREE.Name = "THREE";
            this.THREE.Size = new System.Drawing.Size(42, 119);
            this.THREE.TabIndex = 34;
            this.THREE.Text = "3";
            this.THREE.UseVisualStyleBackColor = false;
            // 
            // FIVE
            // 
            this.FIVE.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FIVE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FIVE.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FIVE.Location = new System.Drawing.Point(660, 290);
            this.FIVE.Name = "FIVE";
            this.FIVE.Size = new System.Drawing.Size(42, 119);
            this.FIVE.TabIndex = 35;
            this.FIVE.Text = "5";
            this.FIVE.UseVisualStyleBackColor = false;
            // 
            // SIX
            // 
            this.SIX.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SIX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SIX.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.SIX.Location = new System.Drawing.Point(735, 290);
            this.SIX.Name = "SIX";
            this.SIX.Size = new System.Drawing.Size(42, 119);
            this.SIX.TabIndex = 36;
            this.SIX.Text = "6";
            this.SIX.UseVisualStyleBackColor = false;
            // 
            // SEVEN
            // 
            this.SEVEN.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SEVEN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SEVEN.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.SEVEN.Location = new System.Drawing.Point(810, 290);
            this.SEVEN.Name = "SEVEN";
            this.SEVEN.Size = new System.Drawing.Size(42, 119);
            this.SEVEN.TabIndex = 37;
            this.SEVEN.Text = "7";
            this.SEVEN.UseVisualStyleBackColor = false;
            // 
            // ShiftButton
            // 
            this.ShiftButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ShiftButton.Location = new System.Drawing.Point(242, 290);
            this.ShiftButton.Name = "ShiftButton";
            this.ShiftButton.Size = new System.Drawing.Size(135, 126);
            this.ShiftButton.TabIndex = 38;
            this.ShiftButton.Text = "Shift";
            this.ShiftButton.UseVisualStyleBackColor = true;
            // 
            // ControlButton
            // 
            this.ControlButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ControlButton.Location = new System.Drawing.Point(242, 422);
            this.ControlButton.Name = "ControlButton";
            this.ControlButton.Size = new System.Drawing.Size(135, 126);
            this.ControlButton.TabIndex = 39;
            this.ControlButton.Text = "CTRL";
            this.ControlButton.UseVisualStyleBackColor = true;
            // 
            // settingsButton
            // 
            this.settingsButton.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.settingsButton.BackgroundImage = global::OHMidiPlayer035.Properties.Resources.settings;
            this.settingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.settingsButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info;
            this.settingsButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.settingsButton.Font = new System.Drawing.Font("Century", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.settingsButton.Location = new System.Drawing.Point(634, 210);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(108, 25);
            this.settingsButton.TabIndex = 40;
            this.settingsButton.UseVisualStyleBackColor = false;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // ignorekeys
            // 
            this.ignorekeys.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ignorekeys.BackgroundImage = global::OHMidiPlayer035.Properties.Resources.Ignore;
            this.ignorekeys.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ignorekeys.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info;
            this.ignorekeys.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ignorekeys.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ignorekeys.Font = new System.Drawing.Font("Century", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ignorekeys.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.ignorekeys.Location = new System.Drawing.Point(634, 142);
            this.ignorekeys.Name = "ignorekeys";
            this.ignorekeys.Size = new System.Drawing.Size(108, 25);
            this.ignorekeys.TabIndex = 41;
            this.ignorekeys.UseVisualStyleBackColor = false;
            this.ignorekeys.Click += new System.EventHandler(this.ignorekeys_Click);
            // 
            // loop
            // 
            this.loop.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.loop.BackgroundImage = global::OHMidiPlayer035.Properties.Resources.loop;
            this.loop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.loop.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info;
            this.loop.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.loop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.loop.Font = new System.Drawing.Font("Century", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loop.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.loop.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.loop.Location = new System.Drawing.Point(634, 77);
            this.loop.Name = "loop";
            this.loop.Size = new System.Drawing.Size(108, 25);
            this.loop.TabIndex = 42;
            this.loop.UseVisualStyleBackColor = false;
            this.loop.Click += new System.EventHandler(this.loop_Click);
            // 
            // playall
            // 
            this.playall.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.playall.BackgroundImage = global::OHMidiPlayer035.Properties.Resources.PlayAll;
            this.playall.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.playall.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info;
            this.playall.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.playall.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.playall.Font = new System.Drawing.Font("Century", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playall.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.playall.Location = new System.Drawing.Point(785, 210);
            this.playall.Name = "playall";
            this.playall.Size = new System.Drawing.Size(117, 25);
            this.playall.TabIndex = 43;
            this.playall.UseVisualStyleBackColor = false;
            this.playall.Click += new System.EventHandler(this.playall_Click);
            // 
            // nextTrack
            // 
            this.nextTrack.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.nextTrack.BackgroundImage = global::OHMidiPlayer035.Properties.Resources.next;
            this.nextTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.nextTrack.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info;
            this.nextTrack.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.nextTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextTrack.Font = new System.Drawing.Font("Century", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextTrack.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.nextTrack.Location = new System.Drawing.Point(274, 119);
            this.nextTrack.Name = "nextTrack";
            this.nextTrack.Size = new System.Drawing.Size(35, 35);
            this.nextTrack.TabIndex = 44;
            this.nextTrack.UseVisualStyleBackColor = false;
            // 
            // prevTrack
            // 
            this.prevTrack.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.prevTrack.BackgroundImage = global::OHMidiPlayer035.Properties.Resources.prev;
            this.prevTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.prevTrack.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info;
            this.prevTrack.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.prevTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prevTrack.Font = new System.Drawing.Font("Century", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevTrack.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.prevTrack.Location = new System.Drawing.Point(0, 119);
            this.prevTrack.Name = "prevTrack";
            this.prevTrack.Size = new System.Drawing.Size(35, 35);
            this.prevTrack.TabIndex = 45;
            this.prevTrack.UseVisualStyleBackColor = false;
            // 
            // midiLibrary
            // 
            this.midiLibrary.BackColor = System.Drawing.Color.SteelBlue;
            this.midiLibrary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.midiLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.midiLibrary.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.midiLibrary.FormattingEnabled = true;
            this.midiLibrary.ItemHeight = 20;
            this.midiLibrary.Location = new System.Drawing.Point(23, 51);
            this.midiLibrary.Name = "midiLibrary";
            this.midiLibrary.Size = new System.Drawing.Size(263, 202);
            this.midiLibrary.TabIndex = 46;
            // 
            // Info
            // 
            this.Info.BackColor = System.Drawing.Color.Transparent;
            this.Info.Controls.Add(this.songTime);
            this.Info.Controls.Add(this.songTotal);
            this.Info.Controls.Add(this.nextTrack);
            this.Info.Controls.Add(this.label2);
            this.Info.Controls.Add(this.label1);
            this.Info.Controls.Add(this.speedLabel);
            this.Info.Controls.Add(this.prevTrack);
            this.Info.Controls.Add(this.trackInfo);
            this.Info.Controls.Add(this.currentSong);
            this.Info.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Info.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Info.Location = new System.Drawing.Point(302, 43);
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(309, 208);
            this.Info.TabIndex = 47;
            this.Info.TabStop = false;
            // 
            // songTime
            // 
            this.songTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.songTime.Location = new System.Drawing.Point(203, 6);
            this.songTime.Name = "songTime";
            this.songTime.Size = new System.Drawing.Size(45, 20);
            this.songTime.TabIndex = 4;
            this.songTime.Text = "0:00";
            this.songTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // songTotal
            // 
            this.songTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.songTotal.Location = new System.Drawing.Point(254, 6);
            this.songTotal.Name = "songTotal";
            this.songTotal.Size = new System.Drawing.Size(45, 20);
            this.songTotal.TabIndex = 54;
            this.songTotal.Text = "0:00";
            this.songTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(207, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "/";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(59, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "F5 To Start Playback F6 to Stop Playback";
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.speedLabel.Location = new System.Drawing.Point(96, 8);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(105, 20);
            this.speedLabel.TabIndex = 2;
            this.speedLabel.Text = "Speed: 100%";
            // 
            // trackInfo
            // 
            this.trackInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackInfo.Location = new System.Drawing.Point(0, 92);
            this.trackInfo.Name = "trackInfo";
            this.trackInfo.Size = new System.Drawing.Size(309, 24);
            this.trackInfo.TabIndex = 1;
            this.trackInfo.Text = "Track 0/0";
            // 
            // currentSong
            // 
            this.currentSong.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentSong.Location = new System.Drawing.Point(-1, 61);
            this.currentSong.Name = "currentSong";
            this.currentSong.Size = new System.Drawing.Size(255, 31);
            this.currentSong.TabIndex = 0;
            this.currentSong.Text = "No Midi Selected";
            // 
            // selectLib
            // 
            this.selectLib.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.selectLib.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info;
            this.selectLib.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.selectLib.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.selectLib.Font = new System.Drawing.Font("Century", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectLib.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.selectLib.Location = new System.Drawing.Point(150, 20);
            this.selectLib.Name = "selectLib";
            this.selectLib.Size = new System.Drawing.Size(108, 25);
            this.selectLib.TabIndex = 48;
            this.selectLib.Text = "Select Library";
            this.selectLib.UseVisualStyleBackColor = false;
            // 
            // loopind
            // 
            this.loopind.BackColor = System.Drawing.Color.Transparent;
            this.loopind.Image = global::OHMidiPlayer035.Properties.Resources.off;
            this.loopind.Location = new System.Drawing.Point(678, 51);
            this.loopind.Name = "loopind";
            this.loopind.Size = new System.Drawing.Size(19, 21);
            this.loopind.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.loopind.TabIndex = 49;
            this.loopind.TabStop = false;
            // 
            // speedSlider
            // 
            this.speedSlider.LargeChange = 1;
            this.speedSlider.Location = new System.Drawing.Point(387, 34);
            this.speedSlider.Maximum = 150;
            this.speedSlider.Minimum = 50;
            this.speedSlider.Name = "speedSlider";
            this.speedSlider.Size = new System.Drawing.Size(124, 17);
            this.speedSlider.TabIndex = 50;
            this.speedSlider.Value = 100;
            // 
            // Discord
            // 
            this.Discord.BackColor = System.Drawing.Color.Transparent;
            this.Discord.BackgroundImage = global::OHMidiPlayer035.Properties.Resources.Discord;
            this.Discord.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Discord.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Discord.Location = new System.Drawing.Point(33, 361);
            this.Discord.Name = "Discord";
            this.Discord.Size = new System.Drawing.Size(169, 48);
            this.Discord.TabIndex = 51;
            this.Discord.UseVisualStyleBackColor = false;
            this.Discord.Click += new System.EventHandler(this.Discord_Click_1);
            // 
            // Coffee
            // 
            this.Coffee.BackColor = System.Drawing.Color.Transparent;
            this.Coffee.BackgroundImage = global::OHMidiPlayer035.Properties.Resources.Coffee;
            this.Coffee.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Coffee.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Coffee.Location = new System.Drawing.Point(33, 422);
            this.Coffee.Name = "Coffee";
            this.Coffee.Size = new System.Drawing.Size(169, 48);
            this.Coffee.TabIndex = 52;
            this.Coffee.UseVisualStyleBackColor = false;
            this.Coffee.Click += new System.EventHandler(this.Coffee_Click);
            // 
            // searchBox
            // 
            this.searchBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.searchBox.Location = new System.Drawing.Point(23, 23);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(121, 20);
            this.searchBox.TabIndex = 53;
            // 
            // refreshLib
            // 
            this.refreshLib.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.refreshLib.BackgroundImage = global::OHMidiPlayer035.Properties.Resources.refresh;
            this.refreshLib.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.refreshLib.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info;
            this.refreshLib.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.refreshLib.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.refreshLib.Font = new System.Drawing.Font("Century", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshLib.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.refreshLib.Location = new System.Drawing.Point(264, 20);
            this.refreshLib.Name = "refreshLib";
            this.refreshLib.Size = new System.Drawing.Size(22, 25);
            this.refreshLib.TabIndex = 54;
            this.refreshLib.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::OHMidiPlayer035.Properties.Resources.finalframe;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(925, 570);
            this.Controls.Add(this.refreshLib);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.Coffee);
            this.Controls.Add(this.Discord);
            this.Controls.Add(this.speedSlider);
            this.Controls.Add(this.loopind);
            this.Controls.Add(this.selectLib);
            this.Controls.Add(this.Info);
            this.Controls.Add(this.midiLibrary);
            this.Controls.Add(this.playall);
            this.Controls.Add(this.loop);
            this.Controls.Add(this.ignorekeys);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.ControlButton);
            this.Controls.Add(this.ShiftButton);
            this.Controls.Add(this.SEVEN);
            this.Controls.Add(this.SIX);
            this.Controls.Add(this.FIVE);
            this.Controls.Add(this.THREE);
            this.Controls.Add(this.TWO);
            this.Controls.Add(this.Q);
            this.Controls.Add(this.W);
            this.Controls.Add(this.E);
            this.Controls.Add(this.R);
            this.Controls.Add(this.T);
            this.Controls.Add(this.Y);
            this.Controls.Add(this.U);
            this.Controls.Add(this.OHind);
            this.Controls.Add(this.Keyboardind);
            this.Controls.Add(this.trackind);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "OH Midi Player";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Keyboardind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OHind)).EndInit();
            this.Info.ResumeLayout(false);
            this.Info.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loopind)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox trackind;
        private System.Windows.Forms.PictureBox Keyboardind;
        private System.Windows.Forms.PictureBox OHind;
        public System.Windows.Forms.Button U;
        public System.Windows.Forms.Button Y;
        public System.Windows.Forms.Button T;
        public System.Windows.Forms.Button R;
        public System.Windows.Forms.Button E;
        public System.Windows.Forms.Button W;
        public System.Windows.Forms.Button Q;
        public System.Windows.Forms.Button TWO;
        public System.Windows.Forms.Button THREE;
        public System.Windows.Forms.Button FIVE;
        public System.Windows.Forms.Button SIX;
        public System.Windows.Forms.Button SEVEN;
        public System.Windows.Forms.Button ShiftButton;
        public System.Windows.Forms.Button ControlButton;
        public System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Button ignorekeys;
        private System.Windows.Forms.Button loop;
        private System.Windows.Forms.Button playall;
        private System.Windows.Forms.Button nextTrack;
        private System.Windows.Forms.Button prevTrack;
        private System.Windows.Forms.ListBox midiLibrary;
        private System.Windows.Forms.GroupBox Info;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.Label trackInfo;
        private System.Windows.Forms.Label currentSong;
        private System.Windows.Forms.Button selectLib;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox loopind;
        private System.Windows.Forms.HScrollBar speedSlider;
        private System.Windows.Forms.Button Discord;
        private System.Windows.Forms.Button Coffee;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label songTime; // Label to display time
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Timer songTimer = new System.Windows.Forms.Timer(); // Timer for updating song time
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label songTotal;
        private System.Windows.Forms.Button refreshLib;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}

