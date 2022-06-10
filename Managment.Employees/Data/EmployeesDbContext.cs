using Managment.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Managment.Employees.Data;

public class EmployeesDbContext : DbContext
{
    public EmployeesDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<EmployeeBase> Employees { get; set; }

    public DbSet<EmployeeManager> Managers { get; set; }

    public DbSet<EmployeeSubordinate> Subordinates { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeBase>()
            .HasDiscriminator<string>("employee_type")
            .HasValue<EmployeeBase>("employee_base")
            .HasValue<EmployeeManager>("employee_manager")
            .HasValue<EmployeeSubordinate>("employee_subordinate");
        modelBuilder.Entity<EmployeeManager>()
            .HasMany(p => p.Subordinates)
            .WithOne(p => p.Manager)
            .HasForeignKey(p => p.ManagerId);
        modelBuilder.Entity<EmployeeSubordinate>()
            .HasOne(p => p.Manager)
            .WithMany(p => p.Subordinates)
            .HasForeignKey(p => p.ManagerId);
    }
}