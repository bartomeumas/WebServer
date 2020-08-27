using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer
{
    public class MessagesController
    {

        public Dictionary<int, string> myMessages = new Dictionary<int, string>
        {
        { 1, "Hola" },
        { 2, "Mundo" }
        };

        // GET
        public string Get(int id)
        {
            return myMessages[id];
        }

        // POST 
        public void Post(int id, string bodyMessage)
        {
            myMessages.Add(id, bodyMessage);
        }

        // PUT
        public void Put(int id, string bodyMessage)
        {
            myMessages[id] = bodyMessage;
        }

        // DELETE 
        public void Delete(int id)
        {
            myMessages.Remove(id);
        }

    }
}
