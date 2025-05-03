using System;
using System.Collections.Generic;
using UI;

namespace Resources
{
    public class AwardManager
    {
        private readonly Dictionary<int, string> _awards;
        private readonly int _awardScore = 1000; // todo: ver uma maneira melhor de definir isso

        public AwardManager(Dictionary<int, string> awards)
        {
            _awards = awards;
        }

        public void SetRandomAward()
        {
            if (_awards.Count > 0)
            {
                var randomAward = GetRandomEntry(_awards);

                HudControl.StaticHudControl.ShowAward(randomAward.Value, _awardScore);
            }
        }

        private static KeyValuePair<int, string> GetRandomEntry(Dictionary<int, string> dict)
        {
            var keys = new List<int>(dict.Keys);

            var random = new Random();
            var randomIndex = random.Next(keys.Count);

            var randomKey = keys[randomIndex];
            var value = dict[randomKey];

            dict.Remove(randomKey);

            return new KeyValuePair<int, string>(randomKey, value);
        }
    }
}