namespace Library
{
    public interface IGameCore
    {
        int Score { get; }

        int BestScore { get; }

        void MoveUp();

        void MoveDown();

        void MoveLeft();

        void MoveRight();

        void StartNewGame();

        bool NextStepIsAvailable();

        void CheckBestScore(int bestScore);

        ISquare[,] GetArray(bool startNewGame);
    }
}
