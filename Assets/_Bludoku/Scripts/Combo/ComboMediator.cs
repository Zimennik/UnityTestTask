using _Bludoku.Scripts.Boards;
using UnityEngine;

namespace _Bludoku.Scripts.Combo
{
    public class ComboMediator : MonoBehaviour
    {
        [SerializeField] private Board board;
        [SerializeField] private ComboView comboView;
        [SerializeField] private ComboPopupView popupView;

        public ComboSystem Combo { get; } = new();

        private void Awake()
        {
            ComboSaveLoad.Load(Combo);
            board.OnFigurePlaced += FigurePlaced;
            Combo.OnComboIncreased += ComboIncreased;
        }

        private void Start()
        {
            UpdateView();
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

        private void ComboIncreased(int level)
        {
            comboView.PlayLevelUp();
            popupView.Show(level);
        }

        private void ApplyState()
        {
            UpdateView();
            ComboSaveLoad.Save(Combo);
        }

        private void UpdateView()
        {
            comboView.SetState(Combo.IsActive, Combo.Level, Combo.MovesLeft);
        }
    }
}
