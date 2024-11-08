using System;

namespace GerenciadorTarefas.Communication.Response
{
    public class ResponseTarefa
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataFinalizacao { get; set; }
        public bool Status { get; set; }
    }
}
