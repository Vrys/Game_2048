using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    enum Direction
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight
    }

    public class GameCore : IGameCore
    {
        private const int BOARD_SIZE = 4;
        private const int SQUARE_FILL_VALUE = 2;

        private List<Square> squaresList;
        private Random rand = new Random();
        private bool twoSameNumbers = false;

        public int Score { get; private set; }
        public int BestScore { get; private set; }
       
        public GameCore()
        {
            InitData();
        }
       
        private void InitData()
        {
            squaresList = new List<Square>(BOARD_SIZE * BOARD_SIZE);
            int y = 0;

            for (int x = 0; x < BOARD_SIZE * BOARD_SIZE; x++)
            {
                if ((x > 0) && (x % BOARD_SIZE == 0))
                    y++;

                squaresList.Add(new Square { Xpos = y, Ypos = (x - (y * BOARD_SIZE)) });
            }

            FillOneSquare();
        }

        public void StartNewGame()
        {
            InitData();
            Score = 0;
        }

        public void CheckBestScore(int score)
        {
            if (score > BestScore)
                BestScore = score;
        }

        public ISquare[,] GetArray(bool startNewGame)
        {
            var newArray = new ISquare[BOARD_SIZE, BOARD_SIZE];

            if (startNewGame)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        newArray[i, j] = new Square();
                    }
                }
                return newArray;
            }

            foreach (var item in squaresList)
            {
                newArray[item.Ypos, item.Xpos] = new Square { Xpos = item.Xpos, Ypos = item.Ypos, Value = item.Value };
            }
            return newArray;
        }

        private bool HasEqualInVector(Func<Square, int> check)
        {
            for (int c = 0; c < BOARD_SIZE; c++)
            {
                var listData = new List<Square>(squaresList.Where(x => check(x) == c));
                for (int i = 0; i < BOARD_SIZE - 1; i++)
                    if (listData[i].Value == listData[i + 1].Value)
                        return true;
            }
            return false;
        }

        private void Step(Direction dir, Func<Square, int> check)
        {
            for (int c = 0; c < BOARD_SIZE; c++)
            {
                var dataList = new List<Square>(squaresList.Where(x => check(x) == c));

                for (int i = 0; i < BOARD_SIZE - 1; i++)
                    if (dir == Direction.MoveLeft || dir == Direction.MoveUp)
                        StepUp(dataList);
                    else
                        StepDown(dataList);
                twoSameNumbers = false;

            }
            FillOneSquare();
        }

        private void StepDown(List<Square> dataList)
        {
            for (int y = BOARD_SIZE - 1; y > 0; y--)
            {
                if (dataList[y].Value == dataList[y - 1].Value || dataList[y].Value == 0)
                {
                    if ((dataList[y].Value == dataList[y - 1].Value) && (dataList[y].Value != 0 && dataList[y - 1].Value != 0))
                    {
                        if (twoSameNumbers)
                            continue;
                        twoSameNumbers = true;
                    }

                    if (dataList[y].Value == dataList[y - 1].Value)
                        Score += dataList[y].Value;
                    dataList[y].Value = dataList[y].Value + dataList[y - 1].Value;
                    dataList[y - 1].Value = 0;
                }
            }
        }

        private void StepUp(List<Square> dataList)
        {
            for (int y = 0; y < BOARD_SIZE - 1; y++)
            {
                if (dataList[y].Value == dataList[y + 1].Value || dataList[y].Value == 0)
                {
                    if ((dataList[y].Value == dataList[y + 1].Value) && (dataList[y].Value != 0 && dataList[y + 1].Value != 0))
                    {
                        if (twoSameNumbers)
                            continue;
                        twoSameNumbers = true;
                    }

                    if (dataList[y].Value == dataList[y + 1].Value)
                        Score += dataList[y].Value;
                    dataList[y].Value = dataList[y].Value + dataList[y + 1].Value;
                    dataList[y + 1].Value = 0;
                }
            }
        }

        private bool FillOneSquare()
        {
            var emptySquares = GetFreeSquares();
            int emptyCount = emptySquares.Count();

            if (emptyCount == 0)
                return false;

            int squareNumber = rand.Next(emptyCount);
            var square = emptySquares.Skip(squareNumber).First();
            square.Value = SQUARE_FILL_VALUE;

            return true;
        }

        public bool NextStepIsAvailable()
        {
            return HasEqualInVector(x => x.Ypos) || HasEqualInVector(x => x.Xpos) || GetFreeSquares().Any();
        }

        IEnumerable<Square> GetFreeSquares()
        {
            return squaresList.Where(square => square.Value == 0);
        }

        public void MoveDown()
        {
            Step(Direction.MoveDown, square => square.Ypos);
        }

        public void MoveLeft()
        {
            Step(Direction.MoveLeft, square => square.Xpos);
        }

        public void MoveRight()
        {
            Step(Direction.MoveRight, square => square.Xpos);
        }

        public void MoveUp()
        {
            Step(Direction.MoveUp, square => square.Ypos);
        }


    }
}
