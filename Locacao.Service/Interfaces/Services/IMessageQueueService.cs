using Locacao.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Interfaces.Services
{
    public interface IMessageQueueService
    {
        Task PublicarEventoMotoCadastrada(Moto moto);
        void ConsumirEventosMotos2024();
    }

}
