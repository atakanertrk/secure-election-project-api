using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WebAPI.Helpers
{
    public class RSAHelper
    {
        public static string EncryptRSA(string plainText, string publicKey, string utfValue)
        {
            //Text olarak aldığımız publicKey'i RSACryptoServiceProvider'da kullanabilmek için
            //tekrar XML formatına dönüştürüyoruz
            dynamic pubKey;
            {
                var sr = new System.IO.StringReader(publicKey);
                // deserialize işlemi
                // RSAParameters -> RSA için gereken paramatreleri barındırır (xml formatında bu değerler mevcut)
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                pubKey = (RSAParameters)xs.Deserialize(sr);
            }
            // 2048 bitlik RSA instance'ı oluşturuyoruz
            var csp = new RSACryptoServiceProvider(2048);
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);

            byte[] bytesPlainTextData;
            //Şifreleme işlemini text yerine byte üstünden yapmak için text'i byte array'e dönüştürüyoruz
            if (utfValue == "utf8")
            {
                bytesPlainTextData = System.Text.Encoding.UTF8.GetBytes(plainText);
            }
            else
            {
                // utf16 support
                bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainText);
            }


            //ikinci paramatre olarak; şifreleme için OAEP veya PKCS pattern'i kullanılabilir. Biz PKCS kullanacağız
            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

            // PKCS pattern'i ile 2048 bit uzunluğunda şifrelenmiş verimizin text'e dönüştürüyoruz
            var encryptedText = Convert.ToBase64String(bytesCypherText);

            return encryptedText;
        }
        public static string DecryptRSA(string encryptedText, string privateKey, string utfValue)
        {
            dynamic privKey;
            //text formatında gelen privateKey'imizi kullanabilmek için XML serileştirmesi yapıyoruz
            {
                var sr = new System.IO.StringReader(privateKey);
                //deserialize işlemi
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
                privKey = (RSAParameters)xs.Deserialize(sr);
            }
            // 2048 bitlik RSA instance'ı oluşturuyoruz. Şifrelerken de bunu kullanmıştık
            var csp = new RSACryptoServiceProvider(2048);
            var bytesEncryptedText = Convert.FromBase64String(encryptedText);

            // Şifre çözme işlemi için privateKey'imizi yüklüyoruz
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);

            //PKCS pattern'i ile şifre çözmeyi gerçekleştiriyoruz (byte array olarak) 
            var bytesPlainTextData = csp.Decrypt(bytesEncryptedText, false);

            //Byte dizisinden tekrar text formatına dönüyoruz. Şifre çözülmüş oluyor
            string plainTextData;
            if (utfValue == "utf8")
            {
                plainTextData = System.Text.Encoding.UTF8.GetString(bytesPlainTextData);
            }
            else
            {
                // UTF16 support
                plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
            }
            return plainTextData;
        }
    }
}
