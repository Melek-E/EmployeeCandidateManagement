using System.ComponentModel.DataAnnotations;

namespace EmployeeCandidateManagement.API.Models
{
    public class ResumeProcessRequest
    {
        [Required]
        public string ResumeContent { get; set; } = string.Empty;
    }

    public class EvaluateRequest
    {
        [Required]
        public string JobRequirements { get; set; } = string.Empty;
    }

    public class SalarySuggestionRequest
    {
        [Required]
        public string Position { get; set; } = string.Empty;
    }

    public class SalarySuggestionResponse
    {
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public string Position { get; set; } = string.Empty;
    }
}