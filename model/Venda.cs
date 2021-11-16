using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ControleBovideo.model
{
    [Table("venda")]
    public class Venda
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("rebanho_origem")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Rebanho_origem { get; set; }
        [Column("rebanho_destino")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Rebanho_destino { get; set; }
        [Column("id_finalidade_venda")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Id_finalidade_venda { get; set; }
        [Column("qtde_vendida")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int Qtde_vendida { get; set; }
        [Column("data")]
        public DateTime Data { get; set; }

        
    }
}
