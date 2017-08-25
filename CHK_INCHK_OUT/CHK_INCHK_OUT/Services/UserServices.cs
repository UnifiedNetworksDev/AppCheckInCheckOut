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
    public class UserServices
    {
        public async Task<Token> PostUserAsync(Login data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var result = await HttpSingleton.GetInstance().PostAsync(String.Format("{0}api/Account/token", APISettings.API_URL), httpContent).ConfigureAwait(false);
                if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new WebException("Usuario o contraseña incorrectos");
                }

                string jsonResult = await result.Content.ReadAsStringAsync();
                Token token = JsonConvert.DeserializeObject<Token>(jsonResult);
                return token;
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
