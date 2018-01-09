namespace MovieCatalog.Web.Infrastructure.Extensions
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using MovieCatalog.Common;

    public static class ExceptionExtensions
    {
        public static void Log(this Exception exception, string controller, string action)
        {
            Task.Run(async () =>
            {
                DateTime dateTime = DateTime.Now;
                StringBuilder sb = new StringBuilder();

                string filename = $"{dateTime.ToString("yyyy-MM-dd_HH.mm")}_{controller}{action}";
                string filePath = $@"{GlobalConstants.LogExceptionsDir}/{filename}.txt";

                string exceptionMessage = exception.Message;
                string exceptionStackTrace = exception.StackTrace;
                string exceptionType = exception.GetType().Name;

                sb.AppendLine($"{dateTime} – {controller}.{action}")
                    .AppendLine()
                    .AppendLine($"{exceptionType} - {exceptionMessage}")
                    .AppendLine(exceptionStackTrace);

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    await writer.WriteLineAsync(sb.ToString().Trim());
                }
            })
            .GetAwaiter()
            .GetResult();
        }
    }
}
