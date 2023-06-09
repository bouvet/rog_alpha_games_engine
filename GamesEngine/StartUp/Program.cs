using GamesEngine.Service.Communication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesEngine.Communication;
using GamesEngine.Patterns;
using GamesEngine.Service;
using Microsoft.AspNetCore.SignalR;


namespace GamesEngine.Communication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            GameHandler.CommunicationDispatcher = new CommunicationDispatcher();
            GameHandler.CommunicationStrategy = new SignalRCommunicationStrategy(((id, message) => GameHandler.Communication.OnMessage(id, message)));
            GameHandler.Communication = new Service.Communication.Communication(GameHandler.CommunicationStrategy, GameHandler.CommunicationDispatcher);

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // TODO: must remove signalR specific
            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:7247")
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            // TODO: must remove signalR specific
            app.MapHub<SignalRHub>("/gamehub");

            SignalRCommunicationStrategy.HubContext = ((IApplicationBuilder)app).ApplicationServices.GetService<IHubContext<SignalRHub>>();

            GameHandler.Start();

            app.Run();
        }
    }
}
