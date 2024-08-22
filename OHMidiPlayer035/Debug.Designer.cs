namespace OHMidiPlayer035
{
    partial class DebugForm
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
            this.debugWin = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // debugWin
            // 
            this.debugWin.Location = new System.Drawing.Point(12, 12);
            this.debugWin.Multiline = true;
            this.debugWin.Name = "debugWin";
            this.debugWin.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugWin.Size = new System.Drawing.Size(629, 295);
            this.debugWin.TabIndex = 0;
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 319);
            this.Controls.Add(this.debugWin);
            this.MaximizeBox = false;
            this.Name = "DebugForm";
            this.Text = "Debug";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.TextBox debugWin;
    }
}