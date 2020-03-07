namespace RPM.Web.Infrastructure.Extensions.Alerts
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.DependencyInjection;

    public class AlertDecoratorResult : IActionResult
    {
        public AlertDecoratorResult(IActionResult result, string type, string title, string body)
        {
            this.Result = result;
            this.Type = type;
            this.Title = title;
            this.Body = body;
        }

        public IActionResult Result { get; }
        public string Type { get; }
        public string Title { get; }
        public string Body { get; }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            //NOTE: Be sure you add a using statement for Microsoft.Extensions.DependencyInjection, otherwise
            //      this overload of GetService won't be available!
            var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();

            var tempData = factory.GetTempData(context.HttpContext);
            tempData["_alert.type"] = Type;
            tempData["_alert.title"] = Title;
            tempData["_alert.body"] = Body;

            await this.Result.ExecuteResultAsync(context);
        }
    }
}
