using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToolGood.Bedrock.Web.ModelBinders
{
    public class RsaDecryptModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {

            return new RsaDecryptModelBinder();
        }
    }


}
