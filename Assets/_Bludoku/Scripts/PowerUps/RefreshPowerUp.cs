using UnityEngine;

namespace _Bludoku.Scripts.PowerUps
{
    [CreateAssetMenu(fileName = "Refresh", menuName = "Bludoku/Power-ups/Refresh")]
    public class RefreshPowerUp : PowerUpDefinition
    {
        public override void Apply(PowerUpContext context)
        {
            context.Figures.RefreshFigures();
        }
    }
}
