using Microsoft.Extensions.Options;
using GameServer.Configurations;
using GameServer.Handlers;
using GameServer.Interfaces;
using GameServer.Interfaces.WebSocketInterfaces;
using WebSocketSharp.Server;

namespace GameServer.Services
{
    public class MatchWebSocketService : IMatchWebSocketService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<WebSocketSharpConfiguration> _webSocketSharpConfiguration;
        
        private WebSocketServer? _webSocketServer;

        public MatchWebSocketService(IServiceProvider serviceProvider,
            IOptions<WebSocketSharpConfiguration> webSocketSharpConfiguration) 
        {
            _serviceProvider = serviceProvider;
            _webSocketSharpConfiguration = webSocketSharpConfiguration;
        }

        public void Start()
        {
            _webSocketServer = new WebSocketServer(_webSocketSharpConfiguration.Value.Port!.Value);
            _webSocketServer.AddWebSocketService<ConnectionHandler>(_webSocketSharpConfiguration.Value.Path, service =>
            {
                service.Initialize(_serviceProvider,
                    _serviceProvider.GetRequiredService<ICloseRequest>(),
                    _serviceProvider.GetRequiredService<IProcessRequest>()
                    );
            });

            _webSocketServer.Start();
        }

        public void Stop()
        {
            _webSocketServer?.Stop();
        }
    }
}
