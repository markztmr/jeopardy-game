using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Jeopardy
{
    public class GameModeSelectionForm : Form
    {
        private GameState gameState;
        private GameMode selectedMode = GameMode.Timer;

        public GameModeSelectionForm(GameState state)
        {
            gameState = state;
            InitializeComponent();
            BuildUI();
        }

        private void InitializeComponent()
        {
            this.Text = "Jeopardy! - Select Game Mode";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(25, 25, 50);
            this.ForeColor = Color.White;
            this.Font = new Font("Arial", 12);
        }

        private void BuildUI()
        {
            // Main layout table
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 4,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(25, 25, 50),
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));      // Title
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 180));     // Mode selection
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));     // Players
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));      // Bottom buttons
            this.Controls.Add(mainLayout);

            // Title
            Label lblTitle = new Label
            {
                Text = "SELECT GAME MODE",
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.Cyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            // Mode selection panel - 2 columns for buttons
            TableLayoutPanel modeLayout = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                Margin = new Padding(0),
                BackColor = Color.FromArgb(25, 25, 50)
            };
            modeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            modeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            modeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            modeLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));      // Timer duration
            mainLayout.Controls.Add(modeLayout, 0, 1);

            // BUZZER MODE Button
            Button btnBuzzer = new Button
            {
                Text = "🔔 BUZZER MODE\n\nPlayers buzz in to answer.\nFirst to buzz gets to answer.",
                Font = new Font("Arial", 13, FontStyle.Bold),
                BackColor = Color.FromArgb(200, 100, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.TopCenter,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Fill,
                Margin = new Padding(10)
            };
            btnBuzzer.FlatAppearance.BorderSize = 3;
            btnBuzzer.FlatAppearance.BorderColor = Color.Cyan;
            modeLayout.Controls.Add(btnBuzzer, 0, 0);

            // TIMER MODE Button
            Button btnTimer = new Button
            {
                Text = "⏱️ TIMER MODE\n\nEveryone answers within\nthe time limit (1-2 minutes).",
                Font = new Font("Arial", 13, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.TopCenter,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Fill,
                Margin = new Padding(10)
            };
            btnTimer.FlatAppearance.BorderSize = 3;
            btnTimer.FlatAppearance.BorderColor = Color.Lime;
            modeLayout.Controls.Add(btnTimer, 1, 0);

            // Setup click handlers for mode buttons
            btnBuzzer.Click += (s, e) =>
            {
                selectedMode = GameMode.Buzzer;
                btnBuzzer.FlatAppearance.BorderColor = Color.Lime;
                btnBuzzer.FlatAppearance.BorderSize = 4;
                btnTimer.FlatAppearance.BorderColor = Color.Cyan;
                btnTimer.FlatAppearance.BorderSize = 3;
            };

            btnTimer.Click += (s, e) =>
            {
                selectedMode = GameMode.Timer;
                btnTimer.FlatAppearance.BorderColor = Color.Lime;
                btnTimer.FlatAppearance.BorderSize = 4;
                btnBuzzer.FlatAppearance.BorderColor = Color.Cyan;
                btnBuzzer.FlatAppearance.BorderSize = 3;
            };

            // Timer Duration section
            Panel timerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 5, 20, 5),
                BackColor = Color.FromArgb(25, 25, 50)
            };
            modeLayout.Controls.Add(timerPanel, 0, 1);
            modeLayout.SetColumnSpan(timerPanel, 2);

            Label lblTimerDuration = new Label
            {
                Text = "Answer Time Limit:",
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.Cyan,
                AutoSize = true
            };
            timerPanel.Controls.Add(lblTimerDuration);

            Button btn60s = CreateTimerButton(60);
            Button btn90s = CreateTimerButton(90);
            Button btn120s = CreateTimerButton(120);
            btn60s.Location = new Point(200, 0);
            btn90s.Location = new Point(320, 0);
            btn120s.Location = new Point(440, 0);
            
            timerPanel.Controls.Add(btn60s);
            timerPanel.Controls.Add(btn90s);
            timerPanel.Controls.Add(btn120s);

            btn60s.Click += (s, e) =>
            {
                gameState.TimerDuration = 60;
                UpdateTimerButtons(btn60s, btn90s, btn120s);
            };

            btn90s.Click += (s, e) =>
            {
                gameState.TimerDuration = 90;
                UpdateTimerButtons(btn60s, btn90s, btn120s);
            };

            btn120s.Click += (s, e) =>
            {
                gameState.TimerDuration = 120;
                UpdateTimerButtons(btn60s, btn90s, btn120s);
            };

            btn60s.FlatAppearance.BorderColor = Color.Lime;
            btn60s.FlatAppearance.BorderSize = 3;

            // Players Section
            Panel playersPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(25, 25, 50)
            };
            mainLayout.Controls.Add(playersPanel, 0, 2);

            Label lblPlayers = new Label
            {
                Text = "Players:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Cyan,
                AutoSize = true
            };
            playersPanel.Controls.Add(lblPlayers);

            NumericUpDown numPlayers = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 6,
                Value = 2,
                Location = new Point(100, 2),
                Width = 60,
                Height = 25,
                Font = new Font("Arial", 11),
                BackColor = Color.FromArgb(50, 50, 80),
                ForeColor = Color.White
            };
            playersPanel.Controls.Add(numPlayers);

            Panel playerNamesPanel = new Panel
            {
                Location = new Point(200, 0),
                Size = new Size(500, 45),
                AutoScroll = true
            };
            playersPanel.Controls.Add(playerNamesPanel);

            numPlayers.ValueChanged += (s, e) =>
            {
                playerNamesPanel.Controls.Clear();
                int xOffset = 0;
                for (int i = 0; i < (int)numPlayers.Value; i++)
                {
                    Label lbl = new Label
                    {
                        Text = $"P{i + 1}:",
                        Location = new Point(xOffset, 10),
                        AutoSize = true,
                        ForeColor = Color.White,
                        Font = new Font("Arial", 9)
                    };
                    TextBox txt = new TextBox
                    {
                        Name = $"txtPlayer{i + 1}",
                        Location = new Point(xOffset + 35, 5),
                        Width = 60,
                        Height = 25,
                        Text = $"P{i + 1}",
                        BackColor = Color.FromArgb(50, 50, 80),
                        ForeColor = Color.White,
                        Font = new Font("Arial", 9)
                    };
                    playerNamesPanel.Controls.Add(lbl);
                    playerNamesPanel.Controls.Add(txt);
                    xOffset += 105;
                }
            };

            numPlayers.Value = 2;

            // Bottom buttons panel
            Panel buttonsBottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(0, 0, 80)
            };
            mainLayout.Controls.Add(buttonsBottomPanel, 0, 3);

            Button btnStart = new Button
            {
                Text = "START GAME",
                Location = new Point(450, 5),
                Size = new Size(120, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnStart.Click += (s, e) =>
            {
                gameState.GameMode = selectedMode;
                gameState.PlayerNames.Clear();
                for (int i = 0; i < (int)numPlayers.Value; i++)
                {
                    Control[] found = playerNamesPanel.Controls.Find($"txtPlayer{i + 1}", false);
                    if (found.Length > 0 && found[0] is TextBox txt)
                    {
                        string name = txt.Text.Trim();
                        gameState.PlayerNames.Add(string.IsNullOrEmpty(name) ? $"Player {i + 1}" : name);
                    }
                }

                CategorySelectionForm catForm = new CategorySelectionForm(gameState);
                catForm.Show();
                this.Hide();
            };
            buttonsBottomPanel.Controls.Add(btnStart);

            Button btnBack = new Button
            {
                Text = "BACK",
                Location = new Point(20, 5),
                Size = new Size(100, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 100, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBack.Click += (s, e) =>
            {
                LobbyForm lobby = new LobbyForm(gameState);
                lobby.Show();
                this.Hide();
            };
            buttonsBottomPanel.Controls.Add(btnBack);
        }

        private Button CreateTimerButton(int seconds)
        {
            Button btn = new Button
            {
                Text = $"{seconds}s",
                Size = new Size(80, 30),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(50, 50, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = Color.Gray;
            return btn;
        }

        private void UpdateTimerButtons(Button btn60, Button btn90, Button btn120)
        {
            // Reset all
            btn60.FlatAppearance.BorderColor = Color.Gray;
            btn60.FlatAppearance.BorderSize = 2;
            btn90.FlatAppearance.BorderColor = Color.Gray;
            btn90.FlatAppearance.BorderSize = 2;
            btn120.FlatAppearance.BorderColor = Color.Gray;
            btn120.FlatAppearance.BorderSize = 2;

            // Highlight selected
            if (gameState.TimerDuration == 60)
            {
                btn60.FlatAppearance.BorderColor = Color.Lime;
                btn60.FlatAppearance.BorderSize = 3;
            }
            else if (gameState.TimerDuration == 90)
            {
                btn90.FlatAppearance.BorderColor = Color.Lime;
                btn90.FlatAppearance.BorderSize = 3;
            }
            else if (gameState.TimerDuration == 120)
            {
                btn120.FlatAppearance.BorderColor = Color.Lime;
                btn120.FlatAppearance.BorderSize = 3;
            }
        }
    }
}
