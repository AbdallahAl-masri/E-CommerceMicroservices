﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.DTOs
{
    public class UserDTO
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? MobileNumber { get; set; }

        [Required]
        public string? Role { get; set; }
    }
}
