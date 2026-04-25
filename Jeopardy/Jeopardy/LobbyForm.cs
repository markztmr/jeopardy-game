using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Jeopardy
{
    public partial class LobbyForm : Form
    {
        private GameState gameState;

        public LobbyForm(GameState state)
        {
            gameState = state ?? new GameState();
            InitializeComponent();
            BuildUI();
            ApplyTheme();
        }

        private void InitializeComponent()
        {
            this.Text = "Jeopardy! - Game Lobby";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(25, 25, 50);
            this.ForeColor = Color.White;
            this.Font = new Font("Arial", 12);
        }

        private void BuildUI()
        {
            // Main layout - TableLayoutPanel for proper spacing
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 3,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(25, 25, 50),
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            this.Controls.Add(mainLayout);

            // Title
            Label lblTitle = new Label
            {
                Text = "SELECT GAME MODE",
                Font = new Font("Arial", 28, FontStyle.Bold),
                ForeColor = Color.Cyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            // Buttons container - TableLayoutPanel
            TableLayoutPanel buttonsLayout = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                Margin = new Padding(0),
                BackColor = Color.FromArgb(25, 25, 50)
            };
            buttonsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttonsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            buttonsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.Controls.Add(buttonsLayout, 0, 1);

            // Local Game Button
            Button btnLocal = new Button
            {
                Text = "LOCAL GAME\n(Pass & Play)",
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Fill,
                Margin = new Padding(10)
            };
            btnLocal.FlatAppearance.BorderSize = 2;
            btnLocal.FlatAppearance.BorderColor = Color.Cyan;
            btnLocal.Click += (s, e) =>
            {
                gameState.GameType = GameType.LocalGame;
                OpenGameModeSelection();
            };
            buttonsLayout.Controls.Add(btnLocal, 0, 0);

            // Online Game Button
            Button btnOnline = new Button
            {
                Text = "ONLINE GAME\n(with Friends)",
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(204, 0, 102),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Fill,
                Margin = new Padding(10)
            };
            btnOnline.FlatAppearance.BorderSize = 2;
            btnOnline.FlatAppearance.BorderColor = Color.Cyan;
            btnOnline.Click += (s, e) =>
            {
                gameState.GameType = GameType.OnlineGame;
                OpenOnlineLobby();
            };
            buttonsLayout.Controls.Add(btnOnline, 1, 0);

            // Info panel
            Panel infoPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40),
                BackColor = Color.FromArgb(25, 25, 50)
            };
            mainLayout.Controls.Add(infoPanel, 0, 2);

            Label lblLocalInfo = new Label
            {
                Text = "LOCAL GAME:\nPlay with friends on the same computer.\nPass the controller for each turn.\n\nONLINE GAME:\nConnect with players across the internet.\nHost creates a lobby code to share with others.",
                Font = new Font("Arial", 12),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Location = new Point(10, 10)
            };
            infoPanel.Controls.Add(lblLocalInfo);
        }

        private void ApplyTheme()
        {
            // Jeopardy dark blue theme
            this.BackColor = Color.FromArgb(25, 25, 50);
        }

        private void OpenGameModeSelection()
        {
            GameModeSelectionForm modeForm = new GameModeSelectionForm(gameState);
            modeForm.Show();
            this.Hide();
        }

        private void OpenOnlineLobby()
        {
            OnlineLobbyForm onlineForm = new OnlineLobbyForm(gameState);
            onlineForm.Show();
            this.Hide();
        }
    }
}
