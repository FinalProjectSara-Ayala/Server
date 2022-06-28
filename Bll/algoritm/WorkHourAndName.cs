using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.algoritm
{
    public class WorkHourAndName
    {
        public int id { get; set; }
        public string nameInspector { get; set; }
        public int dayWork { get; set; }
        public System.TimeSpan start_shift { get; set; }
        public System.TimeSpan stop_shift { get; set; }
        public WorkHourAndName(int id,string nameInspector, int dayWork, System.TimeSpan start_shift, System.TimeSpan stop_shift)
        {
            this.id=id;
            this.nameInspector = nameInspector;
            this.dayWork = dayWork;
            this.start_shift = start_shift;
            this.stop_shift = stop_shift;   
        }
    }
}
