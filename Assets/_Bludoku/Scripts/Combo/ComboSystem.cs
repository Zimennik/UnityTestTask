using System;

namespace _Bludoku.Scripts.Combo
{
    public class ComboSystem
    {
        public event Action<int> OnComboIncreased;
        public event Action<int> OnComboEnded;

        public int Level { get; private set; }
        public int MovesWithoutClear { get; private set; }
        public int MovesLeft => MovesToBreak - MovesWithoutClear;
        public bool IsActive => Level >= ActivationLevel;

        private const int ActivationLevel = 2;
        private const int MovesToBreak = 3;

        public void RegisterMove(bool hasClears)
        {
            if (hasClears)
            {
                MovesWithoutClear = 0;
                Level++;

                if (IsActive)
                    OnComboIncreased?.Invoke(Level);

                return;
            }

            MovesWithoutClear++;

            if (MovesWithoutClear >= MovesToBreak)
                Reset();
        }

        public void Reset()
        {
            var endedLevel = Level;
            var wasActive = IsActive;

            Level = 0;
            MovesWithoutClear = 0;

            if (wasActive)
                OnComboEnded?.Invoke(endedLevel);
        }

        public void Restore(int level, int movesWithoutClear)
        {
            Level = level;
            MovesWithoutClear = movesWithoutClear;
        }
    }
}