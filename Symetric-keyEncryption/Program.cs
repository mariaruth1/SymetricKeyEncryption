using System.Text.Json;

namespace Symetric_keyEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            var filepath = "C:/Users/USER_NAME/Downloads/".Replace("USER_NAME", Environment.UserName);
            string? value;
            do
            {
                Console.Write("1: Safely store message\n2: Read message\n0: Exit\n");
                var numChoice = Convert.ToInt32(Console.ReadLine());
                var fileName = "";
                
                switch (numChoice){
                    case 1:
                        try
                        {
                            Console.WriteLine("Enter password");
                            var password = Console.ReadLine();
                            Console.WriteLine("Type a message to encrypt:");
                            var message = Console.ReadLine();
                            var encryptedMessage = EncryptDecrypt.Encrypt(message, password);
                            var jsonString = JsonSerializer.Serialize(encryptedMessage);
                            Console.WriteLine("Enter a name for the file: ");
                            fileName = Console.ReadLine();
                            using (var writer = new StreamWriter(filepath + fileName + ".txt", true))
                            {
                                writer.WriteLine(jsonString);
                            }
                            Console.WriteLine("Message encrypted and saved as " + fileName + ".txt in Downloads folder");
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Oops, something went wrong");
                        }
                        break;
                    
                    case 2:
                        try
                        {
                            Console.WriteLine("Enter password");
                            var enteredPassword = Console.ReadLine();
                            Console.WriteLine("Enter file name to decrypt within Downloads folder: ");
                            fileName = Console.ReadLine();
                            var encryptedFile = File.ReadAllText(filepath + fileName + ".txt");
                            var messageToDecrypt = JsonSerializer.Deserialize(encryptedFile, typeof(SecretMessage));
                            var decryptedMessage = EncryptDecrypt.Decrypt(enteredPassword, messageToDecrypt as SecretMessage);
                            Console.WriteLine(decryptedMessage);
                        }
                        catch (Exception e)
                        {
                            throw new Exception("File not found or password incorrect");
                        }
                        break;
                    
                    case 0:
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                    
                    default:
                        Console.WriteLine("Not a valid choice");
                        break;              
                }
                Console.Write("Do you want to continue(y/n):");
                value = Console.ReadLine();          

            }
            while (value == "y" || value == "Y");
            Environment.Exit(0);
        }
       
    }
}