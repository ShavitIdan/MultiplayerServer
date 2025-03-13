using LobbyServer.Interfaces;
using LobbyServer.Services;

namespace LobbyServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IRedisBaseService, RedisBaseService>()
                .AddSingleton<IPlayersRedisService, PlayersRedisService>()
            ;
        }
    }
}
