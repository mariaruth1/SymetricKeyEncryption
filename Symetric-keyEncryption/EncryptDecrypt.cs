using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace Symetric_keyEncryption;
public class EncryptDecrypt
{
public static SecretMessage Encrypt(string plaintext, string password)
{
    //generate key
    var salt = new byte[32];
    RandomNumberGenerator.Fill(salt);
    
    var key = KeyDerivation.Pbkdf2(
        password: password!,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 600000,
        numBytesRequested: 256 / 8);

    using var aes = new AesGcm(key, 16);

    //generate nonce
    var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
    RandomNumberGenerator.Fill(nonce);
        
    var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
    var ciphertext = new byte[plaintextBytes.Length];
    var tag = new byte[AesGcm.TagByteSizes.MaxSize];
    
    aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);
    return new SecretMessage(){Salt = salt, Ciphertext = ciphertext, Nonce = nonce, Tag = tag};
}

public static string Decrypt(string password, SecretMessage secretMessage)
{
    var key = KeyDerivation.Pbkdf2(
        password: password!,
        salt: secretMessage.Salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 600000,
        numBytesRequested: 256 / 8);

    using var aes = new AesGcm(key, 16);
    var plaintext = new byte[secretMessage.Ciphertext.Length];
    aes.Decrypt(secretMessage.Nonce, secretMessage.Ciphertext, secretMessage.Tag, plaintext);
    return Encoding.UTF8.GetString(plaintext);
}


}