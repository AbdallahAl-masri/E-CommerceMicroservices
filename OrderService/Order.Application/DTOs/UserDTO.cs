﻿using System.ComponentModel.DataAnnotations;

namespace Order.Application.DTOs
{
    public class UserDTO
    {

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
