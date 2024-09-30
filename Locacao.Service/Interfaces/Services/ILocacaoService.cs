using Locacao.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Interfaces.Services
{
    public interface ILocacaoService
    {
        Task<LocacaoMoto> AlugarMotoAsync(LocacaoMoto locacao);
        Task<decimal> CalcularValorLocacaoAsync(int locacaoId, DateTime dataDevolucao);
    }

}
