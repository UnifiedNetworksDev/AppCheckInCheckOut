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
    public class ProjectService
    {
        public async Task ValidateProject(string projectID)
        {
            try
            {
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                var response = await HttpSingleton.GetInstance().GetAsync(String.Format("{0}/api/Project/{1}", APISettings.API_URL, projectID)).ConfigureAwait(false);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    ErrorResponse errorFromAPI = JsonConvert.DeserializeObject<ErrorResponse>(jsonResult);
                    throw new Exception(errorFromAPI.message);
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.Message == "404 (Not Found)")
                {
                    throw new Exception(ex.Message);
                }
                else
                {
                    throw new Exception("Ha ocurrido un error al consultar los datos", ex);
                }
            }
        }
    }

   
}