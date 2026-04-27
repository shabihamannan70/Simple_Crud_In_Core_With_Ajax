using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace work_01.Models
{
    public class EmpVM
    {
        public int EmployeeId { get; set; }
        [Required, StringLength(50), Display(Name = "Employee Name")]
        public string EmployeeName { get; set; } = default!;
        [Column(TypeName = "date"), Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        [StringLength(150), Display(Name = "Picture")]

        public string? Picture { get; set; } = default!;
        [ForeignKey("Designation")]
        public int DesignationId { get; set; }
        public IFormFile? PictureFile { get; set; } 

    }
}
