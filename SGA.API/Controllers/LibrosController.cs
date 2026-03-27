using Microsoft.AspNetCore.Mvc;
using SGA.Application.DTOs;
using SGA.Application.Interfaces;

namespace SGA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroService _libroService;

        public LibrosController(ILibroService libroService)
        {
            _libroService = libroService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var libros = await _libroService.GetAllAsync();
            return Ok(libros);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var libro = await _libroService.GetByIdAsync(id);
                return Ok(libro);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchTerm)
        {
            var libros = await _libroService.SearchAsync(searchTerm);
            return Ok(libros);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLibroDto dto)
        {
            try
            {
                var libro = await _libroService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = libro.Id }, libro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLibroDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("El id no coincide");

                var libro = await _libroService.UpdateAsync(dto);
                return Ok(libro);
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
                var result = await _libroService.DeleteAsync(id);
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
