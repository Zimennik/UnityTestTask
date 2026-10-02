using System;

namespace _Bludoku.Scripts.PowerUps
{
    public class PowerUpCharges
    {
        public event Action OnChanged;
        public event Action OnRecharged;

        public int MaxCharges { get; }
        public int CooldownMoves { get; }
        public int Charges { get; private set; }
        public int CooldownLeft { get; private set; }
        public bool IsReady => Charges > 0;

        public PowerUpCharges(int maxCharges, int cooldownMoves)
        {
            MaxCharges = maxCharges;
            CooldownMoves = cooldownMoves;
            Charges = maxCharges;
        }

        public bool TryUse()
        {
            if (!IsReady)
                return false;

            Charges--;

            if (CooldownLeft == 0)
                CooldownLeft = CooldownMoves;

            OnChanged?.Invoke();
            return true;
        }

        public void RegisterMove()
        {
            if (CooldownLeft == 0)
                return;

            CooldownLeft--;

            if (CooldownLeft == 0)
            {
                Charges++;

                if (Charges < MaxCharges)
                    CooldownLeft = CooldownMoves;

                OnRecharged?.Invoke();
            }

            OnChanged?.Invoke();
        }

        public void Reset()
        {
            Charges = MaxCharges;
            CooldownLeft = 0;
            OnChanged?.Invoke();
        }

        public void Restore(int charges, int cooldownLeft)
        {
            Charges = Math.Min(charges, MaxCharges);
            CooldownLeft = Math.Min(cooldownLeft, CooldownMoves);
        }
    }
}
