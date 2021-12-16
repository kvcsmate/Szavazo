using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Persistence
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Szavazás")]
        public Poll Poll { get; set; }

        [Required]
        [DisplayName("Válasz")]
        public string Text { get; set; }
    }
}
