using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Communication.Response
{
    public class ResponseUsuario
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; }
    }

}
