using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Locacao.Repository
{
    public class EntregadorRepository : BaseRepository<Entregador>, IEntregadorRepository
    {
        public EntregadorRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExisteCnpjAsync(string cnpj)
        {
            return await _dbSet.AnyAsync(e => e.Cnpj == cnpj);
        }

        public async Task<bool> ExisteCnhAsync(string cnh)
        {
            return await _dbSet.AnyAsync(e => e.NumeroCnh == cnh);
        }
    }

}
