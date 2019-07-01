using System.Linq;

namespace ToolGood.Bedrock
{
    public static class Base62
    {
        private const string ALPHABET = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string ToBase62String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }

        public static byte[] FromBase62String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }


    public static class Base58
    {
        private const string ALPHABET = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        public static string ToBase58String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }

        public static byte[] FromBase58String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }

    public static class Base52
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string ToBase52String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }

        public static byte[] FromBase52String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }


    public static class Base36
    {
        private const string ALPHABET = "0123456789abcdefghijklmnopqrstuvwxyz";

        public static string ToBase36String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }

        public static byte[] FromBase36String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }

    public static class Base32
    {
        private const string ALPHABET = "0123456789abcdefghijklmnopqrstuvwxyz";

        public static string ToBase32String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }

        public static byte[] FromBase32String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }

    public static class Base26
    {
        private const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";

        public static string ToBase26String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }

        public static byte[] FromBase26String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }
}
