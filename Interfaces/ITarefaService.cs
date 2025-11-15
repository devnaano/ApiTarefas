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
        Task<IEnumerable<TarefaResumoDto>> PegarTodasAsync();
        Task<TarefaDetalheDto?> PegarPorIdAsync(int id);
        Task<IEnumerable<TarefaResumoDto>> PegarPorStatusAsync(StatusTarefa status);
        Task<IEnumerable<TarefaResumoDto>> PegarPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<TarefaDetalheDto> CriarAsync(CriarTarefaDto dto);
        Task<TarefaDetalheDto?> EditarAsync(int id, AtualizarTarefaDto dto);
        Task RemoverAsync(int id);
        Task IniciarAsync(int id);
        Task ConcluirAsync(int id);
    }
}