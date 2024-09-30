using Locacao.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Interfaces.Repositories
{
    public interface IEntregadorRepository : IBaseRepository<Entregador>
    {
        Task<bool> ExisteCnpjAsync(string cnpj);
        Task<bool> ExisteCnhAsync(string cnh);
    }

}
