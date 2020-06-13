using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Championships.Models
{
    public class Tournament
    {

        public Tournament()
        {
            this.RegisteredUsers = new List<User>();
        }

        [Key]
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        //[GreaterThan("Date", ErrorMessage = "Date of tournament must be greater than end of application deadline")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Max amount of participants")]
        [Range(0, 10000)]
        public int MaxAmountOfUsers { get; set; }

        [Required]
        public string Discipline { get; set; }

        public string Organizer { get; set; }

        public string Sponsors { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name="End of application deadline")]
        public DateTime DateToRegister { get; set; }

        [Display(Name="Registered participants")]
        public virtual List<User> RegisteredUsers { get; set; }

    }
}