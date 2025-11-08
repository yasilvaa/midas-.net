using Microsoft.AspNetCore.Mvc;
using Midas.Infrastructure.Persistence.Entities;
using Midas.Infrastructure.Persistence.Repositories;
using Midas.API.DTOs;

namespace Midas.API.Controllers
{
    [ApiController]
    [Route("api/receitas")]
    public class ReceitaController : ControllerBase
    {
        private readonly IReceitaRepository _receitaRepository;
        private readonly ILogger<ReceitaController> _logger;

        public ReceitaController(IReceitaRepository receitaRepository, ILogger<ReceitaController> logger)
        {
            _receitaRepository = receitaRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Receita>>> GetAll()
        {
            try
            {
                var receitas = await _receitaRepository.GetAllAsync();
                return Ok(receitas.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Receita>> GetById(int id)
        {
            try
            {
                var receita = await _receitaRepository.GetByIdAsync(id);
                if (receita == null)
                    return NotFound();

                return Ok(receita);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Receita>> Create([FromBody] CreateReceitaDTO createReceitaDTO)
        {
            try
            {
                if (createReceitaDTO == null)
                    return BadRequest("Dados da receita são obrigatórios");

                if (string.IsNullOrEmpty(createReceitaDTO.Titulo))
                    return BadRequest("Título é obrigatório");

                if (createReceitaDTO.UsuarioId <= 0)
                    return BadRequest("UsuarioId é obrigatório");

                if (createReceitaDTO.Valor <= 0)
                    return BadRequest("Valor deve ser maior que zero");

                var receita = new Receita
                {
                    UsuarioId = createReceitaDTO.UsuarioId,
                    Titulo = createReceitaDTO.Titulo,
                    Data = createReceitaDTO.Data,
                    Valor = createReceitaDTO.Valor,
                    Fixo = NormalizeFixoValue(createReceitaDTO.Fixo)
                };

                var result = await _receitaRepository.CreateAsync(receita);

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Receita receita)
        {
            try
            {
                if (receita == null)
                    return BadRequest("Dados da receita são obrigatórios");

                if (id != receita.Id)
                    return BadRequest("ID do parâmetro não confere com o ID da receita");
                receita.Fixo = NormalizeFixoValue(receita.Fixo);

                var existingReceita = await _receitaRepository.GetByIdAsync(id);
                if (existingReceita == null)
                    return NotFound();

                await _receitaRepository.UpdateAsync(receita);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var receita = await _receitaRepository.GetByIdAsync(id);
                if (receita == null)
                    return NotFound();

                await _receitaRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<Receita>>> Search([FromQuery] ReceitaSearchParameters parameters)
        {
            try
            {
                var result = await _receitaRepository.SearchAsync(parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar receitas com parâmetros: {@Parameters}", parameters);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        private char NormalizeFixoValue(char fixo)
        {
            return char.ToUpper(fixo) == 'T' ? 'T' : 'F';
        }
    }
}
