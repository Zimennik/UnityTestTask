using System.Linq;
using UnityEngine;

namespace _Bludoku.Scripts.Analytics
{
    public class DebugLogAnalyticsProvider : IAnalyticsProvider
    {
        public void Track(AnalyticsEvent analyticsEvent)
        {
            var parameters = string.Join(", ",
                analyticsEvent.Parameters.Select(parameter => $"{parameter.Key}: {parameter.Value}"));
            Debug.Log($"[Analytics] {analyticsEvent.Name} {{ {parameters} }}");
        }
    }
}