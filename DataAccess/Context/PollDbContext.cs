using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class PollDbContext : DbContext
    {

        public PollDbContext(DbContextOptions<PollDbContext> options)
            : base(options)
        {
        }

        public DbSet<Poll> Polls { get; set; }
    }
}