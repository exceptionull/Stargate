using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using System.Net;

namespace StargateAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AstronautDutyController> _logger;
        
        public AstronautDutyController(IMediator mediator, ILogger<AstronautDutyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(GetAstronautDutiesByNameResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAstronautDutiesByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetAstronautDutiesByName
                {
                    Name = name
                });

                _logger.LogInformation("AstronautDutyController -> GetAstronautDutiesByName() call successful.");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AstronautDutyController -> GetAstronautDutiesByName() call failed.");
                return this.GetResponse(new BaseResponse()
                {
                    Message = Constants.InternalErrorMessage,
                    Success = false,
                    ResponseCode = (int)HttpStatusCode.InternalServerError
                });
            }            
        }

        
        [HttpPost("")]
        [ProducesResponseType(typeof(CreateAstronautDutyResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDuty request)
        {
            try
            {
                var result = await _mediator.Send(request);

                _logger.LogInformation("AstronautDutyController -> CreateAstronautDuty() call successful.");
                return this.GetResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AstronautDutyController -> CreateAstronautDuty() call failed.");
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