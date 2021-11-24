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

        public string FormataCpf(string cpf)
        {
            cpf = cpf.Trim();
            return cpf.Replace(".", "").Replace("-", "");
        }
        public bool ValidaCpf(string cpf)
        {
			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			string tempCpf;
			string digito;
			int soma;
			int resto;

			cpf = cpf.Trim();
			cpf = cpf.Replace(".", "").Replace("-", "");

            if (
                cpf == "00000000000" ||
                cpf == "11111111111" ||
                cpf == "22222222222" ||
                cpf == "33333333333" ||
                cpf == "44444444444" ||
                cpf == "55555555555" ||
                cpf == "66666666666" ||
                cpf == "77777777777" ||
                cpf == "88888888888" ||
                cpf == "99999999999"
                )
            {
                return false;
            }

            if (cpf.Length != 11)
            {
				return false;
            }
			tempCpf = cpf.Substring(0, 9);
			soma = 0;

			for (int i = 0; i < 9; i++)
            {
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }
			resto = soma % 11;
			
			if (resto < 2)
            {
				resto = 0;
            }
            else
            {
				resto = 11 - resto;
            }
			digito = resto.ToString();
			tempCpf = tempCpf + digito;
			soma = 0;
			
			for (int i = 0; i < 10; i++)
            {
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }
			resto = soma % 11;
			
			if (resto < 2)
            {
				resto = 0;
            }
            else
            {
				resto = 11 - resto;
            }
			digito = digito + resto.ToString();
			
            return cpf.EndsWith(digito);
		}
    }
}
