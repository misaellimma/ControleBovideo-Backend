using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControleBovideo.model
{
    [Table("rebanho")]
    public class Rebanho
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_especie")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Id_especie { get; set; }
        [Column("id_propriedade")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Id_propriedade { get; set; }
        [Column("qtde")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Qtde { get; set; }
    }
}
