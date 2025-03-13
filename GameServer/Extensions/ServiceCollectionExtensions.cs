using GameServer.Configurations;
using GameServer.Interfaces;
using GameServer.Managers;
using GameServer.Services;
using GameServer.Services.HostedServices;
using GameServer.Services.Redis;
using GameServer.Services.Requests;
using System.Reflection;
using GameServer.Handlers;

using GameServer.Interfaces.WebSocketInterfaces;

namespace GameServer.Extensions
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddService(this IServiceCollection serviceCollection)
        {
            var serviceHandlers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IServiceHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();

            foreach (var type in serviceHandlers)
            {
                serviceCollection.AddSingleton(typeof(IServiceHandler), type);
            }

            serviceCollection.AddSingleton<IDictionary<string, IServiceHandler>>(sp =>
            {
                var handlers = sp.GetServices<IServiceHandler>();
                return handlers.ToDictionary(h => h.ServiceName, h => h);
            });

            return serviceCollection
                .AddConfiguration()
                .AddSingleton<IMatchWebSocketService, MatchWebSocketService>()
                .AddSingleton<ICloseRequest, CloseRequest>()
                .AddSingleton<IConnectionRequest, ConnectionRequest>()
                .AddSingleton<IRedisBaseService, RedisBaseService>()
                .AddSingleton<IProcessRequest, ProcessRequest>()
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<IRoomIdRedisService, RoomIdRedisService>()
                .AddSingleton<IRandomizerService, RandomizerService>()
                .AddSingleton<IDateTimeService, DateTimeService>()
                .AddTransient<ConnectionHandler>()
                .AddSingleton<SessionManager>()
                .AddSingleton<IdToUserIdManager>()
                .AddSingleton<RoomsManager>()
                .AddHostedService<GameLoopHostedService>()
                .AddHostedService<MatchWebSocketHostedService>();

        }

        private static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection
           .AddOptions<WebSocketSharpConfiguration>()
           .BindConfiguration("WebSocketSharp")
           .ValidateDataAnnotations()
           .ValidateOnStart();

            return serviceCollection;
        }
    }
}
