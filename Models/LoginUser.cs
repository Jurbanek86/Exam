using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class ExamUser
    {
        [Required]
        [Display(Name="Email: ")]
        [EmailAddress]
        public string ExamEmail {get; set;}
        [Required]
        [Display(Name="Password: ")]
        [DataType(DataType.Password)]
        public string ExamPassword {get; set;}

    }
}