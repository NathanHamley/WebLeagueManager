using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLeague.Models
{
    public class Matchday
    {
        public virtual int Id { get; set; }

        public virtual int Number { get; set; }
        public virtual IList<Match> Matches { get; set; }

        public Matchday()
        {
        }

        public Matchday(int number, IList<Match> matches)
        {
            Number = number;
            Matches = matches;
        }
    }
}
