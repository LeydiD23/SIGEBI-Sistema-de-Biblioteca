using Microsoft.AspNetCore.Mvc;
using SGA.Persistence;

namespace SGA.Web.Controllers
{
    public class testController : Controller
    {
        private readonly AppDbContext _context;

        public testController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            try
            {
                var connection = _context.Database.CanConnect();

                return Content(connection ? "Connection successful!" : "Cant conecction database");
            }
            catch (Exception ex)
            {
                return Content($"Connection failed: {ex.Message}");


            }
        }
    }
}
