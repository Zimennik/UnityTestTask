using _Bludoku.Scripts.Combo;
using _Bludoku.Scripts.Core;
using _Bludoku.Scripts.PowerUps;
using _Bludoku.Scripts.Score;
using UnityEngine;

namespace _Bludoku.Scripts.Analytics
{
    public class AnalyticsInstaller : MonoBehaviour
    {
        [SerializeField] private FiguresController figuresController;
        [SerializeField] private ComboMediator comboMediator;
        [SerializeField] private ScoreMediator scoreMediator;
        [SerializeField] private PowerUpMediator powerUpMediator;

        private GameplayAnalyticsTracker _tracker;

        private void Awake()
        {
            var analytics = new AnalyticsService(new DebugLogAnalyticsProvider());

            _tracker = new GameplayAnalyticsTracker(analytics, figuresController, comboMediator.Combo,
                scoreMediator, powerUpMediator);
        }

        private void OnDestroy()
        {
            _tracker?.Dispose();
        }
    }
}
