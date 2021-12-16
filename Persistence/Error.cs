using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public class Error
    {
        public string GreetingMessage {get;set;}

        public string ErrorMessage { get; set; }
        public Error()
        {
            GreetingMessage = "Hoppá! nem találjuk az oldalt amire kerestél.";
        }
    }
}
