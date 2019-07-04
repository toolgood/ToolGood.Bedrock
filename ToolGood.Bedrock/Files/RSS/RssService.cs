using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using ToolGood.Bedrock.Files.RSS;

namespace ToolGood.Bedrock.Files
{
    public class RssFile
    {
        XmlNamespaceManager namespaceMgr;
        public RssEntity Get(string feed)
        {
            XmlTextReader reader = new XmlTextReader(feed);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            namespaceMgr = new XmlNamespaceManager(doc.NameTable);
            XmlNodeList channels = doc.SelectNodes("rss/channel");
            RssEntity entity = new RssEntity();

            foreach (XmlNode item in channels) {
                RssChannel channel = new RssChannel();
                channel.description = GetText(item, "description");
                channel.generator = GetText(item, "generator");
                channel.language = GetText(item, "language");
                string lastBuildDate = GetText(item, "lastBuildDate");
                if (!string.IsNullOrEmpty(lastBuildDate)) {
                    channel.lastBuildDate = Convert.ToDateTime(lastBuildDate);
                }
                channel.link = GetText(item, "link");
                channel.title = GetText(item, "title");
                channel.Items = new List<RssItem>();
                XmlNodeList items = item.SelectNodes("item");
                if (items == null) continue;
                foreach (XmlNode rssItem in items) {
                    RssItem ritem = new RssItem();
                    foreach (XmlNode itemDetail in rssItem.ChildNodes) {
                        SetPropertyValue<RssItem>(ritem, itemDetail.LocalName, itemDetail.InnerText);
                    }
                    channel.Items.Add(ritem);
                }
                entity.Channels.Add(channel);
            }

            return entity;
        }
        private string GetText(XmlNode node, string nodeName)
        {
            XmlNode tempNode = node.SelectSingleNode(nodeName, namespaceMgr);
            if (tempNode != null)
                return tempNode.InnerText;
            return string.Empty;
        }


        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        private void SetPropertyValue<T>(T item, string property, object value)
        {
            Type entityType = typeof(T);
            PropertyInfo proper = entityType.GetProperty(property);
            if (proper != null && proper.CanWrite) {

                proper.SetValue(item, ValueConvert(proper, value), null);
            }
        }
        private object ValueConvert(PropertyInfo property, object obj)
        {
            return ValueConvert(property.PropertyType, obj);
        }
        private object ValueConvert(Type type, object obj)
        {
            if (obj == null) return null;
            TypeCode code = type.IsGenericType ? Type.GetTypeCode(type.GetGenericArguments()[0]) : Type.GetTypeCode(type);
            switch (code) {
                case TypeCode.Boolean: {
                    if (obj != null) {
                        string result = obj.ToString().ToLower();
                        if (result == "true" || result == "1")
                            return true;
                        else return false;
                    } else return false;
                }
                case TypeCode.Byte: return Convert.ToByte(obj);
                case TypeCode.Char: return Convert.ToChar(obj);
                case TypeCode.DBNull: return null;
                case TypeCode.DateTime: return Convert.ToDateTime(obj);
                case TypeCode.Decimal: return Convert.ToDecimal(obj);
                case TypeCode.Double: return Convert.ToDouble(obj);
                case TypeCode.Empty: return null;
                case TypeCode.Int16: return Convert.ToInt16(obj);
                case TypeCode.Int32: return Convert.ToInt32(obj);
                case TypeCode.Int64: return Convert.ToInt64(obj);
                case TypeCode.Object: return obj;
                case TypeCode.SByte: return Convert.ToSByte(obj);
                case TypeCode.Single: return Convert.ToSingle(obj);
                case TypeCode.String: return Convert.ToString(obj);
                case TypeCode.UInt16: return Convert.ToUInt16(obj);
                case TypeCode.UInt32: return Convert.ToUInt32(obj);
                case TypeCode.UInt64: return Convert.ToUInt64(obj);
                default: return obj;
            }
        }
    }

}
