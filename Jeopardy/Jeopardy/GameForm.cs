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
        private List<string> playerNames;
        private int currentPlayerIndex;
        private List<int> playerScores;

        // Board data
        private List<string> categories;
        private Clue[,] board;        // 5 rows (values) x 5 columns (categories)
        private int selectedRow = -1, selectedCol = -1;

        // UI controls
        private FlowLayoutPanel scorePanel;
        private Label lblTurn;
        private TableLayoutPanel boardPanel;
        private Label lblClue;
        private TextBox txtAnswer;
        private Button btnSubmit;
        private Label lblTimer;
        private System.Windows.Forms.Timer responseTimer;
        private int timeLeft = 5;

        public GameForm(List<string> names)
        {
            playerNames = names;
            currentPlayerIndex = 0;
            playerScores = new List<int>(new int[playerNames.Count]);
            InitializeComponent();

            // Disable the default designer view
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Jeopardy! Game";
            this.Size = new Size(1200, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 700);
            BuildUI();
            InitializeGameData();
            BuildBoardUI();
            UpdateScoreDisplay();
            UpdateTurnLabel();
        }

        private void BuildUI()
        {
            // Score panel
            scorePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 45,
                Padding = new Padding(10),
                BackColor = Color.Navy
            };
            this.Controls.Add(scorePanel);

            // Current player label
            lblTurn = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.DarkBlue,
                Text = ""
            };
            this.Controls.Add(lblTurn);

            // Clue display
            lblClue = new Label
            {
                Dock = DockStyle.Top,
                Height = 55,
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.LightYellow,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "Select a clue to begin"
            };
            this.Controls.Add(lblClue);

            // Answer panel
            Panel answerPanel = new Panel { Dock = DockStyle.Top, Height = 38 };
            Label lblAnswerInstruction = new Label
            {
                Text = "Your response (must be in question form):",
                Location = new Point(10, 10),
                AutoSize = true,
                Font = new Font("Arial", 10)
            };
            txtAnswer = new TextBox
            {
                Location = new Point(280, 6),
                Width = 300,
                Enabled = false
            };
            btnSubmit = new Button
            {
                Text = "Submit",
                Location = new Point(590, 5),
                Width = 100,
                Enabled = false
            };
            btnSubmit.Click += BtnSubmit_Click;
            lblTimer = new Label
            {
                Text = "",
                Location = new Point(710, 10),
                AutoSize = true,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.Red
            };
            answerPanel.Controls.Add(lblAnswerInstruction);
            answerPanel.Controls.Add(txtAnswer);
            answerPanel.Controls.Add(btnSubmit);
            answerPanel.Controls.Add(lblTimer);
            this.Controls.Add(answerPanel);

            // Timer
            responseTimer = new System.Windows.Forms.Timer();
            responseTimer.Interval = 1000;
            responseTimer.Tick += ResponseTimer_Tick;

            // Board panel
            boardPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                AutoScroll = true
            };
            this.Controls.Add(boardPanel);
        }

        private void InitializeGameData()
        {
            categories = new List<string> { "Science", "History", "Movies", "Sports", "Geography" };
            int[] values = { 200, 400, 600, 800, 1000 };
            board = new Clue[5, 5];

  
            string[,] sampleQuestions = new string[5, 5]
            {
                { "This gas makes up most of Earth's atmosphere.", "Who was the first President of the United States?", "In 'The Shawshank Redemption', this actor played Andy Dufresne.", "This country won the FIFA World Cup in 2018.", "The longest river in Africa." },
                { "H2O is the chemical formula for this.", "This ancient wonder was located at Alexandria.", "Which movie featured a character named 'Forrest Gump'?", "Michael Jordan wore this number for most of his career.", "The capital of Japan." },
                { "The force that pulls objects toward Earth.", "The year World War II ended.", "Who played Jack Dawson in 'Titanic'?", "The sport known as 'the beautiful game'.", "The driest desert on Earth (excluding poles)." },
                { "The planet known as the 'Red Planet'.", "The name of the ship Charles Darwin sailed on.", "Which film won the Oscar for Best Picture in 1994?", "The athlete with the most Olympic gold medals.", "The smallest country in the world." },
                { "The unit of electrical resistance.", "The name of the first satellite in space.", "Who directed 'Jurassic Park'?", "The team that won the first Super Bowl.", "The largest ocean on Earth." }
            };

            string[,] sampleAnswers = new string[5, 5]
            {
                { "what is nitrogen", "who is george washington", "who is tim robbins", "what is france", "what is the nile" },
                { "what is water", "what is the lighthouse of alexandria", "what is forrest gump", "what is 23", "what is tokyo" },
                { "what is gravity", "what is 1945", "who is leonardo dicaprio", "what is soccer", "what is the atacama" },
                { "what is mars", "what is the beagle", "what is forrest gump", "who is michael phelps", "what is vatican city" },
                { "what is ohm", "what is sputnik", "who is steven spielberg", "what is green bay packers", "what is pacific" }
            };

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    board[row, col] = new Clue
                    {
                        Value = values[row],
                        Question = sampleQuestions[row, col],
                        Answer = sampleAnswers[row, col],
                        IsAnswered = false
                    };
                }
            }
        }

        private void BuildBoardUI()
        {
            boardPanel.Controls.Clear();
            boardPanel.ColumnCount = 5;
            for (int i = 0; i < 5; i++)
                boardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));

            // Row 0: category headers
            boardPanel.RowCount = 6;
            boardPanel.RowStyles.Clear();
            boardPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f)); // header row
            for (int i = 0; i < 5; i++)
                boardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));

            // Add category headers
            for (int col = 0; col < 5; col++)
            {
                Label header = new Label
                {
                    Text = categories[col],
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    BackColor = Color.LightBlue,
                    ForeColor = Color.Navy
                };
                boardPanel.Controls.Add(header, col, 0);
            }

            // Add clue buttons
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    Button btn = new Button
                    {
                        Text = $"${board[row, col].Value}",
                        Dock = DockStyle.Fill,
                        Tag = new Point(row, col),
                        BackColor = Color.Gold,
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        FlatStyle = FlatStyle.Flat
                    };
                    btn.Click += ClueButton_Click;
                    boardPanel.Controls.Add(btn, col, row + 1);
                }
            }
        }

        private void ClueButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Point pos = (Point)btn.Tag;
            int row = pos.X, col = pos.Y;

            if (board[row, col].IsAnswered)
            {
                MessageBox.Show("This clue has already been answered!", "Already Used", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            selectedRow = row;
            selectedCol = col;

            // Display the clue
            lblClue.Text = board[row, col].Question;

            // Enable answer entry
            txtAnswer.Clear();
            txtAnswer.Enabled = true;
            btnSubmit.Enabled = true;
            txtAnswer.Focus();

            // Start timer
            timeLeft = 30;
            lblTimer.Text = $"Time: {timeLeft}s";
            responseTimer.Start();

            // Disable all clue buttons during answering
            foreach (Control ctrl in boardPanel.Controls)
            {
                if (ctrl is Button b && b != btnSubmit)
                    b.Enabled = false;
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            responseTimer.Stop();
            string playerAnswer = txtAnswer.Text.Trim().ToLower();
            string correctAnswer = board[selectedRow, selectedCol].Answer.ToLower();

            bool isCorrect = (playerAnswer == correctAnswer);

            if (isCorrect)
            {
                int points = board[selectedRow, selectedCol].Value;
                playerScores[currentPlayerIndex] += points;
                MessageBox.Show($"Correct! +${points}", "Correct!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                board[selectedRow, selectedCol].IsAnswered = true;
                DisableBoardButton(selectedRow, selectedCol);
            }
            else
            {
                int points = board[selectedRow, selectedCol].Value;
                playerScores[currentPlayerIndex] -= points;
                MessageBox.Show($"Wrong! Correct answer: \"{correctAnswer}\"\n-${points}", "Incorrect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Clue remains available (IsAnswered stays false)
            }

            // Update scores UI
            UpdateScoreDisplay();

            // Move to next player
            currentPlayerIndex = (currentPlayerIndex + 1) % playerNames.Count;
            UpdateTurnLabel();

            // Clear answer UI
            txtAnswer.Enabled = false;
            btnSubmit.Enabled = false;
            txtAnswer.Clear();
            lblClue.Text = "Select a clue";
            lblTimer.Text = "";

            // Re-enable remaining clue buttons
            EnableRemainingClueButtons();

            // Check for end of game
            CheckGameEnd();
        }

        private void ResponseTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            lblTimer.Text = $"Time: {timeLeft}s";
            if (timeLeft <= 0)
            {
                responseTimer.Stop();
                MessageBox.Show("Time's up! No answer given.", "Time Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Treat as wrong answer (no penalty in this version, but you can deduct if desired)
                txtAnswer.Enabled = false;
                btnSubmit.Enabled = false;
                lblClue.Text = "Select a clue";
                lblTimer.Text = "";

                // Move to next player
                currentPlayerIndex = (currentPlayerIndex + 1) % playerNames.Count;
                UpdateTurnLabel();

                // Re-enable clue buttons
                EnableRemainingClueButtons();
            }
        }

        private void DisableBoardButton(int row, int col)
        {
            foreach (Control ctrl in boardPanel.Controls)
            {
                if (ctrl is Button btn && btn.Tag is Point p && p.X == row && p.Y == col)
                {
                    btn.Enabled = false;
                    btn.BackColor = Color.Gray;
                    btn.Text = "Used";
                    break;
                }
            }
        }

        private void EnableRemainingClueButtons()
        {
            foreach (Control ctrl in boardPanel.Controls)
            {
                if (ctrl is Button btn && btn.Tag is Point p)
                {
                    if (!board[p.X, p.Y].IsAnswered)
                        btn.Enabled = true;
                }
            }
        }

        private void UpdateScoreDisplay()
        {
            scorePanel.Controls.Clear();
            for (int i = 0; i < playerNames.Count; i++)
            {
                Label lbl = new Label
                {
                    Text = $"{playerNames[i]}: ${playerScores[i]}",
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.White,
                    AutoSize = true,
                    Margin = new Padding(20, 5, 20, 5)
                };
                scorePanel.Controls.Add(lbl);
            }
        }

        private void UpdateTurnLabel()
        {
            lblTurn.Text = $"Current Player: {playerNames[currentPlayerIndex]}";
        }

        private void CheckGameEnd()
        {
            bool allAnswered = true;
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (!board[row, col].IsAnswered)
                    {
                        allAnswered = false;
                        break;
                    }
                }
            }

            if (allAnswered)
            {
                // Find winner
                int maxScore = playerScores.Max();
                List<string> winners = new List<string>();
                for (int i = 0; i < playerNames.Count; i++)
                {
                    if (playerScores[i] == maxScore)
                        winners.Add(playerNames[i]);
                }

                string winnerMsg = (winners.Count == 1) ? $"Winner: {winners[0]}!" : $"It's a tie between: {string.Join(", ", winners)}";
                MessageBox.Show($"Game Over!\nFinal scores:\n{string.Join("\n", playerNames.Select((n, i) => $"{n}: ${playerScores[i]}"))}\n\n{winnerMsg}",
                                "Game Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the game form or return to start screen
                this.Close();
            }
        }
    }

    // Helper class for a clue
    public class Clue
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Value { get; set; }
        public bool IsAnswered { get; set; }
    }
}