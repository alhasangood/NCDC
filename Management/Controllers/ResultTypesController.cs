using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    [Route("api/[controller]")]
    public class ResultTypesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
