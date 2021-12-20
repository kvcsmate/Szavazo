using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DTO
{
    public class PollCreateRequest
    {
        public PollDto Poll { get; set; }

        public List<string> UserIds { get; set; }

        public List<AnswerDto>  Answers{ get; set; }
    }
}
