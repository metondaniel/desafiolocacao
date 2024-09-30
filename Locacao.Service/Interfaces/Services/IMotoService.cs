using Locacao.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Interfaces.Services
{
    public interface IMotoService
    {
        Task<Moto> CadastrarMotoAsync(Moto moto);
        Task<IEnumerable<Moto>> ConsultarMotosAsync();
        Task<Moto> AtualizarMotoAsync(int id, string novaPlaca);
        Task RemoverMotoAsync(int id);
        Task<Moto> ConsultarMotoPorIdAsync(int id);
    }

}
