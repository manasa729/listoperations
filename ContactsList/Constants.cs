using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsList
{
    static class Constants
    {
        public const string siteUrl = "manasa@technoverg.onmicrosoft.com";
        public const string passWord = "Enter your password:";
        public const string availableContacts = "Available Contacts:";
        public const string menuToPerformOperations = " \n 1.Press 1 to Add a list \n 2.Press 2 to Delete a list ";
        public const string id = "ID";
        public const string contactName = "ContactName";
        public const string department = "Department";
        public const string phoneNumber = "Phone";
        public const string title = "Title";
        public const string email = "Email";
        public const string location = "Location";
        public const string requiredInformation = "Please enter the following information to add the contact";
        public const string contacts = "Contacts";
        public const string enterIdToUpdate = "Enter the ID of the contact from above displayed Contacts to update";
        public const string field = "Enter the field which you want to update or -1 to exit";
        public const string changedValue = "Enter the changes";
        public const string enterIdToDelete = "Enter the ID of the contact from above displayed Contacts to delete";
        public const string titleForList = "Please enter the title for the list";
        public const string errMsg = "Please enter the  valid option";
        public const string choose = "Enter the option";
        public const string listCreated = "List Creation is done";
        public const string nameField="<Field DisplayName='Name' Type='Text' Required='TRUE'/>";
        public const string addressField="<Field DisplayName='Address' Type='Note'/>";
        public const string numberField="<Field DisplayName='Mobile' Type='Number'/>";
        public const string lookUpField = "<Field DisplayName='Department' Type='Lookup' Required='FALSE' StaticName='Department' Name='Department'/>";

    }
}
