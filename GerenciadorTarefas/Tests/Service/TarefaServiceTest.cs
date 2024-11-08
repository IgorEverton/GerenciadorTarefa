using Xunit;
using Moq;
using GerenciadorTarefas.Repository.Interface;
using GerenciadorTarefas.Service;
using GerenciadorTarefas.Service.Interface;

namespace GerenciadorTarefas.Tests.Service
{
    public class TarefaServiceTest
    {
        private readonly Mock<ITarefaRepository> _mockRespository;
        private readonly TarefaService _TarefaService;
        public TarefaServiceTest()
        {
            _mockRespository = new Mock<ITarefaRepository>();
            _TarefaService = new TarefaService( _mockRespository.Object );
        }
    }
}
