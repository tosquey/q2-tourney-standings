using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace q2_tourney_standings
{
    public class PlayerRank
    {
        public Player Player { get; set; }
        public List<Match> Matches { get; set; }
        public int Rank { get; set; }
        public double Score { get; set; }
    }
}