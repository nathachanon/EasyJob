using System;
using System.Collections.Generic;
using System.Text;

namespace EasyJob.Models
{
    public class Work_all
    {
        public System.Guid work_id { get; set; }
        public string work_name { get; set; }
        public string work_desc { get; set; }
        public string tel { get; set; }
        public int labor_cost { get; set; }
        public string duration { get; set; }
        public System.DateTime datetime { get; set; }
        public System.Guid member_id { get; set; }
        public string status { get; set; }
        public float lat { get; set; }
        public float @long {get;set;}
        public string loc_name { get; set; }

    }

    public class Work_dis
    {
        public System.Guid work_id { get; set; }
        public string work_name { get; set; }
        public string work_desc { get; set; }
        public string tel { get; set; }
        public int labor_cost { get; set; }
        public string duration { get; set; }
        public System.DateTime datetime { get; set; }
        public System.Guid member_id { get; set; }
        public string status { get; set; }
        public float lat { get; set; }
        public float @long { get; set; }
        public string loc_name { get; set; }
        public string distance { get; set; }
    }
    public class Member
    {
        public System.Guid member_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string tel { get; set; }

    }

    public class Work_Detail
    {
        public System.Guid work_id { get; set; }
        public string work_name { get; set; }
        public string work_desc { get; set; }
        public string tel { get; set; }
        public int labor_cost { get; set; }
        public string duration { get; set; }
        public System.DateTime datetime { get; set; }
        public System.Guid member_id { get; set; }
        public string status { get; set; }
        public float lat { get; set; }
        public float @long { get; set; }
        public string loc_name { get; set; }
        public string job_status { get; set; }
        public string job_owner_name { get; set; }
    }

    public class Jobs
    {
        public System.Guid work_id { get; set; }
        public string work_name { get; set; }
        public string work_desc { get; set; }
        public int labor_cost { get; set; }
        public string duration { get; set; }
        public System.Guid member_id { get; set; }
        public System.Guid location_id { get; set; }
        public System.Guid status_id { get; set; }
    }

    public class Job
    {
        public System.Guid work_id { get; set; }
        public string work_name { get; set; }
        public string work_desc { get; set; }
        public int labor_cost { get; set; }
        public string duration { get; set; }
        public System.Guid member_id { get; set; }
        public System.Guid location_id { get; set; }
        public string status { get; set; }
    }
}
