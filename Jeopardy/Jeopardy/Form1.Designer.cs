namespace Jeopardy
{
    partial class StartForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            numPlayers = new NumericUpDown();
            panelPlayerNames = new Panel();
            btnStart = new Button();
            btnRefreshPlayers = new Button();
            ((System.ComponentModel.ISupportInitialize)numPlayers).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(329, 88);
            label1.Name = "label1";
            label1.Size = new Size(135, 20);
            label1.TabIndex = 0;
            label1.Text = "Number of players:";
            label1.Click += label1_Click;
            // 
            // numPlayers
            // 
            numPlayers.Location = new Point(320, 127);
            numPlayers.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            numPlayers.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            numPlayers.Name = "numPlayers";
            numPlayers.Size = new Size(150, 27);
            numPlayers.TabIndex = 1;
            numPlayers.Value = new decimal(new int[] { 2, 0, 0, 0 });
            numPlayers.ValueChanged += numPlayers_ValueChanged;
            // 
            // panelPlayerNames
            // 
            panelPlayerNames.Location = new Point(271, 160);
            panelPlayerNames.Name = "panelPlayerNames";
            panelPlayerNames.Size = new Size(250, 197);
            panelPlayerNames.TabIndex = 2;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(351, 363);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(94, 29);
            btnStart.TabIndex = 3;
            btnStart.Text = "Start Game";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnRefreshPlayers
            // 
            btnRefreshPlayers.Location = new Point(351, 398);
            btnRefreshPlayers.Name = "btnRefreshPlayers";
            btnRefreshPlayers.Size = new Size(94, 29);
            btnRefreshPlayers.TabIndex = 4;
            btnRefreshPlayers.Text = "Refresh";
            btnRefreshPlayers.UseVisualStyleBackColor = true;
            // 
            // StartForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnRefreshPlayers);
            Controls.Add(btnStart);
            Controls.Add(panelPlayerNames);
            Controls.Add(numPlayers);
            Controls.Add(label1);
            Name = "StartForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)numPlayers).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private NumericUpDown numPlayers;
        private Panel panelPlayerNames;
        private Button btnStart;
        private Button btnRefreshPlayers;
    }
}
