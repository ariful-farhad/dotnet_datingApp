using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
  [Required]
  // [MaxLength(100)]
  public required string Username { get; set; }
  [Required]
  public required string Password { get; set; }
}


// [required] here is an annotator which will validate our data
// however, it may feel we are duplicating, but we can utilize other thing as well such as ensuring max MaxLength
// here, one required for compiler and one for the validation