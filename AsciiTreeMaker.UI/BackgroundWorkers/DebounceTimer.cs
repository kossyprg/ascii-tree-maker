using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsciiTreeMaker.UI.BackgroundWorkers
{
    /// <summary>
    /// デバウンス用タイマー
    /// </summary>
    internal sealed class DebounceTimer : IDisposable
    {
        /// <summary>
        /// 最後の要求から処理を実行するまでの待機時間
        /// </summary>
        private readonly TimeSpan _interval;

        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private readonly object _lock = new();

        /// <summary>
        /// CancellationTokenSource
        /// </summary>
        private CancellationTokenSource? _cts;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="interval"></param>
        public DebounceTimer(TimeSpan interval)
        {
            _interval = interval;
        }

        /// <summary>
        /// 処理実行を要求する
        /// </summary>
        /// <param name="action"></param>
        public void Request(Action action)
        {
            lock (_lock)
            {
                _cts?.Cancel();
                _cts = new CancellationTokenSource();
                var token = _cts.Token;

                Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(_interval, token);
                        action();
                    }
                    catch (TaskCanceledException)
                    {
                    }
                });
            }
        }

        /// <summary>
        /// リソースを解放する
        /// </summary>
        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
