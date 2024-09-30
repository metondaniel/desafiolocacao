using System;
using System.Threading.Tasks;
using FluentValidation;
using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("api/motos")]
    public class MotosController : ControllerBase
    {
        private readonly IMotoService _motoService;

        public MotosController(IMotoService motoService)
        {
            _motoService = motoService;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarMoto([FromBody] Moto moto)
        {
            try
            {
                var motoCriada = await _motoService.CadastrarMotoAsync(moto);
                return CreatedAtAction(nameof(ConsultarMotoPorId), new { id = motoCriada.Id }, motoCriada);
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

        [HttpGet]
        public async Task<IActionResult> ConsultarMotos()
        {
            var motos = await _motoService.ConsultarMotosAsync();
            return Ok(motos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ConsultarMotoPorId(int id)
        {
            var moto = await _motoService.ConsultarMotoPorIdAsync(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarMoto(int id, [FromBody] Moto moto)
        {
            try
            {
                var motoAtualizada = await _motoService.AtualizarMotoAsync(id, moto.Placa);
                return Ok(motoAtualizada);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverMoto(int id)
        {
            try
            {
                await _motoService.RemoverMotoAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }
    }

}
