using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Draken.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected DatabaseContext()
        {
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasData(new User()
        //    {
        //        Email = "admin@drakern.web",
        //        Password = "xMtMAtSF+ArM9k9926kigiHdfWhNHx0iRU8R83MptxA=" //Encrypted value for "Password@123"
        //    });
        //}
    }
}
