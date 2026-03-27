using Microsoft.AspNetCore.Mvc;
using SGA.Application.DTOs;
using SGA.Application.Interfaces;

namespace SGA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly IEstudianteService _estudianteService;

        public EstudiantesController(IEstudianteService estudianteService)
        {
            _estudianteService = estudianteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var estudiantes = await _estudianteService.GetAllAsync();
            return Ok(estudiantes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var estudiante = await _estudianteService.GetByIdAsync(id);
                return Ok(estudiante);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("matricula/{matricula}")]
        public async Task<IActionResult> GetByMatricula(string matricula)
        {
            try
            {
                var estudiante = await _estudianteService.GetByMatriculaAsync(matricula);
                return Ok(estudiante);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEstudianteDto dto)
        {
            try
            {
                var estudiante = await _estudianteService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = estudiante.Id }, estudiante);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEstudianteDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("El id no coincide");

                var estudiante = await _estudianteService.UpdateAsync(dto);
                return Ok(estudiante);
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
                var result = await _estudianteService.DeleteAsync(id);
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
