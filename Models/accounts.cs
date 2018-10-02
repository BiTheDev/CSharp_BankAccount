using System;
using System.ComponentModel.DataAnnotations;


namespace BankAccounts.Models
{
    public class accounts{

        [Key]
        public long id{get;set;}
        public long userid{get;set;}
        public double change{get;set;}

        public Users user{get;set;}

        public DateTime created_at{get;set;}
        public DateTime updated_at{get; set;}

    }
}