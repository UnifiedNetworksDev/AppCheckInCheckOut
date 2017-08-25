using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHK_INCHK_OUT.Model
{
    public class SelectedData<T>
    {
        public T activityInformation { get; set; }
        public bool Selected { get; set; }

        public SelectedData(T data, bool selected)
        {
            this.activityInformation = data;
            this.Selected = selected;
        }

        public SelectedData()
        {

        }

    }
}
