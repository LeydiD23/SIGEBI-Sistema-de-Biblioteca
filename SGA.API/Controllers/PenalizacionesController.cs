using Microsoft.AspNetCore.Mvc;
using SGA.Application.DTOs;
using SGA.Application.Interfaces;

namespace SGA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PenalizacionesController : ControllerBase
    {
        private readonly IPenalizacionService _penalizacionService;

        public PenalizacionesController(IPenalizacionService penalizacionService)
        {
            _penalizacionService = penalizacionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var penalizaciones = await _penalizacionService.GetAllAsync();
            return Ok(penalizaciones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var penalizacion = await _penalizacionService.GetByIdAsync(id);
                return Ok(penalizacion);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("usuario")]
        public async Task<IActionResult> GetByUsuarioId([FromQuery] int? estudianteId, [FromQuery] int? docenteId)
        {
            var penalizaciones = await _penalizacionService.GetByUsuarioIdAsync(estudianteId, docenteId);
            return Ok(penalizaciones);
        }

        [HttpGet("activas")]
        public async Task<IActionResult> GetActivas([FromQuery] int? estudianteId, [FromQuery] int? docenteId)
        {
            var penalizaciones = await _penalizacionService.GetActivasAsync(estudianteId, docenteId);
            return Ok(penalizaciones);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePenalizacionDto dto)
        {
            try
            {
                var penalizacion = await _penalizacionService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = penalizacion.Id }, penalizacion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePenalizacionDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("El id no coincide");

                var penalizacion = await _penalizacionService.UpdateAsync(dto);
                return Ok(penalizacion);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("pagar/{id}")]
        public async Task<IActionResult> RegistrarPago(int id)
        {
            try
            {
                var result = await _penalizacionService.RegistrarPagoAsync(id);
                if (result)
                    return NoContent();
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _penalizacionService.DeleteAsync(id);
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
