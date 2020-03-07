using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLeague.Models
{
    public class Season
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual League League {get; set;}

        public virtual IList<Team> Teams { get; set; }

        public SeasonStatus Status { get; set; }

    }

    public enum SeasonStatus
    {
        [Display(Name = "Not Started")]
        Not_Started,
        [Display(Name = "Ongoing")]
        Ongoing,
        [Display(Name = "Finished")]
        Finished
    }
}
