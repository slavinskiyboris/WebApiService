using Microsoft.AspNetCore.Mvc;
using WebApiServer.Services;
using CommonLibrary;

namespace WebApiServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("calculate")]
        public IActionResult Calculate([FromQuery] string exp)
        {
            if (string.IsNullOrWhiteSpace(exp))
            {
                _logger.LogWarning("Empty expression received.");
                return BadRequest("Expression cannot be empty.");
            }

            _logger.LogInformation($"Received expression: {exp}");

            if (!ExpressionValidator.IsValidExpression(exp))
            {
                _logger.LogWarning("Invalid expression format.");
                return BadRequest("Invalid expression format.");
            }

            try
            {
                double result = Calculator.EvaluateExpression(exp);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "ArgumentException occurred.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred.");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
