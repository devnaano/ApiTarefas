using ApiTarefas.Dtos.Tarefa;
using ApiTarefas.Enums;
using ApiTarefas.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiTarefas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;

        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet]
        public async Task<IActionResult> PegarTodas()
        {
            var tarefas = await _tarefaService.PegarTodasAsync();
            return Ok(tarefas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> PegarPorId(int id)
        {
            var tarefa = await _tarefaService.PegarPorIdAsync(id);
            return Ok(tarefa);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> PegarPorStatus(StatusTarefa status)
        {
            var tarefas = await _tarefaService.PegarPorStatusAsync(status);
            return Ok(tarefas);
        }

        [HttpGet("periodo")]
        public async Task<IActionResult> PegarPorPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            if (inicio > fim)
                return BadRequest("A data de início não pode ser maior que a data final.");

            var tarefas = await _tarefaService.PegarPorPeriodoAsync(inicio, fim);
            return Ok(tarefas);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarTarefaDto dto)
        {
            var tarefa = await _tarefaService.CriarAsync(dto);
            return CreatedAtAction(nameof(PegarPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarTarefaDto dto)
        {
            var tarefa = await _tarefaService.EditarAsync(id, dto);
            return Ok(tarefa);
        }

        [HttpPatch("{id}/iniciar")]
        public async Task<IActionResult> Iniciar(int id)
        {
            await _tarefaService.IniciarAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/concluir")]
        public async Task<IActionResult> Concluir(int id)
        {
            await _tarefaService.ConcluirAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            await _tarefaService.RemoverAsync(id);
            return NoContent();
        }
    }
}