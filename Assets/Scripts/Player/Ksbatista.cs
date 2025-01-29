using System.Collections.Generic;

namespace Player
{
    public class Ksbatista : Player
    {
        private readonly Dictionary<int, string> awards = new Dictionary<int, string>()
        {
            {1, Library.Disc},
            {2, Library.Mouth},
        };

        private void Awake(){
            Awards = new Dictionary<int, string>(awards);

            base.Awake();
        }
    }
}