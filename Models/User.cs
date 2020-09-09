using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
    
namespace Exam.Models
{
    public class User 
        {
        [Key]
        public int UserId {get;set;}

        [Required]
        [MinLength(2, ErrorMessage="Name must be at least 2 characters")]
        [Display(Name="Name")]
        public string Name {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        public string Password {get;set;}
        [NotMapped]
        [Display(Name="Confirm Password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm { get;set; }
        public List<Sport> MySports { get;set; }
        public List<Join> JoinSports { get;set; }
        public DateTime CreatedAt { get;set; } = DateTime.Now;
        public DateTime UpdatedAt { get;set; } = DateTime.Now;

    }
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [DataType(DataType.Password)]
        public string Password{get;set;}
    }
}