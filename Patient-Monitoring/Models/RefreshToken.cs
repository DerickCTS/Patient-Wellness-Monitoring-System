using Microsoft.EntityFrameworkCore;
using Patient_Monitoring.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Patient_Monitoring.Models
{
    [Index(nameof(Token), Name = "IX_Token_Unique", IsUnique = true)]
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = null!;

        
        [Required]
        public string JwtId { get; set; } = null!;

  
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Expires { get; set; }

        
        public bool IsRevoked { get; set; } = false;

        [DataType(DataType.DateTime)]
        public DateTime? RevokedAt { get; set; }


        [Required]
        public int UserId { get; set; }

        [Required]
        public UserType UserType { get; set; }
        
        [NotMapped]
        public object User { get; set; } = null!;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //The client associated with the refresh token
        //[Required]
        //public int ClientId { get; set; }

        //[ForeignKey(nameof(ClientId))]
        //public Client Client { get; set; } = null!;
    }
}
