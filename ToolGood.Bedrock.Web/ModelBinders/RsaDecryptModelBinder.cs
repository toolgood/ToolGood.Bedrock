using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ToolGood.AntiDuplication;
using Newtonsoft.Json;

namespace ToolGood.Bedrock.Web.ModelBinders
{
    public class RsaDecryptModelBinder : IModelBinder
    {
        private static string RsaFile = "/App_Data/Rsa.xml";
        private static AntiDupCache<string, string> cache = new AntiDupCache<string, string>(5, 60);

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var request = bindingContext.HttpContext.Request;

            if (request.Method == "POST") {
                if (request.Form.ContainsKey("rsaData")) {
                    var data = request.Form["rsaData"].ToSafeString();
                    var key = cache.GetOrAdd(DateTime.Now.ToString("yyyyMMdd"), async () => {
                        return await File.ReadAllTextAsync(RsaFile);
                    });
                    var json = RsaUtil.PrivateDecrypt(key.Result, data);
                    var model = JsonConvert.DeserializeObject(json, bindingContext.ModelType);

                    bindingContext.Result = ModelBindingResult.Success(model);
                }
            }
        }
    }


}
