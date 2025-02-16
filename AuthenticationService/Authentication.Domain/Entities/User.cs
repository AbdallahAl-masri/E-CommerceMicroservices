using System.ComponentModel.DataAnnotations;

namespace Authentication.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? MobileNumber { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
