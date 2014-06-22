using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vintage.Rabbit.Web.Areas.Emails
{
    public static class Styles
    {
        public static string Font = "font-family: Arial, Helvetica, sans-serif;";
        public static string Text = Font + " font-size: 15px; color: #333333; font-weight: normal; line-height: 24px;";

        public static string H2 = "font-size: 16px; font-weight: bold; " + Font + " text-decoration: none; color: #333333; line-height: 24px;";
        public static string P = Text + " margin-top: 10px; margin-bottom: 10px;";

        public static string A = "font-size: 15px; " + Font + " line-height: 24px; vertical-align: top; padding:0; margin:0; text-decoration: none; color: #0078C5; font-weight: bold;";

        public static string Small = "font-size: 13px; color: #333333; font-weight: normal; " + Font + " line-height: 18px; vertical-align: top; padding:0; margin:0; text-decoration: none;";

        public static string List = "margin: 0px; padding: 0px;";
        public static string ListItem = "font-size: 15px; color: #333333; font-weight: normal; " + Font + " line-height: 24px; vertical-align: top; padding: 0px; margin: 0px; text-decoration: none;";

        public static string Light = "color: #999999;";
        public static string Black = "color: #000000;";

        public static class Alert
        {
            public static string Warning = "font-size: 15px; color: #31708f; line-height: 24px; " + Font;
            public static string Positive = "font-size: 15px; color: #3c763d; line-height: 24px; " + Font;
            public static string Security = "font-size: 15px; color: #333333; line-height: 24px; " + Font;
        }
    }
}