using UnityEngine;

namespace _Bludoku.Scripts.Score
{
    public static class ScoreSystem
    {
        private static int _score;
        private static int _highScore;

        private const string ScoreKey = "CurrentScore";
        private const string HighScoreKey = "HighScore";
        private const int ScorePerCell = 1;
        private const int ComboBonusPerLevel = 2;

        public static int Score => _score;
        public static int HighScore => _highScore;

        public static void LoadScore()
        {
            _score = PlayerPrefs.GetInt(ScoreKey, 0);
            _highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        }

        public static int AddClearScore(int clearedCells)
        {
            var points = clearedCells * ScorePerCell;
            AddScore(points);
            return points;
        }

        public static int AddComboBonus(int comboLevel)
        {
            var points = comboLevel * ComboBonusPerLevel;
            AddScore(points);
            return points;
        }
        
        public static void AddScore(int score)
        {
            _score = Score + score;
            if (HighScore < Score)
            {
                _highScore = Score;
            }
            
            SaveScore();
        }

        public static void ResetScore()
        {
            _score = 0;
            SaveScore();
        }

        private static void SaveScore()
        {
            PlayerPrefs.SetInt(ScoreKey, Score);
            PlayerPrefs.SetInt(HighScoreKey, HighScore);
            PlayerPrefs.Save();
        }
    }
}