using System.Collections.Generic;

namespace Characters.Models
{
    public class HeroModel : BaseCharacterModel
    {
        public HeroType HeroType;
        public List<string> Equipment = new List<string>();
        public List<string> Inventory = new List<string>();
    }
}