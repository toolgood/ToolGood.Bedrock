using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace ToolGood.Bedrock.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class RxTaskUtil : IDisposable
    {
        private static RxTaskUtil Instance = new RxTaskUtil();
        private List<Subject<SubjectObject>> Subjects = new List<Subject<SubjectObject>>();

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public static void Subscribe(string key, Action<object[]> action)
        {
            var subject = new Subject<SubjectObject>();
            subject.Subscribe(p => {
                if (p.Key == key) {
                    action(p.Args);
                }
            });
            Instance.Subjects.Add(subject);
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        public static void RunTask(string key, params object[] args)
        {
            SubjectObject subjectObject = new SubjectObject(key, args);
            foreach (var subject in Instance.Subjects) {
                subject.OnNext(subjectObject);
            }
        }

        public void Dispose()
        {
            foreach (var inputs in Instance.Subjects) {
                inputs.Dispose();
            }
            Subjects.Clear();
        }


        class SubjectObject
        {
            public SubjectObject(string key, object[] args)
            {
                Key = key;
                Args = args;
            }
            public string Key { get; private set; }
            public object[] Args { get; private set; }
        }
    }
}
