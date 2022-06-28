using Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.algoritm
{
    public class stopTimeAndSeq
    {
        public string tripId { get; set; }
        public int stopSequence { get; set; }


        public stopTimeAndSeq(string tripId, int stopSequence)
        {
            this.tripId = tripId;
            this.stopSequence = stopSequence;
        }
    }

    
    }
