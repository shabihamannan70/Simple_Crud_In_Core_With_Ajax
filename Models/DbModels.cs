using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace work_01.Models
{
    public class Designation
    {
        public int DesignationId { get; set; }
        [Required,StringLength(50),Display(Name = "Designation Name")]
        public string DesignationName { get; set; } = default!;
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required, StringLength(50), Display(Name = "Employee Name")]
        public string EmployeeName { get; set; } = default!;
        [Column(TypeName = "date"), Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [StringLength(150), Display(Name = "Picture")]

        public string Picture { get; set; } = default!;
        [ForeignKey("Designation")]
        public int DesignationId { get; set; }
        public virtual Designation? Designation { get; set; }

    }
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext>options): base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; } 
        public DbSet<Designation> Designations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Designation>().HasData(
                    new Designation { DesignationId = 1, DesignationName = "Manager" },
                    new Designation { DesignationId = 2, DesignationName = "HR" },
                    new Designation { DesignationId = 3, DesignationName = "Senior Developer" },
                    new Designation { DesignationId = 4, DesignationName = "Junior Developer" }
                );
        }
      
    }
}
