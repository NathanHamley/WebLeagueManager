
using System.ComponentModel.DataAnnotations;


namespace WebLeague.Models
{
    public class Match
    {
		public int Id { get; set; }
		[Display(Name = "Home")]
		public virtual Team HomeTeam { get; set; }
		[Display(Name = "Away")]
		public virtual Team AwayTeam { get; set; }

		[Display(Name = "")]
		public virtual int? HomeScore { get; set; }
		[Display(Name = "")]
		public virtual int? AwayScore { get; set; }

		public Match()
		{
		}

		public Match(Team homeTeam, Team awayTeam, int? homeScore, int? awayScore)
		{
			HomeTeam = homeTeam;
			AwayTeam = awayTeam;
			HomeScore = homeScore;
			AwayScore = awayScore;
		}
	}
}
