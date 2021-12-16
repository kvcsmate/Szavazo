using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persistence
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("szavazás")]
        public Poll Poll { get; set; }


        [DisplayName("válasz")]
        public Answer Answer { get; set; }
    }
}
