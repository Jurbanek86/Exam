using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Job
    {
        [Key]
        public int JobId {get;set;}
        [Required]
        public string JobName {get;set;}
        [Required]
        [FutureDate]
        public DateTime Date {get;set;}
        [Required]
        public string Description {get;set;}
        public int Duration {get;set;}
        public string Increment {get;set;}
        public string Time {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId {get;set;}
        public User Planner {get;set;}
        public List<RSVP> WorkerList {get;set;}
        




        

    }
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date;
            if(value is DateTime)
            {
                date = (DateTime)value;
            }
            else
            {
                return new ValidationResult("Invalid DateTime");
            }
            if( date < DateTime.Now)
            {
                return new ValidationResult("Date must be in the future");
            }
            return ValidationResult.Success;
        }
    }
}