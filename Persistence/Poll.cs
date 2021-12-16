using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistence
{
    public class Poll
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Kérdés")]
        public string Question { get; set; }

        [Required]
        [DisplayName("Szavazás kezdete")]
        public DateTime Start { get; set; }

        [Required]
        [DisplayName("Szavazás vége")]
        public DateTime End { get; set; }

        [DisplayName("Kiíró felhasználó")]
        public User Creator { get; set; }
        
        public ICollection<Answer> Options { get; set; }
    }

}
