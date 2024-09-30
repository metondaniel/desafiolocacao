using FluentValidation;
using Locacao.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Validators
{
    public class EntregadorValidator : AbstractValidator<Entregador>
    {
        public EntregadorValidator()
        {
            RuleFor(e => e.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.");

            RuleFor(e => e.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.");

            RuleFor(e => e.NumeroCnh)
                .NotEmpty().WithMessage("O número da CNH é obrigatório.")
                .Matches("[0-9]{11}").WithMessage("CNH deve conter 11 números.");

            RuleFor(e => e.TipoCnh)
                .Must(tipo => tipo == "A" || tipo == "B" || tipo == "A+B")
                .WithMessage("Tipo de CNH inválido.");
        }

        public static class CnpjValidator
        {
            public static bool IsValidCnpj(string cnpj)
            {
                cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

                if (cnpj.Length != 14)
                    return false;

                int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                string tempCnpj;
                string digito;
                int soma;
                int resto;

                tempCnpj = cnpj.Substring(0, 12);
                soma = 0;

                for (int i = 0; i < 12; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = resto.ToString();
                tempCnpj = tempCnpj + digito;
                soma = 0;
                for (int i = 0; i < 13; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = digito + resto.ToString();

                return cnpj.EndsWith(digito);
            }
        }

    }

}
