using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadfestBot
{
    public class PlayerData
    {
        public ulong Id { get; set; }
        public string Name { get; set; }

        public int PersonalScore {  get; set; }

        public PlayerData(ulong ID, string name)
        {
            Id = ID;
            Name = name;
        }
    }
}
