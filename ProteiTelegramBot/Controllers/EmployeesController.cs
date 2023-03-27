using Microsoft.AspNetCore.Mvc;
using ProteiTelegramBot.Exceptions;
using System.Net;
using ProteiTelegramBot.Localization;
using ProteiTelegramBot.Models;
using ProteiTelegramBot.Repository;

namespace ProteiTelegramBot.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;
    private readonly IEmployeRepository _employeeRepository;

    public EmployeesController(ILogger<EmployeesController> logger, IEmployeRepository employeeRepository)
    {
        _logger = logger;
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeesResponse))]
    public async Task<ActionResult> GetEmployeesAsync()
    {
        var employees = await _employeeRepository.GetEmployeesAsync();
        var response = new EmployeesResponse()
        {
            Items = employees.Select(Convert)
        };
        return Ok(response);
    }

    [HttpGet("{employeeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetEmployeeByIdAsync(int employeeId)
    {
        try
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            return Ok(Convert(employee));
        }
        catch (EntityNotFoundException e)
        {
            var message = e.ToString();
            _logger.LogError(message);
            return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int)HttpStatusCode.NotFound,
                detail: message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EmployeeResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> CreateEmployeeAsync(EmployeeRequest employee)
    {
        try
        {
            var createdEmployee = await _employeeRepository.CreateEmployeeAsync(Convert(employee));
            return CreatedAtAction("CreateEmployee", new {id = createdEmployee.Id}, Convert(createdEmployee));
        }
        catch (EntityAlreadyExistsException e)
        {
            _logger.LogError(e.ToString());
            return Problem(statusCode: (int)HttpStatusCode.Conflict, detail: e.Message,
                title: Errors.Entities_Entity_already_exits);
        }
    }

    [HttpPut("{employeeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateEmployeeAsync(int employeeId, EmployeeRequest employee)
    {
        try
        {
            var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(employeeId, Convert(employee));
            return Ok(Convert(updatedEmployee));
        }
        catch (EntityNotFoundException e)
        {
            var message = e.ToString();
            _logger.LogError(message);
            return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int)HttpStatusCode.NotFound,
                detail: message);
        }
        catch (EntityAlreadyExistsException e)
        {
            _logger.LogError(e.ToString());
            return Problem(statusCode: (int)HttpStatusCode.Conflict, detail: e.Message,
                title: Errors.Entities_Entity_already_exits);
        }
    }

    [HttpDelete("{employeeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteEmployeeAsync(int employeeId)
    {
        try
        {
            await _employeeRepository.DeleteEmployeeAsync(employeeId);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            var message = e.ToString();
            _logger.LogError(message);
            return Problem(title: Errors.Entities_Entity_not_found, statusCode: (int)HttpStatusCode.NotFound,
                detail: message);
        }
    }

    private Employee Convert(EmployeeRequest employeeRequest)
    {
        return new Employee()
            {TelegramLogin = employeeRequest.TelegramLogin, ProteiLogin = employeeRequest.ProteiLogin };
    }

    private EmployeeResponse Convert(Employee employee)
    {
        return new EmployeeResponse()
        {
            Id = employee.Id,
            ProteiLogin = employee.ProteiLogin,
            TelegramLogin = employee.TelegramLogin
        };
    }
}