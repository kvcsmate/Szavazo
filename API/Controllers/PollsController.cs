using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PollsController : ControllerBase
    {
        private readonly SzavazoService _service;

        public PollsController(SzavazoService service)
        {
            _service = service;
        }
        // GET: api/<PollsController>
        [Authorize]
        [HttpGet]
        public IEnumerable<PollDto> Get()
        {
            return _service.GetPollsByCreator(GetCurrentUser());
        }
        [Authorize]
        [HttpGet("~/WHOAMI")]
        public string WhoAmI()
        {
            return GetCurrentUser().Email;
        }
        [Authorize]
        [HttpGet("~/CurrentUser")]
        public User CurrentUser()
        {
            return GetCurrentUser();
        }
        [Authorize]
        [HttpGet("GetPollBindingsByPollId/{pollId}")]
        public List<PollBindingDto> GetPollBindings(int pollId)
        {
            try
            {
                return _service.GetPollBindingDtos(pollId);
 
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Authorize]
        [HttpGet("GetUsers")]
        public List<User> GetUsers()
        {
            return _service.GetUsers();
        }

        // GET api/<PollsController>/5
        [Authorize]
        [HttpGet("{id}")]
        public PollDto Get(int id)
        {
            var poll =  _service.GetPollById(id);
            return new PollDto
            {
                Id = poll.Id,
                Creator = poll.Creator,
                End = poll.End,
                Start = poll.Start
            };
        }

        // PUT api/<PollsController>/5
        [Authorize]
        [HttpPut("CreatePoll")]
        public IActionResult Put(PollCreateRequest request)
        {
            if (request.Poll.End < DateTime.Now
                || request.Poll.Start < DateTime.Now
                || request.Poll.Start.AddMinutes(15) > request.Poll.End
                || request.UserIds.Count < 2
                || request.Answers.Count < 2
                || String.IsNullOrEmpty(request.Poll.Question)
                                )
            {
                return BadRequest();
            }
            if (_service.CreatePoll(request))
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }
        [Authorize]
        [HttpGet("~/GetAnswers/{pollId}")]
        public List<AnswerDto> GetAnswers(int pollId)
        {
            try
            {
                List<AnswerDto> answerDtos = new List<AnswerDto>();
                var answers = _service.GetAnswersByPollId(pollId);

                foreach (var answer in answers)
                {
                    answerDtos.Add(new AnswerDto
                    {
                        Id = answer.Id,
                        PollId = pollId,
                        Text = answer.Text
                    });
                }
                return answerDtos;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private User GetCurrentUser()
        {
            string CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _service.GetUser(CurrentUserId);
        }

        
    }
}
