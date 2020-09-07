using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ToolGood.Bedrock.Utils
{
    public class IPAddressPortUtil
    {
        /// <summary>
        /// 查找 空闲端口
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static int FindFreePort(IPAddress address = null)
        {
            if (address == null)
                address = IPAddress.Any;

            int port;
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                var pEndPoint = new IPEndPoint(address, 0);
                socket.Bind(pEndPoint);
                pEndPoint = (IPEndPoint) socket.LocalEndPoint;
                port = pEndPoint.Port;
            }
            finally
            {
                socket.Close();
            }
            return port;
        }
    }
}
