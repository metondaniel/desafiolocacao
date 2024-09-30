using FluentValidation;
using Locacao.Service;
using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Repositories;
using Locacao.Service.Interfaces.Services;
using Locacao.Service.Validators;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Locacao.Test
{
    public class MotoServiceTests
    {
        private readonly Mock<IMotoRepository> _motoRepositoryMock;
        private readonly Mock<IMessageQueueService> _messageQueueMock;
        private readonly MotoService _motoService;
        private readonly Mock<IValidator<Moto>> _motoValidatorMock;
        private readonly Mock<ILocacaoRepository> _locacaoRepositoryMock;

        public MotoServiceTests()
        {
            _motoRepositoryMock = new Mock<IMotoRepository>();
            _messageQueueMock = new Mock<IMessageQueueService>();
            _motoValidatorMock = new Mock<IValidator<Moto>>();
            _locacaoRepositoryMock = new Mock<ILocacaoRepository>();

            _motoService = new MotoService(
                _motoRepositoryMock.Object,
                _motoValidatorMock.Object,
                _locacaoRepositoryMock.Object,
                _messageQueueMock.Object
            );
        }

        [Fact]
        public async Task CadastrarMoto_Sucesso_DevePublicarMensagemSeAno2024()
        {
            // Arrange
            var moto = new Moto { Id = 1, Ano = 2024, Placa = "ABC-1234" };
            _motoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Moto>())).ReturnsAsync(moto);
            _motoValidatorMock.Setup(v => v.Validate(It.IsAny<Moto>())).Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _motoService.CadastrarMotoAsync(moto);

            // Assert
            Assert.NotNull(result);
            _messageQueueMock.Verify(q => q.PublicarEventoMotoCadastrada(It.IsAny<Moto>()), Times.Once);
        }

        [Fact]
        public async Task CadastrarMoto_Falha_PlacaJaExiste()
        {
            // Arrange
            var moto = new Moto { Id = 1, Placa = "ABC-1234" };
            _motoRepositoryMock.Setup(repo => repo.ExistePlacaAsync(It.IsAny<string>())).ReturnsAsync(true);
            _motoValidatorMock.Setup(v => v.Validate(It.IsAny<Moto>())).Returns(new FluentValidation.Results.ValidationResult());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _motoService.CadastrarMotoAsync(moto));
        }

        [Fact]
        public async Task RemoverMoto_Sucesso_DeveRemoverMoto()
        {
            // Arrange
            var moto = new Moto { Id = 1 };
            _motoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(moto);
            _locacaoRepositoryMock.Setup(repo => repo.ExisteLocacaoAtiva(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            await _motoService.RemoverMotoAsync(moto.Id);

            // Assert
            _motoRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task RemoverMoto_Falha_ComLocacaoAtiva()
        {
            // Arrange
            _locacaoRepositoryMock.Setup(repo => repo.ExisteLocacaoAtiva(It.IsAny<int>())).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _motoService.RemoverMotoAsync(1));
        }
    }
}
