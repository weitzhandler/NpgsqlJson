using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace NpgsqlJson
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=127.0.0.1;Database=npgsql_tests;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var customerConfig = modelBuilder.Entity<Customer>();

            customerConfig
                .Property(customer => customer.Names)
                .HasColumnType("jsonb");

            customerConfig
                .Property(customer => customer.Phones)
                .HasColumnType("jsonb");
        }
    }

    public class Customer
    {
        public int Id { get; set; }

        HashSet<string>? _Names;
        public ICollection<string> Names
        {
            get => _Names ??= new HashSet<string>();
            set => _Names = value.ToHashSet();
        }

        HashSet<Phone>? _Phones;
        public ICollection<Phone> Phones
        {
            get => _Phones ??= new HashSet<Phone>();
            set => _Phones = value?.ToHashSet();
        }
    }

    public class Phone
    {
        public string? Place { get; set; }
        public string? Number { get; set; }
    }
}