using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace BankAccounts.Models
{
    public class Users{

        public Users(){
            accounts = new List<accounts>();
        }
        [Key]
        public long id{get;set;}
        
        public List<accounts> accounts{get;set;}

        [Required]
        public string first_name{get; set;}

        [Required]
        public string last_name{get; set;}


        [Required]
        public string email {get; set;}


        [Required]
        public string password {get; set;}

        public DateTime created_at{get;set;}
        public DateTime updated_at{get; set;}
    }
}