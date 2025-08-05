using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using MdXaml;

namespace SUNWODA_SEVB.Component.CustomControls
{
    /// <summary>
    /// 流式AI对话控件主类
    /// </summary>
    [TemplatePart(Name = "PART_MessagesContainer", Type = typeof(ItemsControl))]
    [TemplatePart(Name = "PART_ScrollViewer", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_LoadingIndicator", Type = typeof(FrameworkElement))]
    public class StreamingAIChatControl : Control
    {
        private ItemsControl _messagesContainer;
        private ScrollViewer _scrollViewer;
        private FrameworkElement _loadingIndicator;
        private readonly Subject<ChatMessage> _messageStream = new Subject<ChatMessage>();
        private readonly DispatcherTimer _autoScrollTimer;

        static StreamingAIChatControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StreamingAIChatControl),
                new FrameworkPropertyMetadata(typeof(StreamingAIChatControl)));
        }

        public StreamingAIChatControl()
        {
            Messages = new ObservableCollection<ChatMessageViewModel>();
            SetupMessageStream();

            _autoScrollTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _autoScrollTimer.Tick += OnAutoScrollTimerTick;
        }

        #region Dependency Properties

        public static readonly DependencyProperty MessagesProperty =
            DependencyProperty.Register(nameof(Messages), typeof(ObservableCollection<ChatMessageViewModel>),
                typeof(StreamingAIChatControl), new PropertyMetadata(null));

        public ObservableCollection<ChatMessageViewModel> Messages
        {
            get => (ObservableCollection<ChatMessageViewModel>)GetValue(MessagesProperty);
            set => SetValue(MessagesProperty, value);
        }

        public static readonly DependencyProperty MaxMessagesProperty =
            DependencyProperty.Register(nameof(MaxMessages), typeof(int),
                typeof(StreamingAIChatControl), new PropertyMetadata(1000));

        public int MaxMessages
        {
            get => (int)GetValue(MaxMessagesProperty);
            set => SetValue(MaxMessagesProperty, value);
        }

        public static readonly DependencyProperty EnableMarkdownProperty =
            DependencyProperty.Register(nameof(EnableMarkdown), typeof(bool),
                typeof(StreamingAIChatControl), new PropertyMetadata(true));

        public bool EnableMarkdown
        {
            get => (bool)GetValue(EnableMarkdownProperty);
            set => SetValue(EnableMarkdownProperty, value);
        }

        public static readonly DependencyProperty ShowThinkingProcessProperty =
            DependencyProperty.Register(nameof(ShowThinkingProcess), typeof(bool),
                typeof(StreamingAIChatControl), new PropertyMetadata(true));

        public bool ShowThinkingProcess
        {
            get => (bool)GetValue(ShowThinkingProcessProperty);
            set => SetValue(ShowThinkingProcessProperty, value);
        }

        public static readonly DependencyProperty StreamingSpeedProperty =
            DependencyProperty.Register(nameof(StreamingSpeed), typeof(int),
                typeof(StreamingAIChatControl), new PropertyMetadata(20));

        public int StreamingSpeed
        {
            get => (int)GetValue(StreamingSpeedProperty);
            set => SetValue(StreamingSpeedProperty, value);
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(nameof(IsLoading), typeof(bool),
                typeof(StreamingAIChatControl), new PropertyMetadata(false, OnIsLoadingChanged));

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.Register(nameof(AutoScroll), typeof(bool),
                typeof(StreamingAIChatControl), new PropertyMetadata(true));

        public bool AutoScroll
        {
            get => (bool)GetValue(AutoScrollProperty);
            set => SetValue(AutoScrollProperty, value);
        }

        #endregion

        #region Routed Events

        public static readonly RoutedEvent MessageSentEvent =
            EventManager.RegisterRoutedEvent(nameof(MessageSent), RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(StreamingAIChatControl));

        public event RoutedEventHandler MessageSent
        {
            add { AddHandler(MessageSentEvent, value); }
            remove { RemoveHandler(MessageSentEvent, value); }
        }

        public static readonly RoutedEvent ThinkingCompletedEvent =
            EventManager.RegisterRoutedEvent(nameof(ThinkingCompleted), RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(StreamingAIChatControl));

        public event RoutedEventHandler ThinkingCompleted
        {
            add { AddHandler(ThinkingCompletedEvent, value); }
            remove { RemoveHandler(ThinkingCompletedEvent, value); }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _messagesContainer = GetTemplateChild("PART_MessagesContainer") as ItemsControl;
            _scrollViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            _loadingIndicator = GetTemplateChild("PART_LoadingIndicator") as FrameworkElement;

            if (_messagesContainer != null)
            {
                _messagesContainer.ItemsSource = Messages;
            }
        }

        private void SetupMessageStream()
        {
            // 使用Reactive Extensions处理流式消息
            _messageStream
                .Buffer(TimeSpan.FromMilliseconds(50)) // 批量处理以提高性能
                .Where(messages => messages.Any())
                .ObserveOn(Scheduler.CurrentThread)
                .Subscribe(messages =>
                {
                    foreach (var message in messages)
                    {
                        var viewModel = new ChatMessageViewModel(message, EnableMarkdown);
                        Messages.Add(viewModel);

                        // 限制消息数量
                        while (Messages.Count > MaxMessages)
                        {
                            Messages.RemoveAt(0);
                        }
                    }

                    if (AutoScroll)
                    {
                        _autoScrollTimer.Start();
                    }
                });
        }

        private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StreamingAIChatControl control && control._loadingIndicator != null)
            {
                control._loadingIndicator.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OnAutoScrollTimerTick(object sender, EventArgs e)
        {
            _scrollViewer?.ScrollToBottom();
            _autoScrollTimer.Stop();
        }

        /// <summary>
        /// 添加消息到聊天流
        /// </summary>
        public async Task AddMessageAsync(string content, MessageRole role,
            CancellationToken cancellationToken = default)
        {
            var message = new ChatMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = content,
                Role = role,
                Timestamp = DateTime.Now
            };

            if (role == MessageRole.Assistant)
            {
                // AI消息使用流式显示
                await StreamAssistantMessageAsync(message, cancellationToken);
            }
            else
            {
                _messageStream.OnNext(message);
            }

            RaiseEvent(new RoutedEventArgs(MessageSentEvent, this));
        }

        /// <summary>
        /// 流式显示AI助手消息
        /// </summary>
        private async Task StreamAssistantMessageAsync(ChatMessage message,
            CancellationToken cancellationToken)
        {
            var streamingMessage = new ChatMessage
            {
                Id = message.Id,
                Role = message.Role,
                Timestamp = message.Timestamp,
                Content = "",
                IsStreaming = true
            };

            var viewModel = new ChatMessageViewModel(streamingMessage, EnableMarkdown);

            await Dispatcher.InvokeAsync(() =>
            {
                Messages.Add(viewModel);
                IsLoading = false;
            });

            // 流式输出，支持自定义速度
            var chunks = SplitIntoChunks(message.Content);
            foreach (var chunk in chunks)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                await Dispatcher.InvokeAsync(() => viewModel.AppendContent(chunk));

                if (AutoScroll)
                {
                    await Dispatcher.InvokeAsync(() => _scrollViewer?.ScrollToBottom());
                }

                await Task.Delay(StreamingSpeed, cancellationToken);
            }

            await Dispatcher.InvokeAsync(() => viewModel.IsStreaming = false);
        }

        /// <summary>
        /// 将文本分割成合适的块以优化流式显示
        /// </summary>
        private string[] SplitIntoChunks(string text)
        {
            var chunks = new List<string>();
            var words = text.Split(' ');
            var currentChunk = "";

            foreach (var word in words)
            {
                if (currentChunk.Length + word.Length > 5) // 每5个字符一个块
                {
                    if (!string.IsNullOrEmpty(currentChunk))
                        chunks.Add(currentChunk + " ");
                    currentChunk = word;
                }
                else
                {
                    currentChunk += (string.IsNullOrEmpty(currentChunk) ? "" : " ") + word;
                }
            }

            if (!string.IsNullOrEmpty(currentChunk))
                chunks.Add(currentChunk);

            return chunks.ToArray();
        }

        /// <summary>
        /// 显示AI思考过程
        /// </summary>
        public async Task ShowThinkingStepsAsync(ThinkingProcess process,
            CancellationToken cancellationToken = default)
        {
            if (!ShowThinkingProcess || process == null || !process.Steps.Any())
                return;

            var thinkingMessage = new ChatMessage
            {
                Id = Guid.NewGuid().ToString(),
                Role = MessageRole.System,
                Content = "AI正在思考...",
                Timestamp = DateTime.Now,
                ThinkingSteps = process.Steps
            };

            var viewModel = new ChatMessageViewModel(thinkingMessage, EnableMarkdown);

            await Dispatcher.InvokeAsync(() =>
            {
                Messages.Add(viewModel);
                IsLoading = true;
            });

            // 逐步显示思考过程
            foreach (var step in process.Steps)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                await Dispatcher.InvokeAsync(() =>
                {
                    viewModel.UpdateThinkingStep(step);
                    if (AutoScroll)
                    {
                        _scrollViewer?.ScrollToBottom();
                    }
                });

                await Task.Delay(500, cancellationToken);
            }

            await Dispatcher.InvokeAsync(() =>
            {
                IsLoading = false;
                RaiseEvent(new RoutedEventArgs(ThinkingCompletedEvent, this));
            });
        }

        /// <summary>
        /// 清空所有消息
        /// </summary>
        public void ClearMessages()
        {
            Messages.Clear();
        }

        /// <summary>
        /// 删除指定消息
        /// </summary>
        public void RemoveMessage(string messageId)
        {
            var message = Messages.FirstOrDefault(m => m.Id == messageId);
            if (message != null)
            {
                Messages.Remove(message);
            }
        }

        /// <summary>
        /// 停止当前的流式输出
        /// </summary>
        public void StopStreaming()
        {
            var streamingMessage = Messages.FirstOrDefault(m => m.IsStreaming);
            if (streamingMessage != null)
            {
                streamingMessage.IsStreaming = false;
            }
        }
    }

    /// <summary>
    /// 聊天消息视图模型
    /// </summary>
    public class ChatMessageViewModel : INotifyPropertyChanged
    {
        private readonly ChatMessage _message;
        private string _displayContent;
        private bool _isStreaming;
        private ThinkingStep _currentThinkingStep;
        private readonly Markdown _markdownEngine;
        private readonly bool _enableMarkdown;
        private FlowDocument _cachedDocument;
        private DateTime _lastUpdate;

        public ChatMessageViewModel(ChatMessage message, bool enableMarkdown = true)
        {
            _message = message;
            _displayContent = message.Content;
            _isStreaming = message.IsStreaming;
            _enableMarkdown = enableMarkdown;
            _lastUpdate = DateTime.Now;

            if (_enableMarkdown)
            {
                _markdownEngine = new Markdown();
                // 配置Markdown引擎
                _markdownEngine.AssetPathRoot = Environment.CurrentDirectory;
            }
        }

        public string Id => _message.Id;
        public MessageRole Role => _message.Role;
        public DateTime Timestamp => _message.Timestamp;
        public ObservableCollection<ThinkingStep> ThinkingSteps => _message.ThinkingSteps;

        public string DisplayContent
        {
            get => _displayContent;
            private set
            {
                if (_displayContent != value)
                {
                    _displayContent = value;
                    _cachedDocument = null; // 清除缓存
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(MarkdownDocument));
                }
            }
        }

        public FlowDocument MarkdownDocument
        {
            get
            {
                if (!_enableMarkdown)
                {
                    return CreatePlainTextDocument();
                }

                // 使用缓存以提高性能
                if (_cachedDocument != null && (DateTime.Now - _lastUpdate).TotalMilliseconds < 100)
                {
                    return _cachedDocument;
                }

                try
                {
                    _cachedDocument = _markdownEngine.Transform(DisplayContent ?? string.Empty);
                    _lastUpdate = DateTime.Now;

                    // 应用样式
                    ApplyMarkdownStyles(_cachedDocument);

                    return _cachedDocument;
                }
                catch (Exception)
                {
                    return CreatePlainTextDocument();
                }
            }
        }

        private FlowDocument CreatePlainTextDocument()
        {
            var doc = new FlowDocument();
            var paragraph = new Paragraph(new Run(DisplayContent ?? string.Empty))
            {
                Margin = new Thickness(0),
                LineHeight = 20
            };
            doc.Blocks.Add(paragraph);
            return doc;
        }

        private void ApplyMarkdownStyles(FlowDocument document)
        {
            if (document == null) return;

            // 设置文档样式
            document.FontFamily = new FontFamily("Segoe UI");
            document.FontSize = 14;
            document.LineHeight = 22;
            document.Foreground = Role == MessageRole.User ?
                Brushes.DarkBlue : Brushes.DarkGreen;

            // 遍历所有块元素应用样式
            foreach (var block in document.Blocks)
            {
                if (block is Paragraph para)
                {
                    para.Margin = new Thickness(0, 5, 0, 5);
                }
                else if (block is List list)
                {
                    list.Margin = new Thickness(20, 5, 0, 5);
                }
            }
        }

        public bool IsStreaming
        {
            get => _isStreaming;
            set
            {
                if (_isStreaming != value)
                {
                    _isStreaming = value;
                    OnPropertyChanged();
                }
            }
        }

        public ThinkingStep CurrentThinkingStep
        {
            get => _currentThinkingStep;
            set
            {
                if (_currentThinkingStep != value)
                {
                    _currentThinkingStep = value;
                    OnPropertyChanged();
                }
            }
        }

        public void AppendContent(string content)
        {
            DisplayContent += content;
        }

        public void UpdateThinkingStep(ThinkingStep step)
        {
            CurrentThinkingStep = step;
            step.IsCompleted = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// 聊天消息数据模型
    /// </summary>
    public class ChatMessage
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public MessageRole Role { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsStreaming { get; set; }
        public ObservableCollection<ThinkingStep> ThinkingSteps { get; set; } =
            new ObservableCollection<ThinkingStep>();
    }

    /// <summary>
    /// 消息角色枚举
    /// </summary>
    public enum MessageRole
    {
        User,
        Assistant,
        System
    }

    /// <summary>
    /// AI思考过程
    /// </summary>
    public class ThinkingProcess
    {
        public ObservableCollection<ThinkingStep> Steps { get; set; } =
            new ObservableCollection<ThinkingStep>();

        public void AddStep(string title, string description, double confidence = 0.5)
        {
            Steps.Add(new ThinkingStep
            {
                Title = title,
                Description = description,
                ConfidenceLevel = confidence,
                StartTime = DateTime.Now
            });
        }
    }

    /// <summary>
    /// 思考步骤
    /// </summary>
    public class ThinkingStep : INotifyPropertyChanged
    {
        private bool _isCompleted;
        private string _result;

        public string Title { get; set; }
        public string Description { get; set; }
        public double ConfidenceLevel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    if (value)
                    {
                        EndTime = DateTime.Now;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                if (_result != value)
                {
                    _result = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeSpan Duration => EndTime.HasValue ?
            EndTime.Value - StartTime : TimeSpan.Zero;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
