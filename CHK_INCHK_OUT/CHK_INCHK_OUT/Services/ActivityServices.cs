using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using CHK_INCHK_OUT.Model;

namespace CHK_INCHK_OUT.Services
{
    public class ActivityServices
    {
        public async Task<List<SelectedData<TareasModel>>> GetActivityAsync(string idProyecto)
        {
            try
            {
                var json = await HttpSingleton.GetInstance().GetStringAsync(String.Format("{0}/api/Project/{1}/Activities", APISettings.API_URL, idProyecto)).ConfigureAwait(false);
                var taskModels = JsonConvert.DeserializeObject<List<TareasModel>> (json);

                List <SelectedData<TareasModel>> result = new List<SelectedData<TareasModel>>();
                foreach(TareasModel obj in taskModels)
                {
                    result.Add(new SelectedData<TareasModel>(obj, false));
                }

                return result;

            }
            catch (HttpRequestException ex)
            {
                if (ex.Message == "404 (Not Found)")
                {
                    throw new Exception("No fueron encontradas actividades asignadas a este proyecto", ex);
                }
                else
                {
                    throw new Exception("Ha ocurrido un error al consultar los datos", ex);
                }
            }
        }
    }
}
