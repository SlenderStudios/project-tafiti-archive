using System;
using System.Collections.Generic;
using System.Text;
using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.StatisticsDataSetTableAdapters;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class StatisticsManager
    {        
        static public Stats GetStatistics()
        {
            return GetStatistics(DateTime.Now.AddYears(-100), DateTime.Now.AddYears(100));
        }
        
        static public Stats GetStatistics(DateTime startDateTime, DateTime endDateTime)
        {
            using (StatisticsTableAdapter statisticsTableAdapter = new StatisticsTableAdapter())
            {
                 StatisticsDataSet.StatisticsDataTable statTable = statisticsTableAdapter.GetStatistics(startDateTime, endDateTime, 
                     Constants.MediaTypes.Audio, Constants.MediaTypes.File, Constants.MediaTypes.Picture, Constants.MediaTypes.Video);

                 return new Stats(statTable[0]);
            }       
        }

        static public int GetBaseItemSubTypeCount(string subType)
        {
            return GetBaseItemSubTypeCount(subType, DateTime.Now.AddYears(-100), DateTime.Now.AddYears(100));
        }
        
        static public int GetBaseItemSubTypeCount(string subType, DateTime startDate, DateTime endDate)
        {
            using (StatisticsTableAdapter statisticsTableAdapter = new StatisticsTableAdapter())
            {
                return (int)statisticsTableAdapter.GetBaseItemCountBySubTypeAndCreateDate(subType, startDate, endDate);
            }
        }
    }
}
