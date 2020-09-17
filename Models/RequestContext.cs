using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebShell.Models
{
    public class RequestContext : DbContext
    {
        public DbSet<Request> Requests { get; set; }

        public RequestContext(DbContextOptions<RequestContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
