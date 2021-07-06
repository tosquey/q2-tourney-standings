using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace q2_tourney_standings
{
    public class Match
    {
        public Match()
        {
            Players = new List<Player>();
        }
        public string Identifier { get; set; }
        public string RoundName { get; set; }
        public List<Player> Players { get; set; }
        public int? WinnerId { get; set; }
        public int? LoserId { get; set; }

        public Player Winner
        {
            get { return Players.Where(a => a.Id == WinnerId).FirstOrDefault(); }
        }
    }
}