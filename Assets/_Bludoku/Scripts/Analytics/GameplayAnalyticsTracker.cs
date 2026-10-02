using System;
using _Bludoku.Scripts.Blocks;
using _Bludoku.Scripts.Boards;
using _Bludoku.Scripts.Combo;
using _Bludoku.Scripts.Core;
using _Bludoku.Scripts.PowerUps;
using _Bludoku.Scripts.Score;

namespace _Bludoku.Scripts.Analytics
{
    public class GameplayAnalyticsTracker : IDisposable
    {
        private readonly IAnalyticsService _analytics;
        private readonly FiguresController _figures;
        private readonly ComboSystem _combo;
        private readonly ScoreMediator _score;
        private readonly PowerUpMediator _powerUps;

        public GameplayAnalyticsTracker(IAnalyticsService analytics, FiguresController figures, ComboSystem combo,
            ScoreMediator score, PowerUpMediator powerUps)
        {
            _analytics = analytics;
            _figures = figures;
            _combo = combo;
            _score = score;
            _powerUps = powerUps;

            _figures.OnFigurePicked += FigurePicked;
            _figures.OnFigurePlaced += FigurePlaced;
            _figures.OnFigureReturned += FigureReturned;
            _combo.OnComboIncreased += ComboIncreased;
            _combo.OnComboEnded += ComboEnded;
            _score.OnComboBonusAwarded += ComboBonusAwarded;
            _powerUps.OnPowerUpUsed += PowerUpUsed;
            _powerUps.OnPowerUpRecharged += PowerUpRecharged;
        }

        public void Dispose()
        {
            _figures.OnFigurePicked -= FigurePicked;
            _figures.OnFigurePlaced -= FigurePlaced;
            _figures.OnFigureReturned -= FigureReturned;
            _combo.OnComboIncreased -= ComboIncreased;
            _combo.OnComboEnded -= ComboEnded;
            _score.OnComboBonusAwarded -= ComboBonusAwarded;
            _powerUps.OnPowerUpUsed -= PowerUpUsed;
            _powerUps.OnPowerUpRecharged -= PowerUpRecharged;
        }

        private void FigurePicked(Figure figure)
        {
            _analytics.Track(AnalyticsEvents.FigurePicked(figure.ID));
        }

        private void FigurePlaced(Figure figure, ClearResult result)
        {
            _analytics.Track(AnalyticsEvents.FigurePlaced(figure.ID, result.ClearedCount, result.ClearedSegmentsCount));
        }

        private void FigureReturned(Figure figure)
        {
            _analytics.Track(AnalyticsEvents.FigureReturned(figure.ID));
        }

        private void ComboIncreased(int level)
        {
            _analytics.Track(AnalyticsEvents.ComboLevelUp(level));
        }

        private void ComboEnded(int level)
        {
            _analytics.Track(AnalyticsEvents.ComboEnded(level));
        }

        private void ComboBonusAwarded(int points)
        {
            _analytics.Track(AnalyticsEvents.ComboPointsReceived(points, _combo.Level));
        }

        private void PowerUpUsed(string powerUpId)
        {
            _analytics.Track(AnalyticsEvents.PowerUpUsed(powerUpId));
        }

        private void PowerUpRecharged(string powerUpId)
        {
            _analytics.Track(AnalyticsEvents.PowerUpChargeReceived(powerUpId));
        }
    }
}
