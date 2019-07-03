namespace ToolGood.Bedrock.Web
{
    public class MyConfig
    {
        public bool UseResponseCaching { get; set; } = true;
        public bool UseResponseCompression { get; set; } = true;
        public bool UseSession { get; set; } = true;
        public bool UseCookie { get; set; } = true;
        public bool UselLetsEncrypt { get; set; } = true;
        public bool UseHsts { get; set; } = false;
        public bool UseMvc { get; set; } = true;
        public bool UseCors { get; set; } = false;

        public bool UseIHttpContextAccessor { get; set; } = true;
    }
}
