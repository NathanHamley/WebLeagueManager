using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLeague.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Season> Seasons { get; set; }

    }
}
