using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Entities
{
    public class LocacaoMoto
    {
        public int Id { get; set; }
        public int MotoId { get; set; }
        public int EntregadorId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataTerminoPrevisto { get; set; }
        public DateTime? DataTermino { get; set; }
        public decimal ValorTotal { get; set; }
        public virtual Entregador? Entregador { get; set; }
        public virtual Moto? Moto { get; set; }
    }
}
