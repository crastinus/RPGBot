using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace RPGBot.Controllers.Ask
{
    abstract class Ask
    {
        protected Message _msg;

        // Receives a tail of the message after two first words
        public abstract MessagesSendParams ComputeAnswer(string question);


        // command = second word in the message
        public static Ask Create(string command, Message mes)
        {
            Ask result = null;
            switch(command.ToLower())
            {
                case "стата":
                    {
                        result = new Statistics();
                    } break;
                case "бойцы":
                    {
                        result = new Fighters();                        
                    } break;
                default:
                    {
                        result = new Unknown();
                    } break;
            }
            result._msg = mes;
            return result;
        }

        
    }




}
