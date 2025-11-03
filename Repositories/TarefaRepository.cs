using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTarefas.Enums;
using ApiTarefas.Interfaces;
using ApiTarefas.Models;
using ApiTarefas.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiTarefas.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly AppDbContext _context;
        public TarefaRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CriarAsync(Tarefa tarefa)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tarefa>> PegarTodasAsync()
        {
            return await _context.Tarefas
                .AsNoTracking()
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task<Tarefa?> PegarPorIdAsync(int id)
        {
            return await _context.Tarefas
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task EditarAsync(Tarefa tarefa)
        {
            _context.Tarefas.Update(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Tarefa tarefa)
        {
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tarefa>> PegarPorStatusAsync(StatusTarefa status)
        {
            return await _context.Tarefas
                .AsNoTracking()
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tarefa>> PegarPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _context.Tarefas
                .AsNoTracking()
                .Where(t => t.DataCriacao >= inicio && t.DataCriacao <= fim)
                .OrderByDescending(t => t.DataCriacao)
                .ToListAsync();
        }


    }
}