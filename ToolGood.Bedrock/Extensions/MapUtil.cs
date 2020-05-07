using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Bedrock.Extensions
{
    public static class MapUtil
    {
        /// <summary>
        /// 获取百度地图静态图片
        /// </summary>
        /// <param name="lng">中心点经度</param>
        /// <param name="lat">中心点维度</param>
        /// <param name="scale">返回图片大小会根据此标志调整。取值范围为1或2：
        ///1表示返回的图片大小为size= width * height;
        ///2表示返回图片为(width*2)*(height *2)，且zoom加1
        ///注：如果zoom为最大级别，则返回图片为（width*2）*（height*2），zoom不变。</param>
        /// <param name="zoom">地图级别。高清图范围[3, 18]；低清图范围[3,19]</param>
        /// <param name="markersList">标记列表，如果为null则不输出标记</param>
        /// <param name="width">图片宽度。取值范围：(0, 1024]。</param>
        /// <param name="height">图片高度。取值范围：(0, 1024]。</param>
        /// <returns></returns>
        public static string GetBaiduStaticMap(double lng, double lat, int scale, int zoom, IList<BaiduMarkers> markersList, int width = 400, int height = 300)
        {
            var url = new StringBuilder();
            url.Append("http://api.map.baidu.com/staticimage?");

            url.AppendFormat("center={0},{1}", lng, lat);
            url.AppendFormat("&width={0}", width);
            url.AppendFormat("&height={0}", height);
            url.AppendFormat("&scale={0}", scale);
            url.AppendFormat("&zoom={0}", zoom);

            if (markersList != null && markersList.Count > 0) {
                url.AppendFormat("&markers={0}", string.Join("|", markersList.Select(z => string.Format("{0},{1}", z.Longitude, z.Latitude)).ToArray()));
                url.AppendFormat("&markerStyles={0}", string.Join("|", markersList.Select(z => string.Format("{0},{1},{2}", z.Size.ToString(), z.Label, z.Color)).ToArray()));
            }

            return url.ToString();
        }

        /// <summary>
        /// 获取谷歌静态地图Url。API介绍：https://developers.google.com/maps/documentation/staticmaps/?hl=zh-CN
        /// </summary>
        /// <returns></returns>
        public static string GetGoogleStaticMap(int scale, IList<GoogleMapMarkers> markersList, string size = "640x640")
        {
            markersList = markersList ?? new List<GoogleMapMarkers>();
            StringBuilder markersStr = new StringBuilder();
            foreach (var markers in markersList) {
                markersStr.Append("&markers=");
                if (markers.Size != GoogleMapMarkerSize.mid) {
                    markersStr.AppendFormat("size={0}%7C", markers.Size);
                }
                if (!string.IsNullOrEmpty(markers.Color)) {
                    markersStr.AppendFormat("color:{0}%7C", markers.Color);
                }
                markersStr.Append("label:");
                if (!string.IsNullOrEmpty(markers.Label)) {
                    markersStr.AppendFormat("{0}%7C", markers.Label);
                }
                markersStr.AppendFormat("{0},{1}", markers.X, markers.Y);
            }
            string parameters = string.Format("center=&zoom=&size={0}&maptype=roadmap&format=jpg&sensor=false&language=zh&{1}",
                                             size, markersStr.ToString());
            string url = "http://maps.googleapis.com/maps/api/staticmap?" + parameters;
            return url;
        }



        /// <summary>
        /// 标记大小
        /// </summary>
        public enum BaiduMarkerSize
        {
            Default = m,
            s = 0, m = 1, l = 2
        }

        /// <summary>
        /// 百度地图标记
        /// </summary>
        public class BaiduMarkers
        {
            /// <summary>
            /// （可选）有大中小三个值，分别为s、m、l。
            /// </summary>
            public BaiduMarkerSize Size { get; set; }
            /// <summary>
            /// （可选）Color = [0x000000, 0xffffff]或使用css定义的颜色表。
            /// black 0x000000 
            /// silver 0xC0C0C0 
            /// gray 0x808080 
            /// white 0xFFFFFF 
            /// maroon 0x800000
            /// red 0xFF0000 
            /// purple 0x800080 
            /// fuchsia 0xFF00FF 
            /// green 0x008000
            /// lime 0x00FF00 
            /// olive 0x808000 
            /// yellow 0xFFFF00 
            /// navy 0x000080 
            /// blue 0x0000FF
            /// teal 0x008080 
            /// aqua 0x00FFFF
            /// </summary>
            public string Color { get; set; }
            /// <summary>
            /// （可选）指定集合 {A-Z, 0-9} 中的一个大写字母数字字符。不指定时显示A。
            /// </summary>
            public string Label { get; set; }

            /// <summary>
            /// 自定义icon的地址，图片格式目前仅支持png32的。设置自定义图标标注时，忽略Size、Color、Label三个属性，只设置该属性且该属性前增加-1，如markerStyles=-1, http://api.map.baidu.com/images/marker_red.png，图标大小需小于5k，超过该值会导致加载不上图标的情况发生。
            /// </summary>
            public string url { get; set; }

            /// <summary>
            /// 经度longitude（对应GoogleMap的X）
            /// </summary>
            public double Longitude { get; set; }
            /// <summary>
            /// 纬度latitude（对应GoogleMap的Y）
            /// </summary>
            public double Latitude { get; set; }
        }


        /// <summary>
        /// 标记大小
        /// </summary>
        public enum GoogleMapMarkerSize
        {
            Default = mid,
            tiny = 0, mid = 1, small = 2
        }

        /// <summary>
        /// 谷歌地图标记
        /// </summary>
        public class GoogleMapMarkers
        {
            /// <summary>
            /// （可选）指定集合 {tiny, mid, small} 中的标记大小。如果未设置 size 参数，标记将以其默认（常规）大小显示。
            /// </summary>
            public GoogleMapMarkerSize Size { get; set; }
            /// <summary>
            /// （可选）指定 24 位颜色（例如 color=0xFFFFCC）或集合 {black, brown, green, purple, yellow, blue, gray, orange, red, white} 中预定义的一种颜色。
            /// </summary>
            public string Color { get; set; }
            /// <summary>
            /// （可选）指定集合 {A-Z, 0-9} 中的一个大写字母数字字符。
            /// </summary>
            public string Label { get; set; }
            /// <summary>
            /// 经度longitude
            /// </summary>
            public double X { get; set; }
            /// <summary>
            /// 纬度latitude
            /// </summary>
            public double Y { get; set; }
        }

    }




}
