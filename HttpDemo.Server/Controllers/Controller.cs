using Microsoft.AspNetCore.Mvc;

namespace HttpDemo.Server.Controllers;

[Route("/api/cinema/[controller]")]
public abstract class Controller : ControllerBase
{
    protected readonly ILogger<Controller> Logger;
    
    protected Controller(ILogger<Controller> logger)
    {
        Logger = logger;
    }
}
