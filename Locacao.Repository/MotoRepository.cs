using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Repository
{
    public class MotoRepository : BaseRepository<Moto>, IMotoRepository
    {
        public MotoRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> ExistePlacaAsync(string placa)
        {
            return await _dbSet.AnyAsync(m => m.Placa == placa);
        }
    }

}
