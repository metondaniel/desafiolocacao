using FluentValidation;
using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Locacao.Controllers
{
    [ApiController]
    [Route("api/entregadores")]
    public class EntregadoresController : ControllerBase
    {
        private readonly IEntregadorService _entregadorService;

        public EntregadoresController(IEntregadorService entregadorService)
        {
            _entregadorService = entregadorService;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarEntregador([FromBody] Entregador entregador)
        {
            try
            {
                var novoEntregador = await _entregadorService.CadastrarEntregadorAsync(entregador);
                return CreatedAtAction(nameof(ConsultarEntregadorPorId), new { id = novoEntregador.Id }, novoEntregador);
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

        [HttpPost("uploadcnh/{entregadorId}")]
        public async Task<IActionResult> UploadCNH(int entregadorId, IFormFile file)
        {
            try
            {
                using var stream = file.OpenReadStream();
                await _entregadorService.EnviarCnhAsync(entregadorId, file);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ConsultarEntregadorPorId(int id)
        {
            var entregador = await _entregadorService.GetEntregadorByIdAsync(id);
            if (entregador == null) return NotFound();
            return Ok(entregador);
        }
    }

}
