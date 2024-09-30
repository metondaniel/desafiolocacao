using FluentValidation;
using Locacao.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Validators
{
    public class MotoValidator : AbstractValidator<Moto>
    {
        public MotoValidator()
        {
            RuleFor(m => m.Identificador)
                .NotEmpty().WithMessage("O identificador é obrigatório.");

            RuleFor(m => m.Ano)
                .GreaterThan(1900).WithMessage("Ano inválido.");

            RuleFor(m => m.Modelo)
                .NotEmpty().WithMessage("O modelo é obrigatório.");

            RuleFor(m => m.Placa)
                .NotEmpty().WithMessage("A placa é obrigatória.")
                .Matches("[A-Z]{3}-[0-9]{4}").WithMessage("Formato da placa inválido.");
        }
    }

}
