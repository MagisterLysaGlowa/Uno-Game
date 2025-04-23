using api.RealTime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IHubContext<ChatHub> chatHub;
        public WeatherForecastController(IHubContext<ChatHub> chatHub)
        {
            this.chatHub = chatHub;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] MessageDto message)
        {
            await chatHub.Clients.All.SendAsync("ReceiveMessage", message.User, message.Text);
            return Ok();
        }

        public class MessageDto
        {
            public string User { get; set; }
            public string Text { get; set; }
        }
    }
}
