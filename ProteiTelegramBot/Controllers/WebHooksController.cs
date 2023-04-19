using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProteiTelegramBot.Localization;
using ProteiTelegramBot.Models;
using ProteiTelegramBot.Services;
using System.Net;
using System.Web;

namespace ProteiTelegramBot.Controllers;

//ТУТ НАДО БУДЕТ ОБРАБОТКУ PIPELINE EVENT НАПИСАТЬ, А ДЛЯ ЭТОГО ПОНАДОБИТСЯ ТО ЖЕ. ЧТО  И ДЛЯ MERGEREQUEST.

//вебхук как раз с ботом и работает
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
            var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            Console.WriteLine(body);
            Event requestEvent = JsonConvert.DeserializeObject<Event>(body);
            _logger.LogInformation(
                 $"Receive new WebHook with kind: {requestEvent.object_kind}");
            //await _notifier.Notify(new SimpleNotification($"Мы просериализировали запрос, у которого requestEvent.ObjectKind: {requestEvent.object_kind}"));
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
                        //await _notifier.Notify(new SimpleNotification("Тип полученного запрса - мерджреквест"));
                        MergeRequestEvent? mergeRequestEvent = JsonConvert.DeserializeObject<MergeRequestEvent>(body);
                        if (await GitLabWebHookMergeRequestAsync(mergeRequestEvent))
                            return Ok();
                        break;
                    }/*
                default:
                    {
                        MergeRequestEvent? mergeRequestEvent = JsonConvert.DeserializeObject<MergeRequestEvent>(body);
                        if (await GitLabWebHookMergeRequestAsync(mergeRequestEvent))
                            return Ok();
                        break;
                    }
                    /*case "merge_request":
                        {
                            await _notifier.Notify(new MergeRequestOpenedNotification(mergeRequestEvent.Project.Name,
                                mergeRequestEvent.Project.Name,
                                mergeRequestEvent.User.Username));
                            break;
                        }
                    case "merge":
                        {
                            await _notifier.Notify(new MergeRequestMergedNotification(mergeRequestEvent.Project.Name,
                                mergeRequestEvent.Project.Name));
                            break;
                        }*/
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
    /*
    public async Task<ActionResult> GitLabWebHookAsync()
    {
        try
        {
            /*switch (Request.Query.FirstOrDefault(p => p.Key == "object_kind").Value)
            {
                case "pipeline":
                    {
                        if (await GitLabWebHookPipelineAsync(Request.Body))
                            return Ok();
                        break;
                    }
                case "merge_request":
                    {
                        if (await GitLabWebHookMergeRequestAsync(Request.Body))
                            return Ok();
                        break;
                    }
            }
            foreach (var param in Request.Query)
            {
                Console.WriteLine($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");
            }
            Console.WriteLine("first");
            string b = Request.QueryString.ToString();
            Console.WriteLine($"{b}");
            await Response.WriteAsync(b);
            foreach (var header in Request.Query)
            {
                Console.WriteLine("first");
                Console.WriteLine($"{header.Key} is {header.Value}");
            }
            /*
            var a = Request.Query["object_kind"];
            Console.WriteLine($"\n\n\n\n\nPipeline SOURCE: \n\n\n\n\n");
            Console.WriteLine(Request.Query.FirstOrDefault(p => p.Key == "object_kind").Value);
            return Ok();

        }   
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return Problem(statusCode: (int)HttpStatusCode.InternalServerError, detail: e.Message,
                title: Errors.Common_Internal_Server_Error); ;
        }
    }
    }
    */
    private async Task<bool> GitLabWebHookMergeRequestAsync(MergeRequestEvent mergeRequestEvent)
    {
        try
        {
            //await _notifier.Notify(new SimpleNotification("Пук"));
            _logger.LogInformation(
                        $"Received WebHook is for {mergeRequestEvent.project.name}\n\rTitle:{mergeRequestEvent.object_attributes.title}");
            switch (mergeRequestEvent.object_attributes.action)
            {
                case "open":
                    {
                        await _notifier.Notify(new MergeRequestOpenedNotification(mergeRequestEvent.object_attributes.url,
                            mergeRequestEvent.object_attributes.title,
                            mergeRequestEvent.user.username)); //эта функция отправляет уведомление то самое
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
/*

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
                        mergeRequestEvent.User.Username));
                    break;
                }
            case "merge":
                {
                    await _notifier.Notify(new MergeRequestMergedNotification(mergeRequestEvent.ObjectAttributes.Url,
                        mergeRequestEvent.ObjectAttributes.Title));
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
/*
[HttpPost("gitlab")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
public async Task<ActionResult> GitLabWebHookAsync(PipelineEvent pipelineEvent)
{
    try
    {
        _logger.LogInformation(
                    $"Receive new WebHook for {pipelineEvent.Project.Name}\n\rTitle:{pipelineEvent.Project.Name}");
        await _notifier.Notify(new PipelineNotification(pipelineEvent.Source.Project.Url,
            pipelineEvent.Project.Name,
            pipelineEvent.User.Username));
        return Ok();
    }
    catch (Exception e)
    {
        _logger.LogError(e.ToString());
        return Problem(statusCode: (int)HttpStatusCode.InternalServerError, detail: e.Message,
            title: Errors.Common_Internal_Server_Error);
    }
}
}*/