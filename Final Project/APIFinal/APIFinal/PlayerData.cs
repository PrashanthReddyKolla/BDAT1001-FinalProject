using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIFinal
{
    public class PlayerData
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string Nationality { get; set; }
        public string FootballClub { get; set; }
        public string ClubNationality { get; set; }
        public string Role { get; set; }
        public int PerceivedRank { get; set; }
    }
}
