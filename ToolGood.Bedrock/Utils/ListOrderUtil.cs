﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// 列排列
    /// </summary>
    public class ListOrderUtil
    {
        /// <summary>
        /// 依据父ID 倒序排列
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="list"></param>
        /// <param name="pid"></param>
        /// <param name="pidFunc"></param>
        /// <param name="idFunc"></param>
        /// <returns></returns>
        public List<T1> OrderByDescOnPid<T1, T2>(List<T1> list, T2 pid, Func<T1, T2> pidFunc, Func<T1, T2> idFunc)
        {
            TreeList<T1, T2> treeList = new TreeList<T1, T2>();
            BuildTree(treeList, list, pid, pidFunc, idFunc);
            return treeList.ToList2();
        }
        /// <summary>
        /// 依据父ID 正序排列
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="list"></param>
        /// <param name="pid"></param>
        /// <param name="pidFunc"></param>
        /// <param name="idFunc"></param>
        /// <returns></returns>
        public List<T1> OrderByAscOnPid<T1, T2>(List<T1> list, T2 pid, Func<T1, T2> pidFunc, Func<T1, T2> idFunc)
        {
            TreeList<T1, T2> treeList = new TreeList<T1, T2>();
            BuildTree(treeList, list, pid, pidFunc, idFunc);
            return treeList.ToList();
        }

        /// <summary>
        /// 依据父ID 正序排列
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="list"></param>
        /// <param name="pid"></param>
        /// <param name="pidFunc"></param>
        /// <param name="idFunc"></param>
        /// <param name="orderFunc"></param>
        /// <returns></returns>
        public List<T1> OrderByDescOnPidAndOrderNum<T1, T2, T3>(List<T1> list, T2 pid, Func<T1, T2> pidFunc, Func<T1, T2> idFunc, Func<T1, T3> orderFunc)
        {
            TreeList<T1, T2, T3> treeList = new TreeList<T1, T2, T3>();
            BuildTree(treeList, list, pid, pidFunc, idFunc, orderFunc);
            return treeList.ToList2();
        }

        /// <summary>
        /// 依据父ID 倒序排列
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="list"></param>
        /// <param name="pid"></param>
        /// <param name="pidFunc"></param>
        /// <param name="idFunc"></param>
        /// <param name="orderFunc"></param>
        /// <returns></returns>
        public List<T1> OrderByAscOnPidAndOrderNum<T1, T2, T3>(List<T1> list, T2 pid, Func<T1, T2> pidFunc, Func<T1, T2> idFunc, Func<T1, T3> orderFunc)
        {
            TreeList<T1, T2, T3> treeList = new TreeList<T1, T2, T3>();
            BuildTree(treeList, list, pid, pidFunc, idFunc, orderFunc);
            return treeList.ToList();
        }

        private void BuildTree<T1, T2>(TreeList<T1, T2> treeList, List<T1> list, T2 pid, Func<T1, T2> pidFunc, Func<T1, T2> idFunc)
        {
            var ls = list.Where(q => object.Equals(pidFunc(q), pid)).ToList();

            foreach (var item in ls) {
                var id = idFunc(item);
                TreeList<T1, T2> tree = new TreeList<T1, T2>() {
                    Value = item,
                    Nodes = new Dictionary<T2, TreeList<T1, T2>>()
                };
                treeList.Nodes[id] = tree;
                BuildTree(tree, list, id, pidFunc, idFunc);
            };
        }

        private void BuildTree<T1, T2, T3>(TreeList<T1, T2, T3> treeList, List<T1> list, T2 pid, Func<T1, T2> pidFunc, Func<T1, T2> idFunc, Func<T1, T3> orderFunc)
        {
            var ls = list.Where(q => object.Equals(pidFunc(q), pid)).ToList();

            foreach (var item in ls) {
                var id = idFunc(item);
                TreeList<T1, T2, T3> tree = new TreeList<T1, T2, T3>() {
                    Value = item,
                    OrderNum = orderFunc(item),
                    Nodes = new Dictionary<T2, TreeList<T1, T2, T3>>()
                };
                treeList.Nodes[id] = tree;
                BuildTree(tree, list, id, pidFunc, idFunc, orderFunc);
            };
        }

        class TreeList<T1, T2>
        {
            public T1 Value;
            public Dictionary<T2, TreeList<T1, T2>> Nodes { get; set; }

            public List<T1> ToList()
            {
                List<T1> list = new List<T1>();
                ToList(list);
                return list;
            }
            private void ToList(List<T1> list)
            {
                foreach (var item in Nodes) {
                    list.Add(item.Value.Value);
                    item.Value.ToList(list);
                }
            }

            public List<T1> ToList2()
            {
                List<T1> list = new List<T1>();
                ToList2(list);
                return list;
            }
            private void ToList2(List<T1> list)
            {
                foreach (var item in Nodes.Reverse()) {
                    list.Add(item.Value.Value);
                    item.Value.ToList2(list);
                }
            }
        }


        class TreeList<T1, T2, T3>
        {
            public T1 Value;
            public T3 OrderNum;
            public Dictionary<T2, TreeList<T1, T2, T3>> Nodes { get; set; }

            public List<T1> ToList()
            {
                List<T1> list = new List<T1>();
                ToList(list);
                return list;
            }
            private void ToList(List<T1> list)
            {
                var ls = Nodes.OrderBy(q => q.Value.OrderNum).Select(q => q.Value);
                foreach (var item in ls) {
                    list.Add(item.Value);
                    item.ToList(list);
                }
            }

            public List<T1> ToList2()
            {
                List<T1> list = new List<T1>();
                ToList2(list);
                return list;
            }
            private void ToList2(List<T1> list)
            {
                var ls = Nodes.OrderByDescending(q => q.Value.OrderNum).Select(q => q.Value);
                foreach (var item in ls) {
                    list.Add(item.Value);
                    item.ToList2(list);
                }
            }


        }
    }
}
