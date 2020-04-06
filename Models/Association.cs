using System.ComponentModel.DataAnnotations;

namespace ActivityCenter.Models
{
    public class Association
    {
        [Key]
        public int AssociationId { get; set;}
        public int UserId { get; set;}
        public int GameId { get; set;}
        public User User { get; set; }
        public Game Game { get; set; }          
    }
}