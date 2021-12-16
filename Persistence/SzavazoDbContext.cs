using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence
{
    public class SzavazoDbContext: IdentityDbContext<User>
    {
        public DbSet<Answer> Answers { get; set; }

        public DbSet<Poll> Polls { get; set; }

        public DbSet<PollBinding> PollBindings { get; set; }

        public DbSet<Vote> Votes{ get; set; }
        public SzavazoDbContext(DbContextOptions options) : base(options)
        { 
            
        }

    }
}
