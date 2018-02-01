using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slack
{
    class Program
    {
        static void Main(string[] args)
        {
            //Get a list of slack channels
            List<SlackChannel> channels = SlackApi.GetChannelsList();
            //Get a channel by name ( change name with your channel name )
            SlackChannel channel = channels.SingleOrDefault(c => c.name == "test");
            //Post a message into this channel
            SlackApi.PostMessage(channel, "Hello World !");
        }
    }
}
