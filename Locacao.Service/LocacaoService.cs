using FluentValidation;
using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Repositories;
using Locacao.Service.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service
{
    public class LocacaoService : ILocacaoService
    {
        private readonly ILocacaoRepository _locacaoRepository;
        private readonly IEntregadorRepository _entregadorRepository;
        private readonly IValidator<LocacaoMoto> _locacaoValidator;

        public LocacaoService(ILocacaoRepository locacaoRepository, IValidator<LocacaoMoto> locacaoValidator, IEntregadorRepository entregadorRepository)
        {
            _locacaoRepository = locacaoRepository;
            _locacaoValidator = locacaoValidator;
            _entregadorRepository = entregadorRepository;
        }

        public async Task<LocacaoMoto> AlugarMotoAsync(LocacaoMoto locacao)
        {
            var result = _locacaoValidator.Validate(locacao);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
            locacao.DataInicio = locacao.DataInicio.ToUniversalTime();
            locacao.DataTerminoPrevisto = locacao.DataTerminoPrevisto.ToUniversalTime();
            var entregador = await _entregadorRepository.GetByIdAsync(1);
            if (!entregador.TipoCnh.Contains("A"))
                throw new Exception("Somente entregadores com CNH da categoria A podem alugar motos.");

            locacao.DataInicio = DateTime.Now.AddDays(1).ToUniversalTime();
            return await _locacaoRepository.AddAsync(locacao);
        }

        public async Task<decimal> CalcularValorLocacaoAsync(int locacaoId, DateTime dataDevolucao)
        {
            var locacao = await _locacaoRepository.GetByIdAsync(locacaoId);
            if (locacao == null)
                throw new Exception("Locação não encontrada.");

            var diasEfetuados = (dataDevolucao - locacao.DataInicio).Days;
            var diasPrevistos = (locacao.DataTerminoPrevisto - locacao.DataInicio).Days;

            // Calcula multas e valor adicional
            decimal valorFinal = locacao.ValorTotal;
            if (dataDevolucao < locacao.DataTerminoPrevisto)
            {
                var diasRestantes = diasPrevistos - diasEfetuados;
                valorFinal += valorFinal*diasRestantes * 0.10m;
            }
            else if (dataDevolucao > locacao.DataTerminoPrevisto)
            {
                var diasAdicionais = (dataDevolucao - locacao.DataTerminoPrevisto).Days;
                valorFinal += diasAdicionais * 50m; 
            }

            return valorFinal;
        }
    }
}
