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
        
        public int Qtde_total { get; set; }
        [Column("qtde_vacinado_aftosa")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        
        public int Qtde_vacinado_aftosa { get; set; }
        [Column("qtde_vacinado_brucelose")]
        [Required(ErrorMessage = "Campo obrigatório!")]

        public int Qtde_vacinado_brucelose { get; set; }

        public void CreditarSaldoVacinadoBrucelose(int saldo)
        {
            this.Qtde_vacinado_brucelose += saldo;
        }
        public void DebitarSaldoVacinadoBrucelose(int saldo)
        {
            this.Qtde_vacinado_brucelose -= saldo;
        }
        public void CreditarSaldoVacinadoAftosa(int saldo)
        {
            this.Qtde_vacinado_aftosa += saldo;
        }
        public void DebitarSaldoVacinadoAftosa(int saldo)
        {
            this.Qtde_vacinado_aftosa += saldo;
        }

        public bool ValidarQtdeVacinado()
        {
            if(Qtde_total >= Qtde_vacinado_aftosa && Qtde_total >= Qtde_vacinado_brucelose)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreditarSaldoRebanhoVenda(int valor)
        {
            Qtde_total += valor;
            Qtde_vacinado_aftosa += valor;
            Qtde_vacinado_brucelose += valor;
        }
        public void CreditarSaldoRebanho(int valor)
        {
            Qtde_total += valor;
        }

        public bool DebitarSaldoRebanhoVenda(int valor)
        {
            if(valor > Qtde_total || valor > Qtde_vacinado_aftosa || valor > Qtde_vacinado_brucelose)
            {
                return false;
            }
            else
            {
                Qtde_total -= valor;
                Qtde_vacinado_aftosa -= valor;
                Qtde_vacinado_brucelose -= valor;
                return true;
            }
        }
    }
}
