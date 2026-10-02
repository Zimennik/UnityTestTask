namespace _Bludoku.Scripts.Analytics
{
    public static class AnalyticsEvents
    {
        private const string BonusReceived = "bonus_received";

        private const string FigureIdKey = "figure_id";
        private const string LevelKey = "level";
        private const string PowerUpKey = "power_up";
        private const string TypeKey = "type";

        public static AnalyticsEvent FigurePicked(int figureId)
        {
            return new AnalyticsEvent("figure_picked")
                .With(FigureIdKey, figureId);
        }

        public static AnalyticsEvent FigurePlaced(int figureId, int clearedCells, int clearedSegments)
        {
            return new AnalyticsEvent("figure_placed")
                .With(FigureIdKey, figureId)
                .With("cleared_cells", clearedCells)
                .With("cleared_segments", clearedSegments);
        }

        public static AnalyticsEvent FigureReturned(int figureId)
        {
            return new AnalyticsEvent("figure_returned")
                .With(FigureIdKey, figureId);
        }

        public static AnalyticsEvent ComboLevelUp(int level)
        {
            return new AnalyticsEvent("combo_level_up")
                .With(LevelKey, level);
        }

        public static AnalyticsEvent ComboEnded(int level)
        {
            return new AnalyticsEvent("combo_ended")
                .With(LevelKey, level);
        }

        public static AnalyticsEvent ComboPointsReceived(int amount, int comboLevel)
        {
            return new AnalyticsEvent(BonusReceived)
                .With(TypeKey, "combo_points")
                .With("amount", amount)
                .With("combo_level", comboLevel);
        }

        public static AnalyticsEvent PowerUpChargeReceived(string powerUpId)
        {
            return new AnalyticsEvent(BonusReceived)
                .With(TypeKey, "power_up_charge")
                .With(PowerUpKey, powerUpId);
        }

        public static AnalyticsEvent PowerUpUsed(string powerUpId)
        {
            return new AnalyticsEvent("power_up_used")
                .With(PowerUpKey, powerUpId);
        }
    }
}
