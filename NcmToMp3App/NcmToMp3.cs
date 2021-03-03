using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json.Linq;

namespace NcmToMp3App
{
    public class NcmToMp3
    {
        private static readonly byte[] AesCoreKey = { 0x68, 0x7A, 0x48, 0x52, 0x41, 0x6D, 0x73, 0x6F, 0x35, 0x6B, 0x49, 0x6E, 0x62, 0x61, 0x78, 0x57 };
        private static readonly byte[] AesModifyKey = { 0x23, 0x31, 0x34, 0x6C, 0x6A, 0x6B, 0x5F, 0x21, 0x5C, 0x5D, 0x26, 0x30, 0x55, 0x3C, 0x27, 0x28 };

        public static void ProcessFile(string filePath, string savePath)
        {
            using FileStream fs = File.Open(filePath, FileMode.Open);
            var lenBytes = new byte[4];
            fs.Read(lenBytes);
            if (BitConverter.ToInt32(lenBytes) != 0x4e455443)
            {
                Console.WriteLine("输入文件并非网易云加密文件.");
                return;
            }

            fs.Read(lenBytes);
            if (BitConverter.ToInt32(lenBytes) != 0x4d414446)
            {
                Console.WriteLine("输入文件并非网易云加密文件.");
                return;
            }

            fs.Seek(2, SeekOrigin.Current);
            fs.Read(lenBytes);
            var keyBytes = new byte[BitConverter.ToInt32(lenBytes)];
            fs.Read(keyBytes);

            for (int i = 0; i < keyBytes.Length; i++)
            {
                keyBytes[i] ^= 0x64;
            }

            // 此处解析出来的值应该为减去字符串 "neteasecloudmusic" 长度之后的信息
            var deKeyDataBytes = GetBytesByOffset(DecryptAex128Ecb(AesCoreKey, keyBytes), 17);

            fs.Read(lenBytes);
            var modifyData = new byte[BitConverter.ToInt32(lenBytes)];
            fs.Read(modifyData);

            for (int i = 0; i < modifyData.Length; i++)
            {
                modifyData[i] ^= 0x63;
            }

            // 从 Base64 字符串进行解码
            var decryptBase64Bytes = Convert.FromBase64String(Encoding.UTF8.GetString(GetBytesByOffset(modifyData, 22)));
            var decryptModifyData = DecryptAex128Ecb(AesModifyKey, decryptBase64Bytes);
            // 确定歌曲后缀名
            var musicJson = JObject.Parse(Encoding.UTF8.GetString(GetBytesByOffset(decryptModifyData, 6)));

            // 歌曲 JSON 数据读取
            var extensions = musicJson.SelectToken("$.format").Value<string>();

            // CRC 校验
            fs.Seek(4, SeekOrigin.Current);
            fs.Seek(5, SeekOrigin.Current);

            // 获取专辑图像数据
            fs.Read(lenBytes);
            var imgLength = BitConverter.ToInt32(lenBytes);
            var imageBytes = new byte[imgLength];
            if (imgLength > 0)
            {
                fs.Read(imageBytes);
            }

            var box = BuildKeyBox(deKeyDataBytes);

            var n = 0x8000;
            // 输出歌曲文件
            string saveFile = Path.Combine(savePath, $"{Path.GetFileNameWithoutExtension(filePath)}.{extensions}");
            bool exist = File.Exists(saveFile);
            if (exist)
            {
                File.Delete(saveFile);
            }
            using (var outputFile = File.Create(saveFile))
            {
                while (true)
                {
                    var tb = new byte[n];
                    var result = fs.Read(tb);
                    if (result <= 0) break;

                    for (int i = 0; i < n; i++)
                    {
                        var j = (byte)((i + 1) & 0xff);
                        tb[i] ^= box[box[j] + box[(box[j] + j) & 0xff] & 0xff];
                    }

                    outputFile.Write(tb);
                }
                outputFile.Flush();
            }
        }

        /// <summary>
        /// 使用 KEY 解密 AES 数据
        /// </summary>
        /// <param name="keyBytes">Key 的字节数组</param>
        /// <param name="data">待解密的字节数组</param>
        /// <returns>解密成功的数据</returns>
        private static byte[] DecryptAex128Ecb(byte[] keyBytes, byte[] data)
        {
            var aes = Aes.Create();
            if (aes != null)
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.ECB;
                using (var decryptor = aes.CreateDecryptor(keyBytes, null))
                {
                    byte[] result = decryptor.TransformFinalBlock(data, 0, data.Length);
                    return result;
                }
            }

            return null;
        }

        private static byte[] BuildKeyBox(byte[] key)
        {
            byte[] box = new byte[256];
            for (int i = 0; i < 256; ++i)
            {
                box[i] = (byte)i;
            }

            byte keyLength = (byte)key.Length;
            byte c;
            byte lastByte = 0;
            byte keyOffset = 0;
            byte swap;

            for (int i = 0; i < 256; ++i)
            {
                swap = box[i];
                c = (byte)((swap + lastByte + key[keyOffset++]) & 0xff);

                if (keyOffset >= keyLength)
                {
                    keyOffset = 0;
                }

                box[i] = box[c];
                box[c] = swap;
                lastByte = c;
            }

            return box;
        }

        /// <summary>
        /// 从源字节组的指定偏移截取指定长度的数据，并生成新的字节数
        /// </summary>
        /// <param name="srcBytes">源字节组</param>
        /// <param name="offset">偏移量</param>
        /// <param name="length">截取的长度</param>
        /// <returns></returns>
        private static byte[] GetBytesByOffset(byte[] srcBytes, int offset = 0, long length = 0)
        {
            if (length == 0)
            {
                var resultBytes = new byte[srcBytes.Length - offset];
                Array.Copy(srcBytes, offset, resultBytes, 0, srcBytes.Length - offset);
                return resultBytes;
            }

            var resultBytes2 = new byte[length];
            Array.Copy(srcBytes, 0, resultBytes2, 0, length);
            return resultBytes2;
        }

        /// <summary>
        /// 根据指定的后缀名称搜索文件
        /// </summary>
        /// <param name="dirPath">要搜索的文件夹</param>
        /// <param name="extensions">指定的后缀集合</param>
        /// <returns>搜索完成的文件字典</returns>
        private static Dictionary<string, List<string>> FindFiles(string dirPath, string[] extensions)
        {
            if (extensions != null && extensions.Length != 0)
            {
                var files = new Dictionary<string, List<string>>();

                foreach (var extension in extensions)
                {
                    var result = new List<string>();
                    SearchFile(result, dirPath, extension);
                    files.Add(extension, result);
                }

                return files;
            }

            return null;
        }

        private static void SearchFile(List<string> files, string folder, string extension)
        {
            foreach (var file in Directory.GetFiles(folder, extension))
            {
                files.Add(file);
            }

            try
            {
                foreach (var directory in Directory.GetDirectories(folder))
                {
                    SearchFile(files, directory, extension);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}