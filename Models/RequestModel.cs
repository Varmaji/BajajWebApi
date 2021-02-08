using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BajajWebApi
{
    public class RequestModel
    {
        public int Request_ID { get; set; }
        public string Query_Type { get; set; }
        public string WI_Type { get; set; }
        public string Project_Name { get; set; }
        public string Field_Name { get; set; }
        public int WI_ID { get; set; }
        public string Iteration_path { get; set; }
        public string Area_path { get; set; }
    }
}