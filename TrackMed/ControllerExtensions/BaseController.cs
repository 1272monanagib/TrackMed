using Microsoft.AspNetCore.Mvc;

namespace TrackMed.ControllerExtensions
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController: ControllerBase
    {
    }
}
