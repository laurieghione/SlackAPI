using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Slack
{
    public static class SlackApi
    {
        private const string Token = "xoxp-XXXXXX"; // Your authentication Token
        private const string SlackURL = "https://xxxxx.slack.com/api/"; // Url Slack ( put your slack url here )
        private const string Username = "Lau"; // Post a message by this name

        /// <summary>
        /// Define a proxy
        /// Not necessary if you don't no have a proxy serveur
        /// </summary>
        /// <returns></returns>
        public static WebProxy SetProxy()
        {
            return new WebProxy("http://proxy", 8080); // proxy URL and Port
        }

        /// <summary>
        /// Get the list of Channels
        /// Return a list of SlackChannel Object
        /// </summary>
        /// <returns></returns>
        public static List<SlackChannel> GetChannelsList()
        {
            List<SlackChannel> channels = new List<SlackChannel>();

            try
            {
                WebRequest request = WebRequest.Create(string.Format("{0}channels.list?token={1}", SlackURL, Token));
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Proxy = SetProxy();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Console.WriteLine(response.StatusDescription);
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                JObject data = (JObject)JsonConvert.DeserializeObject(responseFromServer);
                if (data["channels"] != null)
                {
                    foreach (var c in data["channels"])
                    {
                        SlackChannel sc = new SlackChannel();
                        sc.id = c["id"].ToString();
                        sc.name = c["name"].ToString();
                        sc.is_channel = c["is_channel"].ToString();
                        sc.created = c["created"].ToString();

                        channels.Add(sc);
                    }
                }
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return channels;
        }
        /// <summary>
        /// Post a message in a specific channel
        /// Return a Http Web Response
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        public static HttpWebResponse PostMessage(SlackChannel channel, string message)
        {
            try
            {
                if (channel != null)
                {
                    WebRequest request = WebRequest.Create(string.Format("{0}chat.postMessage?token={1}&channel={2}&text={3}&username={4}", SlackURL, Token, channel.id, message, Username));
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded,application/json";
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Proxy = SetProxy();
                    return (HttpWebResponse)request.GetResponse();

                }
                else
                {
                    throw new Exception("Le Channel n'existe pas");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

