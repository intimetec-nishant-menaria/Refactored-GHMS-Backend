namespace guest_house_management_backend.DTOs
{
    public class UpdateUserDto
    {
        public string Name { get; set; }   = string.Empty;
        public string Email { get; set; }  = string.Empty ;
        public string RoleName {  get; set; } = string.Empty ;
    }
}
