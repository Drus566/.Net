using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteSample
{
    public class RemotingSample : MarshalByRefObject
    {
        public void RemoteLog(string value)
        {
            Console.WriteLine(value);
        }
    }
}
