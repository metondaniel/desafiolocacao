using Locacao.Service.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Interfaces.Services
{
    public interface IEntregadorService
    {
        Task<Entregador> CadastrarEntregadorAsync(Entregador entregador);
        Task EnviarCnhAsync(int entregadorId, IFormFile cnhImage);
        Task<Entregador> GetEntregadorByIdAsync(int id);
    }

}
