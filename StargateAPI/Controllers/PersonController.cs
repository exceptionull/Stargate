using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PersonController> _logger;
        
        public PersonController(IMediator mediator, ILogger<PersonController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(GetPeopleResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                var result = await _mediator.Send(new GetPeople());
                
                _logger.LogInformation("PersonController -> GetPeople() call successful.");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PersonController -> GetPeople() call failed.");
                return this.GetResponse(new BaseResponse
                {
                    Message = Constants.InternalErrorMessage,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(GetPersonByNameResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPersonByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetPersonByName
                {
                    Name = name
                });
                
                _logger.LogInformation("PersonController -> GetPersonByName() call successful.");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PersonController -> GetPersonByName() call failed.");
                return this.GetResponse(new BaseResponse
                {
                    Message = Constants.InternalErrorMessage,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(CreatePersonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePerson request)
        {
            try
            {
                var result = await _mediator.Send(request);

                _logger.LogInformation("PersonController -> CreatePerson() call successful.");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PersonController -> CreatePerson() call failed.");
                return this.GetResponse(new BaseResponse
                {
                    Message = ex is BadHttpRequestException ? ex.Message : Constants.InternalErrorMessage,
                    Success = false,
                    ResponseCode = ex is BadHttpRequestException ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError
                });
            }
        }

        [HttpPut("")]
        [ProducesResponseType(typeof(UpdatePersonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePerson([FromBody] UpdatePerson request)
        {
            try
            {
                var result = await _mediator.Send(request);

                _logger.LogInformation("PersonController -> UpdatePerson() call successful.");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PersonController -> UpdatePerson() call failed.");
                return this.GetResponse(new BaseResponse
                {
                    Message = ex is BadHttpRequestException ? ex.Message : Constants.InternalErrorMessage,
                    Success = false,
                    ResponseCode = ex is BadHttpRequestException ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError
                });
            }
        }
    }
}