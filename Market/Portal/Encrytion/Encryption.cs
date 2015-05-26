namespace Portal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Routines to create private and public RSA keys and to encrypt and decrypt text strings.
    /// </summary>
    internal class Encryption
    {
        /// <summary>
        /// Defines and initialize a variable publicKeyXml.
        /// </summary>
        private static string publicKeyXML = "<RSAKeyValue><Modulus>spojf3aVtIHl1vA77X+yg6omm8H5ok8MB4OwC0qpHaX8ao40XN5XEPOD///zu7sfgOtxJ+cXlPxNzgozd/qvpEzN8aElPS9cIxZQowiWkHZzvwEqymew//cOzaMOsD6Yq+18s01qCa8vxs2G3yENccvCPlukfIcf</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        /// <summary>
        /// Defines and initialize a variable privateKeyXml.
        /// </summary>
        private static string privateKeyXML = "<RSAKeyValue><Modulus>spojf3aVtIHl1vA77X+yg6omm8H5ok8MB4OwC0qpHaX8ao40XN5XEPOD///zu7sfgOtxJ+cXlPxNzgozd/qvpEzN8aElPS9cIxZQowiWkHZzvwEqymew//cOzaMOsD6Yq+18s01qCa8vxs2G3yENccvCPlukfIcf</Modulus><Exponent>AQAB</Exponent><P>2p6zQC7V1jd8WA4By6G1BcFjS5q9zvCrrf0UOOpoIPYOXe+Hw1D1QpXfst3YgmtmHmKTflMRw3SImuE3</P><Q>0SPNdabd5eAPyXxPOvWyfAiNdAy0mQRH6BUb9kAGgxUlvHHsPB6S4xZwuQqQmGoLfU8aW53nvRjyZx1Z</Q><DP>eTopYbzW3Mu1ysoxmq5XyBI9sm3jNL5mJLvCm/D3vtdSjipF2TuqVLrw6al05pURcmXtLc54ei7DlUav</DP><DQ>r6K/o3SVmb3HxFAPQdahJCUSlkktSewcbz17FBzE20ThQhbya7LJbMilteC3eihkqcHKwvIjcd0Hha1R</DQ><InverseQ>vz0IYzZCppSccGaDZnf3wJm150yEoCkhXW2urIOaL6zYg2JJb+Y0cjYJSyz0bH2gDUXr9xqZmhsgttmZ</InverseQ><D>kTRkG+Mrf2AEnyUdc8/YMNeLICMqc91UaF+WJvgCWopyl6cZx0809iEldmJ/pGdUC5pfmxN0xroB/7umFOSws0u90T0Xb7RVPkun+lFWulcyPbU+xJunaRqUNHyzSBhNErgstWD+1JmhOHsvvm20CF/lyPruGiMh</D></RSAKeyValue>";

        /// <summary>
        /// Defines a variable KeySize.
        /// </summary>
        private static int keySize = 120;  // keysize in bytes

        /// <summary>
        /// Creates the RSA keys using 2048 bit (256 byte) encrypting.
        /// </summary>
        /// <returns>The public key is in the first list element and private/public keys in the second element.</returns>
        public static List<string> RSACreateKeys()
        {
            List<string> xmlKeys = new List<string>();
            try
            {
                ////Create a new RSACryptoServiceProvider object. 
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize * 8))
                {
                    //// Convert public key to XML
                    publicKeyXML = rsa.ToXmlString(false);
                    xmlKeys.Add(publicKeyXML);

                    //// Convert private/public key to XML
                    privateKeyXML = rsa.ToXmlString(true);
                    xmlKeys.Add(privateKeyXML);
                    return xmlKeys;
                }
            }
            catch
            {
            }

            return xmlKeys;
        }

        /// <summary>
        /// Encrypts the encoded text string using the public key.
        /// </summary>
        /// <param name="stringToEncrypt">Encrypt string</param>
        /// <returns>Returns the encrypted string</returns>
        public static string RSAEncrypt(string stringToEncrypt)
        {
            try
            {
                byte[] encryptedData;
                byte[] dataToEncrypt = Encoding.Unicode.GetBytes(stringToEncrypt);
                string encryptedString = string.Empty;

                //// Create a new instance of RSACryptoServiceProvider. 
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    //// Import the RSA Key information. This only needs 
                    //// to include the public key information.
                    RSACryptoServiceProvider rSA = new RSACryptoServiceProvider();
                    rSA.FromXmlString(publicKeyXML);
                    RSAParameters rsap = rSA.ExportParameters(false);
                    rsa.ImportParameters(rsap);

                    //// Calculate how many bytes we can process allowing for OAEP padding
                    int textBlockSizeBytes = (((keySize * 8) - 384) / 8) + 5;

                    //// calculate number of _keySize blocks
                    //// because of the OAEP padding, the most bytes we can process is 210
                    int blockCnt = 0;
                    int numBlocks = (dataToEncrypt.Count() / textBlockSizeBytes) + 1;

                    //// create a buffer large enough for all 256 byte encrypted blocks.
                    byte[] encryptedBuffer = new byte[numBlocks * keySize];

                    //// Break the text string into 210 byte blocks (256 bytes after encoding)
                    for (int i = 0; i < dataToEncrypt.Count(); i = i + textBlockSizeBytes)
                    {
                        int bytesToCopy = textBlockSizeBytes;

                        //// calculate remaining bytes in last block
                        if ((dataToEncrypt.Count() - i) < textBlockSizeBytes)
                        {
                            bytesToCopy = dataToEncrypt.Count() - i;
                        }

                        //// copy the 210 byte block to a temp byte array
                        byte[] tempData = new byte[bytesToCopy];
                        Buffer.BlockCopy(dataToEncrypt, i, tempData, 0, bytesToCopy);

                        //// Encrypt the byte array using OAEP padding (true)
                        encryptedData = rSA.Encrypt(tempData, true);

                        //// copy the encrypted data to the full buffer
                        encryptedData.CopyTo(encryptedBuffer, blockCnt);
                        blockCnt += keySize;
                    }
                    //// convert the full byte buffer to a string
                    encryptedString += Convert.ToBase64String(encryptedBuffer);
                }

                return encryptedString;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Decrypts the encoded text string using the private/public key.
        /// </summary>
        /// <param name="stringToDecrypt">Decrypt string</param>
        /// <returns>Returns the decrypted string</returns>
        public static string RSADecrypt(string stringToDecrypt)
        {
            try
            {
                string decryptedString = string.Empty;
                byte[] decryptedData;
                byte[] dataToDecrypt = Convert.FromBase64String(stringToDecrypt.Trim('\0'));

                //// Create a new instance of RSACryptoServiceProvider. 
                using (RSACryptoServiceProvider rSA = new RSACryptoServiceProvider())
                {
                    //// Import the RSA Key information. This needs 
                    //// to include the private key information.
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    rsa.FromXmlString(privateKeyXML);
                    RSAParameters rsap = rsa.ExportParameters(true);
                    rSA.ImportParameters(rsap);

                    for (int i = 0; i < dataToDecrypt.Count(); i = i + keySize)
                    {
                        //// the encoded blocks will all be _keySize bytes
                        int bytesToCopy = keySize;
                        byte[] tempData = new byte[keySize];

                        //// copy each 256 byte block to a temp byte array for decrypting
                        Buffer.BlockCopy(dataToDecrypt, i, tempData, 0, bytesToCopy);

                        //// Decrypt the byte array and use OAEP padding.   
                        decryptedData = rSA.Decrypt(tempData, true);

                        //// convert the decoded byte array into a string
                        decryptedString += Encoding.Unicode.GetString(decryptedData);
                    }
                }

                return decryptedString;
            }
            catch
            {
                return stringToDecrypt;
            }
        }
    }
}
