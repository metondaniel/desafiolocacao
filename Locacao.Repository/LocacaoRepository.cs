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
    public class LocacaoRepository : BaseRepository<LocacaoMoto>, ILocacaoRepository
    {
        private readonly ApplicationDbContext _context;

        public LocacaoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteLocacaoAtiva(int motoId)
        {
            return await _dbSet.AnyAsync(l => l.MotoId == motoId && l.DataTermino == null && l.DataTerminoPrevisto >= DateTime.Now);
        }

        public async Task<IEnumerable<LocacaoMoto>> ObterLocacoesPorEntregadorAsync(int entregadorId)
        {
            return await _dbSet.Where(l => l.EntregadorId == entregadorId).ToListAsync();
        }
    }

}
