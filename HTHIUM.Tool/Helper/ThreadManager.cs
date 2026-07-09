using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace HTHIUM.Tool.Helper
{
    /// <summary>
    /// <para>
    /// 线程管理类(单例 <see cref="Instance"/>)
    /// </para>
    /// <para>
    /// 初始化线程管理器调用 <see cref="Init()"/>()
    /// </para>
    /// </summary>
    public class ThreadManager
    {
        private static object _lock = new object();

        /// <summary>
        /// 线程管理类(<see cref="ThreadManager"/>)实例
        /// </summary>
        public static ThreadManager? Instance { get; private set; }

        private static Dispatcher? MainThreadDispatcher { get; set; }

        public ObservableCollection<ThreadTaskCancelSignal> ThreadTaskCancelSignals { get; set; } =
            new ObservableCollection<ThreadTaskCancelSignal>();

        /// <summary>
        /// 运行的线程集合
        /// </summary>
        public ObservableCollection<ThreadInfo> ThreadsInfo { get; private set; } =
            new ObservableCollection<ThreadInfo>();

        public delegate void ThreadInfoEventHandler(object obj);

        public delegate void ThreadTask(ThreadTaskCancelSignal cancelSignal);

        public delegate void ThreadTaskWithParameter(
            ThreadTaskCancelSignal cancelSignal,
            object parameter
        );

        /// <summary>
        /// 创建线程
        /// </summary>
        /// <param name="name">线程名称</param>
        /// <param name="cancelSignal">任务取消信号<see cref="ThreadTaskCancelSignal"/></param>
        /// <param name="threadTask">任务</param>
        /// <returns>线程</returns>
        /// <exception cref="ArgumentException">线程名已存在，再次命名会造成冲突</exception>
        /// <example>
        /// 这是参数<paramref name="threadTask"/>的传入例子
        /// <code>
        /// (cancelSignal) =>
        /// {
        ///     while (!cancelSignal.CancelSignal)
        ///     {
        ///         // 任务内容
        ///     }
        /// }
        /// </code>
        /// </example>
        public Thread CreateThread(
            string name,
            ThreadTaskCancelSignal cancelSignal,
            ThreadTask threadTask
        )
        {
            if (name == null || cancelSignal == null || threadTask == null)
            {
                throw new ArgumentNullException("创建线程失败：传入空对象引用");
            }

            lock (_lock)
            {
                if (ThreadsInfo.Where((t) => t.Name == name).Count() != 0)
                {
                    throw new ArgumentException("创建线程失败：线程名已存在，再次命名会造成冲突");
                }
            }

            uint threadId;
            Thread newThread;

            var thread = new Thread(() =>
            {
                threadId = WinApiHelper.GetCurrentThreadId();
                newThread = Thread.CurrentThread;
                MainThreadDispatcher?.Invoke(() =>
                {
                    lock (_lock)
                    {
                        ThreadsInfo.Add(new ThreadInfo(threadId, newThread));
                        ThreadTaskCancelSignals.Add(cancelSignal);
                    }
                });
                threadTask(cancelSignal);
            });

            thread.Name = cancelSignal.ThreadName = name;
            thread.IsBackground = true;

            cancelSignal.CancelEvent += (o) =>
            {
                RemoveThreadByName(o?.ToString());
            };

            return thread;
        }

        /// <summary>
        /// 添加线程
        /// </summary>
        /// <param name="name">线程名称</param>
        /// <param name="cancelSignal">任务取消信号<see cref="ThreadTaskCancelSignal"/></param>
        /// <param name="threadTask">任务</param>
        /// <returns>线程</returns>
        public static Thread AddThread(
            string name,
            ThreadTaskCancelSignal cancelSignal,
            ThreadTask threadTask
        )
        {
            Thread thread;
            if (Instance == null)
            {
                thread = new Thread(() =>
                {
                    threadTask(cancelSignal);
                });
                thread.Name = name;
                thread.IsBackground = true;
            }
            else
            {
                thread = Instance.CreateThread(name, cancelSignal, threadTask);
            }
            return thread;
        }

        /// <summary>
        /// 多核线程并行运行，单核线程并发运行
        /// </summary>
        /// <param name="threads">线程</param>
        public void ThreadParallelRun(params Thread[] threads)
        {
            Parallel.ForEach(
                threads,
                t =>
                {
                    t?.Start();
                }
            );
        }

        private void RemoveThreadByName(string? name)
        {
            lock (_lock)
            {
                if (ThreadsInfo.Where((t) => t.Name == name).FirstOrDefault() is null)
                {
                    return;
                }
                ThreadsInfo.Remove(ThreadsInfo.Where((t) => t.Name == name).First());
                ThreadTaskCancelSignals.Remove(
                    ThreadTaskCancelSignals.Where(t => t.ThreadName == name).First()
                );
            }
        }

        private void StartUpdateThreadState()
        {
            CreateThread(
                    "更新状态线程",
                    new ThreadTaskCancelSignal(),
                    (cancelSignal) =>
                    {
                        while (!cancelSignal.CancelSignal)
                        {
                            lock (_lock)
                            {
                                foreach (var item in ThreadsInfo)
                                {
                                    item.UpdateState();
                                }
                            }
                            Thread.Sleep(5000);
                        }
                    }
                )
                .Start();
        }

        /// <summary>
        /// 初始化线程管理器
        /// </summary>
        public static void Init()
        {
            MainThreadDispatcher = Dispatcher.CurrentDispatcher;
            if (Instance != null)
            {
                return;
            }
            Instance = new ThreadManager();
        }

        private ThreadManager()
        {
            StartUpdateThreadState();
        }
    }

    /// <summary>
    /// <para>
    /// 线程任务取消信号
    /// </para>
    /// <para>
    /// > 取消任务调用 <see cref="Cancel"/>()
    /// </para>
    /// <para>
    /// > 重置信号调用 <see cref="ReSet"/>()
    /// </para>
    /// </summary>
    public class ThreadTaskCancelSignal
    {
        private bool isSetThreadName;

        public delegate void ThreadTaskCancelSignalEventHandler(object? obj);
        public event ThreadTaskCancelSignalEventHandler? CancelEvent;

        /// <summary>
        /// 取消信号
        /// </summary>
        public bool CancelSignal { get; private set; } = false;

        private string? threadName;

        /// <summary>
        /// 线程名称
        /// </summary>
        public string? ThreadName
        {
            get => threadName;
            set
            {
                if (!isSetThreadName)
                {
                    threadName = value;
                }
                else
                {
                    throw new InvalidOperationException("线程名已设置，不可修改");
                }
                isSetThreadName = true;
            }
        }

        /// <summary>
        /// 任务取消
        /// </summary>
        public void Cancel()
        {
            if (CancelEvent != null)
            {
                CancelSignal = true;
                CancelEvent.Invoke(ThreadName);
            }
        }

        /// <summary>
        /// 重置信号位
        /// </summary>
        public void ReSet()
        {
            CancelSignal = false;
            CancelEvent = null;
            isSetThreadName = false;
        }
    }

    /// <summary>
    /// 线程信息
    /// </summary>
    public class ThreadInfo : INotifyPropertyChanged
    {
        private string? name;

        /// <summary>
        /// 线程名称
        /// </summary>
        public string? Name
        {
            get => name;
            private set => SetProperty(ref name, value);
        }

        private uint id;

        /// <summary>
        /// 线程Id
        /// </summary>
        public uint Id
        {
            get => id;
            private set => SetProperty(ref id, value);
        }

        private ThreadState state;

        /// <summary>
        /// 线程状态
        /// </summary>
        public ThreadState State
        {
            get => state;
            private set => SetProperty(ref state, value);
        }

        private bool isAlive;

        /// <summary>
        /// 线程执行状态
        /// </summary>
        public bool IsAlive
        {
            get => isAlive;
            private set => SetProperty(ref isAlive, value);
        }

        private Thread ThreadInstance { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetProperty<T>(
            ref T field,
            T value,
            [CallerMemberName] string propertyName = ""
        )
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return;
            field = value;
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// 更新线程状态
        /// </summary>
        public void UpdateState()
        {
            State = ThreadInstance.ThreadState;
            IsAlive = ThreadInstance.IsAlive;
        }

        public ThreadInfo(uint id, Thread threadInstance)
        {
            Name = threadInstance.Name;
            Id = id;
            State = threadInstance.ThreadState;
            ThreadInstance = threadInstance;
            IsAlive = threadInstance.IsAlive;
        }
    }
}
