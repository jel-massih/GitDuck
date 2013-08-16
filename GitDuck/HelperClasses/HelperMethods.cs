using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitDuck.HelperClasses
{
    public class HelperMethods
    {
        public static String FormatTimeOffset(TimeSpan timeOffset)
        {
            String time = "";

            if (timeOffset.Days != 0)
            {
                if (timeOffset.Days > 368)
                {
                    if (timeOffset.Days / 368 > 1)
                    {
                        time = timeOffset.Days / 368 + " years ago";
                    }
                    else
                    {
                        time = timeOffset.Days / 368 + " year ago";
                    }

                }
                else if (timeOffset.Days > 1)
                {
                    time = timeOffset.Days + " days ago";
                }
                else
                {
                    time = timeOffset.Days + " day ago";
                }
            }
            else if (timeOffset.Hours != 0)
            {
                if (timeOffset.Hours > 1)
                {
                    time = timeOffset.Hours + " hours ago";
                }
                else
                {
                    time = timeOffset.Hours + " hour ago";
                }
            }
            else if (timeOffset.Minutes != 0)
            {
                if (timeOffset.Minutes > 1)
                {
                    time = timeOffset.Minutes + " minutes ago";
                }
                else
                {
                    time = timeOffset.Minutes + " minute ago";
                }
            }

            return time;
        }

        public static DateTime GitHubDateToDateTime(string timestamp)
        {
            timestamp = timestamp.Replace("T", " ");
            timestamp = timestamp.Replace("Z", "");
            string pattern = "yyyy-MM-dd HH:mm:ss";
            DateTime parsedDate;
            if (DateTime.TryParseExact(timestamp, pattern, null, System.Globalization.DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }
            else
            {
                return DateTime.Now;
            }
        }

        public static String GetMonth(int index)
        {
            string[] monthArr = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            if (index >= 12)
            {
                index = 11;
            }
            else if (index < 0)
            {
                index = 0;
            }
            return monthArr[index];
        }
    }
}
