using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Entities
{
    public class Moto
    {
        public int Id { get; set; }
        public string Identificador { get; set; }
        public int Ano { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
        [IgnoreDataMember]
        public virtual List<LocacaoMoto>? Locacoes { get; set; }
    }
}
