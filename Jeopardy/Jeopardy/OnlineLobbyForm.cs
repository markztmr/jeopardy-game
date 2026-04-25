using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Jeopardy
{
    public class OnlineLobbyForm : Form
    {
        private GameState gameState;
        private string generatedCode;

        public OnlineLobbyForm(GameState state)
        {
            gameState = state ?? new GameState();
            InitializeComponent();
            BuildUI();
        }

        private void InitializeComponent()
        {
            this.Text = "Jeopardy! - Online Lobby";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(25, 25, 50);
            this.ForeColor = Color.White;
            this.Font = new Font("Arial", 12);
        }

        private void BuildUI()
        {
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(25, 25, 50)
            };
            this.Controls.Add(mainPanel);

            // Title
            Label lblTitle = new Label
            {
                Text = "ONLINE GAME SETUP",
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.Cyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 80,
                Dock = DockStyle.Top
            };
            mainPanel.Controls.Add(lblTitle);

            // Role Selection
            Panel rolePanel = new Panel
            {
                Height = 200,
                Dock = DockStyle.Top,
                Padding = new Padding(40)
            };
            mainPanel.Controls.Add(rolePanel);

            Label lblRole = new Label
            {
                Text = "SELECT YOUR ROLE:",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Cyan,
                Location = new Point(0, 0),
                AutoSize = true
            };
            rolePanel.Controls.Add(lblRole);

            Button btnHost = new Button
            {
                Text = "👑 CREATE LOBBY\n(I am the Host)",
                Location = new Point(50, 40),
                Size = new Size(280, 120),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(200, 100, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnHost.FlatAppearance.BorderSize = 2;
            btnHost.FlatAppearance.BorderColor = Color.Cyan;
            btnHost.Click += (s, e) => OpenHostSetup();
            rolePanel.Controls.Add(btnHost);

            Button btnJoin = new Button
            {
                Text = "👥 JOIN LOBBY\n(Enter a Code)",
                Location = new Point(470, 40),
                Size = new Size(280, 120),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnJoin.FlatAppearance.BorderSize = 2;
            btnJoin.FlatAppearance.BorderColor = Color.Cyan;
            btnJoin.Click += (s, e) => OpenJoinSetup();
            rolePanel.Controls.Add(btnJoin);

            // Info panel
            Panel infoPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40)
            };
            mainPanel.Controls.Add(infoPanel);

            Label lblInfo = new Label
            {
                Text = "HOST:\nYou create a lobby and receive a unique code.\nShare the code with other players.\nYou approve correct/incorrect answers.\n\nJOINER:\nEnter the code received from the host.\nConnect to the host's game.\nWait for host to start the game.",
                Font = new Font("Arial", 11),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(0, 0)
            };
            infoPanel.Controls.Add(lblInfo);

            // Back button
            Panel buttonsPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                Padding = new Padding(20)
            };
            this.Controls.Add(buttonsPanel);

            Button btnBack = new Button
            {
                Text = "BACK",
                Location = new Point(50, 10),
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
            buttonsPanel.Controls.Add(btnBack);
        }

        private void OpenHostSetup()
        {
            Form hostForm = new Form
            {
                Text = "Host Setup",
                Size = new Size(600, 500),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(25, 25, 50),
                ForeColor = Color.White,
                Font = new Font("Arial", 11)
            };

            Panel panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20), AutoScroll = true };
            hostForm.Controls.Add(panel);

            Label lblCode = new Label
            {
                Text = "LOBBY CODE (Auto-Generated):",
                ForeColor = Color.Cyan,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(0, 20),
                AutoSize = true
            };
            panel.Controls.Add(lblCode);

            generatedCode = GenerateLobbyCode();
            TextBox txtCode = new TextBox
            {
                Text = generatedCode,
                Location = new Point(0, 50),
                Width = 200,
                Height = 30,
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(50, 50, 100),
                ForeColor = Color.Lime,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Center
            };
            panel.Controls.Add(txtCode);

            Button btnCopy = new Button
            {
                Text = "COPY CODE",
                Location = new Point(220, 50),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCopy.Click += (s, e) =>
            {
                Clipboard.SetText(generatedCode);
                MessageBox.Show("Code copied to clipboard!", "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            panel.Controls.Add(btnCopy);

            Label lblPlayers = new Label
            {
                Text = "\nHOST NAME:",
                ForeColor = Color.Cyan,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(0, 100),
                AutoSize = true
            };
            panel.Controls.Add(lblPlayers);

            TextBox txtHostName = new TextBox
            {
                Text = "Host",
                Location = new Point(0, 130),
                Width = 300,
                Height = 25,
                BackColor = Color.FromArgb(50, 50, 100),
                ForeColor = Color.White
            };
            panel.Controls.Add(txtHostName);

            Label lblGameMode = new Label
            {
                Text = "\nSELECT GAME MODE:",
                ForeColor = Color.Cyan,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(0, 170),
                AutoSize = true
            };
            panel.Controls.Add(lblGameMode);

            ComboBox cmbMode = new ComboBox
            {
                Location = new Point(0, 200),
                Width = 300,
                Height = 25,
                BackColor = Color.FromArgb(50, 50, 100),
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMode.Items.AddRange(new[] { "Buzzer Mode", "Timer Mode (60s)", "Timer Mode (90s)", "Timer Mode (120s)" });
            cmbMode.SelectedIndex = 0;
            panel.Controls.Add(cmbMode);

            Panel buttonsPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                Padding = new Padding(20)
            };
            hostForm.Controls.Add(buttonsPanel);

            Button btnStart = new Button
            {
                Text = "START HOSTING",
                Location = new Point(350, 10),
                Size = new Size(150, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnStart.Click += (s, e) =>
            {
                gameState.IsHost = true;
                gameState.LobbyCode = generatedCode;
                gameState.PlayerNames.Add(txtHostName.Text.Trim());

                switch (cmbMode.SelectedIndex)
                {
                    case 0:
                        gameState.GameMode = GameMode.Buzzer;
                        break;
                    case 1:
                        gameState.GameMode = GameMode.Timer;
                        gameState.TimerDuration = 60;
                        break;
                    case 2:
                        gameState.GameMode = GameMode.Timer;
                        gameState.TimerDuration = 90;
                        break;
                    case 3:
                        gameState.GameMode = GameMode.Timer;
                        gameState.TimerDuration = 120;
                        break;
                }

                // For now, proceed with category selection
                // In production, this would wait for players to join
                CategorySelectionForm catForm = new CategorySelectionForm(gameState);
                catForm.Show();
                hostForm.Close();
                this.Hide();
            };
            buttonsPanel.Controls.Add(btnStart);

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                Location = new Point(50, 10),
                Size = new Size(100, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 100, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => hostForm.Close();
            buttonsPanel.Controls.Add(btnCancel);

            hostForm.ShowDialog(this);
        }

        private void OpenJoinSetup()
        {
            Form joinForm = new Form
            {
                Text = "Join Game",
                Size = new Size(600, 350),
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(25, 25, 50),
                ForeColor = Color.White,
                Font = new Font("Arial", 11)
            };

            Panel panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            joinForm.Controls.Add(panel);

            Label lblCode = new Label
            {
                Text = "ENTER LOBBY CODE:",
                ForeColor = Color.Cyan,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(0, 20),
                AutoSize = true
            };
            panel.Controls.Add(lblCode);

            TextBox txtCode = new TextBox
            {
                Location = new Point(0, 50),
                Width = 300,
                Height = 30,
                Font = new Font("Arial", 14),
                BackColor = Color.FromArgb(50, 50, 100),
                ForeColor = Color.White,
                TextAlign = HorizontalAlignment.Center
            };
            panel.Controls.Add(txtCode);

            Label lblName = new Label
            {
                Text = "\nYOUR NAME:",
                ForeColor = Color.Cyan,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(0, 110),
                AutoSize = true
            };
            panel.Controls.Add(lblName);

            TextBox txtName = new TextBox
            {
                Text = "Player",
                Location = new Point(0, 140),
                Width = 300,
                Height = 25,
                BackColor = Color.FromArgb(50, 50, 100),
                ForeColor = Color.White
            };
            panel.Controls.Add(txtName);

            Panel buttonsPanel = new Panel
            {
                Height = 60,
                Dock = DockStyle.Bottom,
                Padding = new Padding(20)
            };
            joinForm.Controls.Add(buttonsPanel);

            Button btnJoin = new Button
            {
                Text = "JOIN",
                Location = new Point(350, 10),
                Size = new Size(150, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnJoin.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtCode.Text))
                {
                    MessageBox.Show("Please enter a lobby code!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                gameState.IsHost = false;
                gameState.LobbyCode = txtCode.Text.Trim().ToUpper();
                gameState.PlayerNames.Add(txtName.Text.Trim());

                // In production, connect to host and wait for game to start
                MessageBox.Show($"Connected to lobby: {gameState.LobbyCode}\nWaiting for host to start the game...", "Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                joinForm.Close();
                this.Hide();
            };
            buttonsPanel.Controls.Add(btnJoin);

            Button btnCancel = new Button
            {
                Text = "CANCEL",
                Location = new Point(50, 10),
                Size = new Size(100, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 100, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.Click += (s, e) => joinForm.Close();
            buttonsPanel.Controls.Add(btnCancel);

            joinForm.ShowDialog(this);
        }

        private string GenerateLobbyCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                code += chars[random.Next(chars.Length)];
            }
            return code;
        }
    }
}
