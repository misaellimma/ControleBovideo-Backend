using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControleBovideo.model
{
    [Table("registro_vacina")]
    public class RegistroVacina
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_rebanho")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Id_rebanho { get; set; }
        [Column("id_vacina")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Id_vacina { get; set; }
        [Column("data")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public DateTime Data { get; set; }
        [Column("qtde_vacinado")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Qtde_vacinado { get; set; }
    }
}
