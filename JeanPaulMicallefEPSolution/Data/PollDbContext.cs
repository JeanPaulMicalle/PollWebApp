using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeanPaulMicallefEPSolution.Data
{
    public class PollDbContext : IdentityDbContext
    {
        public PollDbContext(DbContextOptions<PollDbContext> options)
            : base(options)
        {
        }
    }
}
