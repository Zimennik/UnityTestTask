using System.Collections.Generic;

namespace _Bludoku.Scripts.Analytics
{
    public class AnalyticsEvent
    {
        private readonly Dictionary<string, object> _parameters = new();

        public string Name { get; }
        public IReadOnlyDictionary<string, object> Parameters => _parameters;

        public AnalyticsEvent(string name)
        {
            Name = name;
        }

        public AnalyticsEvent With(string key, object value)
        {
            _parameters[key] = value;
            return this;
        }
    }
}
