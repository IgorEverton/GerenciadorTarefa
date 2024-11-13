//using Xunit;
//using Moq;
//using GerenciadorTarefas.Repository.Interface;
//using GerenciadorTarefas.Service;
//using GerenciadorTarefas.Service.Interface;
//using System.Threading.Tasks;
//using System;
//using GerenciadorTarefas.Model;
//using FluentAssertions;
//using GerenciadorTarefas.Communication.Request;
//using Azure.Core;

//namespace GerenciadorTarefas.Tests.Service
//{
//    public class TarefaServiceTest
//    {
//        private readonly Mock<ITarefaRepository> _mockRespository;
//        private readonly TarefaService _tarefaService;
//        public TarefaServiceTest()
//        {
//            _mockRespository = new Mock<ITarefaRepository>();
//            _tarefaService = new TarefaService( _mockRespository.Object );
//        }
//        [Fact]
//        public async Task CreateAsync_DeveChamarRepositoryERetornarTarefa()
//        {
//            var tarefa = new Tarefa
//            {
//                Id = Guid.NewGuid(),
//                Titulo = "Varrer o quintal",
//                Descricao = "Varrer o quintal toda a semana",
//                DataCriacao = DateTime.Now,
//                DataFinalizacao = DateTime.Now.AddDays(3),
//                Status = false
//            };

//            var tarefaTest = new RequestTarefa
//            {
//                Id = Guid.NewGuid(),
//                Titulo = "Varrer o quintal",
//                Descricao = "Varrer o quintal toda a semana",
//                DataCriacao = DateTime.Now,
//                DataFinalizacao = DateTime.Now.AddDays(3),
//                Status = false
//            };
//            _mockRespository.Setup(resp => resp.CreateAsync(tarefaTest)).ReturnsAsync(tarefa);

//            var result = await _tarefaService.CreateAsync(tarefaTest);

//            result.Should().NotBeNull();
//        }

//    }
//}
