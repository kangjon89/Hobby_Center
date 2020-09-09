using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class Join 
    {
        [Key]
        public int JoinId {get;set;}
        public int SportId {get;set;}
        public int UserId {get;set;}
        public User User {get;set;}
        public Sport Sport {get;set;}

    }
}