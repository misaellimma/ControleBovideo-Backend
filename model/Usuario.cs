﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControleBovideo.model
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(255, ErrorMessage = "O campo deve ter entre de 3 a 255 caracteres")]
        [MinLength(3, ErrorMessage = "O campo deve ter entre de 3 a 255 caracteres")]
        public string Username { get; set; }
        [Column("password")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(255, ErrorMessage = "O campo deve ter entre de 3 a 255 caracteres")]
        [MinLength(3, ErrorMessage = "O campo deve ter entre de 3 a 255 caracteres")]
        public string Password { get; set; }
        [Column("role")]
        public string Role { get; set; }
    }
}
