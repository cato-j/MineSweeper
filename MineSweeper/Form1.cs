namespace MineSweeper
{
    public partial class Form1 : Form
    {
        private bool[,] isMine = new bool[,] { };
        private Random rdm = new();
        private List<string> clickedMines = [];
        private Dictionary<string, Button> buttons = new Dictionary<string, Button>();
        private bool gameOver = false;

        public Form1()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            isMine = new bool[this.Width / 25, this.Height / 25];

            for (int x = 0; x * 25 < this.Width - 25; x++)
            {
                for (int y = 0; y * 25 < this.Height - 25; y++)
                {
                    isMine[x, y] = rdm.Next(0, 10) == 0 ? true : false;
                    Button panel = new()
                    {
                        Size = new Size(25, 25),
                        Location = new(x * 25, y * 25),
                        Name = $"Mine-{x}-{y}"
                    };
                    panel.Click += Panel_Click;
                    Controls.Add(panel);
                    buttons.Add($"{x}-{y}", panel);
                }
            }
        }

        private void Panel_Click(object? sender, EventArgs e)
        {
            if (sender is not Button button) return;
            if (clickedMines.Contains(button.Name)) return;
            if (gameOver) return;

            int x = button.Location.X / 25;
            int y = button.Location.Y / 25;

            if (CheckMine(x,y))
            {
                GameOver();
            } else
            {
                button.BackColor = Color.Green;
            }

            int mineCount = 0;
            List<string> surroundingMines = [];

            for (int cx = -1; cx <= 1; cx++)
            {
                for ( int cy = -1; cy <= 1; cy++)
                {
                    if (cx == 0 && cy == 0) continue;
                    try
                    {
                        if (CheckMine(x + cx, y + cy))
                            mineCount++;
                    }
                    catch { }
                    surroundingMines.Add($"{x + cx}-{y + cy}");
                }
            }

            button.Text = mineCount.ToString();

            clickedMines.Add(button.Name);

            if (mineCount == 0)
            {
                foreach (string key in surroundingMines)
                {
                    if (buttons.TryGetValue(key, out Button? mine))
                        if (mine != null)
                        {
                            Panel_Click(mine, e);
                        }
                }
            }
        }

        private void GameOver()
        {
            foreach (string key in buttons.Keys)
            {
                Button button = buttons[key];

                int x = button.Location.X / 25;
                int y = button.Location.Y / 25;

                if (CheckMine(x, y))
                {
                    button.BackColor = Color.Red;
                }
            }

            gameOver = true;
        }

        private bool CheckMine(int x, int y)
        {

            return isMine[x, y];
        }
    }
}
