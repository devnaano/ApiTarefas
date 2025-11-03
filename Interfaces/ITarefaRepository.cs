using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTarefas.Enums;
using ApiTarefas.Models;

namespace ApiTarefas.Interfaces
{
    public interface ITarefaRepository
    {
        Task<IEnumerable<Tarefa>> PegarTodasAsync();
        Task<Tarefa?> PegarPorIdAsync(int id);
        Task CriarAsync(Tarefa tarefa);
        Task EditarAsync(Tarefa tarefa);
        Task RemoverAsync(Tarefa tarefa);


        Task<IEnumerable<Tarefa>> PegarPorStatusAsync(StatusTarefa status);
        Task<IEnumerable<Tarefa>> PegarPorPeriodoAsync(DateTime inicio, DateTime fim);


        // Task<Tarefa?> ConcluirTarefaAsync(int id);
        // Task<Tarefa?> AlterarStatusAsync(int id, StatusTarefa novoStatus);

    }
}