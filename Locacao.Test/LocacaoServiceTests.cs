using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Repositories;
using Locacao.Service;
using Moq;
using Xunit;
using Locacao.Service.Validators;
using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace Locacao.Test
{
    public class LocacaoServiceTests
    {
        private readonly Mock<ILocacaoRepository> _locacaoRepositoryMock;
        private readonly Mock<IEntregadorRepository> _entregadorRepositoryMock;
        private readonly LocacaoService _locacaoService;
        private readonly Mock<IValidator<LocacaoMoto>> _locacaoValidatorMock;

        public LocacaoServiceTests()
        {
            _locacaoRepositoryMock = new Mock<ILocacaoRepository>();
            _entregadorRepositoryMock = new Mock<IEntregadorRepository>();
            _locacaoValidatorMock = new Mock<IValidator<LocacaoMoto>>();

            _locacaoService = new LocacaoService(_locacaoRepositoryMock.Object, _locacaoValidatorMock.Object, _entregadorRepositoryMock.Object);
        }

        [Fact]
        public async Task AlugarMoto_Sucesso_DeveCalcularValorTotal()
        {
            // Arrange
            var locacao = new LocacaoMoto { MotoId = 1, Entregador = new Entregador { TipoCnh = "A" }, DataInicio = DateTime.Now, DataTerminoPrevisto = DateTime.Now.AddDays(7) };
            _locacaoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<LocacaoMoto>())).ReturnsAsync(locacao);
            _entregadorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Entregador { TipoCnh = "A" });
            _locacaoValidatorMock.Setup(v => v.Validate(It.IsAny<LocacaoMoto>())).Returns(new ValidationResult());

            // Act
            var result = await _locacaoService.AlugarMotoAsync(locacao);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, (result.DataTerminoPrevisto - result.DataInicio).Days);
        }

        [Fact]
        public async Task AlugarMoto_Falha_CnhInvalida()
        {
            // Arrange
            var locacao = new LocacaoMoto { MotoId = 1, Entregador = new Entregador { TipoCnh = "B" } };
            _entregadorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Entregador { TipoCnh = "B" });
            _locacaoValidatorMock.Setup(v => v.Validate(It.IsAny<LocacaoMoto>())).Returns(new ValidationResult());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _locacaoService.AlugarMotoAsync(locacao));
        }

        [Fact]
        public async Task CalcularValorLocacao_DevolucaoAntesDoPrevisto_DeveAplicarMulta()
        {
            // Arrange
            var locacao = new LocacaoMoto
            {
                DataInicio = DateTime.Now.AddDays(-5),
                DataTerminoPrevisto = DateTime.Now.AddDays(2),
                ValorTotal = 300
            };
            _locacaoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(locacao);

            // Act
            var valorFinal = await _locacaoService.CalcularValorLocacaoAsync(1, DateTime.Now);

            // Assert
            Assert.Equal(360, valorFinal); 
        }

        [Fact]
        public async Task CalcularValorLocacao_DevolucaoAposPrevisto_DeveAplicarDiariaExtra()
        {
            // Arrange
            var locacao = new LocacaoMoto
            {
                DataInicio = DateTime.Now.AddDays(-10),
                DataTerminoPrevisto = DateTime.Now.AddDays(-3),
                ValorTotal = 220
            };
            _locacaoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(locacao);

            // Act
            var valorFinal = await _locacaoService.CalcularValorLocacaoAsync(1, DateTime.Now);

            // Assert
            Assert.Equal(370, valorFinal); // Diária extra de R$50,00 por 3 dias extras
        }
    }
}
