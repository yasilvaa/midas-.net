using Microsoft.AspNetCore.Mvc;
using Midas.Infrastructure.Persistence.Entities;
using Midas.Infrastructure.Persistence.Repositories;
using Midas.API.DTOs;

namespace Midas.Controllers
{
    [ApiController]
    [Route("api/cofrinhos")]
    public class CofrinhoController : ControllerBase
    {
        private readonly ICofrinhoRepository _cofrinhoRepository;

        public CofrinhoController(ICofrinhoRepository cofrinhoRepository)
        {
            _cofrinhoRepository = cofrinhoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cofrinho>>> GetAll()
        {
            return await _cofrinhoRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cofrinho>> GetById(int id)
        {
            var cofrinho = await _cofrinhoRepository.GetByIdAsync(id);
            if (cofrinho == null)
                return NotFound();

            return cofrinho;
        }

        [HttpPost]
        public async Task<ActionResult<Cofrinho>> Create([FromBody] CreateCofrinhoDTO createCofrinhoDTO)
        {
            if (createCofrinhoDTO == null)
                return BadRequest("Dados do cofrinho são obrigatórios");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cofrinho = new Cofrinho
            {
                UsuarioId = createCofrinhoDTO.UsuarioId,
                Titulo = createCofrinhoDTO.Titulo,
                Meta = createCofrinhoDTO.Meta,
                Atingido = createCofrinhoDTO.Atingido,
                Aplicado = NormalizeAplicadoValue(createCofrinhoDTO.Aplicado)
            };

            try
            {
                var result = await _cofrinhoRepository.AddAsync(cofrinho);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar cofrinho: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cofrinho cofrinho)
        {
            if (id != cofrinho.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            cofrinho.Aplicado = NormalizeAplicadoValue(cofrinho.Aplicado);

            try
            {
                await _cofrinhoRepository.UpdateAsync(cofrinho);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar cofrinho: {ex.Message}");
            }
        }

        [HttpPut("{id}/progresso")]
        public async Task<IActionResult> AtualizarProgresso(int id, [FromBody] decimal valorAtingido)
        {
            var result = await _cofrinhoRepository.AtualizarProgresso(id, valorAtingido);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cofrinhoRepository.DeleteAsync(id);
            return NoContent();
        }

        private char NormalizeAplicadoValue(char aplicado)
        {
            return char.ToUpper(aplicado) == 'T' ? 'T' : 'F';
        }
    }
}
