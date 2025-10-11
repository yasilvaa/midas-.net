using Microsoft.AspNetCore.Mvc;
using Midas.Infrastructure.Persistence.Entities;
using Midas.Infrastructure.Persistence.Repositories;

namespace Midas.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioRepository usuarioRepository, ILogger<UsuarioController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetAll()
        {
            try
            {
                var usuarios = await _usuarioRepository.GetAllAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null)
                    return NotFound();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> Create([FromBody] Usuario usuario)
        {
            try
            {
                _logger.LogInformation("Tentando criar usuário: {Email}", usuario?.Email);

                if (usuario == null)
                    return BadRequest("Dados do usuário são obrigatórios");

                if (string.IsNullOrEmpty(usuario.Nome))
                    return BadRequest("Nome é obrigatório");

                if (string.IsNullOrEmpty(usuario.Email))
                    return BadRequest("Email é obrigatório");

                if (string.IsNullOrEmpty(usuario.Senha))
                    return BadRequest("Senha é obrigatória");

                if (await _usuarioRepository.EmailExistsAsync(usuario.Email))
                    return BadRequest("Email já está em uso");

                var result = await _usuarioRepository.AddAsync(usuario);
                
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (usuario == null)
                    return BadRequest("Dados do usuário são obrigatórios");

                if (id != usuario.Id)
                    return BadRequest("ID do parâmetro não confere com o ID do usuário");

                var existingUser = await _usuarioRepository.GetByIdAsync(id);
                if (existingUser == null)
                    return NotFound();

                await _usuarioRepository.UpdateAsync(usuario);
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
                var usuario = await _usuarioRepository.GetByIdAsync(id);
                if (usuario == null)
                    return NotFound();

                await _usuarioRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
