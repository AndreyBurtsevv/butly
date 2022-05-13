﻿using System.ComponentModel.DataAnnotations;

namespace Bitly.Core.Dto
{
    public class LoginView
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
