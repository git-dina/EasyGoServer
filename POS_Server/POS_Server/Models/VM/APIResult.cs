using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace POS_Server.Models.VM
{

    public class APIResult
    {
        private static bool isPoorConnection = false;
        public static string APIUri = "https://localhost:443/api/";
     // private static string Secret = "EREMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";
        private static string Secret = "EREMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bX6CUYTR==";
        private static Random random = new Random();
       
        public static string writeToTmpFile(string text)
        {
           // string dir = Directory.GetCurrentDirectory();
            string tmpPath = System.Web.Hosting.HostingEnvironment.MapPath("~\\images\\temp");
            if (!Directory.Exists(tmpPath))
                Directory.CreateDirectory(tmpPath);
            //check if file in use
            string fileName = "";
            Random rnd = new Random();
            int fileNum = rnd.Next();
            string filePath = "";
            while (true)
            {
                fileName = "tmp" + fileNum.ToString() + ".txt";
                filePath = Path.Combine(tmpPath, fileName);
                if (File.Exists(filePath))
                {
                    fileNum = rnd.Next();
                }
                else
                {
                    File.WriteAllText(@filePath, text);
                    break;
                }
            }
 
            return filePath;
        }
        
      
        #region encryption & decryption
        public static string Encrypt(string Text)
        {
            byte[] b = ConvertToBytes(Text);
            b = Encrypt(b);
            return ConvertToText(b);
        }
        private static byte[] ConvertToBytes(string text)
        {
            return System.Text.Encoding.Unicode.GetBytes(text);
        }
        public static byte[] Encrypt(byte[] ordinary)
        {
            BitArray bits = ToBits(ordinary);
            BitArray LHH = SubBits(bits, 0, bits.Length / 2);
            BitArray RHH = SubBits(bits, bits.Length / 2, bits.Length / 2);
            BitArray XorH = LHH.Xor(RHH);
            RHH = RHH.Not();
            XorH = XorH.Not();
            BitArray encr = ConcateBits(XorH, RHH);
            byte[] b = new byte[encr.Length / 8];
            encr.CopyTo(b, 0);
            return b;
        }
        public static string Decrypt(string EncryptedText)
        {
            byte[] b = ConvertToBytes(EncryptedText);
            b = Decrypt(b);
            return ConvertToText(b);
        }
        public static byte[] Decrypt(byte[] Encrypted)
        {
            BitArray enc = ToBits(Encrypted);
            BitArray XorH = SubBits(enc, 0, enc.Length / 2);
            XorH = XorH.Not();
            BitArray RHH = SubBits(enc, enc.Length / 2, enc.Length / 2);
            RHH = RHH.Not();
            BitArray LHH = XorH.Xor(RHH);
            BitArray bits = ConcateBits(LHH, RHH);
            byte[] decr = new byte[bits.Length / 8];
            bits.CopyTo(decr, 0);
            return decr;
        }
        private static string ConvertToText(byte[] ByteAarry)
        {
            return System.Text.Encoding.Unicode.GetString(ByteAarry);
        }
        private static BitArray ToBits(byte[] Bytes)
        {
            BitArray bits = new BitArray(Bytes);
            return bits;
        }
        private static BitArray SubBits(BitArray Bits, int Start, int Length)
        {
            BitArray half = new BitArray(Length);
            for (int i = 0; i < half.Length; i++)
                half[i] = Bits[i + Start];
            return half;
        }
        private static BitArray ConcateBits(BitArray LHH, BitArray RHH)
        {
            BitArray bits = new BitArray(LHH.Length + RHH.Length);
            for (int i = 0; i < LHH.Length; i++)
                bits[i] = LHH[i];
            for (int i = 0; i < RHH.Length; i++)
                bits[i + LHH.Length] = RHH[i];
            return bits;
        }
        #endregion

        #region Compress & Decompress
        public static byte[] Compress(byte[] bytData)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                Stream s = new GZipStream(ms, CompressionMode.Compress);
                s.Write(bytData, 0, bytData.Length);
                s.Close();
                byte[] compressedData = (byte[])ms.ToArray();
                return compressedData;

            }
            catch
            {
                return null;
            }
        }       
        public static byte[] DeCompress(byte[] bytInput)
        {
            //string strResult = "";
            //int totalLength = 0;
            //byte[] writeData = new byte[100000];

            //Stream s2 = new GZipStream(new MemoryStream(bytInput), CompressionMode.Decompress,true);

            ////try
            ////{
            //    while (true)
            //{
            //    int size = s2.Read(writeData, 0, writeData.Length);
            //    if (size > 0)
            //    {
            //        totalLength += size;
            //        strResult += System.Text.Encoding.UniCode.GetString(writeData, 0, size);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //s2.Close();
            //return Encoding.UniCode.GetBytes(strResult);
            Stream ms = new MemoryStream(bytInput);
            Stream deCodedStream = new MemoryStream();
            byte[] buffer = new byte[4096];

            using (Stream inGzipStream = new GZipStream(ms, CompressionMode.Decompress,true))
            {
                int bytesRead;
                while ((bytesRead = inGzipStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    deCodedStream.Write(buffer, 0, bytesRead);
                }
                using (var memoryStream = new MemoryStream())
                {
                    deCodedStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            
            //}
            //    catch
            //    {
            //        return null;
            //    }
        }

        #endregion
        #region reverse string
        static string ReverseString(string str)
        {
            int Length;
            string reversestring = "";
            Length = str.Length - 1;
            while (Length >= 0)
            {
                reversestring = reversestring + str[Length];
                Length--;
            }
            return reversestring;
        }
        #endregion
        public static string EncryptThenCompress(string text)
        {
            string str1 = Encrypt(text);
            //var bytes = Encoding.UniCode.GetBytes(text);

            //string str2 = Compress(text);
            //string str2 = Encoding.UniCode.GetString(bytes1);
            //return str2;
          //var str2 = ReverseString(str1);
            var bytes = Encoding.UTF8.GetBytes(str1);
            return (Encoding.UTF8.GetString(bytes));
        }

        public static string DeCompressThenDecrypt(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            text = Encoding.UTF8.GetString(bytes);
            //string reversedStr = ReverseString(text);
            //var bytes = Encoding.UniCode.GetBytes(reversedStr);
            //var bytes1 = DeCompress(bytes);
            //string str = Encoding.UniCode.GetString(bytes1);
            return( Decrypt(text));
        }

        private static string generateRequestToken()
        {
            //if(MainWindow.posLogin != null)
            //    return DateTime.Now.ToFileTime() + random.Next() + MainWindow.posLogin.PosId.ToString();
            //else
                return DateTime.Now.ToFileTime() + random.Next() + "0";

        }

    }

}
