using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace q2_tourney_standings
{
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
}