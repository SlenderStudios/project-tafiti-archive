using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    [TestClass]
    public class AssemblyMethods
    {
        private static TemporaryUser _ownerUser;
        private static TemporaryUser _nonOwnerUser;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            AssemblyMethods._ownerUser = new TemporaryUser(true);
            Utilities.OwnerUser = AssemblyMethods._ownerUser.TempUser;

            AssemblyMethods._nonOwnerUser = new TemporaryUser(true);
            Utilities.NonOwnerUser = AssemblyMethods._nonOwnerUser.TempUser;
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Utilities.SwitchToAdminUser();

            if (AssemblyMethods._ownerUser != null)
            {
                AssemblyMethods._ownerUser.Dispose();
                AssemblyMethods._ownerUser = null;
            }
            if (AssemblyMethods._nonOwnerUser != null)
            {
                AssemblyMethods._nonOwnerUser.Dispose();
                AssemblyMethods._nonOwnerUser = null;
            }
        }

    }
}
