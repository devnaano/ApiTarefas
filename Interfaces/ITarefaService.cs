using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTarefas.Dtos.Tarefa;
using ApiTarefas.Enums;

namespace ApiTarefas.Interfaces
{
    public interface ITarefaService
    {
        Task<TarefaDetalheDto> ObterPorIdAsync(int id);
        Task<IEnumerable<TarefaResumoDto>> ObterPorStatusAsync(StatusTarefa status);
        Task<IEnumerable<TarefaResumoDto>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<TarefaDetalheDto> CriarAsync(CriarTarefaDto dto);
        Task<TarefaDetalheDto> EditarAsync(int id, AtualizarTarefaDto dto);
        Task<bool> IniciarAsync(int id);
        Task<bool> ConcluirAsync(int id);
        Task<bool> RemoverAsync(int id);
    }
}