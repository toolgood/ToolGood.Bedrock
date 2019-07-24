using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// 程序集签名
    /// </summary>
    public class AssemblySignUtil
    {
        [DllImport("mscoree.dll", CharSet = CharSet.Unicode)]
        static extern bool StrongNameSignatureVerificationEx(string wszFilePath, bool fForceVerification, ref bool pfWasVerified);

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static bool CheckSign(string assembly)
        {
            bool notForced = false;
            return StrongNameSignatureVerificationEx(assembly, false, ref notForced);
        }

        /// <summary>
        /// 验证 token
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="expectedToken"></param>
        /// <returns></returns>
        public static bool CheckPublicKeyToken(string assembly, byte[] expectedToken)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            if (expectedToken == null)
                throw new ArgumentNullException("expectedToken");

            try {
                // Get the public key token of the given assembly
                Assembly asm = Assembly.LoadFrom(assembly);
                byte[] asmToken = asm.GetName().GetPublicKeyToken();

                // Compare it to the given token
                if (asmToken.Length != expectedToken.Length)
                    return false;

                for (int i = 0; i < asmToken.Length; i++)
                    if (asmToken[i] != expectedToken[i])
                        return false;

                return true;
            } catch (System.IO.FileNotFoundException) {
                // couldn't find the assembly
                return false;
            } catch (BadImageFormatException) {
                // the given file couldn't get through the loader
                return false;
            }
        }

        /// <summary>
        /// 验证公钥
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="expectedKey"></param>
        /// <returns></returns>
        public static bool CheckPublicKey(string assembly, byte[] expectedKey)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            if (expectedKey == null)
                throw new ArgumentNullException("expectedKey");

            try {
                // Get the public key token of the given assembly
                Assembly asm = Assembly.LoadFrom(assembly);
                byte[] asmToken = asm.GetName().GetPublicKey();

                // Compare it to the given token
                if (asmToken.Length != expectedKey.Length)
                    return false;

                for (int i = 0; i < asmToken.Length; i++)
                    if (asmToken[i] != expectedKey[i])
                        return false;

                return true;
            } catch (System.IO.FileNotFoundException) {
                // couldn't find the assembly
                return false;
            } catch (BadImageFormatException) {
                // the given file couldn't get through the loader
                return false;
            }
        }






    }
}
