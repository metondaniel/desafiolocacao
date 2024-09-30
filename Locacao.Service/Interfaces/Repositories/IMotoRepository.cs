using Locacao.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Interfaces.Repositories
{
    public interface IMotoRepository : IBaseRepository<Moto>
    {
        Task<bool> ExistePlacaAsync(string placa);
    }

}
