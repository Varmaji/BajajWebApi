using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BajajWebApi.Models
{
   
    public class Detail
    {
        public int? Count { get; set; }
        public Double? Sum { get; set; }
        public Double? Avearage { get; set; }
        public Double? Max { get; set; }
        public Double? Min { get; set; }
    }

    public class Avatar
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Avatar avatar { get; set; }
        public Self self { get; set; }
        public WorkItemUpdates workItemUpdates { get; set; }
        public WorkItemRevisions workItemRevisions { get; set; }
        public WorkItemHistory workItemHistory { get; set; }
        public Html html { get; set; }
        public WorkItemType workItemType { get; set; }
        public Fields fields { get; set; }
    }

    public class SystemAssignedTo
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class SystemCreatedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class SystemChangedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Fields
    {
        [JsonProperty("System.Id")]
        public int SystemId { get; set; }
        [JsonProperty("System.AreaId")]
        public int SystemAreaId { get; set; }
        [JsonProperty("System.AreaPath")]
        public string SystemAreaPath { get; set; }
        [JsonProperty("System.TeamProject")]
        public string SystemTeamProject { get; set; }
        [JsonProperty("System.NodeName")]
        public string SystemNodeName { get; set; }
        [JsonProperty("System.AreaLevel1")]
        public string SystemAreaLevel1 { get; set; }
        [JsonProperty("System.Rev")]
        public int SystemRev { get; set; }
        [JsonProperty("System.AuthorizedDate")]
        public DateTime SystemAuthorizedDate { get; set; }
        [JsonProperty("System.RevisedDate")]
        public DateTime SystemRevisedDate { get; set; }
        [JsonProperty("System.IterationId")]
        public int SystemIterationId { get; set; }
        [JsonProperty("System.IterationPath")]
        public string SystemIterationPath { get; set; }
        [JsonProperty("System.IterationLevel1")]
        public string SystemIterationLevel1 { get; set; }
        [JsonProperty("System.WorkItemType")]
        public string SystemWorkItemType { get; set; }
        [JsonProperty("System.State")]
        public string SystemState { get; set; }
        [JsonProperty("System.Reason")]
        public string SystemReason { get; set; }
        [JsonProperty("System.CreatedDate")]
        public DateTime SystemCreatedDate { get; set; }
        [JsonProperty("System.CreatedBy")]
        public SystemCreatedBy SystemCreatedBy { get; set; }
        [JsonProperty("System.ChangedDate")]
        public DateTime SystemChangedDate { get; set; }
        [JsonProperty("System.ChangedBy")]
        public SystemChangedBy SystemChangedBy { get; set; }
        [JsonProperty("System.AuthorizedAs")]
        public SystemAuthorizedAs SystemAuthorizedAs { get; set; }
        [JsonProperty("System.PersonId")]
        public int SystemPersonId { get; set; }
        [JsonProperty("System.Watermark")]
        public int SystemWatermark { get; set; }
        [JsonProperty("System.CommentCount")]
        public int SystemCommentCount { get; set; }
        [JsonProperty("System.Title")]
        public string SystemTitle { get; set; }
        [JsonProperty("Microsoft.VSTS.Common.StateChangeDate")]
        public DateTime MicrosoftVSTSCommonStateChangeDate { get; set; }
        [JsonProperty("Microsoft.VSTS.Common.Priority")]
        public int MicrosoftVSTSCommonPriority { get; set; }
        [JsonProperty("Microsoft.VSTS.Scheduling.TargetDate")]
        public DateTime MicrosoftVSTSSchedulingTargetDate { get; set; }
        [JsonProperty("Microsoft.VSTS.Scheduling.Effort")]
        public double MicrosoftVSTSSchedulingEffort { get; set; }
        [JsonProperty("Microsoft.VSTS.Scheduling.StartDate")]
        public DateTime MicrosoftVSTSSchedulingStartDate { get; set; }
        public string href { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class WorkItemUpdates
    {
        public string href { get; set; }
    }

    public class WorkItemRevisions
    {
        public string href { get; set; }
    }

    public class WorkItemHistory
    {
        public string href { get; set; }
    }

    public class Html
    {
        public string href { get; set; }
    }

    public class WorkItemType
    {
        public string href { get; set; }
    }

    public class WIDetail
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Dictionary<string,object> fields { get; set; }
        public Links _links { get; set; }
        public string url { get; set; }
    }

    public class ResponseModel
    {
        public int Request_ID { get; set; }
        public Detail Details { get; set; }
        public List<WIDetail> WI_Details { get; set; }
        public object Status { get; set; }
        public object Failure_Message { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    

    public class SystemAuthorizedAs
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

   

    public class WorkItemComments
    {
        public string href { get; set; }
    }


    public class Values
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public Links _links { get; set; }
        public string url { get; set; }
    }

    public class WorkItemReponseModel
    {
        public int count { get; set; }
        public List<WIDetail> value { get; set; }
    }


}