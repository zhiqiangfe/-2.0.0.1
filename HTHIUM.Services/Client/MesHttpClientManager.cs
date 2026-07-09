using HTHIUM.Core.Interfaces;
using System.Collections.Concurrent;
using System.Net.Http;

namespace HTHIUM.MES.Client
{
    /// <summary>
    /// MES HTTP客户端管理器接口
    /// </summary>
    public interface IMesHttpClientManager : IDisposable
    {
        HttpClient GetClient(string name = "default");
        HttpClient GetOrCreateClient(string name, Action<HttpClient>? configure = null);
        void ConfigureClient(string name, Action<HttpClient> configure);
    }

    /// <summary>
    /// MES HTTP客户端管理器实现
    /// </summary>
    public class MesHttpClientManager : IMesHttpClientManager
    {
        private readonly ConcurrentDictionary<string, HttpClient> _clients;
        private readonly ILoggerService<MesHttpClientManager> _logger;
        private readonly object _lockObject = new();
        private bool _disposed;

        public MesHttpClientManager(ILoggerService<MesHttpClientManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _clients = new ConcurrentDictionary<string, HttpClient>();
        }

        public HttpClient GetClient(string name = "default")
        {
            ThrowIfDisposed();

            if (_clients.TryGetValue(name, out var client))
            {
                return client;
            }

            throw new InvalidOperationException($"HttpClient '{name}' 未找到，请先使用ConfigureClient进行配置");
        }

        public HttpClient GetOrCreateClient(string name, Action<HttpClient>? configure = null)
        {
            ThrowIfDisposed();

            return _clients.GetOrAdd(name, key =>
            {
                lock (_lockObject)
                {
                    // 双重检查，避免重复创建
                    if (_clients.TryGetValue(key, out var existingClient))
                    {
                        return existingClient;
                    }

                    _logger.Debug($"创建新的HttpClient: {key}");
                    var newClient = new HttpClient();

                    configure?.Invoke(newClient);

                    return newClient;
                }
            });
        }

        public void ConfigureClient(string name, Action<HttpClient> configure)
        {
            ThrowIfDisposed();

            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            var client = GetOrCreateClient(name);
            configure(client);
            _logger.Debug($"配置HttpClient: {name}");
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MesHttpClientManager));
        }

        public void Dispose()
        {
            if (_disposed) return;

            foreach (var kvp in _clients)
            {
                try
                {
                    kvp.Value?.Dispose();
                    _logger.Debug($"释放HttpClient: {kvp.Key}");
                }
                catch (Exception ex)
                {
                    _logger.Error($"释放HttpClient {kvp.Key} 时发生错误", ex);
                }
            }

            _clients.Clear();
            _disposed = true;
            _logger.Info("MesHttpClientManager已释放");
        }
    }
}
