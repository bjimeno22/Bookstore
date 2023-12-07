using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Entities;

[Table("Users")]
public class User
{
    public int UserId { get; set;}
    [Required]
    [MaxLength(100)]
    public string? Email { get; set;}
    [Required]
    [MaxLength(50)]
    public string? FirstName { get; set;}
    [Required]
    [MaxLength(50)]
    public string? LastName { get; set;}
    [MaxLength(50)]
    public string? PasswordHash { get; set;}
    [JsonIgnore]
    [MaxLength(128)]
    public string? PasswordSalt { get; set;}
    public int RoleId { get; set;}
    public virtual Role Role { get; set;}

}
