using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WLQuickApps.ContosoBicycleClub.Data
{
    public class BaseDataManager
    {
        protected ContosoBicycleClubDbDataContext DataContext
        {
            get
            {
				string connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
                ContosoBicycleClubDbDataContext context = new ContosoBicycleClubDbDataContext(connectionString);
                return context;
            }
        }
    }
}
