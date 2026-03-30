using System.ComponentModel.DataAnnotations;

namespace guest_house_management_backend.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string RoleName { get; set; } = string.Empty; 
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
