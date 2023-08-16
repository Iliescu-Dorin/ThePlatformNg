namespace Core.Services.DataEncryption;
public interface IDataEncryption
{
    string Encrypt(string plainInput);
    string Decrypt(string cipherText);
}
