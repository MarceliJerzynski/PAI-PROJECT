using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Dynamic;
using System.Web.Mvc;

namespace Championships.Models
{
    public class User
    {
        public User()
        {
            ForgotPasswordKey = "0000";
            registeredTournaments = new List<Tournament>();
        }

        //public int Id { get; set; }

        [Required]
        [Key]
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }

        public string ForgotPasswordKey { get; set; }

        public virtual List<Tournament> registeredTournaments { get; set; }


    }
}