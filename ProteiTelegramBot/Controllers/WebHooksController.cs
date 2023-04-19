using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public async Task<ActionResult> GitLabWebHookAsync()
    {
        try
        {
            //считыаем JSON
            var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            //преобразуем в базовый класс для событий
            Event requestEvent = JsonConvert.DeserializeObject<Event>(body);
            _logger.LogInformation(
                 $"Receive new WebHook with kind: {requestEvent.object_kind}");
            //в зависимости от типа запроса, вызываем один из методов
            switch (requestEvent.object_kind)
            {
                case "pipeline":
                    { 
                        PipelineEvent? pipelineEvent = JsonConvert.DeserializeObject<PipelineEvent>(body);
                        if (await GitLabWebHookPipelineAsync(pipelineEvent))
                            return Ok();
                        break;
                    }
                case "merge_request":
                    {
                        
                        MergeRequestEvent? mergeRequestEvent = JsonConvert.DeserializeObject<MergeRequestEvent>(body);
                        if (await GitLabWebHookMergeRequestAsync(mergeRequestEvent))
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
                title: Errors.Common_Internal_Server_Error);
        }
    }

    private async Task<bool> GitLabWebHookMergeRequestAsync(MergeRequestEvent mergeRequestEvent)
    {
        try
        {
            _logger.LogInformation(
                        $"Received WebHook is for {mergeRequestEvent.project.name}\n\rTitle:{mergeRequestEvent.object_attributes.title}");
            switch (mergeRequestEvent.object_attributes.action)
            {
                case "open":
                    {
                        await _notifier.Notify(new MergeRequestOpenedNotification(mergeRequestEvent.object_attributes.url,
                            mergeRequestEvent.object_attributes.title,
                            mergeRequestEvent.user.username));
                        break;
                    }
                case "merge":
                    {
                        await _notifier.Notify(new MergeRequestMergedNotification(mergeRequestEvent.object_attributes.url,
                            mergeRequestEvent.object_attributes.title));
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

    private async Task<bool> GitLabWebHookPipelineAsync(PipelineEvent pipelineEvent)
    {
        try
        {
            _logger.LogInformation(
                        $"Received WebHook is for {pipelineEvent.project.name}");
            await _notifier.Notify(new PipelineNotification(pipelineEvent.project.web_url, pipelineEvent.project.name, pipelineEvent.user.username, pipelineEvent.object_attributes.status));
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return false;
        }
    }
}