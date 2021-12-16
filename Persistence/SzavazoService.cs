using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistence
{
    public class SzavazoService
    {
        private readonly SzavazoDbContext _context;
        public SzavazoService(SzavazoDbContext context)
        {
            _context = context;
        }
        public List<PollBinding> GetPollBindings(User user)
        {
            return _context.PollBindings.Where(p => p.User == user).ToList();
        }

        public User GetUser(string currentUserId)
        {
            return _context.Users.First(u => u.Id == currentUserId);
        }

        public List<Poll> GetPollsByCurrentUser(User user)
        {
            return _context.PollBindings.Where(p => p.User == user && !p.IsVoted && p.Poll.End > DateTime.Now).Select(p => p.Poll).OrderBy(p => p.End).ToList();

        }

        public void SelectAnswer(int? id, User user)
        {
            var poll = _context.Answers.Include(p => p.Poll).First(a => a.Id == id).Poll;
            Vote voting = new Vote
            {
                Answer = _context.Answers.Find(id),
                Poll = poll
            };
            var pollBinding = _context.PollBindings.First(p => p.User == user && p.Poll.Id == poll.Id);
            pollBinding.IsVoted = true;
            _context.Update(pollBinding);
            _context.Votes.Add(voting);
            _context.SaveChanges();
        }

        public List<Vote> GetVotesByPollId(int? id)
        {
            return _context.Votes.Where(p => p.Poll.Id == id).Include(a => a.Answer).ToList();
        }

        public List<Poll> GetAnsweredPolls(User user)
        {
            /*var activepolls = _context.Polls
                .Except(_context.PollBindings.Where(p => !p.IsVoted).Select(p => p.Poll).Distinct()).ToList();*/

            return _context.PollBindings.Where(p => p.User == user && (p.IsVoted || p.Poll.End < DateTime.Now)).Select(p => p.Poll).ToList();
        }

        public List<Poll> GetClosedPolls()
        {
            return _context.PollBindings.Where(p => p.Poll.End < DateTime.Now).Select(p => p.Poll).Distinct()
            .Union
            (_context.PollBindings.Select(p=> p.Poll).Distinct().
            Except(_context.PollBindings.Where(p => !p.IsVoted).Select(p=> p.Poll).Distinct()))
            .Distinct()
            .ToList();
        }

        public List<Answer> GetAnswersByPollId(int? id)
        {
            return _context.Answers.Include(p => p.Poll).Where(a => a.Poll.Id == id).ToList();
        }

        public double GetVotePercent(int? id)
        {
            double count1 = _context.PollBindings.Where(a => a.Poll.Id == id && a.IsVoted).ToList().Count;
                double count2 = _context.PollBindings.Where(a => a.Poll.Id == id).ToList().Count;
            double percent = count1 / count2;
                
            return percent * 100;
        }

        public Poll GetPollById(int? id)
        {
            return _context.Polls.First(p => p.Id==id);
        }

        public void OnNewUser(User user)
        {

            Random rand = new Random();
            foreach (var poll in _context.Polls)
            {
                if (rand.Next(0,2) == 1)
                {
                    var binding = new PollBinding
                    {
                        IsVoted = poll.Id == 4,
                        Poll = poll,
                        User = user
                    };
                    _context.PollBindings.Add(binding);
                }
            }
            _context.SaveChanges();
        }
    }
}
