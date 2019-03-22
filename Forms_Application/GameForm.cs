using System.Drawing;
using System.Windows.Forms;
using Library;

namespace Forms_Application
{
    public partial class GameForm : Form
    {
        Graphics graphics, g;
        Bitmap background;
        FormsController controller;
        IGameCore core;

        public GameForm()
        {
            InitializeComponent();

            background = new Bitmap(396, 600);
            graphics = CreateGraphics();
            g = Graphics.FromImage(background);

            core = new GameCore();
            controller = new FormsController(core);
        }

        private void Repaint()
        {
            UpdateGame();
            Draw(g);
            graphics.DrawImage(background, new Point(0, 0));
        }

        public void Draw(Graphics g)
        {
            g.Clear(Color.FromName("#FAF8EF"));
            controller.Draw(g);
        }

        public void UpdateGame()
        {
            controller.Update();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                controller.MoveDown();
            }
            else if (e.KeyCode == Keys.Up)
            {
                controller.MoveUp();
            }
            else if (e.KeyCode == Keys.Left)
            {
                controller.MoveLeft();
            }
            else if (e.KeyCode == Keys.Right)
            {
                controller.MoveRight();
            }
            Repaint();
        }

        private void GameForm_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.X > 161) && (e.X < 161 + 100) && (e.Y > 96) && (e.Y < 96 + 38))
            {
                controller.StartNewGame();
                Repaint();
            }
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            Repaint();
        }

       
    }
}
