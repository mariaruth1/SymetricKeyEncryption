namespace Symetric_keyEncryption;

public class SecretMessage
{
    public byte[] Salt { get; set; }
    public byte[] Ciphertext { get; set; }
    public byte[] Nonce { get; set; }
    public byte[] Tag { get; set; }
}