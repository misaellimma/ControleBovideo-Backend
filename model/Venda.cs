using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControleBovideo.model
{
    [Table("registro_vacina")]
    public class Venda
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("propriedade_origem")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Propriedade_origem { get; set; }
        [Column("propriedade_destino")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Propriedade_destino { get; set; }
        [Column("id_finalidade_venda")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Id_finalidade_venda { get; set; }
        [Column("qtde_vendida")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Qtde_vendida { get; set; }
    }
}
