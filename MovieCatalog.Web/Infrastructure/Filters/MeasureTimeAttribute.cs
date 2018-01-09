namespace MovieCatalog.Web.Infrastructure.Filters
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class MeasureTimeAttribute : ActionFilterAttribute
    {
        private Stopwatch stopwatch;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.stopwatch = Stopwatch.StartNew();

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            this.stopwatch.Stop();

            using (StreamWriter writer = new StreamWriter(@"../Logs/action-times.txt", true))
            {
                DateTime dateTime = DateTime.UtcNow;
                string controller = context.Controller.GetType().Name;
                object action = context.RouteData.Values["action"];
                TimeSpan elapseTime = this.stopwatch.Elapsed;

                string logMessage = $"{dateTime} – {controller}.{action} – {elapseTime}";

                writer.WriteLine(logMessage);
            }

            base.OnActionExecuted(context);
        }
    }
}
