using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Constants.passWord);
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            ReadingPassword readingPassword = new ReadingPassword();
            password = readingPassword.GeneratePassword(info);
            UserInterface userInterface = new UserInterface(password);
            userInterface.PerformOperationOnList(password);     
            Console.ReadKey();
        }
        public static void PrintContent(string text)
        {
            Console.WriteLine(text);
        }

    }
}
