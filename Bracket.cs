using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace q2_tourney_standings
{
    public static class Bracket
    {
        public static List<Match> GetMatches(string file)
        {
            List<Match> matches = new List<Match>();

            string contents = File.ReadAllText(file);
            var json = JObject.Parse(contents);

            var entrants = json["data"]["items"]["entities"]["entrants"].Select(a => new Player() {
                Id = a["id"].Value<int>(),
                Nick = a["mutations"]["players"].First.First["gamerTag"].Value<string>(),
                Seed = a["initialSeedNum"].Value<int>()
            });

            var nulo = new Player() { Nick = "TBD" };

            foreach (var node in json["data"]["items"]["entities"]["sets"])
            {
                if (node["entrant1PrereqType"].Value<string>() != "bye"
                    && node["entrant2PrereqType"].Value<string>() != "bye")
                {
                    Player player1, player2;
                    int player1Id, player2Id = 0;

                    if (int.TryParse(node["entrant1Id"].Value<string>(), out player1Id))
                    {
                        player1 = entrants.Where(a => a.Id == player1Id).FirstOrDefault();
                    }
                    else
                    {
                        player1 = nulo;
                    }

                    if (int.TryParse(node["entrant2Id"].Value<string>(), out player2Id))
                    {
                        player2 = entrants.Where(a => a.Id == player2Id).FirstOrDefault();
                    }
                    else
                    {
                        player2 = nulo;
                    }

                    int winnerId, loserId;
                    int.TryParse(node["winnerId"].Value<string>(), out winnerId);
                    int.TryParse(node["loserId"].Value<string>(), out loserId);

                    matches.Add(new Match()
                    {
                        Identifier = node["identifier"].Value<string>(),
                        RoundName = node["fullRoundText"].Value<string>(),
                        WinnerId = winnerId,
                        LoserId = loserId,
                        Players = { player1, player2 }
                    });
                }
            }

            return matches;
        }
    }

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

    public class Player : IEquatable<Player>
    {
        public bool Equals(Player other)
        {
            if (Object.ReferenceEquals(other, null)) return false;

            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            int hashId = Id.GetHashCode();
            int hashNick = Nick.GetHashCode();

            return hashId ^ hashNick;
        }

        public int Id { get; set; }
        public string Nick { get; set; }
        public int Seed { get; set; }
    }

    public class Rank
    {
        public Player Player { get; set; }

        public List<Match> Matches { get; set; }

        public int MaxRound { get; set; }

        public double Score()
        {
            //get
            {
                double score = 0;

                var winnerRounds = this.Matches.Where(a => a.RoundName.Contains("Winners Round")).Select(b => b.RoundName.Last().ToString()).ToList();
                var loserRounds = this.Matches.Where(a => a.RoundName.Contains("Losers Round")).Select(b => b.RoundName.Last().ToString()).ToList();

                score += this.Matches.Where(a => a.RoundName.Contains("Grand Final") && a.WinnerId == this.Player.Id).Count() * 100000;
                score += this.Matches.Where(a => a.RoundName == "Grand Final").Count() * 50000;
                score += this.Matches.Where(a => a.RoundName == "Losers Final").Count() * 10000;
                score += this.Matches.Where(a => a.RoundName == "Losers Semi-Final").Count() * 5000;
                score += this.Matches.Where(a => a.RoundName == "Losers Quarter-Final").Count() * 1000;
                score += loserRounds.Count > 0 ? MaxRound == loserRounds.Select(a => Int32.Parse(a)).Max() ? 10 : 0 : 0;
                score += this.Matches.Where(a => a.RoundName.StartsWith("Winners") && !a.RoundName.Contains("Round")).Count() * 10;
                score += winnerRounds.Count > 0 ? winnerRounds.Select(a => Int32.Parse(a)).Max() : 0;
                score += loserRounds.Count > 0 ? loserRounds.Select(a => Int32.Parse(a)).Max() : 0;
                score += (double)((double)1 / (double)Player.Seed);

                return score;
            }
        }

    }
}