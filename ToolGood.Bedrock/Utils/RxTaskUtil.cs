using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ToolGood.Bedrock.Utils.TaskSchedulers;

namespace ToolGood.Bedrock.Utils
{
    /// <summary>
    /// 仿 Rx.NET 订阅 分发
    /// </summary>
    public static class RxTaskUtil
    {
        private static ConcurrentBag<Action<SubjectObject>> Subscribes = new ConcurrentBag<Action<SubjectObject>>();
        private static TaskFactory factory;
        private static int _ThreadCount;

        /// <summary>
        /// 线程数
        /// </summary>
        public static int ThreadCount {
            get { return _ThreadCount; }
            set {
                _ThreadCount = value;
                factory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(_ThreadCount));
            }
        }

        static RxTaskUtil()
        {
            ThreadCount = Environment.ProcessorCount;
            factory = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(ThreadCount));
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="action"></param>
        public static void Subscribe(Action<string> action)
        {
            Subscribes.Add((p) => {
                try {
                    action(p.Args);
                } catch { }
            });
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public static void Subscribe(string key, Action<string> action)
        {
            if (string.IsNullOrEmpty(key)) {
                Subscribe(action);
                return;
            }
            Subscribes.Add((p) => {
                if (p.Key == key) {
                    try {
                        action(p.Args);
                    } catch { }
                }
            });
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        public static void RunTask(string key, params object[] args)
        {
            SubjectObject subjectObject = new SubjectObject(key, args);
            var _factory = factory;
            foreach (var subject in Subscribes) {
                _factory.StartNew(() => {
                    subject(subjectObject);
                });
            }
        }

        class SubjectObject
        {
            public SubjectObject(string key, object[] args)
            {
                Key = key;
                if (args != null) {
                    if (args.Length == 1) {
                        Args = args[0].ToJson();
                    } else {
                        Args = args.ToJson();
                    }
                }
            }
            public string Key { get; private set; }
            public string Args { get; private set; }
        }
    }
}
