using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Bedrock
{
    public static class ValidatorRegex
    {
        /// <summary>
        /// 手机正则
        /// </summary>
        internal const string Mobile = "^(0|86|17951)?(13[0-9]|14[5-9]|15[012356789]|16[6]|17[0-8]|18[0-9]|19[89])[0-9]{8}$";
        /// <summary>
        /// 邮箱
        /// </summary>
        internal const string Email = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        /// <summary>
        /// 中文
        /// </summary>
        internal const string Chinese = @"^[\u4E00-\u9FCB\u3400-\u4DB5\u20000-\u2A6D6\u2A700-\u2B734\u2B740-\u2B81D\u3007]+$";
        /// <summary>
        /// 电话
        /// </summary>
        internal const string Telephone = @"^(\d{3,4}-?)?\d{7,8}$";
        /// <summary>
        /// Ipv4
        /// </summary>
        internal const string Ipv4 = @"^(?:(?:25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)\.){3}(?:25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)$";

        internal const string Ipv6 = @"^((([0-9A-F]{1,4}:){7}([0-9A-F]{1,4}|:))|(([0-9A-F]{1,4}:){6}(:[0-9A-F]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-F]{1,4}:){5}(((:[0-9A-F]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-F]{1,4}:){4}(((:[0-9A-F]{1,4}){1,3})|((:[0-9A-F]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-F]{1,4}:){3}(((:[0-9A-F]{1,4}){1,4})|((:[0-9A-F]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-F]{1,4}:){2}(((:[0-9A-F]{1,4}){1,5})|((:[0-9A-F]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-F]{1,4}:){1}(((:[0-9A-F]{1,4}){1,6})|((:[0-9A-F]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-F]{1,4}){1,7})|((:[0-9A-F]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?$";

        internal const string Date = @"^((\d{4})[-/](1[012]|\d)[-/](3[01]|[12]?\d)|\d{4}年(1[012]|\d)月(3[01]|[12]?\d)日)$";

        internal const string Time = @"^((2[0-3]|[01]?\d):([0-5]?\d)(:([0-5]?\d)?|)|(2[0-3]|[01]?\d)时[0-5]?\d分([0-5]?\d秒)?)$";

        internal const string DateTime = @"^((\d{4})[-/](1[012]|\d)[-/](3[01]|[12]?\d) (2[0-3]|[01]?\d):([0-5]?\d)(:([0-5]?\d)?|)|\d{4}年(1[012]|\d)月(3[01]|[12]?\d)日(2[0-3]|[01]?\d)时[0-5]?\d分([0-5]?\d秒)?)$";

        internal const string Numeric = @"^[-]?[0-9]+(\.[0-9]+)?$";

        internal const string Zipcode = @"^\d{6}$";

        internal const string IdCard = @"(^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)|(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}[0-9Xx]$)";

        /// <summary>
        ///     文件路径检测正则表达式
        /// </summary>
        internal const string FileCheck = @"^(?<fpath>([a-zA-Z]:\\)([\s\.\-\w]+\\)*)(?<fname>[\w]+)(?<namext>(\.[\w]+)*)(?<suffix>\.[\w]+)";

        /// <summary>
        ///     URL检测正则表达式
        /// </summary>
        internal const string UrlCheck =
            @"^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&amp;%_\./-~-]*)?$";

        /// <summary>
        ///     是否十六进制字符串检测正则表达式
        /// </summary>
        internal const string HexStringCheck = @"\A\b[0-9a-fA-F]+\b\Z";

        /// <summary>
        /// 用户名
        /// </summary>
        internal const string UserName = @"^[a-zA-Z][a-zA-Z0-9_]{4,20}$";

        /// <summary>
        /// 匹配正整数
        /// </summary>
        internal const string PositiveIntegers = @"^[1-9]\d*$";
        /// <summary>
        /// 匹配负整数
        /// </summary>
        internal const string NegativeIntegers = @"^-[1-9]\d*$";
        /// <summary>
        ///匹配整数
        /// </summary>
        internal const string Integer = @"^-?[0-9]+$";
        /// <summary>
        /// 匹配非负整数（正整数 + 0）
        /// </summary>
        internal const string PositiveIntegersAndZero = @"^[0-9]+$";
        /// <summary>
        /// 匹配非正整数（负整数 + 0）
        /// </summary>
        internal const string NegativeIntegersAndZero = @"^-[0-9]+$";
        /// <summary>
        /// 匹配正浮点数
        /// </summary>
        internal const string Float = @"^(\-|\+)?\d+(\.\d+)?$";

        /// <summary>
        ///     整数检测正则表达式
        /// </summary>
        public const string IntCheck = @"^(0|[0-9]+[0-9]*)$";

        /// <summary>
        /// 匹配由26个英文字母组成的字符串
        /// </summary>
        internal const string Letters = @"^[A-Za-z]+$";
        /// <summary>
        /// 匹配由26个英文字母的大写组成的字符串
        /// </summary>
        internal const string UppercaseLetters = @"^[A-Z]+$";
        /// <summary>
        /// 匹配由26个英文字母的小写组成的字符串
        /// </summary>
        internal const string LowercaseLetters = @"^[a-z]+$";
        /// <summary>
        /// 匹配由数字和26个英文字母组成的字符串
        /// </summary>
        internal const string LettersAndNumber = @"^[A-Za-z0-9]+$";
        /// <summary>
        /// 匹配由数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        internal const string LettersAndNumberAndLine = @"^\w+$";

        /// <summary>
        ///     Mac地址（6-7个长度）正则表达式
        /// </summary>
        internal const string MacCheck = @"^[0-9A-F]{2}([-:][0-9A-F]{2}){5,6}$";

        /// <summary>
        /// 手机正则
        /// </summary>
        private static Regex _MobileRegex;
        public static Regex MobileRegex {
            get {
                if (_MobileRegex == null) {
                    _MobileRegex = new Regex(Mobile);
                }
                return _MobileRegex;
            }
        }

        public static bool IsMobile(string txt)
        {
            return MobileRegex.IsMatch(txt);
        }


        /// <summary>
        /// 邮箱
        /// </summary>
        private static Regex _EmailRegex;
        public static Regex EmailRegex {
            get {
                if (_EmailRegex == null) {
                    _EmailRegex = new Regex(Email);
                }
                return _EmailRegex;
            }
        }
        public static bool IsEmail(string txt)
        {
            return EmailRegex.IsMatch(txt);
        }


        /// <summary>
        /// 中文
        /// </summary>
        private static Regex _ChineseRegex;
        public static Regex ChineseRegex {
            get {
                if (_ChineseRegex == null) {
                    _ChineseRegex = new Regex(Chinese);
                }
                return _ChineseRegex;
            }
        }

        public static bool IsChinese(string txt)
        {
            return ChineseRegex.IsMatch(txt);
        }

        /// <summary>
        /// 电话
        /// </summary>
        private static Regex _TelephoneRegex;
        public static Regex TelephoneRegex {
            get {
                if (_TelephoneRegex == null) {
                    _TelephoneRegex = new Regex(Telephone);
                }
                return _TelephoneRegex;
            }
        }
        public static bool IsTelephone(string txt)
        {
            return TelephoneRegex.IsMatch(txt);
        }

        /// <summary>
        /// Ipv4
        /// </summary>
        private static Regex _Ipv4Regex;
        public static Regex Ipv4Regex {
            get {
                if (_Ipv4Regex == null) {
                    _Ipv4Regex = new Regex(Ipv4);
                }
                return _Ipv4Regex;
            }
        }

        public static bool IsIpv4(string txt)
        {
            return Ipv4Regex.IsMatch(txt);
        }

        private static Regex _Ipv6Regex;
        public static Regex Ipv6Regex {
            get {
                if (_Ipv6Regex == null) {
                    _Ipv6Regex = new Regex(Ipv6, RegexOptions.IgnoreCase);
                }
                return _Ipv6Regex;
            }
        }
        public static bool IsIpv6(string txt)
        {
            return Ipv6Regex.IsMatch(txt);
        }


        private static Regex _DateRegex;
        public static Regex DateRegex {
            get {
                if (_DateRegex == null) {
                    _DateRegex = new Regex(Date);
                }
                return _DateRegex;
            }
        }

        public static bool IsDate(string txt)
        {
            return DateRegex.IsMatch(txt);
        }

        private static Regex _TimeRegex;
        public static Regex TimeRegex {
            get {
                if (_TimeRegex == null) {
                    _TimeRegex = new Regex(Time);
                }
                return _TimeRegex;
            }
        }

        public static bool IsTime(string txt)
        {
            return TimeRegex.IsMatch(txt);
        }

        private static Regex _DateTimeRegex;
        public static Regex DateTimeRegex {
            get {
                if (_DateTimeRegex == null) {
                    _DateTimeRegex = new Regex(DateTime);
                }
                return _DateTimeRegex;
            }
        }
        public static bool IsDateTime(string txt)
        {
            return DateTimeRegex.IsMatch(txt);
        }

        private static Regex _NumericRegex;
        public static Regex NumericRegex {
            get {
                if (_NumericRegex == null) {
                    _NumericRegex = new Regex(Numeric);
                }
                return _NumericRegex;
            }
        }

        public static bool IsNumeric(string txt)
        {
            return NumericRegex.IsMatch(txt);
        }

        private static Regex _ZipcodeRegex;
        public static Regex ZipcodeRegex {
            get {
                if (_ZipcodeRegex == null) {
                    _ZipcodeRegex = new Regex(Zipcode);
                }
                return _ZipcodeRegex;
            }
        }

        public static bool IsZipcode(string txt)
        {
            return ZipcodeRegex.IsMatch(txt);
        }

        private static Regex _IdCardRegex;
        public static Regex IdCardRegex {
            get {
                if (_IdCardRegex == null) {
                    _IdCardRegex = new Regex(IdCard);
                }
                return _IdCardRegex;
            }
        }
        public static bool IsIdCard(string txt)
        {
            return IdCardRegex.IsMatch(txt);
        }


        private static Regex _FileCheckRegex;
        public static Regex FileCheckRegex {
            get {
                if (_FileCheckRegex == null) {
                    _FileCheckRegex = new Regex(FileCheck);
                }
                return _FileCheckRegex;
            }
        }
        /// <summary>
        /// 验证是否是文件路径
        /// <para>eg:CheckHelper.IsFilePath(@"C:\alipay\log.txt");==>true</para>
        /// </summary>
        /// <param name="txt">验证字符串</param>
        /// <returns>是否是文件路径</returns>
        public static bool IsFilePath(string txt)
        {
            return _FileCheckRegex.IsMatch(txt);
        }

        private static Regex _UrlRegex;
        public static Regex UrlRegex {
            get {
                if (_UrlRegex == null) {
                    _UrlRegex = new Regex(UrlCheck);
                }
                return _UrlRegex;
            }
        }
        /// <summary>
        /// 验证是否是URL
        /// <para>eg:CheckHelper.IsURL("www.cnblogs.com/yan-zhiwei");==>true</para>
        /// </summary>
        /// <param name="txt">验证字符串</param>
        /// <returns>是否是URL</returns>
        public static bool IsUrl(string txt)
        {
            return UrlRegex.IsMatch(txt);
        }




        private static Regex _HexStringRegex;
        public static Regex HexStringRegex {
            get {
                if (_HexStringRegex == null) {
                    _HexStringRegex = new Regex(HexStringCheck);
                }
                return _HexStringRegex;
            }
        }
        /// <summary>
        /// 是否是十六进制字符串
        /// </summary>
        /// <param name="txt">验证数据</param>
        /// <returns>是否是十六进制字符串</returns>
        public static bool IsHexString(string txt)
        {
            return HexStringRegex.IsMatch(txt);
        }

        private static Regex _NaturalNumberRegex;
        public static Regex NaturalNumberRegex {
            get {
                if (_NaturalNumberRegex == null) {
                    _NaturalNumberRegex = new Regex(IntCheck);
                }
                return _NaturalNumberRegex;
            }
        }
        /// <summary>
        /// 是否自然数 0. 1. 2.  .....
        /// </summary>
        /// <param name="txt">验证数据</param>
        public static bool IsNaturalNumber(string txt)
        {
            return NaturalNumberRegex.IsMatch(txt);
        }


        /// <summary>
        /// 判断是否是合法纬度
        /// </summary>
        /// <param name="data">需要判断的纬度</param>
        /// <returns>是否合法</returns>
        public static bool IsLatitude(decimal data)
        {
            return !(data < -90 || data > 90);
        }

        /// <summary>
        /// 验证非空
        /// </summary>
        /// <param name="data">验证对象</param>
        /// <returns> 验证非空</returns>
        public static bool NotNull(object data)
        {
            return !(data == null);
        }

        /// <summary>
        /// 检查设置的端口号是否正确
        /// </summary>
        /// <param name="port">端口号</param>
        /// <returns>端口号是否正确</returns>
        public static bool IsValidPort(string port)
        {
            bool result = false;
            int minPORT = 0, _maxPORT = 65535;
            int portValue = -1;

            if (int.TryParse(port, out portValue)) {
                result = !((portValue < minPORT) || (portValue > _maxPORT));
            }

            return result;
        }
    }
}
