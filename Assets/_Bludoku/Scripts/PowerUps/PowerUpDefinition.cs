using UnityEngine;

namespace _Bludoku.Scripts.PowerUps
{
    public abstract class PowerUpDefinition : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxCharges = 1;
        [SerializeField] private int cooldownMoves = 30;

        public string Id => id;
        public Sprite Icon => icon;
        public int MaxCharges => maxCharges;
        public int CooldownMoves => cooldownMoves;

        public abstract void Apply(PowerUpContext context);
    }
}
