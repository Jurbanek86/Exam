using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Exam.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}
        [Required]
        [Display(Name="First Name: ")]
        [MinLength(2)]
        public string FirstName {get; set;}
        [Required]
        [Display(Name="Last Name: ")]
        [MinLength(2)]
        public string LastName{get; set;}
        [Required]
        [Display(Name="Email: ")]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        [MinLength(8)]
        [Display(Name="Password: ")]
        [DataType(DataType.Password)]
        public string Password {get; set;}
        [Required]
        [Custom]
        [Display(Name="Confirm Password: ")]
        [Compare("Password")]
        [NotMapped]
        [DataType(DataType.Password)]
        [MinLength(8)]
        public string ComparePassword {get; set;}
        public List<Job> PlannedJob {get;set;}
        public List<RSVP> GoingTo {get; set;}

        }
        public class CustomAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
             bool validPassword = false;
    string reason = String.Empty;
    string Password = value == null ? String.Empty : value.ToString();
    
    
        Regex reSymbol = new Regex("[^a-zA-Z0-9]");
        if (!reSymbol.IsMatch(Password)) 
        {
            reason += "Your new password must contain at least 1 symbol character.";
        } 
        else
        {
            validPassword = true;
        }
        Regex reNumber = new Regex(@"^\d$");
        if (!reNumber.IsMatch(Password)) 
        {
            reason += "Your new password must contain at least 1 number.";
        } 
        else
        {
            validPassword = true;
        }
        Regex reLetter = new Regex(@"^[a-zA-Z]+$");
        if (!reLetter.IsMatch(Password)) 
        {
            reason += "Your new password must contain at least 1 letter.";
        } 
        else
        {
            validPassword = true;
        }
    
    if (validPassword) 
    {
        return ValidationResult.Success;
    } 
    else
    {
        return new ValidationResult(reason);
    }
        }
    }
}