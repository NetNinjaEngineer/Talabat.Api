﻿using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.DTOs;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string DisplayName { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    public AddressDto Address { get; set; }

    [Required]
    public string Password { get; set; }

}
