using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiTarefas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTarefas.Repositories.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Titulo)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(t => t.Descricao)
                    .HasMaxLength(500);

                entity.Property(t => t.Status)
                    //Salva o Enum como Texto e não como numero
                    .HasConversion<string>();
            });
        }
    }
}