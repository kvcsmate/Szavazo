using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DTO
{
    public class PollBindingDto
    {
        public int Id { get; set; }
        
        public virtual User User { get; set; }
        
        public virtual int PollId { get; set; }
        
        public bool IsVoted { get; set; }
    }
}
