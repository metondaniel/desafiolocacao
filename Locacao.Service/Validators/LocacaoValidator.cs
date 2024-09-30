using FluentValidation;
using Locacao.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Validators
{
    public class LocacaoValidator : AbstractValidator<LocacaoMoto>
    {
        public LocacaoValidator()
        {
            RuleFor(l => l.MotoId)
                .NotEmpty().WithMessage("ID da moto é obrigatório.");

            RuleFor(l => l.EntregadorId)
                .NotEmpty().WithMessage("ID do entregador é obrigatório.");

            RuleFor(l => l.DataTerminoPrevisto)
                .GreaterThan(l => l.DataInicio).WithMessage("A data de término deve ser maior que a data de início.");
        }
    }

}
