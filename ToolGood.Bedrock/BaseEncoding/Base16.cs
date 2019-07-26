using System.Globalization;
using System.Text;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// Base16
    /// </summary>
    public static class Base16
    {
        /// <summary>
        /// 转成 Base16String
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase16String(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes) {
                builder.Append(b.ToString("X2"));
            }
            return builder.ToString();
        }
        /// <summary>
        /// 转成 byte[] 
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] FromBase16String(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2) {
                string s = hexString.Substring(i, 2);
                bytes[i / 2] = byte.Parse(s, NumberStyles.HexNumber, null);
            }
            return bytes;
        }



    }

}
