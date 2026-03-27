using Microsoft.AspNetCore.Mvc;
using SGA.Application.DTOs;
using SGA.Application.Interfaces;

namespace SGA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservas = await _reservaService.GetAllAsync();
            return Ok(reservas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var reserva = await _reservaService.GetByIdAsync(id);
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("usuario")]
        public async Task<IActionResult> GetByUsuarioId([FromQuery] int? estudianteId, [FromQuery] int? docenteId)
        {
            var reservas = await _reservaService.GetByUsuarioIdAsync(estudianteId, docenteId);
            return Ok(reservas);
        }

        [HttpGet("libro/{libroId}")]
        public async Task<IActionResult> GetByLibroId(int libroId)
        {
            try
            {
                var reserva = await _reservaService.GetByLibroIdAsync(libroId);
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservaDto dto)
        {
            try
            {
                var reserva = await _reservaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = reserva.Id }, reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReservaDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("El id no coincide");

                var reserva = await _reservaService.UpdateAsync(dto);
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("cancelar/{id}")]
        public async Task<IActionResult> Cancelar(int id)
        {
            try
            {
                var result = await _reservaService.CancelarAsync(id);
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
                var result = await _reservaService.DeleteAsync(id);
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
