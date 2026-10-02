using _Bludoku.Scripts.Core;

namespace _Bludoku.Scripts.PowerUps
{
    public class PowerUpContext
    {
        public FiguresController Figures { get; }

        public PowerUpContext(FiguresController figures)
        {
            Figures = figures;
        }
    }
}
