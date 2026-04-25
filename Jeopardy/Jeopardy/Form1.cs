namespace Jeopardy
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numPlayers_ValueChanged(object sender, EventArgs e)
        {
            int count = (int)numPlayers.Value;
            panelPlayerNames.Controls.Clear();

            int yOffset = 10;
            for (int i = 0; i < count; i++)
            {
                Label lbl = new Label
                {
                    Text = $"Player {i + 1}:",
                    Location = new Point(10, yOffset),
                    AutoSize = true
                };
                TextBox txt = new TextBox
                {
                    Name = $"txtPlayer{i + 1}",
                    Location = new Point(100, yOffset - 3),
                    Width = 150,
                    Text = $"Player {i + 1}"  
                };
                panelPlayerNames.Controls.Add(lbl);
                panelPlayerNames.Controls.Add(txt);
                yOffset += 35;
            }

            panelPlayerNames.Height = yOffset + 10;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            List<string> playerNames = new List<string>();
            for (int i = 0; i < (int)numPlayers.Value; i++)
            {
                TextBox txt = panelPlayerNames.Controls.Find($"txtPlayer{i + 1}", true).FirstOrDefault() as TextBox;
                string name = txt != null ? txt.Text.Trim() : $"Player {i + 1}";
                if (string.IsNullOrEmpty(name)) name = $"Player {i + 1}";
                playerNames.Add(name);
            }

            GameForm gameForm = new GameForm(playerNames);
            gameForm.Show();
            this.Hide();
        }
    }
}
