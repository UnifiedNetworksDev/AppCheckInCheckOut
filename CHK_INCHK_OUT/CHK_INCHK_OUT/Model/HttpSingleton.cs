using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CHK_INCHK_OUT.Model
{
    public sealed class HttpSingleton
    {
        private static HttpClient httpClient = new HttpClient();

        public static HttpClient GetInstance()
        {
            return httpClient;
        }
    }
}
