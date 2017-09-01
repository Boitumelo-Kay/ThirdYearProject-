using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Dail_a_chef_service.Models
{
    public class Hash
    {
        private const int Saltsize = 6;//size for Salt Byte Array

        public static string ComputeHash(string text)
        {

            if(text == null)
            {
                return "";
            }

            StringBuilder build = new StringBuilder();
            //text = new StringBuilder(text).ToString();

            byte[] saltBytes = new byte[Saltsize]; // Allocate a byte array, which will hold the salt.

           
           // RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider(); // Initialize a random number generator.

           // rng.GetNonZeroBytes(saltBytes);

            byte[] textBytes = Encoding.UTF8.GetBytes(text);// Convert plain text into a byte array.

            

            byte[] textSalt = new byte[saltBytes.Length + textBytes.Length]; // Allocate array, which will hold plain text and salt.

            

            for (int i = 0; i < textBytes.Length; i++)
            {
                textSalt[i] = textBytes[i];// Copy plain text bytes into resulting array.
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                textSalt[textBytes.Length + i] = saltBytes[i];// Append salt bytes to the resulting array.
            }

            HashAlgorithm hash = new SHA256Managed();

            byte[] hashBytes = hash.ComputeHash(textSalt);// Compute hash value of our plain text with appended salt.

            byte[] hashSalt = new byte[hashBytes.Length + saltBytes.Length]; // Create array which will hold hash and original salt bytes.

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashSalt[i] = hashBytes[i]; // Copy hash bytes into resulting array.
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                hashSalt[hashBytes.Length + i] = saltBytes[i];// Append salt bytes to the result.
            }

            string strHash = Convert.ToBase64String(hashSalt);// Convert result into a base64-encoded string.

            return strHash;
        }

    }
}