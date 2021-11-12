using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControleBovideo.model
{
    [Table("propriedade")]
    public class Propriedade
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_produtor")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Id_produtor { get; set; }
        [Column("id_municipio")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Id_municipio { get; set; }
        [Column("incricao_estadual")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [RegularExpression("[0-9]{3}[.][0-9]{3}[.][0-9]{3}[.][-][0-9]{2}")]
        public string Incricao_estadual { get; set; }
        [Column("nome_propriedade")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [MaxLength(255, ErrorMessage = "O campo deve ter entre de 3 a 255 caracteres")]
        [MinLength(3, ErrorMessage = "O campo deve ter entre de 3 a 255 caracteres")]
        public string Nome_propriedade { get; set; }
    }
}
