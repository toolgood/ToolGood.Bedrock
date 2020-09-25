using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using ToolGood.HtmlExtract.HtmlAgilityPack;
using ToolGood.HtmlExtract.HtmlAgilityPack.CssSelectors;
using ToolGood.HtmlExtract.Attributes;

namespace ToolGood.HtmlExtract
{
    public delegate bool FieldMappingFunc(HtmlNode node, object obj, PropertyInfo propertyInfo);
    public delegate List<HtmlNode> LocationFunc(HtmlNode node);
    public class DataLoader<T> where T : class, new()
    {
        private Dictionary<string, PropertyInfo> propertyMappingCache = new Dictionary<string, PropertyInfo>();

        private Dictionary<PropertyInfo, List<FieldMappingFunc>> fieldMappingCache = new Dictionary<PropertyInfo, List<FieldMappingFunc>>();

        private List<LocationFunc> locationsCache = new List<LocationFunc>();


        #region 构造函数

        public DataLoader()
        {
            Init(typeof(T));
        }
        private void Init(Type type)
        {
            var attrs = type.GetCustomAttributes<HtmlLocationAttribute>();

            foreach (var attr in attrs)
            {
                LocationFunc func = (node) =>
                {
                    var nds = node.QuerySelectorAll(attr.JqSelector);
                    if (attr.Index == null)
                    {
                        return nds.ToList();
                    }
                    if (nds.Count() < attr.Index.Value)
                    {
                        return new List<HtmlNode>() { nds[attr.Index.Value] };
                    }
                    return new List<HtmlNode>();
                };
                locationsCache.Add(func);
            }
            var pis = type.GetProperties();
            foreach (var pi in pis)
            {
                var ats = pi.GetCustomAttributes<HtmlDataAttribute>();
                foreach (var at in ats)
                {
                    AddFieldMapping(pi, at.JqSelector, at.Index, at.AttrName);
                }
            }
        }
        #endregion

        #region LoadUrl LoadData

        public List<T> LoadUrl(string address)
        {
            WebClientEx webClient = new WebClientEx();
            webClient.ResetHeaders();
            var html = webClient.DownloadString(address);

            return LoadData(html);
        }

        public List<T> LoadData(string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            List<HtmlNode> list = new List<HtmlNode>();
            foreach (var item in locationsCache)
            {
                var nodes = item(document.DocumentNode);
                list.AddRange(nodes);
            }
            list = list.Distinct().ToList();

            List<T> result = new List<T>();
            foreach (var node in list)
            {
                var t = Parse(node);
                result.Add(t);
            }
            return result;
        }

        private T Parse(HtmlNode node)
        {
            T t = new T();

            foreach (var fieldMapping in fieldMappingCache)
            {
                foreach (var item in fieldMapping.Value)
                {
                    if (item(node, t, fieldMapping.Key))
                    {
                        break;
                    }
                }
            }
            return t;
        }

        #endregion

        #region AddLocation
        public void AddLocation(LocationFunc func)
        {
            locationsCache.Add(func);
        }

        #endregion


        #region AddFieldMapping
        public void AddFieldMapping<TReturn>(Expression<Func<T, TReturn>> fieldExpression, FieldMappingFunc func)
        {
            var vistor = new FieldExpVistor();
            vistor.Visit(fieldExpression);
            var fieldName = vistor.Field;
            AddFieldMapping(fieldName, func);
        }
        public void AddFieldMapping(string fieldName, FieldMappingFunc func)
        {
            PropertyInfo propertyInfo;
            if (propertyMappingCache.TryGetValue(fieldName, out propertyInfo))
            {
                AddFieldMapping(propertyInfo, func);
            }
        }
        private void AddFieldMapping(PropertyInfo propertyInfo, FieldMappingFunc func)
        {
            List<FieldMappingFunc> list;
            if (fieldMappingCache.TryGetValue(propertyInfo, out list) == false)
            {
                list = new List<FieldMappingFunc>();
                fieldMappingCache[propertyInfo] = list;
            }
            list.Add(func);
        }

        public void AddFieldMapping<TReturn>(Expression<Func<T, TReturn>> fieldExpression, string jqSelector)
        {
            AddFieldMapping(fieldExpression, jqSelector, null, null);
        }
        public void AddFieldMapping<TReturn>(Expression<Func<T, TReturn>> fieldExpression, string jqSelector, string attrName)
        {
            AddFieldMapping(fieldExpression, jqSelector, null, attrName);
        }
        public void AddFieldMapping<TReturn>(Expression<Func<T, TReturn>> fieldExpression, string jqSelector, int? index)
        {
            AddFieldMapping(fieldExpression, jqSelector, index, null);
        }
        public void AddFieldMapping<TReturn>(Expression<Func<T, TReturn>> fieldExpression, string jqSelector, int? index, string attrName)
        {
            var vistor = new FieldExpVistor();
            vistor.Visit(fieldExpression);
            var fieldName = vistor.Field;
            AddFieldMapping(fieldName, jqSelector, index, attrName);
        }
        public void AddFieldMapping<TReturn>(string fieldName, string jqSelector)
        {
            AddFieldMapping(fieldName, jqSelector, null, null);
        }
        public void AddFieldMapping(string fieldName, string jqSelector, string attrName)
        {
            AddFieldMapping(fieldName, jqSelector, null, attrName);
        }
        public void AddFieldMapping(string fieldName, string jqSelector, int? index)
        {
            AddFieldMapping(fieldName, jqSelector, index, null);
        }
        public void AddFieldMapping(string fieldName, string jqSelector, int? index, string attrName)
        {
            PropertyInfo propertyInfo;
            if (propertyMappingCache.TryGetValue(fieldName, out propertyInfo))
            {
                AddFieldMapping(propertyInfo, jqSelector, index, attrName);
            }
        }
        private void AddFieldMapping(PropertyInfo propertyInfo, string jqSelector, int? index, string attrName)
        {
            List<FieldMappingFunc> list;
            if (fieldMappingCache.TryGetValue(propertyInfo, out list) == false)
            {
                list = new List<FieldMappingFunc>();
                fieldMappingCache[propertyInfo] = list;
            }
            FieldMappingFunc func = (nd, obj, pi) =>
             {
                 HtmlNode node = null;
                 if (index != null)
                 {
                     var nds = nd.QuerySelectorAll(jqSelector);
                     if (nds.Count > index.Value) { node = nds[index.Value]; }
                 }
                 else
                 {
                     node = nd.QuerySelector(jqSelector);
                 }
                 if (node != null)
                 {
                     if (string.IsNullOrEmpty(attrName) == false)
                     {
                         var attr = node.Attributes[attrName];
                         if (attr != null)
                         {
                             pi.SetValue(obj, To(attr.Value, pi.PropertyType));
                             return true;
                         }
                     }
                     else
                     {
                         pi.SetValue(obj, To(node.InnerText, pi.PropertyType));
                         return true;
                     }
                 }
                 return false;
             };
            list.Add(func);
        }
        private object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }
        private object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                var destinationConverter = TypeDescriptor.GetConverter(destinationType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);

                var sourceConverter = TypeDescriptor.GetConverter(sourceType);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);

                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int) value);

                if (!destinationType.IsInstanceOfType(value))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }
        #endregion

    }

}
