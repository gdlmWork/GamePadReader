using Microsoft.AspNetCore.Mvc;
using SlotmachineRequest.Models;
using System.Text.Json;

namespace SlotmachineRequest.Controllers
{
    [ApiController]
    [Route("buttonstate")]
    public class ButtonStateController : ControllerBase
    {
        public ButtonState state;
        private readonly ILogger<ButtonStateController> _logger;
        
        public ButtonStateController(ILogger<ButtonStateController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void PostButtonState(ButtonState state)
        {
            this.state = state;
            _logger.LogInformation(JsonSerializer.Serialize(this.state));
            //Console.Write(state.ToString());
            //Console.Write(this.state.ToString());
        }

        [HttpGet]
        public IActionResult GetButtonState()
        {
            //Console.Write(this.state.ToString());
            return Ok(JsonSerializer.Serialize(this.state));
        }
    }
}
