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
            var matches = Bracket.GetMatches(@"e:\temp\bracket.json");
            var rank = Tourney.GetRank(matches);
            // var maxRound = matches.Where(a => a.RoundName.Contains("Losers Round")).Select(b => Int32.Parse(b.RoundName.Last().ToString())).Max();
            // List<Rank> ranking = new List<Rank>();
            // var players = matches.SelectMany(a => a.Players).Distinct();

            // foreach (var player in players)
            // {
            //     ranking.Add(new Rank() { Player = player, Matches = matches.Where(b => b.Players.Contains(player)).ToList(), MaxRound = maxRound });
            // }

            // int index = 0;
            // foreach (var rank in ranking.OrderByDescending(a => a.Score))
            // {
            //     index++;
            //     var test = rank.Matches.Where(a => a.RoundName.Contains("Winners Round")).Select(b => b.RoundName.Last());
            //     Console.WriteLine(string.Format("{0}\t{1}\t{2}", index, rank.Player.Nick, rank.Score));
            // }
        }
    }
}
