using Microsoft.AspNetCore.Mvc;
using System.Net;
using ProteiTelegramBot.Localization;
using ProteiTelegramBot.Services;

namespace ProteiTelegramBot.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ForceUpdatesController : ControllerBase
{
    private readonly ILogger<ForceUpdatesController> _logger;
    private readonly IDutyService _dutyService;

    public ForceUpdatesController(ILogger<ForceUpdatesController> logger,
        IDutyService dutyService)
    {
        _logger = logger;
        _dutyService = dutyService;
    }

    [HttpPost("updateDuty")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateDutyAsync()
    {
        try
        {
            await _dutyService.UpdateDutyAsync();
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Problem(statusCode: (int)HttpStatusCode.InternalServerError, detail: e.Message,
                title: Errors.Common_Internal_Server_Error);
        }
    }
}