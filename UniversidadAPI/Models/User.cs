using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UniversidadAPI.Models;

[Table("sampleusers")]
public class User
{
    [Key]
    [Column("username")]
    [JsonPropertyOrder(0)] 
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("password")]
    [JsonPropertyOrder(1)] 
    public string Password { get; set; } = string.Empty;

    [Required]
    [Column("dni")]
    [JsonPropertyOrder(2)] 
    public string Dni { get; set; } = string.Empty;

    [Required]
    [Column("name")]
    [JsonPropertyOrder(3)] 
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column("surnames")]
    [JsonPropertyOrder(4)] 
    public string Surnames { get; set; } = string.Empty;

    [Required]
    [Column("age")]
    [JsonPropertyOrder(5)] 
    public int Age { get; set; }
}
