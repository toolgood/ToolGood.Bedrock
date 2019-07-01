﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ToolGood.Bedrock
{
    public class FileWatchUtil : IDisposable
    {
        private static List<FileWatchUtil> _fileWatchList = new List<FileWatchUtil>();

        private Timer _timer;
        private FileSystemWatcher _fileWatcher;
        private readonly int _timeOutMillis = 500;//延迟执行时间
        private FileInfo _fileInfo;
        private Action<FileStream> _fileChangedHandle = null;//文件变动后执行方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="action">文件改变后的执行的方法</param>
        /// <param name="timeOutMillis">延迟时间</param>
        /// <param name="isStart">是否启动监听</param>
        public FileWatchUtil(string filePath, Action<FileStream> action, int? timeOutMillis = null, bool isStart = false)
        {
            if (action == null) { throw new ArgumentException("action is null"); }
            if (filePath == null) { throw new ArgumentException("filePath is null"); }

            if (File.Exists(filePath)) {
                if (timeOutMillis.HasValue) {
                    _timeOutMillis = timeOutMillis.Value;
                }
                _fileChangedHandle = action;
                _fileInfo = new FileInfo(filePath);
                if (isStart) {
                    OnWatchedFileChange(null);
                    StartWatching();
                }
                _fileWatchList.Add(this);
            } else {
                LogUtil.Warn($"文件不存在【{filePath}】");
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="action">文件改变后的执行的方法</param>
        /// <param name="timeOutMillis">延迟时间</param>
        /// <param name="isStart">是否启动监听</param>
        public FileWatchUtil(FileInfo fileInfo, Action<FileStream> action, int? timeOutMillis = null, bool isStart = false)
        {
            if (action == null) { throw new ArgumentException("action is null"); }
            if (fileInfo == null) { throw new ArgumentException("fileInfo is null"); }

            if (timeOutMillis.HasValue) {
                _timeOutMillis = timeOutMillis.Value;
            }
            _fileChangedHandle = action;
            _fileInfo = fileInfo;
            if (isStart) {
                OnWatchedFileChange(null);
                StartWatching();
            }
        }

        /// <summary>
        /// 开始启动监听文件
        /// </summary>
        public void StartWatching()
        {
            _fileWatcher = new FileSystemWatcher();
            _fileWatcher.Path = _fileInfo.DirectoryName;
            _fileWatcher.Filter = _fileInfo.Name;
            _fileWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName;
            _fileWatcher.Changed += new FileSystemEventHandler(ConfigWacherHandler_OnChanged);
            _fileWatcher.Created += new FileSystemEventHandler(ConfigWacherHandler_OnChanged);
            _fileWatcher.Deleted += new FileSystemEventHandler(ConfigWacherHandler_OnChanged);
            _fileWatcher.Renamed += new RenamedEventHandler(ConfigWacherHandler_OnRenamed);
            _fileWatcher.EnableRaisingEvents = true;

            _timer = new Timer(new TimerCallback(OnWatchedFileChange), null, Timeout.Infinite, Timeout.Infinite);
            LogUtil.Info($"文件监听器开启【{ _fileInfo.FullName}】");
        }

        private void ConfigWacherHandler_OnChanged(object source, FileSystemEventArgs e)
        {
            _timer.Change(_timeOutMillis, Timeout.Infinite);
        }

        private void ConfigWacherHandler_OnRenamed(object source, RenamedEventArgs e)
        {
            _timer.Change(_timeOutMillis, Timeout.Infinite);
        }

        private void OnWatchedFileChange(object state)
        {
            if (_fileInfo == null) {
                LogUtil.Error("文件不存在【InternalConfigure】");
            } else if (File.Exists(_fileInfo.FullName)) {
                FileStream fs = null;
                for (int retry = 5; --retry >= 0;) {
                    try {
                        fs = _fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                        break;
                    } catch (IOException ex) {
                        if (retry == 0) {
                            LogUtil.Error(string.Format("打开文件失败 [{0}],{1}", _fileInfo.FullName, ex.ToErrMsg("OnWatchedFileChange")));
                            fs = null;
                        }
                        Thread.Sleep(250);
                    }
                }
                if (fs != null) {
                    try {
                        _fileChangedHandle?.Invoke(fs);
                        LogUtil.Info($"文件信息变更后加载完毕【{ _fileInfo.FullName}】");
                    } finally {
                        fs.Close();
                    }
                }
            } else {
                LogUtil.Warn($"文件不存在【{_fileInfo.FullName}】");
            }
        }

        /// <summary>
        /// 释放所有的监听文件程序(停止运行时调用)
        /// </summary>
        public static void UnInstall()
        {
            foreach (var item in _fileWatchList) {
                item.Dispose();
            }
        }

        private bool _isDisposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing) {
                _fileWatcher?.Dispose();
                _timer?.Dispose();
                LogUtil.Info($"释放文件【{_fileInfo?.FullName}】监听器");
            }
            _isDisposed = true;
        }

        ~FileWatchUtil()
        {
            Dispose(false);
        }
    }
 

}
