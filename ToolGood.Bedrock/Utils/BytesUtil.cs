using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteBTC.Common.Tools
{
    public class BytesUtil
    {
        public static DateTime Int2Date(int date)
        {
            return new DateTime(date / 10000, date % 10000 / 100, date % 100);
        }

        public static DateTime Int2Date(int date, int time)
        {
            return new DateTime(date / 10000, date % 10000 / 100, date % 100, time / 10000, time % 10000 / 100, time % 100);
        }

        public static int Date2Int(DateTime date)
        {
            return date.Year * 10000 + date.Month * 100 + date.Day;
        }

        public static byte[] Int2Bytes(int s)
        {
            List<byte> bytes = new List<byte>();
            var k = s;
            var temp = (byte)(k & 0x7F);
            bytes.Add(temp);
            k = k >> 7;
            while (k > 0) {
                temp = (byte)(k & 0x7F | 0x80);
                bytes.Add(temp);
                k = k >> 7;
            }
            bytes.Reverse();
            return bytes.ToArray();
        }

        public static byte[] Uint2Bytes(uint s)
        {
            List<byte> bytes = new List<byte>();
            var k = s;
            var temp = (byte)(k & 0x7F);
            bytes.Add(temp);
            k = k >> 7;
            while (k > 0) {
                temp = (byte)(k & 0x7F | 0x80);
                bytes.Add(temp);
                k = k >> 7;
            }
            bytes.Reverse();
            return bytes.ToArray();
        }

        public static byte[] Long2Bytes(long s)
        {
            List<byte> bytes = new List<byte>();
            var k = s;
            var temp = (byte)(k & 0x7F);
            bytes.Add(temp);
            k = k >> 7;
            while (k > 0) {
                temp = (byte)(k & 0x7F | 0x80);
                bytes.Add(temp);
                k = k >> 7;
            }
            bytes.Reverse();
            return bytes.ToArray();
        }

        public static byte[] ULong2Bytes(ulong s)
        {
            List<byte> bytes = new List<byte>();
            var k = s;
            var temp = (byte)(k & 0x7F);
            bytes.Add(temp);
            k = k >> 7;
            while (k > 0) {
                temp = (byte)(k & 0x7F | 0x80);
                bytes.Add(temp);
                k = k >> 7;
            }
            bytes.Reverse();
            return bytes.ToArray();
        }

        public static uint Bytes2Uint(BinaryReader br)
        {
            uint result = 0;
            var b = br.ReadByte();
            var s = b & 0x80;
            b = (byte)(b & 0x7F);
            result = result | b;
            while (s == 128) {
                b = br.ReadByte();
                s = b & 0x80;
                b = (byte)(b & 0x7F);
                result = result << 7;
                result = result | b;
            }
            return result;
        }

        public static int Bytes2Int(BinaryReader br)
        {
            int result = 0;
            var b = br.ReadByte();
            var s = b & 0x80;
            b = (byte)(b & 0x7F);
            result = result | b;
            while (s == 128) {
                b = br.ReadByte();
                s = b & 0x80;
                b = (byte)(b & 0x7F);
                result = result << 7;
                result = result | b;
            }
            return result;
        }

        public static byte[] Floats2Bytes(float[] fs)
        {
            var us = FloatArrToUintArr(fs);
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms)) {
                var bbs = Uint2Bytes((uint)fs.Length);
                bw.Write(bbs);
                foreach (var u in us) {
                    bbs = Uint2Bytes(u);
                    bw.Write(bbs);
                }
                return ms.ToArray();
            }
        }

        public static float[] Bytes2Floats(byte[] bs)
        {
            List<float> fs = new List<float>();
            using (var ms = new MemoryStream(bs))
            using (var br = new BinaryReader(ms)) {
                var len = (int)Bytes2Uint(br);
                uint[] us = new uint[len];
                for (int i = 0; i < len; i++) {
                    us[i] = Bytes2Uint(br);
                }
                return UIntArrToFloatArr(us);
            }
        }

        private static uint[] FloatArrToUintArr(float[] inArr)
        {
            int intSize = sizeof(int) * inArr.Length;
            uint[] bytArr = new uint[inArr.Length];
            Buffer.BlockCopy(inArr, 0, bytArr, 0, intSize);
            return bytArr;
        }

        private static float[] UIntArrToFloatArr(uint[] inArr)
        {
            int intSize = sizeof(int) * inArr.Length;
            float[] bytArr = new float[inArr.Length];
            Buffer.BlockCopy(inArr, 0, bytArr, 0, intSize);
            return bytArr;
        }

        public static byte[] WriteList(Dictionary<int, int> dict)
        {
            var prices = dict.Keys.OrderBy(q => q).ToList();
            byte[] temp;
            using (var ms = new MemoryStream()) {
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(Uint2Bytes((uint)dict.Count));
                if (prices.Count > 0) {
                    var lastPrice = (uint)prices[0];
                    var bs = Uint2Bytes(lastPrice);
                    bw.Write(bs);
                    bs = Uint2Bytes((uint)dict[prices[0]]);
                    bw.Write(bs);
                    for (int i = 1; i < prices.Count; i++) {
                        bs = Uint2Bytes((uint)prices[i] - lastPrice);
                        bw.Write(bs);
                        bs = Uint2Bytes((uint)dict[prices[i]]);
                        bw.Write(bs);
                        lastPrice = (uint)prices[i];
                    }
                }
                return ms.ToArray();
            }
        }

        public static Dictionary<int, int> ReadList(byte[] bytes)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();

            using (var ms = new MemoryStream(bytes)) {
                BinaryReader br = new BinaryReader(ms);
                var len = Bytes2Int(br);
                if (len == 0) return result;

                var lastPrice = Bytes2Int(br);
                var vol = Bytes2Int(br);
                result[lastPrice] = vol;

                for (int i = 1; i < len; i++) {
                    var priceDiff = Bytes2Int(br);
                    lastPrice = priceDiff + lastPrice;
                    vol = Bytes2Int(br);
                    result[lastPrice] = vol;
                }
            }
            return result;
        }
    }

}
