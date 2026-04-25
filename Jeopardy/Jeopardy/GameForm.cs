using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Jeopardy
{
    public partial class GameForm : Form
    {
        // Game data
        private GameState gameState;
        private List<string> playerNames;
        private int currentPlayerIndex;
        private List<int> playerScores;
        private List<bool> playerBuzzed;

        // Board data
        private QuestionSet questionSet;
        private List<string> categories;
        private Dictionary<string, List<ClueData>> board;
        private string selectedCategory = "";
        private int selectedValue = 0;

        // UI controls
        private FlowLayoutPanel scoreboardPanel;
        private Label lblClueCategory;
        private Label lblClueValue;
        private Label lblClueText;
        private TextBox txtAnswer;
        private Button btnSubmit;
        private Label lblTimer;
        private Label lblBuzzer;
        private Panel boardPanel;
        private System.Windows.Forms.Timer gameTimer;
        private int timeLeft = 5;

        // Game state
        private bool isAnsweringPhase = false;
        private bool buzzerLocked = true;

        public GameForm(GameState state, QuestionSet questions)
        {
            gameState = state;
            questionSet = questions;
            playerNames = gameState.PlayerNames;
            currentPlayerIndex = 0;
            playerScores = new List<int>(new int[playerNames.Count]);
            playerBuzzed = new List<bool>(new bool[playerNames.Count]);
            categories = questionSet.GetCategoryNames();
            board = questionSet.Categories;

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Jeopardy!";
            this.BackColor = Color.FromArgb(0, 40, 100);
            this.ForeColor = Color.White;
            this.Font = new Font("Arial", 11);
            this.DoubleBuffered = true;

            InitializeComponent();
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;

            BuildUI();
            BuildBoardUI();
            UpdateScoreboard();
        }

        private void BuildUI()
        {
            // Main layout - vertical stacking
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 4,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(0, 40, 100),
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            // Row styles: Fixed heights for top/bottom, fill for middle
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));    // Scoreboard
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));   // Clue display
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));    // Board
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));   // Answer input
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            this.Controls.Add(mainLayout);

            // ===== SCOREBOARD PANEL =====
            scoreboardPanel = new FlowLayoutPanel
            {
                AutoScroll = false,
                WrapContents = true,
                BackColor = Color.FromArgb(0, 0, 80),
                Padding = new Padding(10),
                Margin = new Padding(0),
                Dock = DockStyle.Fill
            };
            mainLayout.Controls.Add(scoreboardPanel, 0, 0);

            // ===== CLUE DISPLAY PANEL =====
            Panel clueDisplayPanel = new Panel
            {
                BackColor = Color.FromArgb(0, 40, 100),
                Padding = new Padding(15),
                Margin = new Padding(0),
                Dock = DockStyle.Fill
            };
            mainLayout.Controls.Add(clueDisplayPanel, 0, 1);

            // Inner layout for clue display
            TableLayoutPanel clueLayout = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 3,
                Dock = DockStyle.Fill,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            clueLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            clueLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            clueLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            clueLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            clueLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            clueDisplayPanel.Controls.Add(clueLayout);

            // Category label
            lblClueCategory = new Label
            {
                Text = "CATEGORY",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Cyan,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };
            clueLayout.Controls.Add(lblClueCategory, 0, 0);

            // Value label
            lblClueValue = new Label
            {
                Text = "$0",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.Yellow,
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                Margin = new Padding(5)
            };
            clueLayout.Controls.Add(lblClueValue, 1, 0);

            // Clue text
            lblClueText = new Label
            {
                Text = "Select a clue from the board",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(0, 0, 80),
                AutoSize = false,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };
            clueLayout.Controls.Add(lblClueText, 0, 1);
            clueLayout.SetColumnSpan(lblClueText, 2);

            // Timer and Buzzer status
            Panel timerBuzzerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(5)
            };
            clueLayout.Controls.Add(timerBuzzerPanel, 0, 2);
            clueLayout.SetColumnSpan(timerBuzzerPanel, 2);

            lblTimer = new Label
            {
                Text = "",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Left,
                AutoSize = true
            };
            timerBuzzerPanel.Controls.Add(lblTimer);

            lblBuzzer = new Label
            {
                Text = "",
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.Lime,
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Right,
                AutoSize = true,
                Margin = new Padding(10, 0, 0, 0)
            };
            timerBuzzerPanel.Controls.Add(lblBuzzer);

            // ===== BOARD PANEL =====
            boardPanel = new Panel
            {
                BackColor = Color.FromArgb(0, 40, 100),
                Padding = new Padding(15),
                Margin = new Padding(0),
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            mainLayout.Controls.Add(boardPanel, 0, 2);

            // ===== ANSWER INPUT PANEL =====
            Panel answerPanel = new Panel
            {
                BackColor = Color.FromArgb(0, 0, 80),
                Padding = new Padding(15),
                Margin = new Padding(0),
                Dock = DockStyle.Fill
            };
            mainLayout.Controls.Add(answerPanel, 0, 3);

            // Answer input layout
            TableLayoutPanel answerLayout = new TableLayoutPanel
            {
                ColumnCount = 4,
                RowCount = 2,
                Dock = DockStyle.Fill,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            answerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));
            answerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            answerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            answerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            answerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
            answerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            answerPanel.Controls.Add(answerLayout);

            Label lblAnswerLabel = new Label
            {
                Text = "Your Answer:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Cyan,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill
            };
            answerLayout.Controls.Add(lblAnswerLabel, 0, 0);

            txtAnswer = new TextBox
            {
                BackColor = Color.FromArgb(50, 50, 100),
                ForeColor = Color.White,
                Font = new Font("Arial", 11),
                Enabled = false,
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 0, 5, 0)
            };
            answerLayout.Controls.Add(txtAnswer, 1, 0);

            btnSubmit = new Button
            {
                Text = "SUBMIT",
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Fill,
                Margin = new Padding(2)
            };
            btnSubmit.Click += BtnSubmit_Click;
            answerLayout.Controls.Add(btnSubmit, 2, 0);

            Button btnBuzz = new Button
            {
                Text = "BUZZ IN",
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(255, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Fill,
                Margin = new Padding(2),
                Visible = (gameState.GameMode == GameMode.Buzzer),
                Tag = "BuzzButton"
            };
            btnBuzz.Click += (s, e) => HandleBuzz();
            answerLayout.Controls.Add(btnBuzz, 3, 0);

            // Register keyboard shortcut
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Space && gameState.GameMode == GameMode.Buzzer && !buzzerLocked && !isAnsweringPhase)
                {
                    HandleBuzz();
                    e.Handled = true;
                }
            };
        }

        private void BuildBoardUI()
        {
            boardPanel.Controls.Clear();

            if (categories.Count == 0)
            {
                MessageBox.Show("No categories available!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create main board layout
            TableLayoutPanel boardLayout = new TableLayoutPanel
            {
                ColumnCount = categories.Count,
                RowCount = 6,
                Dock = DockStyle.Fill,
                AutoSize = false,
                Padding = new Padding(0),
                Margin = new Padding(0),
                BackColor = Color.FromArgb(0, 40, 100)
            };

            // Set equal column widths - distribute evenly
            float colWidth = (float)1.0 / categories.Count * 100;
            for (int i = 0; i < categories.Count; i++)
            {
                boardLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (float)colWidth));
            }

            // Header row
            boardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));

            // Value rows
            for (int i = 0; i < 5; i++)
            {
                boardLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
            }

            // Add category headers
            for (int col = 0; col < categories.Count; col++)
            {
                Label header = new Label
                {
                    Text = categories[col].ToUpper(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    BackColor = Color.FromArgb(0, 0, 80),
                    ForeColor = Color.Cyan,
                    BorderStyle = BorderStyle.Fixed3D,
                    Margin = new Padding(2),
                    Padding = new Padding(5),
                    AutoSize = false
                };
                boardLayout.Controls.Add(header, col, 0);
            }

            // Add clue buttons (values: 200, 400, 600, 800, 1000)
            int[] values = { 200, 400, 600, 800, 1000 };
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < categories.Count; col++)
                {
                    string categoryName = categories[col];
                    int value = values[row];

                    Button btn = new Button
                    {
                        Text = $"${value}",
                        Dock = DockStyle.Fill,
                        BackColor = Color.FromArgb(0, 0, 200),
                        ForeColor = Color.Yellow,
                        Font = new Font("Arial", 14, FontStyle.Bold),
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Margin = new Padding(2),
                        Padding = new Padding(0),
                        Tag = $"{categoryName}:{value}",
                        AutoSize = false
                    };
                    btn.FlatAppearance.BorderSize = 2;
                    btn.FlatAppearance.BorderColor = Color.Cyan;
                    btn.Click += ClueButton_Click;
                    boardLayout.Controls.Add(btn, col, row + 1);
                }
            }

            boardPanel.Controls.Add(boardLayout);
        }

        private void ClueButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            string[] parts = btn.Tag.ToString().Split(':');
            selectedCategory = parts[0];
            selectedValue = int.Parse(parts[1]);

            // Find the clue
            ClueData clue = board[selectedCategory].FirstOrDefault(c => c.Value == selectedValue);
            if (clue == null || clue.IsAnswered)
            {
                MessageBox.Show("This clue has already been answered!", "Already Used", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Display clue
            lblClueCategory.Text = selectedCategory.ToUpper();
            lblClueValue.Text = $"${selectedValue}";
            lblClueText.Text = clue.Question;

            buzzerLocked = false;

            if (gameState.GameMode == GameMode.Buzzer)
            {
                lblBuzzer.Text = "Press SPACE or BUZZ IN";
                lblBuzzer.ForeColor = Color.Lime;
                lblTimer.Text = "";
            }
            else if (gameState.GameMode == GameMode.Timer)
            {
                isAnsweringPhase = true;
                timeLeft = gameState.TimerDuration;
                lblTimer.Text = $"Time: {timeLeft}s";
                lblTimer.ForeColor = Color.Red;
                lblBuzzer.Text = "Everyone answers!";
                lblBuzzer.ForeColor = Color.Yellow;
                gameTimer.Start();

                txtAnswer.Enabled = true;
                txtAnswer.Focus();
                btnSubmit.Enabled = true;
            }

            DisableAllButtons();
        }

        private void HandleBuzz()
        {
            if (buzzerLocked) return;
            if (string.IsNullOrEmpty(selectedCategory) || selectedValue == 0) return;

            buzzerLocked = true;
            isAnsweringPhase = true;
            gameTimer.Stop();

            lblBuzzer.Text = $"{playerNames[currentPlayerIndex]} buzzed in!";
            lblBuzzer.ForeColor = Color.Lime;
            lblTimer.Text = "Answering...";

            txtAnswer.Enabled = true;
            txtAnswer.Focus();
            btnSubmit.Enabled = true;

            timeLeft = 10;
            gameTimer.Start();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            gameTimer.Stop();

            ClueData clue = board[selectedCategory].FirstOrDefault(c => c.Value == selectedValue);
            if (clue == null) return;

            string playerAnswer = txtAnswer.Text.Trim().ToLower();
            string correctAnswer = clue.Answer.ToLower();
            bool isCorrect = (playerAnswer == correctAnswer);

            if (isCorrect)
            {
                playerScores[currentPlayerIndex] += selectedValue;
                MessageBox.Show($"✓ CORRECT!\n+${selectedValue}", "Correct!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clue.IsAnswered = true;
                MarkButtonAsUsed(selectedCategory, selectedValue);
            }
            else
            {
                if (gameState.GameMode == GameMode.Buzzer)
                {
                    playerScores[currentPlayerIndex] -= selectedValue;
                    MessageBox.Show($"✗ WRONG!\nCorrect answer: \"{correctAnswer}\"\n-${selectedValue}", "Incorrect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    currentPlayerIndex = (currentPlayerIndex + 1) % playerNames.Count;
                }
                else
                {
                    playerScores[currentPlayerIndex] -= selectedValue;
                    MessageBox.Show($"✗ WRONG!\nCorrect answer: \"{correctAnswer}\"\n-${selectedValue}", "Incorrect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    clue.IsAnswered = true;
                    MarkButtonAsUsed(selectedCategory, selectedValue);
                }
            }

            UpdateScoreboard();
            ResetAnswerUI();
            EnableAllButtons();
            CheckGameEnd();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Text = $"Time: {timeLeft}s";

            if (timeLeft <= 0)
            {
                gameTimer.Stop();

                if (gameState.GameMode == GameMode.Buzzer)
                {
                    MessageBox.Show("Time expired! No one buzzed in.", "Time Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ResetAnswerUI();
                    EnableAllButtons();
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtAnswer.Text))
                    {
                        BtnSubmit_Click(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Time's up!", "Time Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ClueData clue = board[selectedCategory].FirstOrDefault(c => c.Value == selectedValue);
                        if (clue != null) clue.IsAnswered = true;
                        MarkButtonAsUsed(selectedCategory, selectedValue);
                        UpdateScoreboard();
                        ResetAnswerUI();
                        EnableAllButtons();
                        CheckGameEnd();
                    }
                }
            }
        }

        private void ResetAnswerUI()
        {
            txtAnswer.Enabled = false;
            btnSubmit.Enabled = false;
            txtAnswer.Clear();
            lblClueText.Text = "Select a clue from the board";
            lblClueCategory.Text = "CATEGORY";
            lblClueValue.Text = "$0";
            lblTimer.Text = "";
            lblBuzzer.Text = "";
            isAnsweringPhase = false;
            buzzerLocked = true;
        }

        private void DisableAllButtons()
        {
            foreach (Control ctrl in boardPanel.Controls)
            {
                if (ctrl is TableLayoutPanel tlp)
                {
                    foreach (Control c in tlp.Controls)
                    {
                        if (c is Button btn) btn.Enabled = false;
                    }
                }
            }
        }

        private void EnableAllButtons()
        {
            foreach (Control ctrl in boardPanel.Controls)
            {
                if (ctrl is TableLayoutPanel tlp)
                {
                    foreach (Control c in tlp.Controls)
                    {
                        if (c is Button btn && btn.Tag is string tag)
                        {
                            string[] parts = tag.Split(':');
                            if (parts.Length == 2)
                            {
                                string cat = parts[0];
                                int val = int.Parse(parts[1]);
                                ClueData clue = board[cat].FirstOrDefault(x => x.Value == val);
                                btn.Enabled = clue != null && !clue.IsAnswered;
                            }
                        }
                    }
                }
            }
        }

        private void MarkButtonAsUsed(string category, int value)
        {
            foreach (Control ctrl in boardPanel.Controls)
            {
                if (ctrl is TableLayoutPanel tlp)
                {
                    foreach (Control c in tlp.Controls)
                    {
                        if (c is Button btn && btn.Tag is string tag && tag == $"{category}:{value}")
                        {
                            btn.Enabled = false;
                            btn.BackColor = Color.FromArgb(50, 50, 50);
                            btn.ForeColor = Color.Gray;
                            btn.Text = "USED";
                            break;
                        }
                    }
                }
            }
        }

        private void UpdateScoreboard()
        {
            scoreboardPanel.Controls.Clear();
            for (int i = 0; i < playerNames.Count; i++)
            {
                Panel playerPanel = new Panel
                {
                    BackColor = (i == currentPlayerIndex) ? Color.FromArgb(0, 100, 200) : Color.FromArgb(0, 0, 80),
                    Height = 50,
                    Margin = new Padding(5),
                    BorderStyle = BorderStyle.FixedSingle,
                    AutoSize = false,
                    Width = 180
                };

                Label lbl = new Label
                {
                    Text = $"{playerNames[i]}\n${playerScores[i]}",
                    Font = new Font("Arial", 11, FontStyle.Bold),
                    ForeColor = Color.White,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                playerPanel.Controls.Add(lbl);
                scoreboardPanel.Controls.Add(playerPanel);
            }
        }

        private void CheckGameEnd()
        {
            foreach (var category in categories)
            {
                if (board[category].Any(c => !c.IsAnswered))
                    return;
            }

            // All clues answered
            int maxScore = playerScores.Max();
            var winners = playerNames.Where((n, i) => playerScores[i] == maxScore).ToList();

            string winnerMsg = winners.Count == 1 ? $"🏆 WINNER: {winners[0]}! 🏆" : $"🏆 TIE: {string.Join(", ", winners)}! 🏆";
            string finalScores = string.Join("\n", playerNames.Select((n, i) => $"{n}: ${playerScores[i]}"));
            MessageBox.Show($"GAME OVER!\n\nFinal Scores:\n{finalScores}\n\n{winnerMsg}", "Game Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }
    }
}
