using Locacao.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Interfaces.Repositories
{
    public interface ILocacaoRepository : IBaseRepository<LocacaoMoto>
    {
        Task<IEnumerable<LocacaoMoto>> ObterLocacoesPorEntregadorAsync(int entregadorId);
        Task<LocacaoMoto> AddAsync(LocacaoMoto locacaoMoto);
        Task<LocacaoMoto> GetByIdAsync(int locacaoMotoId);
        Task<bool> ExisteLocacaoAtiva(int motoId);
    }
}
