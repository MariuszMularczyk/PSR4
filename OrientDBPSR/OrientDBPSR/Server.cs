using System;
using System.Collections.Generic;
using System.Text;
using Orient.Client;
namespace OrientDBPSR
{
    public static class Server
    {
        private static string _hostname = "localhost";
        private static int _port = 2424;
        private static string _user = "root";
        private static string _passwd = "Start.123";

        public static OServer Connect()
        {
            return new OServer(_hostname, _port,
                  _user, _passwd);
        }

    }
}
