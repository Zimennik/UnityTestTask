using _Bludoku.Scripts.Boards;
using _Bludoku.Scripts.Combo;
using UnityEngine;

namespace _Bludoku.Scripts.Score
{
    public class ScoreMediator : MonoBehaviour
    {
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private Board board;
        [SerializeField] private ComboMediator comboMediator;

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
            ScoreSystem.AddClearScore(result.ClearedCount);
            scoreView.UpdateScore();
        }

        private void ComboIncreased(int level)
        {
            ScoreSystem.AddComboBonus(level);
            scoreView.UpdateScore();
        }
    }
}
