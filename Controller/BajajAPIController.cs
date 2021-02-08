using BajajWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace BajajWebApi.Controller
{
    public class BajajAPIController : ApiController
    {

        string org = ConfigurationManager.AppSettings["Organization"];
        string pat = ConfigurationManager.AppSettings["PAT"];
        [ActionName("LoadJson")]
        public JsonResult<ResponseModel> Get()
        {

            try
            {

                string file = HttpContext.Current.Server.MapPath("~/JSON/RequestJson.json");
                using (StreamReader r = new StreamReader(file))
                {
                    string json = r.ReadToEnd();
                    RequestModel requestModel = JsonConvert.DeserializeObject<RequestModel>(json);
                    switch (requestModel.Query_Type)
                    {
                        case "Count":
                            var str1 = Count(requestModel);
                            return str1;
                        case "Sum":
                            var str2 = Sum(requestModel);
                            return str2;
                        case "Average":
                            var str3 = Average(requestModel);
                            return str3;
                        case "Max":
                            var str4 = Max(requestModel);
                            return str4;
                        case "Min":
                            var str5 = Min(requestModel);
                            return str5;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public JsonResult<ResponseModel> Count(RequestModel requestModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                using (var client = new HttpClient())
                {
                    string projName = requestModel.Project_Name;
                    string workitemType = requestModel.WI_Type;
                    if (requestModel.WI_Type != "ALL")
                    {
                        string queryString = @"select [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State], [System.Tags] from workitems WHERE [System.TeamProject] = '" + projName + "' AND [System.WorkItemType] = '" + workitemType + "'";
                        var wiql = new { query = queryString };
                        var content = Newtonsoft.Json.JsonConvert.SerializeObject(wiql);
                        string url = "https://dev.azure.com/" + org + "/_apis/wit/wiql?api-version=5.1";
                        CountvalueModel wiqlResponse = Store.GetApi<CountvalueModel>(url, pat, "POST", content);
                        if (wiqlResponse != null)
                        {
                            responseModel.Request_ID = requestModel.Request_ID;
                            responseModel.Status = "Success";
                            responseModel.Details = new Detail { Count = wiqlResponse.workItems.Count };
                            responseModel.WI_Details = null;
                            return Json(responseModel);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        string queryString = @"select [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State], [System.Tags] from workitems WHERE [System.TeamProject] = '" + projName + "'";
                        var wiql = new { query = queryString };
                        var content = Newtonsoft.Json.JsonConvert.SerializeObject(wiql);
                        string url = "https://dev.azure.com/" + org + "/_apis/wit/wiql?api-version=5.1";
                        CountvalueModel wiqlResponse = Store.GetApi<CountvalueModel>(url, pat, "POST", content);
                        if (wiqlResponse != null)
                        {
                            responseModel.Request_ID = requestModel.Request_ID;
                            responseModel.Status = "Success";
                            responseModel.Details = new Detail { Count = wiqlResponse.workItems.Count };
                            responseModel.WI_Details = null;
                            return Json(responseModel);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public JsonResult<ResponseModel> Sum(RequestModel requestModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                    string projName = requestModel.Project_Name;
                    string workitemType = requestModel.WI_Type;
                    string fieldName = requestModel.Field_Name;

                    if (fieldName == null)
                    {
                        responseModel.Request_ID = requestModel.Request_ID;
                        responseModel.Status = "Failure";
                        responseModel.Failure_Message = "Field Name not entered";
                        return Json(responseModel);
                    }
                    else 
                    {
                    WorkItemReponseModel workItemReponse = new WorkItemReponseModel();
                   var lastBatchResponsedata= FecthWIDetailsinBatch(requestModel);
                    workItemReponse.value = new List<WIDetail>();
                    WorkItemReponseModel lastBatchResponse = (WorkItemReponseModel)lastBatchResponsedata;
                    workItemReponse.count += lastBatchResponse.count;
                    responseModel.Request_ID = requestModel.Request_ID;
                    responseModel.Status = "Success";
                    foreach (var items in lastBatchResponse.value)
                        workItemReponse.value.Add(items);
                    Double summation = 0;
                    for (int i = 0; i < lastBatchResponse.value.Count; i++)
                    {
                        summation += double.Parse(lastBatchResponse.value[i].fields["Microsoft.VSTS.Scheduling.Effort"].ToString());
                    }
                    responseModel.Details = new Detail { Sum = summation };

                    responseModel.WI_Details = new List<WIDetail>();

                    responseModel.WI_Details.AddRange(workItemReponse.value);

                    return Json(responseModel);
                    }

                
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public JsonResult<ResponseModel> Average(RequestModel requestModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string projName = requestModel.Project_Name;
                string workitemType = requestModel.WI_Type;
                string fieldName = requestModel.Field_Name;

                if (fieldName == null)
                {
                    responseModel.Request_ID = requestModel.Request_ID;
                    responseModel.Status = "Failure";
                    responseModel.Failure_Message = "Field Name not entered";
                    return Json(responseModel);
                }
                else
                {
                    WorkItemReponseModel workItemReponse = new WorkItemReponseModel();
                    var lastBatchResponsedata = FecthWIDetailsinBatch(requestModel);
                    workItemReponse.value = new List<WIDetail>();
                    WorkItemReponseModel lastBatchResponse = (WorkItemReponseModel)lastBatchResponsedata;
                    workItemReponse.count += lastBatchResponse.count;
                    responseModel.Request_ID = requestModel.Request_ID;
                    responseModel.Status = "Success";
                    foreach (var items in lastBatchResponse.value)
                        workItemReponse.value.Add(items);
                    Double summation = 0;
                    for (int i = 0; i < lastBatchResponse.value.Count; i++)
                    {
                        summation += double.Parse(lastBatchResponse.value[i].fields["Microsoft.VSTS.Scheduling.Effort"].ToString());
                    }
                    responseModel.Details = new Detail { Average = summation /workItemReponse.count};

                    responseModel.WI_Details = new List<WIDetail>();

                    responseModel.WI_Details.AddRange(workItemReponse.value);

                    return Json(responseModel);
                }


            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public JsonResult<ResponseModel> Max(RequestModel requestModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string projName = requestModel.Project_Name;
                string workitemType = requestModel.WI_Type;
                string fieldName = requestModel.Field_Name;

                if (fieldName == null)
                {
                    responseModel.Request_ID = requestModel.Request_ID;
                    responseModel.Status = "Failure";
                    responseModel.Failure_Message = "Field Name not entered";
                    return Json(responseModel);
                }
                else
                {
                    WorkItemReponseModel workItemReponse = new WorkItemReponseModel();
                    var lastBatchResponsedata = FecthWIDetailsinBatch(requestModel);
                    workItemReponse.value = new List<WIDetail>();
                    WorkItemReponseModel lastBatchResponse = (WorkItemReponseModel)lastBatchResponsedata;
                    workItemReponse.count += lastBatchResponse.count;
                    responseModel.Request_ID = requestModel.Request_ID;
                    responseModel.Status = "Success";
                    foreach (var items in lastBatchResponse.value)
                        workItemReponse.value.Add(items);
                    double[] WIFieldArray = new double[lastBatchResponse.value.Count];

                    for (int i = 0; i < lastBatchResponse.value.Count; i++)
                    {
                        WIFieldArray[i] = double.Parse(lastBatchResponse.value[i].fields["Microsoft.VSTS.Scheduling.Effort"].ToString());
                    }

                    double Maxvalue = WIFieldArray.Max();

                    responseModel.Details = new Detail { Max = Maxvalue };

                    responseModel.WI_Details = new List<WIDetail>();

                    responseModel.WI_Details.AddRange(workItemReponse.value);

                    return Json(responseModel);
                }


            }
            catch (Exception ex)
            {

            }
            return null;

        }
        public JsonResult<ResponseModel> Min(RequestModel requestModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                string projName = requestModel.Project_Name;
                string workitemType = requestModel.WI_Type;
                string fieldName = requestModel.Field_Name;

                if (fieldName == null)
                {
                    responseModel.Request_ID = requestModel.Request_ID;
                    responseModel.Status = "Failure";
                    responseModel.Failure_Message = "Field Name not entered";
                    return Json(responseModel);
                }
                else
                {
                    WorkItemReponseModel workItemReponse = new WorkItemReponseModel();
                    var lastBatchResponsedata = FecthWIDetailsinBatch(requestModel);
                    workItemReponse.value = new List<WIDetail>();
                    WorkItemReponseModel lastBatchResponse = (WorkItemReponseModel)lastBatchResponsedata;
                    workItemReponse.count += lastBatchResponse.count;
                    responseModel.Request_ID = requestModel.Request_ID;
                    responseModel.Status = "Success";
                    foreach (var items in lastBatchResponse.value)
                        workItemReponse.value.Add(items);
                    double[] WIFieldArray = new double[lastBatchResponse.value.Count];

                    for (int i = 0; i < lastBatchResponse.value.Count; i++)
                    {
                        WIFieldArray[i] = double.Parse(lastBatchResponse.value[i].fields["Microsoft.VSTS.Scheduling.Effort"].ToString());
                    }

                    double Minvalue = WIFieldArray.Min();

                    responseModel.Details = new Detail { Min = Minvalue };

                    responseModel.WI_Details = new List<WIDetail>();

                    responseModel.WI_Details.AddRange(workItemReponse.value);

                    return Json(responseModel);
                }


            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public class Store
        {
            public static T GetApi<T>(string url, string PAT, string method = "GET", string requestBody = null)
            {
                try
                {
                    Console.WriteLine("Get Api");
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", PAT))));
                    HttpRequestMessage Request;
                    if (requestBody != null)
                    {
                        HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                        Request = new HttpRequestMessage(new HttpMethod(method), url) { Content = content };
                    }
                    else
                        Request = new HttpRequestMessage(new HttpMethod(method), url);

                    using (HttpResponseMessage response = client.SendAsync(Request).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = response.Content.ReadAsStringAsync().Result;
                            return JsonConvert.DeserializeObject<T>(responseBody);
                        }
                        else
                        {
                            var Error = response.Content.ReadAsStringAsync().Result;
                            return default;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Inside WorkitemGet" + ex.Message);
                    return default;
                }
            }

            public static T GetApiTask<T>(string url, string PAT, string method = "GET", string requestBody = null)
            {
                try
                {
                    Console.WriteLine("Get Api Task");
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", PAT))));
                    HttpRequestMessage Request;
                    if (requestBody != null)
                    {
                        HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                        Request = new HttpRequestMessage(new HttpMethod(method), url) { Content = content };
                    }
                    else
                        Request = new HttpRequestMessage(new HttpMethod(method), url);

                    using (HttpResponseMessage response = client.SendAsync(Request).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = response.Content.ReadAsStringAsync().Result;
                            return JsonConvert.DeserializeObject<T>(responseBody);
                        }
                        else
                        {
                            var Error = response.Content.ReadAsStringAsync().Result;
                            return default;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Inside GetAPITask" + ex.Message);
                    return default;
                }
            }
        }
        public Object FecthWIDetailsinBatch(RequestModel requestModel)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string projName = requestModel.Project_Name;
                    string workitemType = requestModel.WI_Type;
                    string fieldName = requestModel.Field_Name;
                    
                    if (fieldName != null && workitemType != null && projName != null)
                    {
                        string queryString = @"select  " + fieldName + " from workitems WHERE [System.TeamProject] = '" + projName + "' AND [System.WorkItemType] = '" + workitemType + "'";
                        var wiql = new { query = queryString };
                        var content = Newtonsoft.Json.JsonConvert.SerializeObject(wiql);
                        string url = "https://dev.azure.com/" + org + "/_apis/wit/wiql?api-version=5.1";
                        CountvalueModel wiqlResponse = Store.GetApi<CountvalueModel>(url, pat, "POST", content);
                        string defaultUrl = "https://dev.azure.com/" + org + "/_apis/wit/workitems?ids=";
                        url = defaultUrl;
                        WorkItemReponseModel workItemReponse = new WorkItemReponseModel();
                        workItemReponse.value = new List<WIDetail>();
                        string endUrl = "&$expand=all&api-version=5.1";

                        //QueryValue testData = JsonConvert.DeserializeObject<QueryValue>(wiqlResponse); 
                        for (int j = 0; j < wiqlResponse.workItems.Count(); j++)
                        {
                            if (j % 200 == 0 && j != 0)
                            {
                                var batchResponse = Store.GetApi<WorkItemReponseModel>(url + endUrl, pat);
                                workItemReponse.count += batchResponse.count;
                                foreach (var item in batchResponse.value)
                                {
                                    workItemReponse.value.Add(item);
                                }
                                url = defaultUrl;
                            }
                            if (j % 200 == 0)
                            {
                                url += wiqlResponse.workItems[j].id;
                            }
                            else
                            {
                                url += "," + wiqlResponse.workItems[j].id;
                            }
                        }
                        url += endUrl;


                        var lastBatchResponse = Store.GetApi<WorkItemReponseModel>(url, pat);
                        workItemReponse.count += lastBatchResponse.count;
                        return lastBatchResponse;
                    }
                }
                
            }
            catch (Exception ex)
            {

            }
            return null;
        }


    }
}



