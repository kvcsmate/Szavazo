using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DTO
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string Text { get; set; }

    }
}
