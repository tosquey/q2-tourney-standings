using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace q2_tourney_standings
{
    class Program
    {
        static void Main(string[] args)
        {
            Tourney tourney = new Tourney(@"/Users/lmatto1/temp/bracket.json");

            //getting maps pre-event
            foreach (var match in tourney.RandomizeMaps())
            {
                Console.WriteLine(match);
            }

            //getting final standings post-event
            // foreach (var rank in tourney.GetRank().OrderBy(a => a.Rank))
            // {
            //     Console.WriteLine(string.Format("{0}\t{1}\t{2}", rank.Rank, rank.Player.Nick, rank.Score));
            // }
        }
    }
}
