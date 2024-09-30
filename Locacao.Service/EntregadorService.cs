using FluentValidation;
using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Repositories;
using Locacao.Service.Interfaces.Services;
using Locacao.Service.Validators;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service
{
    public class EntregadorService : IEntregadorService
    {
        private readonly IEntregadorRepository _entregadorRepository;
        private readonly IImageUploadService _imageUploadService;
        private readonly IValidator<Entregador> _entregadorValidator;

        public EntregadorService(IEntregadorRepository entregadorRepository, IImageUploadService imageUploadService, IValidator<Entregador> entregadorValidator)
        {
            _entregadorRepository = entregadorRepository;
            _imageUploadService = imageUploadService;
            _entregadorValidator = entregadorValidator;
        }

        public async Task<Entregador> CadastrarEntregadorAsync(Entregador entregador)
        {
            // Valida os dados do entregador
            var result = _entregadorValidator.Validate(entregador);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            if (!EntregadorValidator.CnpjValidator.IsValidCnpj(entregador.Cnpj))
                throw new Exception("CNPJ inválido.");

            // Verifica se CNPJ ou CNH já existem
            if (await _entregadorRepository.ExisteCnpjAsync(entregador.Cnpj))
                throw new Exception("CNPJ já cadastrado.");

            if (await _entregadorRepository.ExisteCnhAsync(entregador.NumeroCnh))
                throw new Exception("CNH já cadastrada.");

            return await _entregadorRepository.AddAsync(entregador);
        }

        public async Task EnviarCnhAsync(int entregadorId, IFormFile cnhImage)
        {
            var entregador = await _entregadorRepository.GetByIdAsync(entregadorId);
            if (entregador == null)
                throw new Exception("Entregador não encontrado.");

            var imageUrl = await _imageUploadService.UploadImageAsync(cnhImage);

            entregador.UrlCnh = imageUrl;
            await _entregadorRepository.UpdateAsync(entregador);
        }

        public async Task<Entregador> GetEntregadorByIdAsync(int id)
        {
            return await _entregadorRepository.GetByIdAsync(id);
        }
    }


}
