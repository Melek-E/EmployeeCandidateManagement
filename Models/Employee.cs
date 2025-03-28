using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeCandidateManagement.API.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal GrossSalary { get; set; }

        public decimal NetSalary { get; set; }
        public decimal Taxes { get; set; }

        public string Position { get; set; } = string.Empty;
    }
}
