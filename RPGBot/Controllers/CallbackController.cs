using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Utils;
using VkNet.Model.RequestParams;
using Microsoft.Extensions.Configuration;
using VkNet.Enums.SafetyEnums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RPGBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        private readonly IConfiguration _configuration;
        private readonly IVkApi _vkApi;

        static readonly string appName = "рпг";

        public CallbackController(IVkApi vkApi, IConfiguration configuration)
        {
            _configuration = configuration;
            _vkApi = vkApi;
        }

        [HttpPost]
        public IActionResult Callback([FromBody] Messages.Updates updates)
        {
            // Тип события
            switch (updates.Type)
            {
                // Confirm vk server existence
                case "confirmation":
                    {
                        return Ok(_configuration["Config:Confirmation"]);
                    }

                // New message somewhere
                // peer_id = 2000000xxx - chat message
                // other peer_id - user message
                case "message_new":
                    {
                        // Parsing message to vk form
                        var msg = Message.FromJson(new VkResponse(updates.Object));

                        if (msg.Action != null)
                        {
                            if (msg.Action.Type == MessageAction.ChatInviteUser)
                            {
                                // TODO: Something on invite user
                            }

                            if (msg.Action.Type == MessageAction.ChatKickUser)
                            {
                                // TODO: Something on kick user
                            }

                            return Ok("ok");
                        }


                        string[] words = msg.Text.Split();
                        var twoWords = words.Take(2);

                        // command has been received
                        if (twoWords.First().ToLower() == appName && twoWords.Count() == 2)
                        {
                            var askProcessor = Ask.Ask.Create(twoWords.Last(), msg);
                            var answer = askProcessor.ComputeAnswer(String.Join(' ', words.Skip(2)));

                            _vkApi.Messages.Send(answer);
                        }


                        break;
                    }
            }

            return Ok("ok");
        }
    }

}
