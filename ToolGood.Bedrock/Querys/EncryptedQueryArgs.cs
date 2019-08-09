using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ToolGood.Bedrock.Attributes;
using ToolGood.RcxCrypto;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// 加密过的查询参数
    /// </summary>
    public abstract class EncryptedQueryArgs : QueryArgs
    {
        /// <summary>
        /// 密码
        /// </summary>
        [JsonIgnore]
        public byte[] Password { get; set; }

        /// <summary>
        /// 钥匙（RSA公匙加密过的）
        /// </summary>
        [JsonProperty("rsaKey")]
        public string RsaKey { get; set; }

        /// <summary>
        /// 密文
        /// </summary>
        [JsonProperty("ciphertext")]
        public string Ciphertext { get; set; }

        /// <summary>
        /// 时间截
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [JsonProperty("sign")]
        public string Sign { get; set; }

        /// <summary>
        /// 核对签名
        /// </summary>
        /// <returns></returns>
        public bool CheckSign(out string errMsg)
        {
            if (string.IsNullOrWhiteSpace(RsaKey)) { errMsg = "rsaKey is null."; return false; }
            if (string.IsNullOrWhiteSpace(Ciphertext)) { errMsg = "ciphertext is null."; return false; }
            if (Timestamp == 0) { errMsg = "timestamp is null."; return false; }
            if (string.IsNullOrWhiteSpace(Sign)) { errMsg = "sign is null."; return false; }

            //var st = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Timestamp);
            //if (Math.Abs(st.Days) >= 1) { errMsg = "timestamp is error."; return false; }

            var txt = $"{Ciphertext.ToSafeString()}|{RsaKey.ToSafeString()}|{Timestamp.ToSafeString()}";
            var hash = HashUtil.GetMd5String(txt);
            if (Sign.ToUpper() != hash) {
                errMsg = "sign is error.";
                return false;
            }
            errMsg = null;
            return true;
        }

        /// <summary>
        /// 获取时间截差值
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimestampDiff()
        {
            return DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Timestamp);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="xmlKey"></param>
        /// <returns></returns>
        public abstract bool DecryptData(string xmlKey);

        /// <summary>
        /// 核对数据
        /// </summary>
        /// <returns></returns>
        public abstract string CheckDate();

        /// <summary>
        /// 核对数据
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckDate(out string errMsg);
    }
    /// <summary>
    /// 加密过的查询参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonRequireAttribute]
    public abstract class EncryptedQueryArgs<T> : EncryptedQueryArgs
    {
        /// <summary>
        /// 解密后的参数
        /// </summary>
        [JsonIgnore]
        public T Data { get; set; }

        /// <summary>
        /// 解密后的参数 JSON 格式
        /// </summary>
        [JsonIgnore]
        public JObject JData { get; set; }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="xmlKey"></param>
        /// <returns></returns>
        public override bool DecryptData(string xmlKey)
        {
            try {
                var bytes = Base64.FromBase64ForUrlString(RsaKey);
                Password = RsaUtil.PrivateDecrypt(xmlKey, bytes);//解密

                var bs = ThreeRCX.Encrypt(Base64.FromBase64ForUrlString(Ciphertext), Password);//解密
                var json = Encoding.UTF8.GetString(bs);


                Data = JsonConvert.DeserializeObject<T>(json);

                JData = JObject.Parse(json, new JsonLoadSettings() {
                    CommentHandling = CommentHandling.Ignore,
                    LineInfoHandling = LineInfoHandling.Ignore,
                    DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Replace
                });
                Data = JData.ToObject<T>();
                return true;
            } catch { }
            return false;
        }

        /// <summary>
        /// 核对数据
        /// </summary>
        /// <returns></returns>
        public override string CheckDate()
        {
            CheckDate(typeof(T), Data, /*JData,*/ null, out string errMsg);
            return errMsg;
        }

        /// <summary>
        /// 核对数据
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public override bool CheckDate(out string errMsg)
        {
            try {
                return CheckDate(typeof(T), Data,/* JData,*/ null, out errMsg);
            } catch (Exception ex) {
                errMsg = ex.ToErrMsg();
                return false;
            }
        }

        private bool CheckDate(Type type, object value, /*JToken jObject, */string baseName, out string errMsg)
        {
            if (type.IsClass == false) { errMsg = null; return true; }
            if (SimpleTypes.Contains(type)) { errMsg = null; return true; }
            //if (type.IsClass == false && SimpleTypes.Contains(type) == false) { errMsg = null; return true; }

            var pis = type.GetProperties();
            foreach (var pi in pis) {
                if (pi.CanRead == false) { continue; }
                object obj = pi.GetGetMethod().Invoke(value, null);
                if (obj is DateTime && (DateTime)obj == DateTime.MinValue) {
                    errMsg = $"{GetPropertyName(baseName, pi.Name)} is null.";
                    return false;
                }

                var atts = pi.GetCustomAttributes<System.ComponentModel.DataAnnotations.ValidationAttribute>(true).ToList();
                if (atts.Count > 0) {
                    foreach (var att in atts) {
                        //if ((att is System.ComponentModel.DataAnnotations.RequiredAttribute || att is RequiredAttribute)/* && jObject[pi.Name] == null*/) {
                        //    errMsg = att.FormatErrorMessage(GetPropertyName(baseName, pi.Name));
                        //    return false;
                        //} else 
                        if (att.IsValid(obj) == false) {
                            errMsg = att.FormatErrorMessage(GetPropertyName(baseName, pi.Name));
                            return false;
                        }
                    }
                }

                if (pi.PropertyType.IsClass && obj != null && SimpleTypes.Contains(pi.PropertyType) == false) {
                    if (obj is IList list) {
                        //JArray jArray = null;
                        //foreach (var item in jObject.Children()) {
                        //    if (item.Path.ToLower() == pi.Name.ToLower()) {
                        //        jArray = item.Values() as JArray;
                        //        break;
                        //    }
                        //}

                        //var jArray = jObject.FirstOrDefault(q => ((JToken)q).Path.ToLower() == pi.Name.ToLower()) as JArray;

                        //var jArray = jObject[pi.Name] as JArray;
                        for (int i = 0; i < list.Count; i++) {
                            var item = list[i];
                            if (object.Equals(null, item) == false) {
                                //var jitem = jArray[i];
                                var itemType = item.GetType();
                                if (itemType.IsClass == false) { continue; }
                                if (SimpleTypes.Contains(itemType)) { continue; }

                                if (CheckDate(itemType, item,/* jitem,*/ GetPropertyName(baseName, pi.Name, i), out errMsg) == false) {
                                    return false;
                                }
                            }
                        }
                    } else {
                        if (CheckDate(pi.PropertyType, obj, /*jObject[pi.Name],*/ GetPropertyName(baseName, pi.Name), out errMsg) == false) {
                            return false;
                        }
                    }
                }
            }
            errMsg = null;
            return true;
        }



        private static HashSet<Type> SimpleTypes = new HashSet<Type>() {
            typeof(string)
            ,typeof(byte),typeof(sbyte),typeof(char),typeof(Boolean),typeof(Guid)
            ,typeof(UInt16),typeof(UInt32),typeof(UInt64),typeof(Int16),typeof(Int32),typeof(Int64)
            ,typeof(Single),typeof(Double),typeof(Decimal)
            ,typeof(DateTime),typeof(DateTimeOffset),typeof(TimeSpan)
            ,typeof(IntPtr),typeof(UIntPtr)
            ,typeof(byte?),typeof(sbyte?),typeof(char?), typeof(Boolean?),typeof(Guid?)
            ,typeof(UInt16?),typeof(UInt32?),typeof(UInt64?),typeof(Int16?),typeof(Int32?),typeof(Int64?)
            ,typeof(Single?),typeof(Double?),typeof(Decimal?)
            ,typeof(DateTime?),typeof(DateTimeOffset?),typeof(TimeSpan?)
            ,typeof(IntPtr?),typeof(UIntPtr?)
        };

        private string GetPropertyName(string baseName, string propertyName, int index = -1)
        {
            if (string.IsNullOrEmpty(baseName)) {
                if (index == -1) {
                    return propertyName;
                }
                return $"{propertyName}[{index}]";
            }
            if (index == -1) {
                return $"{baseName}.{propertyName}";
            }
            return $"{baseName}.{propertyName}[{index}]";
        }


    }




}
