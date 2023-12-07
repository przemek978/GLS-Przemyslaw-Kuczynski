using GlsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlsAPI.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {

        }
        public DbSet<Package> Packages { get; set; }
        public DbSet<User> Users {  get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Error> Errors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
