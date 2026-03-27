using Microsoft.AspNetCore.Mvc;
using SGA.Application.DTOs;
using SGA.Application.Interfaces;

namespace SGA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;

        public PrestamosController(IPrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var prestamos = await _prestamoService.GetAllAsync();
            return Ok(prestamos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var prestamo = await _prestamoService.GetByIdAsync(id);
                return Ok(prestamo);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("usuario")]
        public async Task<IActionResult> GetByUsuarioId([FromQuery] int? estudianteId, [FromQuery] int? docenteId)
        {
            var prestamos = await _prestamoService.GetByUsuarioIdAsync(estudianteId, docenteId);
            return Ok(prestamos);
        }

        [HttpGet("vencidos")]
        public async Task<IActionResult> GetPrestamosVencidos()
        {
            var prestamos = await _prestamoService.GetPrestamosVencidosAsync();
            return Ok(prestamos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePrestamoDto dto)
        {
            try
            {
                var prestamo = await _prestamoService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = prestamo.Id }, prestamo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePrestamoDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("El id no coincide");

                var prestamo = await _prestamoService.UpdateAsync(dto);
                return Ok(prestamo);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("devolver/{id}")]
        public async Task<IActionResult> Devolver(int id)
        {
            try
            {
                var prestamo = await _prestamoService.DevolverAsync(id);
                return Ok(prestamo);
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
                var result = await _prestamoService.DeleteAsync(id);
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
