using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Jeopardy
{
    public class CategorySelectionForm : Form
    {
        private GameState gameState;
        private QuestionSet selectedQuestionSet;
        private ListBox lstQuestionSets;
        private Label lblDescription;

        public CategorySelectionForm(GameState state)
        {
            gameState = state;
            InitializeComponent();
            BuildUI();
            LoadQuestionSets();
        }

        private void InitializeComponent()
        {
            this.Text = "Jeopardy! - Select Question Set";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(25, 25, 50);
            this.ForeColor = Color.White;
            this.Font = new Font("Arial", 11);
        }

        private void BuildUI()
        {
            // Main container
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(20),
                BackColor = Color.FromArgb(25, 25, 50)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            this.Controls.Add(mainLayout);

            // Title
            Label lblTitle = new Label
            {
                Text = "SELECT QUESTION SET",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.Cyan,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            mainLayout.Controls.Add(lblTitle, 0, 0);

            // List of question sets
            lstQuestionSets = new ListBox
            {
                BackColor = Color.FromArgb(50, 50, 100),
                ForeColor = Color.White,
                Font = new Font("Arial", 12),
                Dock = DockStyle.Fill
            };
            lstQuestionSets.SelectedIndexChanged += (s, e) =>
            {
                if (lstQuestionSets.SelectedItem is QuestionSet qs)
                {
                    selectedQuestionSet = qs;
                    lblDescription.Text = $"Categories: {string.Join(", ", qs.GetCategoryNames())}";
                }
            };
            mainLayout.Controls.Add(lstQuestionSets, 0, 1);

            // Description
            lblDescription = new Label
            {
                Text = "Select a question set to see available categories",
                ForeColor = Color.Yellow,
                Font = new Font("Arial", 10),
                AutoSize = false,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(0, 0, 80),
                Padding = new Padding(10)
            };
            mainLayout.Controls.Add(lblDescription, 0, 2);

            // Buttons panel
            Panel buttonsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(25, 25, 50)
            };
            mainLayout.Controls.Add(buttonsPanel, 0, 3);

            Button btnStart = new Button
            {
                Text = "START WITH THIS SET",
                Location = new Point(400, 10),
                Size = new Size(180, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.Green,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnStart.Click += (s, e) =>
            {
                if (selectedQuestionSet == null)
                {
                    MessageBox.Show("Please select a question set first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GameForm gameForm = new GameForm(gameState, selectedQuestionSet);
                gameForm.Show();
                this.Hide();
            };
            buttonsPanel.Controls.Add(btnStart);

            Button btnBack = new Button
            {
                Text = "BACK",
                Location = new Point(50, 10),
                Size = new Size(120, 40),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 100, 100),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBack.Click += (s, e) =>
            {
                GameModeSelectionForm modeForm = new GameModeSelectionForm(gameState);
                modeForm.Show();
                this.Hide();
            };
            buttonsPanel.Controls.Add(btnBack);
        }

        private void LoadQuestionSets()
        {
            List<QuestionSet> sets = QuestionDatabase.GetAvailableQuestionSets();
            foreach (var set in sets)
            {
                lstQuestionSets.Items.Add(set);
            }

            if (lstQuestionSets.Items.Count > 0)
            {
                lstQuestionSets.SelectedIndex = 0;
            }
        }
    }
}
