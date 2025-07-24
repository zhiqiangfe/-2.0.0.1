using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibVLCSharp.Shared;
using NLog;

namespace SUNWODA_SEVB.Component.UserControls
{
    /// <summary>
    /// RTSPVideoView.xaml 的交互逻辑
    /// </summary>
    public partial class RTSPVideoView : UserControl
    {
        private LibVLC? _libVLC;
        private MediaPlayer? _mediaPlayer;
        private Media? _currentMedia;

        private bool _isReconnecting = false;
        private System.Timers.Timer? _timer;

        /// <summary>
        /// 日志器
        /// </summary>
        public ILogger Logger
        {
            get { return (ILogger)GetValue(LoggerProperty); }
            set { SetValue(LoggerProperty, value); }
        }

        public static readonly DependencyProperty LoggerProperty = DependencyProperty.Register(
            "Logger",
            typeof(ILogger),
            typeof(RTSPVideoView),
            new PropertyMetadata(null)
        );

        /// <summary>
        /// RTSP Url
        /// </summary>
        public string RTSPUrl
        {
            get { return (string)GetValue(RTSPUrlProperty); }
            set { SetValue(RTSPUrlProperty, value); }
        }

        public static readonly DependencyProperty RTSPUrlProperty = DependencyProperty.Register(
            "RTSPUrl",
            typeof(string),
            typeof(RTSPVideoView),
            new PropertyMetadata("", new PropertyChangedCallback(OnRTSPUrlChanged))
        );

        /// <summary>
        /// 视频状态设置
        /// </summary>
        public VideoStateSettings VideoStateSetting
        {
            get { return (VideoStateSettings)GetValue(VideoStateSettingProperty); }
            set { SetValue(VideoStateSettingProperty, value); }
        }

        public static readonly DependencyProperty VideoStateSettingProperty =
            DependencyProperty.Register(
                "VideoStateSetting",
                typeof(VideoStateSettings),
                typeof(RTSPVideoView),
                new PropertyMetadata(
                    VideoStateSettings.Play,
                    new PropertyChangedCallback(OnVideoStateSettingChanged)
                )
            );

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            private set { SetValue(IsPlayingProperty, value); }
        }

        public static readonly DependencyProperty IsPlayingProperty = DependencyProperty.Register(
            "IsPlaying",
            typeof(bool),
            typeof(RTSPVideoView),
            new PropertyMetadata(false)
        );

        /// <summary>
        /// 视频状态
        /// </summary>
        public string VideoState
        {
            get { return (string)GetValue(VideoStateProperty); }
            private set { SetValue(VideoStateProperty, value); }
        }

        public static readonly DependencyProperty VideoStateProperty = DependencyProperty.Register(
            "VideoState",
            typeof(string),
            typeof(RTSPVideoView),
            new PropertyMetadata("")
        );

        /// <summary>
        /// 视频帧率
        /// </summary>
        public float Fps
        {
            get { return (float)GetValue(FpsProperty); }
            private set { SetValue(FpsProperty, value); }
        }

        public static readonly DependencyProperty FpsProperty = DependencyProperty.Register(
            "Fps",
            typeof(float),
            typeof(RTSPVideoView),
            new PropertyMetadata(0f)
        );

        /// <summary>
        /// 视频最后帧的系统时间
        /// </summary>
        public DateTime VideoLastFrameSystemTime
        {
            get { return (DateTime)GetValue(VideoCurrentSystemTimeProperty); }
            private set { SetValue(VideoCurrentSystemTimeProperty, value); }
        }

        public static readonly DependencyProperty VideoCurrentSystemTimeProperty =
            DependencyProperty.Register(
                "VideoCurrentSystemTime",
                typeof(DateTime),
                typeof(RTSPVideoView),
                new PropertyMetadata(DateTime.Now)
            );

        public RTSPVideoView()
        {
            InitializeComponent();
            InitializeVLC();
            InitializeTimer();

            Unloaded += RTSPVideoView_Unloaded;
        }

        /// <summary>
        /// 初始化VLC
        /// </summary>
        private void InitializeVLC()
        {
            Core.Initialize();

            _libVLC = new LibVLC();

            _libVLC.Log += OnLibVLCLog;
            CreateMediaPlayer();
        }

        /// <summary>
        /// 创建媒体播放器
        /// </summary>
        private void CreateMediaPlayer()
        {
            _mediaPlayer?.Dispose();

            _mediaPlayer = new MediaPlayer(_libVLC!);
            videoView.MediaPlayer = _mediaPlayer;

            // 订阅事件
            _mediaPlayer.Playing += MediaPlayer_Playing;
            _mediaPlayer.Paused += MediaPlayer_Paused;
            _mediaPlayer.Stopped += MediaPlayer_Stopped;
            _mediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
            _mediaPlayer.Buffering += MediaPlayer_Buffering;
            _mediaPlayer.EndReached += MediaPlayer_EndReached;
            _mediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
            _mediaPlayer.LengthChanged += MediaPlayer_LengthChanged;
        }

        /// <summary>
        /// 初始化视频信息更新线程
        /// </summary>
        private void InitializeTimer()
        {
            _timer = new System.Timers.Timer()
            {
                Interval = 100, // 每100ms更新一次
            };
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        /// <summary>
        /// 播放视频流
        /// </summary>
        private void PlayStream()
        {
            try
            {
                if (_mediaPlayer!.Media is null)
                {
                    _currentMedia = new Media(_libVLC!, RTSPUrl, FromType.FromLocation);

                    // 添加媒体选项
                    //_currentMedia.AddOption(":network-caching=1000");
                    //_currentMedia.AddOption(":live-caching=1000");
                    //_currentMedia.AddOption(":rtsp-tcp");
                    ////_currentMedia.AddOption(":no-audio");

                    _mediaPlayer.Play(_currentMedia);
                    Dispatcher.Invoke(() =>
                    {
                        VideoLastFrameSystemTime = DateTime.Now;
                    });
                }
                else
                {
                    if (!_mediaPlayer.IsPlaying)
                    {
                        _mediaPlayer.Play();
                    }
                }
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    Logger?.Error(ex, $"{RTSPUrl}: 媒体播放失败");
                });
                throw;
            }
        }

        /// <summary>
        /// 暂停视频流
        /// </summary>
        private void PauseStream()
        {
            if (_mediaPlayer is not null)
            {
                if (_mediaPlayer.CanPause)
                {
                    if (_mediaPlayer.IsPlaying)
                        _mediaPlayer.Pause();
                }
            }
        }

        /// <summary>
        /// 停止视频流
        /// </summary>
        private void StopStream()
        {
            _mediaPlayer?.Stop();
            _currentMedia?.Dispose();
            _currentMedia = null;
        }

        /// <summary>
        /// 重新连接并播放视频流
        /// </summary>
        /// <returns></returns>
        private async Task ReconnectAsync()
        {
            if (_isReconnecting)
                return;

            _isReconnecting = true;
            Dispatcher.Invoke(() =>
            {
                Logger?.Info($"{RTSPUrl}: 开始重连...");
            });

            try
            {
                _mediaPlayer?.Stop();
                await Task.Delay(2000);

                CreateMediaPlayer();
                _currentMedia?.Dispose();
                PlayStream();

                Dispatcher.Invoke(() =>
                {
                    Logger?.Info($"{RTSPUrl}: 重连成功");
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    Logger?.Error(ex, $"{RTSPUrl}: 重连失败");
                });
            }
            finally
            {
                _isReconnecting = false;
            }
        }

        /// <summary>
        /// 更新视频信息
        /// </summary>
        private void UpdateTimeInfo()
        {
            if (_mediaPlayer == null)
                return;
            Dispatcher.Invoke(() =>
            {
                VideoState = _mediaPlayer.State.ToString();
                Fps = _mediaPlayer.Fps;
                IsPlaying = _mediaPlayer.IsPlaying;
            });
        }

        #region 事件处理
        private void RTSPVideoView_Unloaded(object sender, RoutedEventArgs e)
        {
            StopStream();
            _mediaPlayer?.Dispose();
            _currentMedia?.Dispose();
            _libVLC?.Dispose();
        }

        private static void OnRTSPUrlChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var rtspVideoView = d as RTSPVideoView;
            if (rtspVideoView is not null)
            {
                if (rtspVideoView.VideoStateSetting == VideoStateSettings.Play)
                {
                    rtspVideoView.PlayStream();
                }
            }
        }

        private static void OnVideoStateSettingChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var rtspVideoView = d as RTSPVideoView;
            if (rtspVideoView is not null)
            {
                if (string.IsNullOrEmpty(rtspVideoView.RTSPUrl))
                {
                    switch (rtspVideoView.VideoStateSetting)
                    {
                        case VideoStateSettings.Play:
                            rtspVideoView.PlayStream();
                            break;
                        case VideoStateSettings.Pause:
                            rtspVideoView.PauseStream();
                            break;
                        case VideoStateSettings.Stop:
                            rtspVideoView.StopStream();
                            break;
                    }
                }
            }
        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (_mediaPlayer!.IsPlaying)
            {
                UpdateTimeInfo();
            }
        }

        private void MediaPlayer_Playing(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Logger?.Info($"{RTSPUrl}: 开始播放");
            });
        }

        private void MediaPlayer_Paused(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Logger?.Info($"{RTSPUrl}: 播放暂停");
            });
        }

        private void MediaPlayer_Stopped(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Logger?.Info($"{RTSPUrl}: 播放停止");
            });
        }

        private void MediaPlayer_EncounteredError(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Logger?.Info($"{RTSPUrl}: 遇到播放错误，尝试重连...");
            });
            Task.Run(() =>
            {
                _ = ReconnectAsync();
            });
        }

        private void MediaPlayer_Buffering(object? sender, MediaPlayerBufferingEventArgs e)
        {
            if (e.Cache < 100)
            {
                Dispatcher.Invoke(() =>
                {
                    Logger?.Info($"{RTSPUrl}: 缓冲中: {e.Cache:F0}%");
                });
            }
        }

        private void MediaPlayer_EndReached(object? sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Logger?.Info($"{RTSPUrl}: 视频流结束");
            });
        }

        private void MediaPlayer_TimeChanged(object? sender, MediaPlayerTimeChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                VideoLastFrameSystemTime = DateTime.Now;
            });
        }

        private void MediaPlayer_LengthChanged(object? sender, MediaPlayerLengthChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Logger?.Info($"{RTSPUrl}: 媒体长度={TimeSpan.FromMilliseconds(e.Length)}");
            });
        }

        private void OnLibVLCLog(object? sender, LogEventArgs e)
        {
            if (e.Level <= LibVLCSharp.Shared.LogLevel.Warning)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    switch (e.Level)
                    {
                        case LibVLCSharp.Shared.LogLevel.Debug:
                            Logger?.Debug($"{e.Module}: {e.Message}");
                            break;
                        case LibVLCSharp.Shared.LogLevel.Notice:
                            Logger?.Info($"{e.Module}: {e.Message}");
                            break;
                        case LibVLCSharp.Shared.LogLevel.Warning:
                            Logger?.Warn($"{e.Module}: {e.Message}");
                            break;
                        case LibVLCSharp.Shared.LogLevel.Error:
                            Logger?.Error($"{e.Module}: {e.Message}");
                            break;
                    }
                });
            }
        }
        #endregion
    }

    public enum VideoStateSettings
    {
        Play,
        Pause,
        Stop,
    }
}
