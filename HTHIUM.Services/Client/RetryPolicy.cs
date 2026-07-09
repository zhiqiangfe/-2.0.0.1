using HTHIUM.Core.Interfaces;
using System.Net.Http;
using System.Net;

namespace HTHIUM.MES.Client
{
    /// <summary>
    /// MES的重试策略实现
    /// </summary>
    public class RetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _delay;
        private readonly ILoggerService? _logger;

        public RetryPolicy(int maxRetries = 3, int delayMilliseconds = 1000, ILoggerService? logger = null)
        {
            _maxRetries = Math.Max(1, maxRetries);
            _delay = TimeSpan.FromMilliseconds(Math.Max(100, delayMilliseconds));
            _logger = logger;
        }

        public async Task<HttpResponseMessage> ExecuteHttpAsync(
            Func<Task<HttpResponseMessage>> operation,
            CancellationToken cancellationToken = default)
        {
            var attempt = 0;
            // 用于收集每次尝试失败时产生的异常
            var exceptions = new List<Exception>();

            while (attempt < _maxRetries)
            {
                try
                {
                    attempt++;

                    // 在执行操作前检查是否已请求取消
                    cancellationToken.ThrowIfCancellationRequested();

                    var response = await operation();

                    // 如果是成功状态码，直接返回
                    if (response.IsSuccessStatusCode)
                    {
                        if (attempt > 1)
                        {
                            _logger?.Info($"请求在第 {attempt} 次尝试后成功");
                        }
                        return response;
                    }

                    // 如果是不应该重试的状态码，或者已经是最后一次尝试，则直接返回该响应
                    if (!ShouldRetry(response.StatusCode) || attempt >= _maxRetries)
                    {
                        // 即使是最后一次尝试，如果HTTP层面有响应，也应该返回它，让调用者处理
                        _logger?.Warn($"请求失败，状态码: {(int)response.StatusCode} {response.ReasonPhrase}，不再重试。");
                        return response;
                    }

                    _logger?.Warn($"HTTP {(int)response.StatusCode} {response.ReasonPhrase} (尝试 {attempt}/{_maxRetries})，准备重试...");

                    // 计算延迟时间
                    var delay = CalculateDelay(attempt);
                    await Task.Delay(delay, cancellationToken);

                    response.Dispose(); // 释放当前失败的响应，避免资源泄露
                }
                // 将所有可重试的异常（网络层、超时）统一处理
                catch (Exception ex) when (ex is HttpRequestException ||
                                           (ex is TaskCanceledException && !cancellationToken.IsCancellationRequested))
                {
                    exceptions.Add(ex); // 收集异常信息

                    // 如果已经是最后一次尝试，则跳出循环，在循环外抛出聚合异常
                    if (attempt >= _maxRetries)
                    {
                        _logger?.Error($"HTTP请求在第 {attempt} 次尝试后仍然失败: {ex.Message}", ex);
                        break; 
                    }

                    _logger?.Warn($"HTTP请求失败 (尝试 {attempt}/{_maxRetries}): {ex.Message}，准备重试...");

                    var delay = CalculateDelay(attempt);
                    await Task.Delay(delay, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger?.Error($"HTTP请求发生非预期错误: {ex.Message}", ex);
                    throw; 
                }
            }

            throw new AggregateException($"HTTP请求在 {_maxRetries} 次尝试后彻底失败。", exceptions);
        }

        private TimeSpan CalculateDelay(int attempt)
        {
            // 使用指数退避策略，但设置最大延迟时间
            var delayMs = _delay.TotalMilliseconds * Math.Pow(2, attempt - 1);
            var maxDelayMs = 30000; // 最大延迟30秒
            return TimeSpan.FromMilliseconds(Math.Min(delayMs, maxDelayMs));
        }

        private bool ShouldRetry(HttpStatusCode statusCode)
        {
            return statusCode >= HttpStatusCode.InternalServerError || // 5xx 错误
                   statusCode == HttpStatusCode.TooManyRequests ||     // 429
                   statusCode == HttpStatusCode.RequestTimeout ||       // 408
                   statusCode == HttpStatusCode.ServiceUnavailable ||   // 503
                   statusCode == HttpStatusCode.GatewayTimeout;        // 504
        }
    }
}
