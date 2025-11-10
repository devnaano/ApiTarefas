using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ApiTarefas.Dtos.Tarefa;
using ApiTarefas.Enums;
using ApiTarefas.Interfaces;
using ApiTarefas.Models;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace ApiTarefas.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly ITarefaRepository _tarefaRepository;
        public TarefaService(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        public async Task<IEnumerable<TarefaResumoDto>> PegarTodasAsync()
        {
            var tarefas = await _tarefaRepository.PegarTodasAsync();

            var resultado = tarefas
                .OrderByDescending(t => t.DataCriacao)
                .Select(t => new TarefaResumoDto
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Status = t.Status,
                    DataCriacao = t.DataCriacao
                });

            return resultado;
        }

        public async Task<TarefaDetalheDto?> PegarPorIdAsync(int id)
        {
            var tarefa = await _tarefaRepository.PegarPorIdAsync(id);
            if (tarefa is null) return null;

            return new TarefaDetalheDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status,
                DataCriacao = tarefa.DataCriacao,
                DataConclusao = tarefa.DataConclusao
            };
        }

        public async Task<IEnumerable<TarefaResumoDto>> PegarPorStatusAsync(StatusTarefa status)
        {
            var tarefas = await _tarefaRepository.PegarPorStatusAsync(status);

            return tarefas.Select(t => new TarefaResumoDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Status = t.Status,
                DataCriacao = t.DataCriacao
            });
        }


        public async Task<IEnumerable<TarefaResumoDto>> PegarPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            var tarefas = await _tarefaRepository.PegarPorPeriodoAsync(dataInicio, dataFim);

            return tarefas.Select(t => new TarefaResumoDto
            {
                Id = t.Id,
                Titulo = t.Titulo,
                Status = t.Status,
                DataCriacao = t.DataCriacao
            });
        }

        public async Task<TarefaDetalheDto> CriarAsync(CriarTarefaDto dto)
        {
            var tarefa = new Tarefa(dto.Titulo, dto.Descricao);
            await _tarefaRepository.CriarAsync(tarefa);

            return new TarefaDetalheDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status,
                DataCriacao = tarefa.DataCriacao,
                DataConclusao = tarefa.DataConclusao
            };
        }

        public async Task<TarefaDetalheDto?> EditarAsync(int id, AtualizarTarefaDto dto)
        {
            var tarefa = await _tarefaRepository.PegarPorIdAsync(id);
            if (tarefa is null) return null;

            if (tarefa.Status == StatusTarefa.Concluida)
                throw new InvalidOperationException("Não é possível editar uma tarefa concluída.");

            tarefa.Atualizar(dto.Titulo, dto.Descricao);
            await _tarefaRepository.AtualizarAsync(tarefa);

            return new TarefaDetalheDto
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Status = tarefa.Status,
                DataCriacao = tarefa.DataCriacao,
                DataConclusao = tarefa.DataConclusao
            };
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var tarefa = await _tarefaRepository.PegarPorIdAsync(id);
            if (tarefa == null) return false;

            if (!tarefa.PodeExcluir())
                throw new InvalidOperationException("Não é possível remover uma tarefa concluída.");

            await _tarefaRepository.RemoverAsync(tarefa);
            return true;
        }

        public async Task<bool> IniciarAsync(int id)
        {
            var tarefa = await _tarefaRepository.PegarPorIdAsync(id);
            if (tarefa == null) return false;

            tarefa.Iniciar();
            await _tarefaRepository.AtualizarAsync(tarefa);
            return true;
        }

        public async Task<bool> ConcluirAsync(int id)
        {
            var tarefa = await _tarefaRepository.PegarPorIdAsync(id);
            if (tarefa == null) return false;

            tarefa.Concluir();
            await _tarefaRepository.AtualizarAsync(tarefa);
            return true;
        }
    }
}