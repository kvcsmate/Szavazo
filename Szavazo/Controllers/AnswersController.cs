using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Szavazo.Controllers
{
    [Authorize]
    public class AnswersController : Controller
    {
        private readonly SzavazoDbContext _context;
        private readonly SzavazoService service;

        public AnswersController(SzavazoDbContext context,SzavazoService Service)
        {
            _context = context;
            service = Service;
        }

        // GET: Answers/5

        public IActionResult Index(int? id)
        {

            try
            {
                return View(service.GetAnswersByPollId(id));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }


        }
        public IActionResult Select(int? id)
        {
            try
            {
                service.SelectAnswer(id, GetCurrentUser());
                return RedirectToAction("Index", "Polls");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { err = e.Message });
            }

        }
        public IActionResult Closed(int? id)
        {
            try
            {
                List<Vote>  votes=  service.GetVotesByPollId(id);
                List<Answer> answers = votes.Select(a => a.Answer).Distinct().ToList();
                List<AnswerStatistics> answerStatistics = new List<AnswerStatistics>();
                foreach (var answer in answers)
                {
                    int answercount = votes.Where(a => a.Answer == answer).ToList().Count;
                    double percent = Convert.ToDouble(answercount) / Convert.ToDouble( votes.Count);
                    answerStatistics.Add (new AnswerStatistics
                    {
                        NumberOfVotes =  answercount,
                        Text = answer.Text,
                        Percent = percent*100
                    });
                }
                ViewData["Question"] = service.GetPollById(id).Question;
                ViewData["PollPercent"] = service.GetVotePercent(id).ToString();
                ViewData["VoteCount"] = votes.Count;
                return View(answerStatistics);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { err = e.Message });
            }
        }
        /*
        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create/Id
        public IActionResult Create(int? id)
        {
            try
            {
                Answer answer = new Answer
                {
                    Poll = _context.Polls.First(p => p.Id == id),
                    Text = ""
                };
                _context.Add(answer);
                _context.SaveChanges();
                int returnid = answer.Id;
                return RedirectToAction("Edit", "Answers", new { id = returnid });
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { err = e.Message });
            }
           
        }

        // POST: Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(answer);
        }

        // GET: Answers/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = _context.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Text")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var edited = _context.Answers.Include(p => p.Poll).First(a => a.Id == id);
                    edited.Text = answer.Text;

                    //var poll = _context.Polls.First(p=> p.Id == id);
                    User currentuser = GetCurrentUser();
                    var pollbinding = _context.PollBindings.First(p => p.User.Id == currentuser.Id && edited.Poll.Id  == p.Poll.Id);
                    pollbinding.IsVoted = true;
                    _context.Update(edited);
                    _context.Update(pollbinding);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Polls");
            }
            return View(answer);
        }

        // GET: Answers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }*/
        private User GetCurrentUser()
        {
            string CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return service.GetUser(CurrentUserId);
        }
    }
}
