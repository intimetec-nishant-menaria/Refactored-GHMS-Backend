using guest_house_management_backend.Enums;

namespace guest_house_management_backend.Models
{
    public class Role
    {
        public int Id { get; set; } 
        public RoleEnum RoleName { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>(); 
    }
}
