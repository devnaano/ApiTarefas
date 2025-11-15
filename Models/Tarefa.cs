using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTarefas.Enums;
using ApiTarefas.Models.Exceptions;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApiTarefas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataConclusao { get; private set; }
        public StatusTarefa Status { get; private set; }


        public Tarefa(string titulo, string descricao)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                throw new RegraDeNegocioException("Título da tarefa é obrigatório.");
            if (titulo.Length > 100)
                throw new RegraDeNegocioException("Título deve ter no máximo 100 caracteres.");
            if (descricao?.Length > 500)
                throw new RegraDeNegocioException("Descrição deve ter no máximo 500 caracteres.");

            Titulo = titulo!;
            Descricao = descricao ?? string.Empty;
            DataCriacao = DateTime.UtcNow;
            Status = StatusTarefa.Pendente;
        }

        public void Atualizar(string? titulo, string? descricao)
        {
            if (titulo != null)
            {
                if (!string.IsNullOrWhiteSpace(titulo) && titulo.Length <= 100)
                    Titulo = titulo;

                if (!string.IsNullOrWhiteSpace(descricao) && descricao.Length <= 500)
                    Descricao = descricao;

                Titulo = titulo;
            }

            if (descricao != null)
            {
                if (descricao.Length > 500)
                    throw new RegraDeNegocioException("Descrição deve ter no máximo 500 caracteres.");

                Descricao = descricao;
            }
        }

        public bool PodeAlterarPara(StatusTarefa novoStatus)
        {
            return (int)novoStatus > (int)Status;
        }

        public void AlterarStatus(StatusTarefa novoStatus)
        {
            if (!PodeAlterarPara(novoStatus))
                throw new InvalidOperationException($"Não é permidito alterar de {Status} para {novoStatus}");

            Status = novoStatus;

            if (novoStatus == StatusTarefa.Concluida)
                DataConclusao = DateTime.UtcNow;
        }

        public void Iniciar()
        {
            if (Status == StatusTarefa.EmAndamento)
                throw new RegraDeNegocioException("Tarefa já esta em andamento.");

            if (Status == StatusTarefa.Concluida)
                throw new RegraDeNegocioException("Não é possível inicia uma tarefa concluída.");

            if (Status != StatusTarefa.Pendente)
                throw new RegraDeNegocioException("Estado atual não permite iniciar a tarefa.");

            Status = StatusTarefa.EmAndamento;
        }

        public void Concluir()
        {
            if (Status == StatusTarefa.Concluida)
                throw new RegraDeNegocioException("Tarefa já esta concluída.");

            if (Status == StatusTarefa.Pendente)
                throw new RegraDeNegocioException("Não é possível concluir uma tarefa que não foi iniciada.");

            Status = StatusTarefa.Concluida;
            DataConclusao = DateTime.UtcNow;
        }

        public bool PodeExcluir()
        {
            return Status != StatusTarefa.Concluida;
        }
    }
}