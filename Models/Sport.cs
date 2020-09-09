using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class NoPastDateAttribute : ValidationAttribute{
        protected override ValidationResult IsValid(object value, ValidationContext validationContext){
            if((DateTime)value < DateTime.Now){
                return new ValidationResult("Date must be in the future!");
            }
            return ValidationResult.Success;
        }
    }
    public class Sport 
    {
        [Key]
        public int SportId{get;set;}
        [Required]
        [Display(Name="Title:")]
        public string Title {get;set;}

        [Required(ErrorMessage = "Enter a Description")]
        [Display(Name="Description:")]
        public string Description {get;set;}
        public int CreatorId{get;set;}
        public User Creator{get;set;}
        public string CreatorName{get;set;}
        public List<Join> Guests{get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}