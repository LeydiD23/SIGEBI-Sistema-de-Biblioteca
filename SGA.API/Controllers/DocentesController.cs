using Microsoft.AspNetCore.Mvc;
using SGA.Application.DTOs;
using SGA.Application.Interfaces;

namespace SGA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocentesController : ControllerBase
    {
        private readonly IDocenteService _docenteService;

        public DocentesController(IDocenteService docenteService)
        {
            _docenteService = docenteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var docentes = await _docenteService.GetAllAsync();
            return Ok(docentes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var docente = await _docenteService.GetByIdAsync(id);
                return Ok(docente);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("cedula/{cedula}")]
        public async Task<IActionResult> GetByCedula(string cedula)
        {
            try
            {
                var docente = await _docenteService.GetByCedulaAsync(cedula);
                return Ok(docente);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDocenteDto dto)
        {
            try
            {
                var docente = await _docenteService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = docente.Id }, docente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDocenteDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("El id no coincide");

                var docente = await _docenteService.UpdateAsync(dto);
                return Ok(docente);
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
                var result = await _docenteService.DeleteAsync(id);
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
