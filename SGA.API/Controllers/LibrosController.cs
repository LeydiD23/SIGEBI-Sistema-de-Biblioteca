using System.Runtime.InteropServices;


using Microsoft.AspNetCore.Mvc;
using SGA.Domain.Entitys;
using SGA.Domain.Repository;



namespace SGA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroRepository _repository;

        public LibrosController(ILibroRepository repository)
        {
            _repository = repository;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var libros = await _repository.GetAllAsync();
            return Ok(libros);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var libro = await _repository.GetByIdAsync(id);

            if (libro == null)
                return NotFound($"No existe el libro con id {id}");

            return Ok(libro);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Libro libro)
        {
            if (libro == null)
                return BadRequest();

            await _repository.AddAsync(libro);

            return CreatedAtAction(nameof(Get), new { id = libro.Id }, libro);
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Libro libro)
        {
            if (id != libro.Id)
                return BadRequest("El id no coincide");

            var existente = await _repository.GetByIdAsync(id);

            if (existente == null)
                return NotFound();

            await _repository.UpdateAsync(libro);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existente = await _repository.GetByIdAsync(id);

            if (existente == null)
                return NotFound();

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
