﻿using System.ComponentModel.DataAnnotations;

namespace DeepOrangeIdentity.ViewModel;
public class RegisterViewModel
{
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public string ReturnUrl { get; set; }
}