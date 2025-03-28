using System.ComponentModel.DataAnnotations;

namespace EmployeeCandidateManagement.API.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Skills { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        public string Education { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
    }
}
