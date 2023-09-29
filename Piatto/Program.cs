using Piatto.P2P;
using Piatto.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;


namespace Piatto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            DotNetEnv.Env.TraversePath().Load();

            
            ServicePool.Add(
                dbService: new DbService(),
                matchmakingService: new MatchmakingService()
            );
            ServicePool.Start();


            IHost host = CreateHostBuilder(args).Build();

            // var game = new Game(matchmakingService: host.Services.GetRequiredService<IMatchmakingService>());
            // game.SearchGame();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .UseConsoleLifetime(opts => opts.SuppressStatusMessages = true)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        var GRPC_WEB_PORT = DotNetEnv.Env.GetInt("GRPC_WEB_PORT");
                        var GRPC_PORT = DotNetEnv.Env.GetInt("GRPC_PORT");

                        options.ListenAnyIP(GRPC_WEB_PORT, listenOptions => listenOptions.Protocols = HttpProtocols.Http1AndHttp2); //webapi
                        options.ListenAnyIP(GRPC_PORT, listenOptions => listenOptions.Protocols = HttpProtocols.Http2); //grpc
                    });

                    // start
                    webBuilder.UseStartup<Startup>()
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.AddConsole();
                        });
                });
    }
}