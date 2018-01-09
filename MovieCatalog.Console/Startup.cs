namespace MovieCatalog.Console
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using MovieCatalog.Services.Html.Contracts;
    using MovieCatalog.Services.Html.Implementations;
    using static TestConstants;

    public class Startup
    {
        public static void Main()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            IBlurayDotComService brService = new BlurayDotComService();
            IBoxOfficeMojoService bomService = new BoxOfficeMojoService();
            IDvdEmpireService deService = new DvdEmpireService();
            IImdbService imdbService = new ImdbService();
            IRottenTomatoesService rtService = new RottenTomatoesService();

            Task.Run(async () =>
            {
                try
                {
                    //var model = await brService.SearchTitleResultsAsync(MadMaxSearchTitle);
                    //var model = await bomService.SearchTitleResultsAsync(MadMaxSearchTitle);
                    //var model = await deService.SearchTitleResultsAsync(MadMaxSearchTitle);
                    //var model = await imdbService.SearchTitleResultsAsync(MadMaxSearchTitle);
                    //var model = await rtService.SearchTitleResultsAsync(MadMaxSearchTitle);

                    //var m1 = await brService.GetMovieDataAsync(MadMaxFuryRoadBrId);
                    //var m2 = await bomService.GetMovieDataAsync(MadMaxFuryRoadBomId);
                    //var m3 = await deService.GetMovieDataAsync(MadMaxFuryRoadDeId);
                    var m4 = await imdbService.GetMovieDataAsync(MadMaxFuryRoadImdbId);
                    //var m5 = await rtService.GetMovieDataAsync(MadMaxFuryRoadRtId);
                    //Console.WriteLine(m1);
                    //Console.WriteLine(m2);
                    //Console.WriteLine(m3);
                    //Console.WriteLine(m4);
                    //Console.WriteLine(m5);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            })
            .GetAwaiter()
            .GetResult();

            stopwatch.Stop();
            Console.WriteLine("Time elapsed: {0}", stopwatch.Elapsed);
        }
    }
}
