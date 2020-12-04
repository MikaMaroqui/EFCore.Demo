using System;
using Companies.Demo.Entities;
using EFCore.Demo.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EFCore.Demo.Db
{
    public class DemoContext : DbContext
    {
        private readonly string _connectionString;

        // Declare my DbSets (the things I'll query through my repository)
        public DbSet<Company> Companies { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }

        // These 3 should be in the same table, with a discriminator column
        public DbSet<Employee> Employees { get; set; }
        public DbSet<StaffMember> StaffMembers { get; set; }
        public DbSet<Consultant> Consultants { get; set; }

        public DbSet<ConsultantProject> ConsultantProjects { get; set; }

        public DemoContext(IOptions<ConnectionStrings> connectionStringsOptions)
        {
            _connectionString = connectionStringsOptions.Value.DbConnectionString ??
                                throw new ArgumentNullException(nameof(connectionStringsOptions));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        // We override this method to set up our relations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationship between Company and Employees (One to Many)
            modelBuilder.Entity<Company>()
                .HasMany<StaffMember>()
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>()
                .HasMany<Consultant>()
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship between Client and Projects (One to Many)
            modelBuilder.Entity<Client>()
                .HasMany<Project>()
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship between Consultant and Project (Many to Many)
            modelBuilder.Entity<ConsultantProject>()
                .HasKey(cp => new {cp.ConsultantId, cp.ProjectId});

            modelBuilder.Entity<ConsultantProject>()
                .HasOne<Consultant>()
                .WithMany(c => c.ConsultantProjects)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConsultantProject>()
                .HasOne<Project>()
                .WithMany(p => p.ConsultantProjects)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
