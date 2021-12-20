using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ApiTest
{
    public class TestDbInit
    {
        private static UserManager<User> _userManager;
        public static void Initialize(SzavazoDbContext context)
        {

            _userManager = new UserManager<User>(
            new UserStore<User>(context), null,
            new PasswordHasher<User>(), null, null, null, null, null, null);
            //context.Database.Migrate(); //progam automatikusan migrál

            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //return;
            var defaultUsers = new User[]
            {
                new User{Email="Polluser1@gmail.com", UserName="asd1" }, // elméletileg a passwordhash már a hashed változatott adja meg, sajnos
                new User{Email="Polluser2@gmail.com", UserName="asd2"},
                new User{Email="Polluser3@gmail.com", UserName="asd3"}
            };



            List<Poll> defaultpolls = new List<Poll>
            {
                new Poll
                {
                    Id = 1,
                    Creator = defaultUsers[0],
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(10),
                    Question = "Lasagna valójában spagetti torta-e?",
                    Options = new List<Answer>
                    {
                        new Answer{Text="Igen"},
                        new Answer{Text="Nem"},
                        new Answer{Text="Hülye kérdés"},
                    }
                },
                new Poll
                {
                    Id = 2,
                    Creator = defaultUsers[1],
                    Start = DateTime.Now.AddDays(-10),
                    End = DateTime.Now.AddDays(1),
                    Question = "Kutya vagy macska?",
                    Options = new List<Answer>
                    {
                        new Answer{Text="Kutya"},
                        new Answer{Text="Macska"},
                        new Answer{Text="Mindkettő"},
                        new Answer{Text="Egyik se"}
                    }
                },
                new Poll
                {
                    Id = 3,
                    Creator = defaultUsers[2],
                    Start = DateTime.Now.AddDays(-1),
                    End = DateTime.Now.AddDays(5),
                    Question = "Kedvenc gyorsétterem?",
                    Options = new List<Answer>
                    {
                        new Answer{Text="McDonalds"},
                        new Answer{Text="KFC"},
                        new Answer{Text="Burger King"},
                        new Answer{Text="Pizza King"}
                    }
                },
                new Poll
                {
                    Id = 4,
                    Creator = defaultUsers[0],
                    Start = DateTime.Now.AddDays(-10),
                    End = DateTime.Now.AddDays(-1),
                    Question = "Lezárt idő által példa",
                    Options = new List<Answer>
                    {
                        new Answer{Text="példa1"},
                        new Answer{Text="példa2"},
                        new Answer{Text="példa3"}
                    }
                },
                new Poll
                {
                    Id = 5,
                    Creator = defaultUsers[0],
                    Start = DateTime.Now.AddDays(-10),
                    End = DateTime.Now.AddDays(10),
                    Question = "Lezárt szavazások által példa",
                    Options = new List<Answer>
                    {
                        new Answer{Text="példa1"},
                        new Answer{Text="példa2"},
                        new Answer{Text="példa3"}
                    }
                }

            };
            foreach (var user in defaultUsers)
            {
                _userManager.CreateAsync(user, "asd").Wait();

            }
            Random rand = new Random();
            List<PollBinding> pollBindings = new List<PollBinding>();
            var users = _userManager.Users.ToList();
            foreach (var poll in defaultpolls)
            {

                foreach (var user in users)
                {
                    pollBindings.Add(new PollBinding
                    {
                        IsVoted = poll == defaultpolls[4], // lezárt szavazás tesztelése
                        Poll = poll,
                        User = user
                    });
                }

            }
            //lezárt szavazás 


            context.AddRange(pollBindings);
            //context.AddRange(defaultUsers);
            context.AddRange(defaultpolls);
            context.SaveChanges();
        }
    }
}
