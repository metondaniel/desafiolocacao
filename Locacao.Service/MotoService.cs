using FluentValidation;
using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Repositories;
using Locacao.Service.Interfaces.Services;

namespace Locacao.Service
{
    public class MotoService : IMotoService
    {
        private readonly IMotoRepository _motoRepository;
        private readonly ILocacaoRepository _locacaoRepository;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IValidator<Moto> _motoValidator;

        public MotoService(IMotoRepository motoRepository, IValidator<Moto> motoValidator, ILocacaoRepository locacaoRepository, IMessageQueueService messageQueueService)
        {
            _motoRepository = motoRepository;
            _motoValidator = motoValidator;
            _locacaoRepository = locacaoRepository;
            _messageQueueService = messageQueueService; 
        }

        public async Task<Moto> CadastrarMotoAsync(Moto moto)
        {
            // Valida os dados da moto
            var result = _motoValidator.Validate(moto);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            // Verifica se a placa já existe
            if (await _motoRepository.ExistePlacaAsync(moto.Placa))
                throw new Exception("Placa já cadastrada.");

            // Cadastra a moto
            var motoCriada = await _motoRepository.AddAsync(moto);
            _messageQueueService.PublicarEventoMotoCadastrada(motoCriada);
            return motoCriada;
        }

        public async Task<IEnumerable<Moto>> ConsultarMotosAsync()
        {
            return await _motoRepository.GetAllAsync();
        }

        public async Task<Moto> ConsultarMotoPorIdAsync(int id)
        {
            return await _motoRepository.GetByIdAsync(id);
        }

        public async Task<Moto> AtualizarMotoAsync(int id, string novaPlaca)
        {
            // Busca a moto existente
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null)
                throw new Exception("Moto não encontrada.");

            // Atualiza a placa
            moto.Placa = novaPlaca;
            return await _motoRepository.UpdateAsync(moto);
        }

        public async Task RemoverMotoAsync(int id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null)
                throw new Exception("Moto não encontrada.");

            if (await _locacaoRepository.ExisteLocacaoAtiva(id))
                throw new Exception("Moto com locação ativa não pode ser removida.");

            await _motoRepository.DeleteAsync(id);
        }
    }

}
