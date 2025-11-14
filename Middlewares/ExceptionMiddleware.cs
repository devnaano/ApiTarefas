using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ApiTarefas.Models.Exceptions;

namespace ApiTarefas.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await TratarExcecaoAsync(context, ex);
            }
        }

        private async Task TratarExcecaoAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var statusCode = ex switch
            {
                RegraDeNegocioException => HttpStatusCode.BadRequest,
                InvalidOperationException => HttpStatusCode.BadRequest,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            string mensagem = ex switch
            {
                RegraDeNegocioException or InvalidOperationException => ex.Message,
                KeyNotFoundException => "Recurso não encontrado.",
                _ => "Ocorreu um erro interno no servidor."
            };

            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(ex, "Erro interno inesperado.");
            else
                _logger.LogWarning(ex, "Exceção controlada.");

            var respostaErro = new ErroPadronizadoDto
            {
                Status = (int)statusCode,
                Mensagem = mensagem,
                Detalhes = (statusCode == HttpStatusCode.InternalServerError)
                    ? "Verifique os logs para mais detalhes."
                    : null
            };

            var json = JsonSerializer.Serialize(respostaErro, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(json);
        }

        public class ErroPadronizadoDto
        {
            public int Status { get; set; }
            public string Mensagem { get; set; } = string.Empty;
            public string? Detalhes { get; set; }
        }


    }
}