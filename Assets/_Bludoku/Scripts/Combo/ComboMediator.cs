using _Bludoku.Scripts.Boards;
using _Bludoku.Scripts.Score;
using UnityEngine;

namespace _Bludoku.Scripts.Combo
{
    public class ComboMediator : MonoBehaviour
    {
        [SerializeField] private Board board;
        [SerializeField] private ScoreBoosterView boosterView;

        public ComboSystem Combo { get; } = new();

        private void Awake()
        {
            ComboSaveLoad.Load(Combo);
            board.OnFigurePlaced += FigurePlaced;
        }

        private void Start()
        {
            boosterView.SetBoosterEnabled(Combo.IsActive);
        }

        private void OnDestroy()
        {
            board.OnFigurePlaced -= FigurePlaced;
        }

        public void ResetCombo()
        {
            Combo.Reset();
            ApplyState();
        }

        private void FigurePlaced(ClearResult result)
        {
            Combo.RegisterMove(result.ClearedCount > 0);
            ApplyState();
        }

        private void ApplyState()
        {
            boosterView.SetBoosterEnabled(Combo.IsActive);
            ComboSaveLoad.Save(Combo);
        }
    }
}
