using Piatto.Grpc;
using Piatto.Services;

namespace Piatto
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            }));
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            appLifetime.ApplicationStarted.Register(() => {
                Console.WriteLine("Press Ctrl+C to shut down.");
            });

            appLifetime.ApplicationStopped.Register(() => {
                Console.WriteLine("Terminating application...");
                Console.WriteLine("Stopping services...");
                ServicePool.Stop();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            });

            app.UseRouting();
            // add support grpc call from web app, Must be added between UseRouting and UseEndpoints
            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
            app.UseCors();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MatckmakingServiceImpl>().RequireCors("AllowAll");
                endpoints.MapGrpcService<MoveServiceImpl>().RequireCors("AllowAll");
                endpoints.MapGrpcService<GameServiceImpl>().RequireCors("AllowAll");

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(
                        "Communication with gRPC endpoints" +
                        " must be made through a gRPC client.");
                });
            });
        }
    }
}