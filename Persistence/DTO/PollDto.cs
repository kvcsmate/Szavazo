using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persistence.DTO
{
    public class PollDto
    {
        [Key]
        public int Id { get; set; }


        [DisplayName("Kérdés")]
        public string Question { get; set; }

        [DisplayName("Szavazás kezdete")]
        public DateTime Start { get; set; }

        [DisplayName("Szavazás vége")]
        public DateTime End { get; set; }

        [DisplayName("Kiíró felhasználó")]
        public User Creator { get; set; }

    }
}
