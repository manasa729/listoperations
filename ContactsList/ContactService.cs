using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;

namespace ContactsList
{
    class ContactService
    {
        Authentication authentication;
        PermissionsClass permissions;
        public ContactService(string password)
        {
            authentication = new Authentication(password);
            permissions = new PermissionsClass(password);
        }
        
        public List CreateList(string password,string title)
        {            
            var clientContext = authentication.Credentials(password);
            ListCreationInformation creationInfo = new ListCreationInformation();
            creationInfo.Title = title;
            creationInfo.Description = "new list created using VS 2012 &CSOM";
            creationInfo.TemplateType = (int)ListTemplateType.GenericList;           
            List newList = clientContext.Web.Lists.Add(creationInfo);
            List list = clientContext.Web.Lists.GetByTitle("ListDemo1");
            clientContext.Load(list);
            clientContext.ExecuteQuery();
            TaxonomyField(clientContext, newList);
            newList.Fields.AddFieldAsXml(Constants.nameField, true, AddFieldOptions.DefaultValue);
            newList.Fields.AddFieldAsXml(Constants.addressField, true, AddFieldOptions.DefaultValue);
            newList.Fields.AddFieldAsXml(Constants.numberField, true, AddFieldOptions.DefaultValue);
            newList.Fields.AddFieldAsXml("<Field Type ='Lookup' "+
                                            " DisplayName='Department' "+
                                            " Required ='FALSE' "+ 
                                            " List ='" +list.Id + "' " +
                                            " ShowField='department'" +  
                                            " StaticName='department'" + 
                                            " Name='Department'/> ",
                                            true, AddFieldOptions.DefaultValue);
            permissions.BreakRoleInheritanceForList(title,password);            
            permissions.CreateGroup(password,title);
            clientContext.Load(newList);
            clientContext.ExecuteQuery();
            Console.ReadLine();
            return newList;
        }

        private void TaxonomyField(ClientContext clientContext,List list)
        {
            List newList = list;           
            Guid termStoreId = Guid.Empty;
            Guid termSetId = Guid.Empty;
            string schemaTaxonomyField ="<Field ID ='{43f72def-9536-4e88-8eaf-23e1f528f420}'"+
                                          " Type='TaxonomyFieldType' " + 
                                          " Name='ManagedDepartment' "+ 
                                          " StaticName='ManagedDepartment' "+
                                          " DisplayName='ManagedDepartment'/>" ;
            Field field = newList.Fields.AddFieldAsXml(schemaTaxonomyField, true, AddFieldOptions.AddFieldInternalNameHint);
            GetTaxonomyFieldInfo(clientContext, out termStoreId, out termSetId);
            TaxonomyField taxonomyField = clientContext.CastTo<TaxonomyField>(field);
            taxonomyField.SspId = termStoreId;
            taxonomyField.TermSetId = termSetId;
            taxonomyField.TargetTemplate = String.Empty;
            taxonomyField.AnchorId = Guid.Empty;
            taxonomyField.Update();
            clientContext.ExecuteQuery();
        }

        private void GetTaxonomyFieldInfo(ClientContext clientContext, out Guid termStoreId, out Guid termSetId)
        {
           
            TaxonomySession session = TaxonomySession.GetTaxonomySession(clientContext);
            TermStore termStore = session.GetDefaultSiteCollectionTermStore();                
            TermSetCollection termSets = termStore.GetTermSetsByName("Department",1033);
            clientContext.Load(termSets, tsc => tsc.Include(ts => ts.Id));
            clientContext.Load(termStore);
            clientContext.ExecuteQuery();
            termStoreId = termStore.Id;
            termSetId = termSets.FirstOrDefault().Id;
        }

        public void DeleteList(string password,string title)
        {
            var clientContext = authentication.Credentials(password);            
            List list = clientContext.Web.Lists.GetByTitle(title);
            list.DeleteObject();
            clientContext.ExecuteQuery();
        }

        public ListItemCollection GetItems(string password)
        {
            var clientContext = authentication.Credentials(password);
            List contactsList = clientContext.Web.Lists.GetByTitle(Constants.contacts);
            CamlQuery query = new CamlQuery();
            query.ViewXml = "<View/>";
            ListItemCollection listItems = contactsList.GetItems(query);
            clientContext.Load(listItems);
            clientContext.ExecuteQuery();
            return listItems;
        }

        public void AddingItem(string password, string contactName, string email, string department, string phone, string location)
        {
            var clientContext = authentication.Credentials(password);
            List contactList = clientContext.Web.Lists.GetByTitle(Constants.contacts);
            ListItemCreationInformation listCreationInformation = new ListItemCreationInformation();
            ListItem contactListItem = contactList.AddItem(listCreationInformation);
            contactListItem[Constants.contactName] = contactName;
            contactListItem[Constants.title] = contactName;
            contactListItem[Constants.email] = email;
            contactListItem[Constants.department] = department;
            contactListItem[Constants.phoneNumber] = phone;
            contactListItem[Constants.location] = location;
            contactListItem.Update();
            clientContext.ExecuteQuery();
        }

        public ListItem UpdateItem(int id, string password, string field, string updatedValue)
        {
            var clientContext = authentication.Credentials(password);
            List contactList = clientContext.Web.Lists.GetByTitle("Contacts");
            ListItem contactListItem = contactList.GetItemById(id);
            contactListItem[field] = updatedValue;
            contactListItem.Update();
            clientContext.ExecuteQuery();
            return contactListItem;
        }

        public void DeleteItem(int id, string password)
        {
            var clientContext = authentication.Credentials(password);
            List contactList = clientContext.Web.Lists.GetByTitle("Contacts");
            ListItem contactListItem = contactList.GetItemById(id);
            contactListItem.DeleteObject();
            clientContext.ExecuteQuery();
        }

        
    }
}
