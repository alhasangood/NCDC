using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{
    [Route("api/[controller]")]
    public class AnalysisTypesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
