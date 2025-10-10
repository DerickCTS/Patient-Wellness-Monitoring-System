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
        [Required]
        [Display(Name = "Refresh Token Id")]
        public int RefreshTokenId { get; set; }


        [Required]
        public required string Token { get; set; } 


        [Required]
        [Display(Name = "JWT ID")]
        public required string JwtId { get; set; } 


        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Expires { get; set; }


        [Required]
        public bool IsRevoked { get; set; } = false;


        [DataType(DataType.DateTime)]
        public DateTime? RevokedAt { get; set; }


        [Required]
        [Display(Name = "User ID")]
        public required string UserId { get; set; } 


        [Required]
        [Display(Name = "User Type")]   
        public UserType UserType { get; set; }


        [NotMapped]
        public required object User { get; set; } 


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //The client associated with the refresh token
        //[Required]
        //public int ClientId { get; set; }

        //[ForeignKey(nameof(ClientId))]
        //public Client Client { get; set; } = null!;
    }
}