using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CHK_INCHK_OUT.Services;
using CHK_INCHK_OUT.Model;

namespace CHK_INCHK_OUT.Model
{
    public class ActivityViewModels
    {
        
        public List <SelectedData<TareasModel>> ActividadesList { get; set; }
        private string idProyecto;

        public ActivityViewModels(string idProyecto)
        {
            this.idProyecto = idProyecto;
            //EntregasList = new List<SelectedData<TareasModel>>();
            Task.Run(() => this.InitializeDataAsync()).Wait();
        }

        private async Task InitializeDataAsync()
        {
            try
            {
                var activiyyService = new ActivityServices();
                ActividadesList = await activiyyService.GetActivityAsync(idProyecto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Return selected activities
        /// </summary>
        /// <returns></returns>
        public List<SelectedData<TareasModel>> GetSelected()
        {
            try
            {
                var filterList = this.ActividadesList.Where(r => r.Selected).ToList();
                return filterList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
