using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace WebServer
{
    class Program
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";
        public static int pageViews = 0;
        public static int requestCount = 0;
        public static string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>Mini Web Server</title>" +
            "  </head>" +
            "  <body>" +
            "    <form method=\"post\" action=\"shutdown\">" +
            "      <input type=\"submit\" value=\"Shutdown\" {1}>" +
            "    </form>" +
            "   <form method=\"get\" action=\"getmessages\">" +
            "      <input type=\"submit\" value=\"Get\" >" +
            "    </form>" +
            "   <form method=\"post\" action=\"postmessages\">" +
            "      <input type=\"submit\" value=\"Post\" >" +
            "    </form>" +
            "   <form method=\"put\" action=\"putmessages\">" +
            "      <input type=\"submit\" value=\"Put\" >" +
            "    </form>" +
            "   <form method=\"delete\" action=\"deletemessages\">" +
            "      <input type=\"submit\" value=\"Delete\" >" +
            "    </form>" +
            "  </body>" +
            "</html>";

        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            while (runServer)
            {
                HttpListenerContext ctx = await listener.GetContextAsync();

                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();

                MessagesController mc = new MessagesController();

                if ((req.HttpMethod == "GET"))
                {
                    
                    mc.Get(1);
                    mc.Get(2);
                }

                if ((req.HttpMethod == "POST"))
                {
                    mc.Post(3, "prueba");
                }

                if ((req.HttpMethod == "PUT"))
                {
                    mc.Post(1, "dulce");
                }

                if ((req.HttpMethod == "DELETE"))
                {
                    mc.Delete(2);
                }

                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }

                if (req.Url.AbsolutePath != "/favicon.ico")
                    pageViews += 1;

                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(String.Format(pageData, pageViews, disableSubmit));
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }


        public static void Main(string[] args)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            listener.Close();
        }
    }
}