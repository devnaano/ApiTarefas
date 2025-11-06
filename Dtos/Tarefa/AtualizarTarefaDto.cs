using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTarefas.Dtos.Tarefa
{
    public class AtualizarTarefaDto
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
    }
}