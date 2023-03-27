using ProteiTelegramBot.Models;
using System.Dynamic;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProteiTelegramBot.Config;
using ProteiTelegramBot.Repository;

namespace ProteiTelegramBot.Services;

public class DutyYouTrackExtractor : IDutyExtractor
{
    private readonly ILogger<DutyYouTrackExtractor> _logger;
    private readonly IDutyRepository _repository;
    private readonly HttpClient _httpClient;
    private readonly YouTrackOptions _youTrackSettings;

    public DutyYouTrackExtractor(ILogger<DutyYouTrackExtractor> logger,
        IDutyRepository repository,
        HttpClient httpClient,
        IOptions<YouTrackOptions> youTrackSettings)
    {
        _logger = logger;
        _repository = repository;
        _httpClient = httpClient;
        _youTrackSettings = youTrackSettings.Value;
    }

    public async Task<Duty> GetCurrentDutyAsync()
    {
        try
        {
            var response = await _httpClient.GetStringAsync(
                $"/api/issues/{_youTrackSettings.Task}?fields=$type,id,summary,customFields($type,id,projectCustomField($type,id,field($type,id,name)),value($type,avatarUrl,buildLink,color(id),fullName,id,isResolved,localizedName,login,minutes,name,presentation,text))");

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All // Or Use .Auto for light weigth output
            };

            dynamic? jsonRoot = JsonConvert.DeserializeObject<ExpandoObject>(response, settings);
            if (jsonRoot == null)
            {
                throw new InvalidOperationException("Can't deserialize object");
            }

            string? login = null;
            var countOfCustomFields = jsonRoot.customFields.Count;

            for (var i = 0; i < countOfCustomFields; i++)
            {
                var field = jsonRoot.customFields[i];
                if (field.projectCustomField.field.name == "Assignee")
                {
                    login = field.value.login;
                    break;
                }
            }

            if (login == null)
            {
                throw new InvalidOperationException("Can't deserialize object");
            }

            var duty = await _repository.GeyByYouTrackLoginAsync(login);
            return duty;
        }
        catch (Exception exception)
        {
            _logger.LogError($"{exception}");
            throw;
        }
    }
}