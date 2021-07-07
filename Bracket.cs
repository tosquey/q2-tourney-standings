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
}