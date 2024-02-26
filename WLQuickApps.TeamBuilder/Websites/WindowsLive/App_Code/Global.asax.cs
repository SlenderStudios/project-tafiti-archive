using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using Calendar;

/// <summary>
/// Summary description for _Global
/// </summary>
public class _Global : System.Web.HttpApplication
{
    // Static variables
    private static string _calendar = null;

    public void Application_Start(object sender, EventArgs e)
    {
        // Initialize member variables from AppSettings
        NameValueCollection appSettings = WebConfigurationManager.AppSettings;
        if (appSettings == null)
        {
            throw new IOException("Error: Failed to load the Web application settings.");
        }

        // Load calendar
        _calendar = appSettings["calendar"];

        // Cache calendar
        RefreshCalendar("Calendar", null, CacheItemRemovedReason.Expired);
    }

    private List<CalendarService.Event> ReadCalendar()
    {
        if (!string.IsNullOrEmpty(_calendar))
        {
            // iCalendar date formats
            string utcDateTimeFormat = "yyyyMMddTHHmmssZ";
            string localDateTimeFormat = "yyyyMMddTHHmmss";
            string localDateFormat = "yyyyMMdd";
            CultureInfo culture = new CultureInfo("en-US");

            // Create syndication feed
            List<CalendarService.Event> events = new List<CalendarService.Event>();
            CalendarService.Event item = null;

            CalendarReader reader = new CalendarReader(_calendar);
            try
            {
                while (reader.Read())
                {
                    switch (reader.ContentLineType)
                    {
                        case ContentLineType.Component:
                            if ("VEVENT".Equals(reader.Name))
                            {
                                item = new CalendarService.Event();
                            }
                            break;
                        case ContentLineType.EndComponent:
                            if ("VEVENT".Equals(reader.Name))
                            {
                                events.Add(item);
                            }
                            break;
                        case ContentLineType.Property:
                            if (item != null)
                            {
                                if ("UID".Equals(reader.Name))
                                {
                                    item.Uid = reader.Value;
                                }
                                else if ("SUMMARY".Equals(reader.Name))
                                {
                                    item.Summary = reader.Value;
                                }
                                else if ("DESCRIPTION".Equals(reader.Name))
                                {
                                    string desc = reader.Value;
                                    item.Description = desc;

                                    // Windows Live Calendar does not generally specify
                                    // a URL in an Event so we will try to extract one
                                    // from the description, if it exists.
                                    Regex regex = new Regex(@"http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?");
                                    Match match = regex.Match(desc);
                                    if (match.Success)
                                    {
                                        item.Url = match.Value; // First one only
                                    }
                                }
                                else if ("CREATED".Equals(reader.Name))
                                {
                                    item.Created = DateTime.ParseExact(reader.Value, utcDateTimeFormat, culture);
                                }
                                else if ("LAST-MODIFIED".Equals(reader.Name))
                                {
                                    item.LastModified = DateTime.ParseExact(reader.Value, utcDateTimeFormat, culture);
                                }
                                else if ("DTSTART".Equals(reader.Name))
                                {
                                    DateTime dt;
                                    if (reader.Value.IndexOf('T') == -1)
                                        dt = DateTime.ParseExact(reader.Value, localDateFormat, culture);
                                    else
                                        dt = DateTime.ParseExact(reader.Value, localDateTimeFormat, culture);

                                    item.DateStart = dt;
                                }
                                else if ("DTEND".Equals(reader.Name))
                                {
                                    DateTime dt;
                                    if (reader.Value.IndexOf('T') == -1)
                                        dt = DateTime.ParseExact(reader.Value, localDateFormat, culture);
                                    else
                                        dt = DateTime.ParseExact(reader.Value, localDateTimeFormat, culture);

                                    item.DateEnd = dt;
                                }
                                else if ("LOCATION".Equals(reader.Name))
                                {
                                    item.Location = reader.Value;
                                }
                            }
                            break;
                    }
                }

                return events;
            }
            catch (Exception)
            {
            }
            finally
            {
                reader.Close();
            }
        }

        return null;
    }

    private void RefreshCalendar(String key, Object item, CacheItemRemovedReason reason)
    {
        List<CalendarService.Event> events;
        try
        {
            events = ReadCalendar();
        }
        catch (Exception)
        {
            events = null;
        }

        if (events != null)
        {
            HttpRuntime.Cache.Insert("Calendar", events, null,
                Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5),
                CacheItemPriority.Default,
                new CacheItemRemovedCallback(RefreshCalendar));
        }
        else if (events == null && item != null)
        {
            // If ReadCalendar failed for some reason but the previous
            // cache is not null, recache the previous value
            HttpRuntime.Cache.Insert("Calendar", item, null,
                Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5),
                CacheItemPriority.Default,
                new CacheItemRemovedCallback(RefreshCalendar));
        }
        else
        {
            throw new Exception("Error reading calendar");
        }
    }
}
