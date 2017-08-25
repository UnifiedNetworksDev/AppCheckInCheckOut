using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHK_INCHK_OUT.Model;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CHK_INCHK_OUT.Services
{
    public class HistorialServices
    {
        public async Task PutAsyncCheckIn(List<HistorialActivity> data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await HttpSingleton.GetInstance().PutAsync(String.Format("{0}api/ActivityHistory", APISettings.API_URL), httpContent).ConfigureAwait(false);
                result.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al consultar los datos", ex);
            }
        }

        public async Task<string> PutAsyncCheckOut(List<HistorialActivity> data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await HttpSingleton.GetInstance().PutAsync(String.Format("{0}api/ActivityHistory", APISettings.API_URL), httpContent).ConfigureAwait(false);
                var respuesta = await result.Content.ReadAsStringAsync();
                ErrorResponse response = JsonConvert.DeserializeObject<ErrorResponse>(respuesta);
                return response.message;
            }
            catch (WebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al consultar los datos", ex);
            }
        }
    }
}

