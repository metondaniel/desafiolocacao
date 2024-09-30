using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service.Entities
{
    public class Entregador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; } 
        public string NumeroCnh { get; set; }
        public string UrlCnh { get; set; }
        public string TipoCnh { get; set; }
        public virtual List<LocacaoMoto>? Locacoes { get; set; }
    }
}
