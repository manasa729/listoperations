using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsList
{
    class PermissionsClass
    {
        Authentication authentication;
        public PermissionsClass(string password)
        {
            authentication = new Authentication(password);
        }
        
        public void BreakRoleInheritanceForList(string title, string password)
        {
            var clientContext = authentication.Credentials(password);
            clientContext.Load(clientContext.Web, a => a.Lists);
            clientContext.ExecuteQuery();
            List list = clientContext.Web.Lists.GetByTitle(title);
            //Stop Inheritance from parent
            list.BreakRoleInheritance(false, false);
            list.Update();
            clientContext.ExecuteQuery();
        }

        public void CreateGroup(string password,string title)
        {
            var clientContext = authentication.Credentials(password);
            GroupCreationInformation groupCreationInfo = new GroupCreationInformation();
            groupCreationInfo.Title = "grourep";
            //groupCreationInfo.Description = ConfigurationManager.AppSettings["GroupDesc"];
            Group oGroup = clientContext.Web.SiteGroups.Add(groupCreationInfo);
            clientContext.Load(oGroup,
                group => group.Title);
            clientContext.ExecuteQuery();
            AssignGroupRole(password, groupCreationInfo.Title, title);
        }

        public void DeleteGroup(string password, string group)
        {
            var clientContext = authentication.Credentials(password);
            Group oGroup = clientContext.Web.SiteGroups.GetByName(group);
            clientContext.Web.SiteGroups.Remove(oGroup);
        }

        public void AssignGroupRole(string password, string permisionGroup, string title)
        {
            var clientContext = authentication.Credentials(password);
            Group oGroup = clientContext.Web.SiteGroups.GetByName(permisionGroup);
            List oList = clientContext.Web.Lists.GetByTitle(title);
            RoleDefinitionBindingCollection collRoleDefinitionBinding = new RoleDefinitionBindingCollection(clientContext);
            RoleDefinition oRoleDefinition = clientContext.Web.RoleDefinitions.GetByType(RoleType.Contributor);
            collRoleDefinitionBinding.Add(oRoleDefinition);
            oList.RoleAssignments.Add(oGroup, collRoleDefinitionBinding);
            clientContext.Load(oGroup,
                group => group.Title);
            clientContext.Load(oRoleDefinition,
                role => role.Name);
            clientContext.ExecuteQuery();
            AddUserToGroup(password, permisionGroup);
            AddAsReader(title, password);
        }

        public void AddUserToGroup(string password, string permisionGroup)
        {
            var clientContext = authentication.Credentials(password);
            GroupCollection collGroup = clientContext.Web.SiteGroups;
            Group oGroup = collGroup.GetByName(permisionGroup);
            User user = clientContext.Web.SiteUsers.GetByEmail("Soukya@technoverg.onmicrosoft.com");
            User oUser = oGroup.Users.AddUser(user);
            clientContext.ExecuteQuery();
        }

        public void AddAsReader(string title, string password)
        {
            var clientContext = authentication.Credentials(password);
            List oList = clientContext.Web.Lists.GetByTitle(title);
            User oUser = clientContext.Web.SiteUsers.GetByEmail("Soukya@technoverg.onmicrosoft.com");
            //oList.RoleAssignments.GetByPrincipal(oUser).DeleteObject(); //delete the users already exisiting roles (if role existed)
            RoleDefinitionBindingCollection collRoleDefinitionBinding = new RoleDefinitionBindingCollection(clientContext);
            collRoleDefinitionBinding.Add(clientContext.Web.RoleDefinitions.GetByType(RoleType.Reader));
            oList.RoleAssignments.Add(oUser, collRoleDefinitionBinding);
            clientContext.ExecuteQuery();
        }
    }
}
