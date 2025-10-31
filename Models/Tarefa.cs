using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTarefas.Enums;

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
                throw new ArgumentException("Título da tarefa é obrigatório.");
            if (titulo.Length > 100)
                throw new ArgumentException("Título deve ter no máximo 100 caracteres.");
            if (descricao?.Length > 500)
                throw new ArgumentException("Descrição deve ter no máximo 500 caracteres.");

            Titulo = titulo;
            Descricao = descricao ?? string.Empty;
            DataCriacao = DateTime.UtcNow;
            Status = StatusTarefa.Pendente;
        }

        public void Concluir()
        {
            if (Status == StatusTarefa.Concluida)
                throw new InvalidOperationException("Tarefa já esta concluída.");

            Status = StatusTarefa.Concluida;
            DataConclusao = DateTime.UtcNow;
        }

        public void Atualizar(string titulo, string descricao)
        {
            if (!string.IsNullOrWhiteSpace(titulo) && titulo.Length <= 100)
                Titulo = titulo;

            if (!string.IsNullOrWhiteSpace(descricao) && descricao.Length <= 500)
                Descricao = descricao;
        }

        public bool PodeAlterarPara(StatusTarefa novoStatus)
        {
            return (int)novoStatus > (int)Status;
        }

        public void AlteraraStatus(StatusTarefa novoStatus)
        {
            if (!PodeAlterarPara(novoStatus))
                throw new InvalidOperationException($"Não é permidito alterar de {Status} para {novoStatus}");

            Status = novoStatus;
        }

        public void Iniciar()
        {
            if (Status == StatusTarefa.Pendente)
                Status = StatusTarefa.EmAndamento;
        }

        public bool PodeExcluir()
        {
            if (Status != StatusTarefa.Concluida)
                return true;
            else
                return false;
        }
    }
}