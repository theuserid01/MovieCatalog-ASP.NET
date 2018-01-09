namespace MovieCatalog.Web.Infrastructure.Filters
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class LogExceptionsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Task.Run(async () =>
            {
                if (context.Exception != null)
                {
                    DateTime dateTime = DateTime.Now;
                    StringBuilder sb = new StringBuilder();

                    object action = context.RouteData.Values["action"];
                    string controller = context.Controller.GetType().Name;
                    string filename = dateTime.ToString("yyyy-MM-dd_HH.mm");
                    string filePath = $@"../Local/Exceptions/{filename}.txt";
                    string username = context.HttpContext.User?.Identity?.Name ?? "Anonymous";

                    string exceptionMessage = context.Exception.Message;
                    string exceptionStackTrace = context.Exception.StackTrace;
                    string exceptionType = context.Exception.GetType().Name;

                    sb.AppendLine($"{dateTime} – {username} – {controller}.{action}")
                        .AppendLine()
                        .AppendLine($"{exceptionType} - {exceptionMessage}")
                        .AppendLine(exceptionStackTrace);

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        await writer.WriteLineAsync(sb.ToString().Trim());
                    }
                }
            })
            .GetAwaiter()
            .GetResult();

            base.OnActionExecuted(context);
        }
    }
}
