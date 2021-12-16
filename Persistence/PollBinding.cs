using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistence
{
    public class PollBinding
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public virtual Poll Poll { get; set; }

        [DisplayName("szavazott-e")]
        public bool IsVoted { get; set; }

    }
}
