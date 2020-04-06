using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActivityCenter.Models
{
    public class Game
    {
        [Key]
        [Required]
        public int GameId { get; set;}
        public int UserId { get; set;}
        [Required]
        [MinLength(2, ErrorMessage = "Password must be at least 2 characters")]
        public string Title { get; set;}
        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Date must be in the Future")]
        public DateTime GameDate { get; set; } 
        [Required]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }  
        [Required]
        public string Description { get; set;}
        
        public User Creator { get; set; }
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
        public List<Association> Participants { get; set; }

        public class FutureDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                return value != null && (DateTime)value > DateTime.Now;
            }
        }
    }
        
}