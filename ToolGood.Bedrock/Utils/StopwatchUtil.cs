using System;
using System.Diagnostics;

namespace ToolGood.Bedrock
{
    public static class StopwatchUtil
    {
        public static long Execute(Action action)
        {
            Stopwatch sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
    }
}
