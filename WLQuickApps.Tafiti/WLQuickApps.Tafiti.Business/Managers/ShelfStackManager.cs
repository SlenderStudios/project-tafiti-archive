using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Text;
using System.Web;

using WLQuickApps.Tafiti.Data;
using WLQuickApps.Tafiti.Data.ShelfStackDataSetTableAdapters;
using WLQuickApps.Tafiti.Data.PendingInviteDataSetTableAdapters;

namespace WLQuickApps.Tafiti.Business
{
    public class ShelfStackManager
    {
        static private ReadOnlyCollection<ShelfStack> GetShelvesFromTable(ShelfStackDataSet.ShelfStackDataTable table)
        {
            List<ShelfStack> list = new List<ShelfStack>();
            foreach (ShelfStackDataSet.ShelfStackRow row in table)
            {
                ShelfStack shelfStack = new ShelfStack(row);
                // TODO: ACL for shelf?
                list.Add(shelfStack);
            }
            return list.AsReadOnly();
        }

        static public bool CheckForUpdates(DateTime dateTime)
        {
            UserManager.VerifyUserIsLoggedIn();

            foreach (ShelfStack shelfStack in ShelfStackManager.GetShelfStacksForUser(UserManager.LoggedInUser))
            {
                if (shelfStack.LastModifiedTimestamp > dateTime)
                {
                    return true;
                }
            }
            return false;
        }

        static public ShelfStack CreateShelfStack(string label)
        {
            UserManager.VerifyUserIsLoggedIn();

            using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
            {
                Guid id = Guid.NewGuid();
                adapter.CreateShelfStack(id, label);
                
                ShelfStack shelfStack = ShelfStackManager.GetShelfStackByID(id);
                ShelfStackManager.AddUserToShelfStack(UserManager.LoggedInUser, shelfStack);
                return shelfStack;
            }
        }

        static public ShelfStack GetShelfStackByID(Guid shelfStackID)
        {
            using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
            {
                ShelfStackDataSet.ShelfStackDataTable table = adapter.GetShelfStackByID(shelfStackID);
                if (table.Rows.Count == 0) { return null; }
                return new ShelfStack(table[0]);
            }
        }

        static public ReadOnlyCollection<ShelfStack> GetShelfStacksForUser(User user)
        {
            using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
            {
                return ShelfStackManager.GetShelvesFromTable(adapter.GetShelfStacksForUserID(user.UserID));
            }
        }

        static private ReadOnlyCollection<ShelfStack> GetPendingShelfStacksForUser(User user)
        {
            using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
            {
                return ShelfStackManager.GetShelvesFromTable(adapter.GetPendingShelfStackInvitesForEmailHash(user.EmailHash));
            }
        }

        static public ReadOnlyCollection<string> GetPendingInvitesForShelfStack(ShelfStack shelfStack)
        {
            using (PendingInvitesTableAdapter adapter = new PendingInvitesTableAdapter())
            {
                List<string> list = new List<string>();
                foreach (PendingInviteDataSet.PendingInvitesRow row in adapter.GetPendingInvitesForShelfStack(shelfStack.ShelfStackID))
                {
                    list.Add(row.EmailHash);
                }
                return list.AsReadOnly();
            }
        }

        static public void AddUserToShelfStack(User user, ShelfStack shelfStack)
        {
            // Don't perform the security check if no owner has yet been assigned since
            // this call is to add the initial owner.
            if (UserManager.GetShelfOwners(shelfStack.ShelfStackID).Count > 0)
            {
                ShelfStackManager.VerifyOwnerAction(shelfStack);
            }

            using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
            {
                adapter.AddUserToShelfStack(user.UserID, shelfStack.ShelfStackID);
                ShelfStackManager.UpdateShelfStack(shelfStack);
            }
        }

        static public void RemoveUserFromShelfStack(User user, ShelfStack shelfStack)
        {
            UserManager.VerifyUserIsLoggedIn();
            if (UserManager.LoggedInUser != user)
            {
                throw new SecurityException("You cannot remove someone else from a shelf stack");
            }

            using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
            {                
                adapter.RemoveUserFromShelfStack(user.UserID, shelfStack.ShelfStackID);

                ReadOnlyCollection<User> owners = UserManager.GetShelfOwners(shelfStack.ShelfStackID);
                if (owners.Count == 0)
                {
                    adapter.DeleteShelfStack(shelfStack.ShelfStackID);
                }
                else
                {
                    ShelfStackManager.UpdateShelfStack(shelfStack);
                }
            }
        }

        static public void DeleteShelfStack(ShelfStack shelfStack)
        {
            ShelfStackManager.VerifyOwnerAction(shelfStack);

            using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
            {
                adapter.DeleteShelfStack(shelfStack.ShelfStackID);
            }
        }

        static private void DeletePendingShelfStacksForUser(User user)
        {
            using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
            {
                adapter.DeletePendingInvitesForEmailHash(user.EmailHash);
            }
        }

        static public void CreatePendingInvite(Guid shelfStackID, string emailHash)
        {
            ShelfStackManager.VerifyOwnerAction(ShelfStackManager.GetShelfStackByID(shelfStackID));

            using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
            {
                adapter.CreatePendingInvite(shelfStackID, emailHash);
            }
        }

        static public void UpdateShelfStack(ShelfStack shelfStack)
        {
            ShelfStackManager.VerifyOwnerAction(shelfStack);

            using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
            {
                adapter.UpdateShelfStack(shelfStack.ShelfStackID, shelfStack.Label);
            }
        }

        static public void UpdatePendingInvites(User user)
        {
            foreach (ShelfStack shelfStack in ShelfStackManager.GetPendingShelfStacksForUser(user))
            {
                using (ShelfStackTableAdapter adapter = new ShelfStackTableAdapter())
                {
                    adapter.AddUserToShelfStack(user.UserID, shelfStack.ShelfStackID);
                }
            }

            ShelfStackManager.DeletePendingShelfStacksForUser(user);
        }

        static private bool CanPerformOwnerAction(ShelfStack item)
        {
            if (!UserManager.IsUserLoggedIn) { return false; }
            return UserManager.GetShelfOwners(item.ShelfStackID).Contains(UserManager.LoggedInUser);
        }

        static private void VerifyOwnerAction(ShelfStack item)
        {
            if (!ShelfStackManager.CanPerformOwnerAction(item))
            {
                throw new SecurityException("Current user does not have permission to perform this action");
            }
        }

    }
}
