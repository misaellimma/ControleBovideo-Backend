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
        [Column("qtde_total")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(1, ErrorMessage = "Campo obrigatório!")]
        public int Qtde_total { get; set; }
        [Column("qtde_vacinado")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(1, ErrorMessage = "Campo obrigatório!")]
        public int Qtde_vacinado { get; set; }

        public void CreditarSaldoVacinado(int saldo)
        {
            this.Qtde_vacinado += saldo;
        }

        public bool ValidarQtdeVacinado()
        {
            if(Qtde_total > Qtde_vacinado)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreditarSaldoRebanho(int valor)
        {
            Qtde_total += valor;
            Qtde_vacinado += valor;
        }

        public bool DebitarSaldoRebanho(int valor)
        {
            if(valor > Qtde_total || valor > Qtde_vacinado)
            {
                return false;
            }
            else
            {
                Qtde_total -= valor;
                Qtde_vacinado -= valor;
                return true;
            }
        }
    }
}
