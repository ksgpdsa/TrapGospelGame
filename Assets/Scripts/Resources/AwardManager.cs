using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Resources
{
    public class AwardManager
    {
        private readonly Dictionary<int,string> _awards;
        private readonly HudControl _hudControl;

        public AwardManager(Dictionary<int, string> awards, HudControl hudControl)
        {
            _awards = awards;
            _hudControl = hudControl;
        }

        public void SetRandomAward()
        {
            if (_awards.Count > 0)
            {
                var randomAward = GetRandomEntry(_awards);
                
                _hudControl.ShowAward(randomAward.Value);
            }
        }

        private static KeyValuePair<int, string> GetRandomEntry(Dictionary<int, string> dict)
        {
            var keys = new List<int>(dict.Keys);
            
            var random = new System.Random();
            var randomIndex = random.Next(keys.Count);
            
            var randomKey = keys[randomIndex];
            var value = dict[randomKey];
            
            dict.Remove(randomKey);
            
            return new KeyValuePair<int, string>(randomKey, value);
        }
    }
}