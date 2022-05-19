using Microsoft.AspNetCore.Mvc;
using SlotmachineRequest.Models;
using SlotmachineRequest.Services;
using System.Text.Json;

namespace SlotmachineRequest.Controllers
{
    [ApiController]
    [Route("buttonstate")]
    public class ButtonStateController : ControllerBase
    {
        private readonly ILogger<ButtonStateController> _logger;
        private IButtonStateservice _btnservice;
        
        public ButtonStateController(ILogger<ButtonStateController> logger, IButtonStateservice btnservice)
        {
            _logger = logger;
            _btnservice = btnservice;
        }

        [HttpPost]
        public void PostButtonState(ButtonState state)
        {
            //buttonState = state;
            _btnservice.ChangeState(state);
            _logger.LogInformation(JsonSerializer.Serialize(_btnservice.GetState()));
        }

        [HttpGet]
        public IActionResult GetButtonState()
        {
            _logger.LogInformation(JsonSerializer.Serialize(_btnservice.GetState()));
            return Ok(_btnservice.GetState());
        }
    }
}
