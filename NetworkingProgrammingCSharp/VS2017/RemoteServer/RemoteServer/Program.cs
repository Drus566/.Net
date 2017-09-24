using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using RemotingSample;



namespace RemoteServer
{
    class RemotingServer
    {
        static void Main(string[] args)
        {
            ChannelServices.RegisterChannel(new HttpChannel(8000));
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingSample), "WroxLog", WellKnownObjectMode.Singleton);
            
        }
    }
}
