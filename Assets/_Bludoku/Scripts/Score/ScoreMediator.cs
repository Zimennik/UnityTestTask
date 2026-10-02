using System;
using _Bludoku.Scripts.Boards;
using _Bludoku.Scripts.Combo;
using UnityEngine;

namespace _Bludoku.Scripts.Score
{
    public class ScoreMediator : MonoBehaviour
    {
        public event Action<int> OnComboBonusAwarded;

        [SerializeField] private ScoreView scoreView;
        [SerializeField] private Board board;
        [SerializeField] private ComboMediator comboMediator;
        [SerializeField] private ScoreGainView gainView;

        private void Awake()
        {
            board.OnFigurePlaced += FigurePlaced;
            comboMediator.Combo.OnComboIncreased += ComboIncreased;
        }

        private void Start()
        {
            ScoreSystem.LoadScore();
            scoreView.UpdateScore(false);
        }

        private void OnDestroy()
        {
            board.OnFigurePlaced -= FigurePlaced;
            comboMediator.Combo.OnComboIncreased -= ComboIncreased;
        }

        public void ResetScore()
        {
            ScoreSystem.ResetScore();
            scoreView.UpdateScore(false);
        }

        private void FigurePlaced(ClearResult result)
        {
            gainView.AddPoints(ScoreSystem.AddClearScore(result.ClearedCount));
            scoreView.UpdateScore();
        }

        private void ComboIncreased(int level)
        {
            var points = ScoreSystem.AddComboBonus(level);
            gainView.AddPoints(points);
            scoreView.UpdateScore();

            OnComboBonusAwarded?.Invoke(points);
        }
    }
}
