using ApiTarefas.Dtos.Tarefa;
using ApiTarefas.Enums;
using ApiTarefas.Interfaces;
using ApiTarefas.Models;
using ApiTarefas.Models.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;

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

            return tarefas
                .OrderByDescending(t => t.DataCriacao)
                .Select(t => new TarefaResumoDto
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Status = t.Status,
                    DataCriacao = t.DataCriacao
                });
        }

        public async Task<TarefaDetalheDto?> PegarPorIdAsync(int id)
        {
            var tarefa = await _tarefaRepository.PegarPorIdAsync(id)
                ?? throw new NotFoundException("Tarefa não encontrada.");

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
            if (string.IsNullOrWhiteSpace(dto.Titulo))
                throw new RegraDeNegocioException("O título da tarefa é obrigatório.");

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
            var tarefa = await _tarefaRepository.PegarPorIdAsync(id)
                ?? throw new NotFoundException("Tarefa não encontrada");

            if (tarefa.Status == StatusTarefa.Concluida)
                throw new RegraDeNegocioException("Não é possível editar uma tarefa concluída.");

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

        public async Task RemoverAsync(int id)
        {
            var tarefa = await _tarefaRepository.PegarPorIdAsync(id)
                ?? throw new NotFoundException("Tarefa não encontrada.");

            if (!tarefa.PodeExcluir())
                throw new RegraDeNegocioException("Não é possível remover uma tarefa concluída.");

            await _tarefaRepository.RemoverAsync(tarefa);
        }

        public async Task IniciarAsync(int id)
        {
            var tarefa = await _tarefaRepository.PegarPorIdAsync(id)
                ?? throw new NotFoundException("Tarefa não encontrada.");


            tarefa.Iniciar();
            await _tarefaRepository.AtualizarAsync(tarefa);
        }

        public async Task ConcluirAsync(int id)
        {
            var tarefa = await _tarefaRepository.PegarPorIdAsync(id)
                ?? throw new NotFoundException("Tarefa não encontrada.");

            tarefa.Concluir();
            await _tarefaRepository.AtualizarAsync(tarefa);
        }
    }
}