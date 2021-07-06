using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace q2_tourney_standings
{
    public class Tourney
    {
        public static List<PlayerRank> GetRank(List<Match> matches)
        {
            var playerRanks = new List<PlayerRank>();
            var players = matches.SelectMany(a => a.Players).Distinct();

            foreach (var player in players)
            {
                var rank = new PlayerRank();
                rank.Player = player;
                rank.Matches = matches.Where(b => b.Players.Contains(player)).ToList();
                rank.Score = GetScore(rank);
                rank.Score += (double)((double)1 / (double)player.Seed);

                playerRanks.Add(rank);
            }

            int index = 0;
            foreach (var rank in playerRanks.OrderByDescending(a => a.Score))
            {
                index++;
                rank.Rank = index;
            }

            return playerRanks;
        }

        static double GetScore(PlayerRank rank)
        {
            var maxWinnersRound = rank.Matches.Where(a => a.RoundName.Contains("Winners Round")).Select(b => Int32.Parse(b.RoundName.Last().ToString())).Max();
            var maxLosersRound = rank.Matches.Where(a => a.RoundName.Contains("Losers Round")).Select(b => Int32.Parse(b.RoundName.Last().ToString())).Max();

            var winnerRounds = rank.Matches.Where(a => a.RoundName.Contains("Winners Round")).Select(b => b.RoundName.Last().ToString()).ToList();
            var loserRounds = rank.Matches.Where(a => a.RoundName.Contains("Losers Round")).Select(b => b.RoundName.Last().ToString()).ToList();

            double score = 0;

            //tourney winner
            score += rank.Matches.Where(a => a.RoundName.Contains("Grand Final") && a.WinnerId == rank.Player.Id).Count() * 100000;
            //tourney finalists
            score += rank.Matches.Where(a => a.RoundName == "Grand Final").Count() * 50000;
            //going backwards from losers final on
            score += rank.Matches.Where(a => a.RoundName == "Losers Final").Count() * 10000;
            score += rank.Matches.Where(a => a.RoundName == "Losers Semi-Final").Count() * 5000;
            score += rank.Matches.Where(a => a.RoundName == "Losers Quarter-Final").Count() * 1000;
            //extra 10 points for getting as far away as possible on the losers bracket
            score += loserRounds.Count > 0 ? maxLosersRound == loserRounds.Select(a => Int32.Parse(a)).Max() ? 10 : 0 : 0;
            //extra 10 points for advancing to quarters/semis on the winners bracket
            score += rank.Matches.Where(a => a.RoundName.StartsWith("Winners") && !a.RoundName.Contains("Round")).Count() * 10;
            //additional points for each regular round on winners or losers
            score += winnerRounds.Count > 0 ? winnerRounds.Select(a => Int32.Parse(a)).Max() : 0;
            score += loserRounds.Count > 0 ? loserRounds.Select(a => Int32.Parse(a)).Max() : 0;
            //tiebreaker = seed
            //score += (double)((double)1 / (double)Player.Seed);

            return score;
        }
    }
}