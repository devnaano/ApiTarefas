using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTarefas.Enums;

namespace ApiTarefas.Dtos.Tarefa
{
    public class TarefaResumoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public StatusTarefa Status { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}