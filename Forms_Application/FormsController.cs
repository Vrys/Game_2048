using System.Collections.Generic;
using System.Drawing;
using Library;

namespace Forms_Application
{
    public enum Image
    {
        Value_0 = 0,
        Value_2 = 2,
        Value_4 = 4,
        Value_8 = 8,
        Value_16 = 16,
        Value_32 = 32,
        Value_64 = 64,
        Value_128 = 128,
        Value_256 = 256,
        Value_512 = 512,
        Value_1024 = 1024,
        Value_2048 = 2048,
        Value_4096 = 4096,
        Value_8192 = 8192,
        Value_16384 = 16384,

        Score_icon,
        Newgame_button,
        Logo_board,
        Main_field,
        No_data
    }

    public class FormsController 
    {
        private List<Bitmap> images = new List<Bitmap>();
        private ISquare[,] board;
        private IGameCore gameCore;

        public FormsController(IGameCore core)
        {
            gameCore = core;

            images.Add(new Bitmap(@"../../images/value_0.png"));
            images.Add(new Bitmap(@"../../images/value_2.png"));
            images.Add(new Bitmap(@"../../images/value_4.png"));
            images.Add(new Bitmap(@"../../images/value_8.png"));
            images.Add(new Bitmap(@"../../images/value_16.png"));
            images.Add(new Bitmap(@"../../images/value_32.png"));
            images.Add(new Bitmap(@"../../images/value_64.png"));
            images.Add(new Bitmap(@"../../images/value_128.png"));
            images.Add(new Bitmap(@"../../images/value_256.png"));
            images.Add(new Bitmap(@"../../images/value_512.png"));
            images.Add(new Bitmap(@"../../images/value_1024.png"));
            images.Add(new Bitmap(@"../../images/value_2048.png"));
            images.Add(new Bitmap(@"../../images/value_4096.png"));
            images.Add(new Bitmap(@"../../images/value_8192.png"));
            images.Add(new Bitmap(@"../../images/value_16384.png"));
                            
            images.Add(new Bitmap(@"../../images/score_icon.png"));
            images.Add(new Bitmap(@"../../images/newgame_button.png"));
            images.Add(new Bitmap(@"../../images/logo_board.png"));
            images.Add(new Bitmap(@"../../images/main_field.png"));
            images.Add(new Bitmap(@"../../images/no_data.png"));
        }

        public void Update()
        {
            board = gameCore.GetArray(false);
        }

        public void FinishGame(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 246, 246, 246)), new Rectangle(21, 170, 322, 321));

            DrawTextTopCenter(g, 185, 280, "GAME OVER", GetFontByValue(32, false), new SolidBrush(Color.DimGray));

            gameCore.CheckBestScore(gameCore.Score);
        }

        public void Draw(Graphics g)
        {
            DrawObjects(g);

            if (!gameCore.NextStepIsAvailable())
                FinishGame(g);
        }

        public void StartNewGame()
        {
            gameCore.StartNewGame();
            board = gameCore.GetArray(true);
        }

        public void DrawObjects(Graphics g)
        {
            g.DrawImage(GetBitmapByImageName(Image.Logo_board), new Point(18, 18));
            g.DrawImage(GetBitmapByImageName(Image.Score_icon), new Point(145, 18));
            g.DrawImage(GetBitmapByImageName(Image.Score_icon), new Point(240, 18));
            g.DrawImage(GetBitmapByImageName(Image.Main_field), new Point(22, 170));
            g.DrawImage(GetBitmapByImageName(Image.Newgame_button), new Point(145, 95));


            DrawTextCenter(g, 78, 80, "2048", GetFontByValue(32, false), new SolidBrush(Color.White));
            DrawTextCenter(g, 188, 115, "NEW GAME", GetFontByValue(8, false), new SolidBrush(Color.White));
            DrawTextTopCenter(g, 188, 20, "SCORE", GetFontByValue(8, false), new SolidBrush(Color.White));
            DrawTextTopCenter(g, 283, 20, "BEST SCORE", GetFontByValue(8, false), new SolidBrush(Color.White));

            DrawTextTopCenter(g, 190, 30, gameCore.Score.ToString(), GetFontByValue(18, false), new SolidBrush(Color.White));
            DrawTextTopCenter(g, 285, 30, gameCore.BestScore.ToString(), GetFontByValue(18, false), new SolidBrush(Color.White));


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    g.DrawImage(GetBitmapByImageName((Image)board[i, j].Value), new Point(30 + 78 * i, 178 + 78 * j));
                    if (board[i, j].Value > 0)
                    {
                        DrawTextCenter(g, 65 + 78 * i, 215 + 78 * j, board[i, j].Value.ToString(),
                            GetFontByValue(board[i, j].Value, true),
                            (board[i, j].Value < 8 ? new SolidBrush(Color.DimGray) : new SolidBrush(Color.White)));
                    }
                }
            }
        }

        private Font GetFontByValue(int number, bool forSquare)
        {
            if (forSquare)
            {
                int i = 10;
                int numberOrder = 0;

                while (number > i / 10)
                {
                    i *= 10;
                    ++numberOrder;
                }

                var font = new Font("Sans Serif", 28, FontStyle.Bold);

                switch (numberOrder)
                {
                    case 1:
                    case 2:
                        break;
                    case 3:
                        font = new Font("Sans Serif", 24, FontStyle.Bold);
                        break;
                    case 4:
                        font = new Font("Sans Serif", 20, FontStyle.Bold);
                        break;
                    case 5:
                        font = new Font("Sans Serif", 16, FontStyle.Bold);
                        break;
                    default:
                        font = new Font("Sans Serif", 14, FontStyle.Bold);
                        break;
                }

                return font;
            }
            else
            {
                return new Font("Sans Serif", number, FontStyle.Bold);
            }
        }

        public void DrawTextCenter(Graphics g, int x, int y, string text, Font font, SolidBrush solidBrush)
        {
            var stringSize = g.MeasureString(text, font);
            g.DrawString(text, font, solidBrush, new PointF(x - stringSize.Width / 2 + 1, y - stringSize.Height / 2 + 1));
        }

        public void DrawTextTopCenter(Graphics g, int x, int y, string text, Font font, SolidBrush nSolidBrush)
        {
            var stringSize = g.MeasureString(text, font);
            g.DrawString(text, font, nSolidBrush, new PointF(x - stringSize.Width / 2, y));
        }

        private Bitmap GetBitmapByImageName(Image imageName)
        {
            switch (imageName)
            {
                case Image.Value_0:
                    return images[0];
                case Image.Value_2:
                    return images[1];
                case Image.Value_4:
                    return images[2];
                case Image.Value_8:
                    return images[3];
                case Image.Value_16:
                    return images[4];
                case Image.Value_32:
                    return images[5];
                case Image.Value_64:
                    return images[6];
                case Image.Value_128:
                    return images[7];
                case Image.Value_256:
                    return images[8];
                case Image.Value_512:
                    return images[9];
                case Image.Value_1024:
                    return images[10];
                case Image.Value_2048:
                    return images[11];
                case Image.Value_4096:
                    return images[12];
                case Image.Value_8192:
                    return images[13];
                case Image.Value_16384:
                    return images[14];
                case Image.Score_icon:
                    return images[15];
                case Image.Newgame_button:
                    return images[16];
                case Image.Logo_board:
                    return images[17];
                case Image.Main_field:
                    return images[18];
                default:
                    return images[19];
            }

        }

        public void MoveUp()
        {
            gameCore.MoveUp();
        }

        public void MoveDown()
        {
            gameCore.MoveDown();
        }

        public void MoveLeft()
        {
            gameCore.MoveLeft();
        }

        public void MoveRight()
        {
            gameCore.MoveRight();
        }
    }
}
