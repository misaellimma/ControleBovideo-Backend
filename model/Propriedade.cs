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
        
        public string Incricao_estadual { get; set; }
        [Column("nome_propriedade")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [MaxLength(255, ErrorMessage = "O campo deve ter entre de 3 a 255 caracteres")]
        [MinLength(3, ErrorMessage = "O campo deve ter entre de 3 a 255 caracteres")]
        public string Nome_propriedade { get; set; }

        public string FormataInscricao()
        {
            Incricao_estadual = Incricao_estadual.Trim();
            return Incricao_estadual.Replace(".", "").Replace("-", "");
        }

        public bool ValidarInscricaoEstadual()
        {
            bool retorno = false;
            string strBase;
            string strBase2;
            string strOrigem;
            string strDigito1;
            int intPos;
            int intValor;
            int intSoma = 0;
            int intResto;
            strBase = "";
            strBase2 = "";
            strOrigem = "";

            if (Incricao_estadual.Trim().ToUpper() == "ISENTO")
            {
                return true;
            }

            for (intPos = 1; intPos <= Incricao_estadual.Trim().Length; intPos++)
            {
                if ((("0123456789P".IndexOf(Incricao_estadual.Substring((intPos - 1), 1), 0, System.StringComparison.OrdinalIgnoreCase) + 1) > 0))
                {
                    strOrigem = (strOrigem + Incricao_estadual.Substring((intPos - 1), 1));
                }
            }

            strBase = (strOrigem.Trim() + "000000000").Substring(0, 9);

            int ie = int.Parse(strBase);
            if (ie >= 285000000)
            {
                intSoma = 0;

                for (intPos = 1; (intPos <= 8); intPos++)
                {
                    intValor = int.Parse(strBase.Substring((intPos - 1), 1));
                    intValor = (intValor * (10 - intPos));
                    intSoma = (intSoma + intValor);
                }

                intResto = (intSoma % 11);
                strDigito1 = ((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Substring((((intResto < 2) ? "0" : Convert.ToString((11 - intResto))).Length - 1));
                strBase2 = (strBase.Substring(0, 8) + strDigito1);

                if ((strBase2 == strOrigem))
                {
                    retorno = true;
                }
            }
            return retorno;
        }
    }
}
