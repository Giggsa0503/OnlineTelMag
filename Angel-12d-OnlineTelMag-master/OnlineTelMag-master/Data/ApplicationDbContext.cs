using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using OnlineTelMag.Data;

namespace OnlineTelMag.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
       // public DbSet<Types> Types{get;set;}
        public DbSet<Telephone> Telephones { get; set; }
       // public DbSet<Products> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
       // public DbSet<OrderList> OrderLists { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<TelephoneImages> TelephonesImages { get; set; }

       // public DbSet<Accessoares> Accessoares { get; set; }
    }
}
