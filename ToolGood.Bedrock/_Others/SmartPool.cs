using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ToolGood.Bedrock.Tools
{
    public class SmartPool<T> where T : class
    {
        #region 属性
        /// <summary>对象池大小。默认CPU*2，初始化后改变无效</summary>
        public Int32 Max { get; set; }

        private Item[] _items;
        private T _current;

        struct Item
        {
            public T Value;
        }
        #endregion

        #region 构造
        /// <summary>实例化对象池。默认大小CPU*2</summary>
        /// <param name="max"></param>
        public SmartPool(Int32 max = 0)
        {
            if (max <= 0) max = Environment.ProcessorCount * 2;

            Max = max;
        }

        private void Init()
        {
            if (_items != null) return;
            lock (this) {
                if (_items != null) return;

                _items = new Item[Max - 1];
            }
        }
        #endregion

        #region 方法
        /// <summary>获取</summary>
        /// <returns></returns>
        public virtual T Get()
        {
            // 最热的一个对象在外层，便于快速存取
            var val = _current;
            if (val != null && Interlocked.CompareExchange(ref _current, null, val) == val) return val;

            Init();

            var items = _items;
            for (var i = 0; i < items.Length; i++) {
                val = items[i].Value;
                if (val != null && Interlocked.CompareExchange(ref items[i].Value, null, val) == val) return val;
            }

            return OnCreate();
        }

        /// <summary>归还</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual Boolean Put(T value)
        {
            // 最热的一个对象在外层，便于快速存取
            if (_current == null && Interlocked.CompareExchange(ref _current, value, null) == null) return true;

            Init();

            var items = _items;
            for (var i = 0; i < items.Length; ++i) {
                if (Interlocked.CompareExchange(ref items[i].Value, value, null) == null) return true;
            }

            return false;
        }

        /// <summary>清空</summary>
        /// <returns></returns>
        public virtual Int32 Clear()
        {
            var count = 0;

            if (_current != null) {
                _current = null;
                count++;
            }

            var items = _items;
            for (var i = 0; i < items.Length; ++i) {
                if (items[i].Value != null) {
                    items[i].Value = null;
                    count++;
                }
            }
            _items = null;

            return count;
        }
        #endregion

        #region 重载
        /// <summary>创建实例</summary>
        /// <returns></returns>
        protected virtual T OnCreate() => Activator.CreateInstance(typeof(T)) as T;
        #endregion
    }
}
