using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Bludoku.Scripts.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IReadOnlyList<IAnalyticsProvider> _providers;

        public AnalyticsService(params IAnalyticsProvider[] providers)
        {
            _providers = providers;
        }

        public void Track(AnalyticsEvent analyticsEvent)
        {
            foreach (var provider in _providers)
            {
                try
                {
                    provider.Track(analyticsEvent);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }
    }
}
