//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Core.Objects;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Dal
//{
//    public partial class inspectorsDBEntities3
//    {
//        [DbFunction("inspectorsDBEntities3", "GetNumStopsForTrip")]
//        public virtual int GetNumStopsForTrip(string trip_id)
//        {


//            var objectContext = ((IObjectContextAdapter)this).ObjectContext;

//            var trip_idParameter = trip_id != null ?
//                new ObjectParameter("trip_id", trip_id) :
//                new ObjectParameter("trip_id", typeof(string));
//            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<int>
//                ("dbo.inspectorsDBEntities2.GetNumStopsForTrip", trip_idParameter).FirstOrDefault();
//            //return objectContext.CreateQuery<int>("inspectorsDBEntities2.Store.GetNumStopsForTrip(@trip_id)", trip_idParameter)
//            //.Execute(MergeOption.NoTracking)
//            //.FirstOrDefault();

//            //return objectContext.CreateQuery<int>(
//            //    "inspectorsDBEntities2.Store.GetNumStopsForTrip",
//            //    new ObjectParameter("trip_id", trip_id)).
//            //Execute(MergeOption.NoTracking).
//            //FirstOrDefault();
//        }
//    }
//}
