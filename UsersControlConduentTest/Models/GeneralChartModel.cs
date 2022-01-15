using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace UsersControlConduentTest.Models
{
    public class GeneralChartModel
    {
        [DataContract]
        public class DataPointDefault
        {
            public DataPointDefault(string x, int y)
            {
                //DateTime dateTime = DateTime.ParseExact(x, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                this.X = x;
                this.Y = y;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "x")]
            public string X = string.Empty;

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public int Y = 0;
        }
    }
}