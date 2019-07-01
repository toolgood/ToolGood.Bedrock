using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Bedrock;

namespace ToolGood.Bedrock
{
    public static class PhoneNumUtil
    {
        /// <summary>
        /// 移动号段
        /// </summary>
        private static readonly int[] mobileYD = { 134, 135, 136, 137, 138, 139, 147, 148, 150, 151, 152, 157, 158, 159, 172, 178, 182, 183, 184, 187, 188, 198 };
        /// <summary>
        /// 联通号段
        /// </summary>
        private static readonly int[] mobileLT = { 130, 131, 132, 145, 146, 155, 156, 166, 171, 175, 176, 185, 186 };
        /// <summary>
        /// 电信号段
        /// </summary>
        private static readonly int[] mobileDX = { 133, 149, 153, 173, 174, 177, 180, 181, 189, 199 };
        /// <summary>
        /// 手机号前三位数据源
        /// </summary>
        private static readonly int[] phonePrefix =
        {
            130, 131, 132, 155, 156, 186, 185, 134, 135, 136, 137, 138, 139,
            150, 151, 152, 157, 158, 159, 182, 183, 188, 187, 133, 153, 180, 181, 189
        };



        /// <summary>
        /// 校验手机号码格式是否正确
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public static bool IsVaildate(string phoneNum)
        {
            return ValidatorRegex.MobileRegex.IsMatch(phoneNum);
        }

        /// <summary>
        /// 根据手机号号段生成邮箱
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        public static string GenPhoneEmail(string phoneNum)
        {
            var phoneNumFisrt3 = int.Parse(phoneNum.Substring(0, 3));
            //移动邮箱后缀
            if (mobileYD.Contains(phoneNumFisrt3)) {
                return phoneNum + "@139.com";
            }
            //联通邮箱后缀
            else if (mobileLT.Contains(phoneNumFisrt3)) {
                return phoneNum + "@wo.cn";
            }
            //电信邮箱后缀
            else if (mobileDX.Contains(phoneNumFisrt3)) {
                return phoneNum + "@189.cn";
            }
            return null;
        }

        /// <summary>
        /// 随机生成手机号码
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<string> GenPhoneNumbers(int count = 20)
        {
            var randomNums = new List<int>(count);
            var phoneNums = new List<string>(count);
            var seed = Math.Abs(Guid.NewGuid().GetHashCode());
            for (var i = 0; i < count; i++) {
                var guidHash = Math.Abs(Guid.NewGuid().GetHashCode());
                var indexRnd = new Random(guidHash);
                var rnd = new Random(seed + guidHash);
                var index = indexRnd.Next(0, phonePrefix.Length - 1);
                var rndNum = rnd.Next(0, 9999);
                if (randomNums.Contains(rndNum)) {
                } else {
                    phoneNums.Add($"{phonePrefix[index]}****{rndNum.ToString().PadLeft(4, '0')}");
                    randomNums.Add(rndNum);
                }
            }
            return phoneNums;
        }

    }

}
