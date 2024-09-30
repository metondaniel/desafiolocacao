using FluentValidation;
using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Locacao.Controllers
{
    [ApiController]
    [Route("api/locacoes")]
    public class LocacoesController : ControllerBase
    {
        private readonly ILocacaoService _locacaoService;

        public LocacoesController(ILocacaoService locacaoService)
        {
            _locacaoService = locacaoService;
        }

        [HttpPost]
        public async Task<IActionResult> AlugarMoto([FromBody] LocacaoMoto locacao)
        {
            try
            {
                var novaLocacao = await _locacaoService.AlugarMotoAsync(locacao);
                return CreatedAtAction(nameof(LocacaoMoto), new { id = novaLocacao.Id }, novaLocacao);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Erros = ex.Errors });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }

        [HttpPost("{id}/devolucao")]
        public async Task<IActionResult> DevolverMoto(int id, [FromBody] DateTime dataDevolucao)
        {
            try
            {
                var valorFinal = await _locacaoService.CalcularValorLocacaoAsync(id, dataDevolucao);
                return Ok(new { ValorFinal = valorFinal });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }
    }

}
