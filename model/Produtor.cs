using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControleBovideo.model
{
    [Table("produtor")]
    public class Produtor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_endereco")]
        public int Id_endereco { get; set; }
        [Column("nome")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [MaxLength(255, ErrorMessage = "O campo deve ter entre de 3 a 255 caracteres")]
        [MinLength(3, ErrorMessage = "O campo deve ter entre de 3 a 255 caracteres")]
        public string Nome { get; set; }
        [Column("cpf")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Cpf { get; set; }
        [Column("id_usuario")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Id_usuario { get; set; }
    }
}
