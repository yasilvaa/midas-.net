using Microsoft.AspNetCore.Mvc;
using Midas.Infrastructure.Persistence.Entities;
using Midas.Infrastructure.Persistence.Repositories;

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
        public async Task<ActionResult<Receita>> Create([FromBody] Receita receita)
        {
            try
            {

                if (receita == null)
                    return BadRequest("Dados da receita são obrigatórios");

                if (string.IsNullOrEmpty(receita.Titulo))
                    return BadRequest("Título é obrigatório");

                if (receita.UsuarioId <= 0)
                    return BadRequest("UsuarioId é obrigatório");

                if (receita.Valor <= 0)
                    return BadRequest("Valor deve ser maior que zero");

                if (string.IsNullOrEmpty(receita.Fixo))
                    receita.Fixo = "F";
                else if (receita.Fixo != "T" && receita.Fixo != "F")
                    return BadRequest("Campo Fixo deve ser 'T' ou 'F'");

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

                // Validar campo Fixo
                if (string.IsNullOrEmpty(receita.Fixo))
                    receita.Fixo = "F";
                else if (receita.Fixo != "T" && receita.Fixo != "F")
                    return BadRequest("Campo Fixo deve ser 'T' ou 'F'");

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
    }
}
