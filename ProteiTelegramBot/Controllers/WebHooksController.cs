using Microsoft.AspNetCore.Mvc;
using ProteiTelegramBot.Localization;
using ProteiTelegramBot.Models;
using ProteiTelegramBot.Services;
using System.Net;

namespace ProteiTelegramBot.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class WebHooksController : ControllerBase
{
    private readonly ILogger<WebHooksController> _logger;
    private readonly INotifier _notifier;

    public WebHooksController(ILogger<WebHooksController> logger,
        INotifier notifier)
    {
        _logger = logger;
        _notifier = notifier;
    }

    [HttpPost("gitlab")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> GitLabWebHookAsync(Event happendEvent)
    {
        try
        {
            switch (happendEvent.ObjectKind)
            {
                case "pipeline":
                    {
                        if (await GitLabWebHookPipelineAsync(happendEvent))
                            return Ok();
                        break;
                    }
                case "merge_request":
                    {
                        if (await GitLabWebHookMergeRequestAsync(happendEvent))
                            return Ok();
                        break;
                    }
            }
            return Ok();

        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Problem(statusCode: (int)HttpStatusCode.InternalServerError, detail: e.Message,
                title: Errors.Common_Internal_Server_Error); ;
        }
    }

    private async Task<bool> GitLabWebHookMergeRequestAsync(Event mergeRequestEvent)
    {
        try
        {
            _logger.LogInformation(
                        $"Receive new WebHook for {mergeRequestEvent.Project.Name}\n\rTitle:{mergeRequestEvent.ObjectAttributes.Title}");
            switch (mergeRequestEvent.ObjectAttributes.Action)
            {
                case "open":
                    {
                        await _notifier.Notify(new MergeRequestOpenedNotification(mergeRequestEvent.ObjectAttributes.Url,
                            mergeRequestEvent.ObjectAttributes.Title,
                            mergeRequestEvent.User.Username)); //эта функция отправляет уведомление то самое
                        break;
                    }
                case "merge":
                    {
                        await _notifier.Notify(new MergeRequestMergedNotification(mergeRequestEvent.ObjectAttributes.Url,
                            mergeRequestEvent.ObjectAttributes.Title));
                        break;
                    }


            }
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return false;
        }
    }

    private async Task<bool> GitLabWebHookPipelineAsync(Event pipelineEvent)
    {
        try
        {
            _logger.LogInformation(
                        $"Receive new WebHook for {pipelineEvent.Project.Name}\n\rTitle:{pipelineEvent.ObjectAttributes.Title}");
            await _notifier.Notify(new MergeRequestOpenedNotification(pipelineEvent.ObjectAttributes.Url,
                pipelineEvent.ObjectAttributes.Title,
                pipelineEvent.User.Username));
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return false;
        }
    }
}