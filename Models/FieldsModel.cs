using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BajajWebApi.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class SupportedOperation
    {
        public string referenceName { get; set; }
        public string name { get; set; }
    }

    public class Value
    {
        public string name { get; set; }
        public string referenceName { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string usage { get; set; }
        public bool readOnly { get; set; }
        public bool canSortBy { get; set; }
        public bool isQueryable { get; set; }
        public List<SupportedOperation> supportedOperations { get; set; }
        public bool isIdentity { get; set; }
        public bool isPicklist { get; set; }
        public bool isPicklistSuggested { get; set; }
        public string url { get; set; }
    }

    public class FieldsModel
    {
        public int count { get; set; }
        public List<Value> value { get; set; }
    }


}