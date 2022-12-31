
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


public static partial class SecurityUtil
{
    public static string GetHMACString(string str, string privateKey, Encoding encoding, HMAC hmac)
    {
        if (str == null || str.Trim().Length == 0)
        {
            return "";
        }
        hmac.Key = encoding.GetBytes(privateKey);
        var bytes = hmac.ComputeHash(encoding.GetBytes(str));
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(HEX_DIGITS[(bytes[i] & 0xf0) >> 4] + ""
                    + HEX_DIGITS[bytes[i] & 0xf]);
        }
        return sb.ToString();
    }

    public static string GetSha1String(string str, string privateKey, Encoding encoding)
    {
        return GetHMACString(str, privateKey, encoding, new HMACSHA1());

    }
    public static string GetSha512String(string str, string privateKey)
    {
        return GetHMACString(str, privateKey, Encoding.UTF8, new HMACSHA512());

    }

    private static char[] HEX_DIGITS = new char[]{'0', '1', '2', '3', '4', '5',
            '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
    public static string GetSha256String(string str, string privateKey)
    {
        return GetHMACString(str, privateKey, Encoding.UTF8, new HMACSHA256());

    }


    /// <summary>
    /// 生成32位大写MD5值
    /// </summary>
    public static String GetMD5String(String str)
    {


        var md = MD5.Create();
        var bytes = md.ComputeHash(Encoding.UTF8.GetBytes(str));

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(HEX_DIGITS[(bytes[i] & 0xf0) >> 4] + ""
                    + HEX_DIGITS[bytes[i] & 0xf]);
        }
        //var mdvalue = BitConverter.ToString(bytes).Replace("-","").ToUpper();

        return sb.ToString();
    }
    public static String GetMD5String(byte[] bytes)
    {


        var md = MD5.Create();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(HEX_DIGITS[(bytes[i] & 0xf0) >> 4] + ""
                    + HEX_DIGITS[bytes[i] & 0xf]);
        }
        return sb.ToString();
    }



    public static string AESEncrypt(string sourceText, string key)
    {
        string result = "";
        using (var ms = new MemoryStream())
        {
            using (var aes = Aes.Create())
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);
                var sourceTextBytes = Encoding.UTF8.GetBytes(sourceText);

                using (var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cryptoStream))
                    {
                        sw.WriteLine(sourceText);
                    }
                }
            }
            result = ms.ToArray().ToHexString();
        }
        return result;
    }

}


