using Microsoft.AspNetCore.Mvc;
using SGA.Application.DTOs;
using SGA.Application.Interfaces;

namespace SGA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BibliotecariosController : ControllerBase
    {
        private readonly IBibliotecarioService _bibliotecarioService;

        public BibliotecariosController(IBibliotecarioService bibliotecarioService)
        {
            _bibliotecarioService = bibliotecarioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bibliotecarios = await _bibliotecarioService.GetAllAsync();
            return Ok(bibliotecarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var bibliotecario = await _bibliotecarioService.GetByIdAsync(id);
                return Ok(bibliotecario);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBibliotecarioDto dto)
        {
            try
            {
                var bibliotecario = await _bibliotecarioService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = bibliotecario.Id }, bibliotecario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBibliotecarioDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("El id no coincide");

                var bibliotecario = await _bibliotecarioService.UpdateAsync(dto);
                return Ok(bibliotecario);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _bibliotecarioService.DeleteAsync(id);
                if (result)
                    return NoContent();
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
