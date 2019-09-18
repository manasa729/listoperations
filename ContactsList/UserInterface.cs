using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsList
{
    class UserInterface
    {
        ContactService contactService;
        public UserInterface(string password)
        {
            contactService = new ContactService(password);
        }
        public void PerformOperationOnList(string password)
        {
            while (true)
            {
                PrintContent(Constants.menuToPerformOperations);
                PrintContent(Constants.choose);
                int.TryParse(Console.ReadLine(), out int choice);
               
                if (choice == 1)
                {
                    PrintContent(Constants.titleForList);
                    string title = Console.ReadLine();
                    contactService.CreateList(password, title);
                    PrintContent(Constants.listCreated +": 'title'");
                }
                else if (choice == 2)
                {
                    PrintContent(Constants.titleForList);
                    string title = Console.ReadLine();
                    contactService.DeleteList(password, title);
                }
                else
                {
                    PrintContent(Constants.errMsg);
                    break;
                }
            }
            
        }
        public static void PrintContent(string text)
        {
            Console.WriteLine(text);
        }
    }
}            
   

