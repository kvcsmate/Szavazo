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
    public class PollsController : Controller
    {
        private readonly SzavazoService service ;

        public PollsController(SzavazoService szavazoService)
        {
            service = szavazoService;
        }

        // GET: Polls

        public IActionResult Index()
        {
            try
            {
                return View(service.GetPollsByCurrentUser(GetCurrentUser()));
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
                
            }
            
        }

        public IActionResult Closed( DateTime? from, DateTime? until, string filter = "")
        {
            try
            {
                ViewData["Filter"] = filter;
                if (from is null || until is null)
                {
                    if (String.IsNullOrEmpty(filter))
                    {
                        return View(service.GetClosedPolls().ToList());
                    }
                    var polls = service.GetClosedPolls().Where(p => p.Question.ToLower().Contains(filter.ToLower())).ToList();
                    return View(polls);
                }
                else
                {
                    if (String.IsNullOrEmpty(filter))
                    {
                        return View(service.GetClosedPolls().Where(p=> p.End < until && p.Start > from).ToList());
                    }
                    var polls = service.GetClosedPolls().Where(p => p.Question.ToLower().Contains(filter.ToLower()) || (p.End < until && p.Start > from)).ToList();
                    return View(polls);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", new { err = e.HelpLink });
            }
        }

        // GET: Polls/Details/5
        /* public async Task<IActionResult> Details(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var poll = await _context.Polls
                 .FirstOrDefaultAsync(m => m.Id == id);
             if (poll == null)
             {
                 return NotFound();
             }

             return View(poll);
         }

         // GET: Polls/Create
         public IActionResult Create()
         {
             return View();
         }

         // POST: Polls/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to, for 
         // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("Id,Question,Start,End")] Poll poll)
         {
             if (ModelState.IsValid)
             {
                 _context.Add(poll);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             return View(poll);
         }

         // GET: Polls/Edit/5
         public async Task<IActionResult> Edit(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var poll = await _context.Polls.FindAsync(id);
             if (poll == null)
             {
                 return NotFound();
             }
             return View(poll);
         }

         // POST: Polls/Edit/5
         // To protect from overposting attacks, enable the specific properties you want to bind to, for 
         // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Start,End")] Poll poll)
         {
             if (id != poll.Id)
             {
                 return NotFound();
             }

             if (ModelState.IsValid)
             {
                 try
                 {
                     _context.Update(poll);
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     if (!PollExists(poll.Id))
                     {
                         return NotFound();
                     }
                     else
                     {
                         throw;
                     }
                 }
                 return RedirectToAction(nameof(Index));
             }
             return View(poll);
         }

         // GET: Polls/Delete/5
         public async Task<IActionResult> Delete(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }

             var poll = await _context.Polls
                 .FirstOrDefaultAsync(m => m.Id == id);
             if (poll == null)
             {
                 return NotFound();
             }

             return View(poll);
         }

         // POST: Polls/Delete/5
         [HttpPost, ActionName("Delete")]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteConfirmed(int id)
         {
             var poll = await _context.Polls.FindAsync(id);
             _context.Polls.Remove(poll);
             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));
         }

         private bool PollExists(int id)
         {
             return _context.Polls.Any(e => e.Id == id);
         }*/
        private User GetCurrentUser()
        {
            string CurrentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return service.GetUser(CurrentUserId);
        }
    }
}
