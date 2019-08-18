using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Bedrock.Web.ThirdPartys.Ztree
{
    public class ZTreeNode
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("pId")]
        public int pId { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("isParent", NullValueHandling = NullValueHandling.Ignore)]
        public bool? isParent { get; set; }

        [JsonProperty("open", NullValueHandling = NullValueHandling.Ignore)]
        public bool? open { get; set; }




        [JsonProperty("isHidden", NullValueHandling = NullValueHandling.Ignore)]
        public bool? isHidden { get; set; }

        [JsonProperty("target", NullValueHandling = NullValueHandling.Ignore)]
        public string target { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string url { get; set; }



        [JsonProperty("checked", NullValueHandling = NullValueHandling.Ignore)]
        public bool? isChecked { get; set; }

        [JsonProperty("chkDisabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? chkDisabled { get; set; }

        [JsonProperty("halfCheck", NullValueHandling = NullValueHandling.Ignore)]
        public bool? halfCheck { get; set; }

        [JsonProperty("nocheck", NullValueHandling = NullValueHandling.Ignore)]
        public bool? nocheck { get; set; }



        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string icon { get; set; }

        [JsonProperty("iconOpen", NullValueHandling = NullValueHandling.Ignore)]
        public string iconOpen { get; set; }

        [JsonProperty("iconClose", NullValueHandling = NullValueHandling.Ignore)]
        public string iconClose { get; set; }

        [JsonProperty("iconSkin", NullValueHandling = NullValueHandling.Ignore)]
        public string iconSkin { get; set; }

        [JsonProperty("extend", NullValueHandling = NullValueHandling.Ignore)]
        public object extend { get; set; }

    }

}
