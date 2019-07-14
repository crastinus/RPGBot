﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Model.RequestParams;

namespace RPGBot.Controllers.Ask
{
    class Unknown : Ask
    {
        public override MessagesSendParams ComputeAnswer(string question)
        {
            return new MessagesSendParams
            {
                RandomId = new DateTime().Millisecond,
                PeerId = _msg.PeerId.Value,
                Message = "Неизвестное сообщение"
            };
        }
    }
}
