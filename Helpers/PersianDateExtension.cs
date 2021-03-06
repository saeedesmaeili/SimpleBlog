﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Helpers
{
    public static class PersianDateExtension
    {
        public static string GetPersianMounthName(this int mounth)
        {
            string MonthName = "فروردین";
            switch (mounth)
            {
                case 1: { MonthName = "فروردین"; break; }
                case 2: { MonthName = "اردیبهشت"; break; }
                case 3: { MonthName = "خرداد"; break; }
                case 4: { MonthName = "تیر"; break; }
                case 5: { MonthName = "مرداد"; break; }
                case 6: { MonthName = "شهریور"; break; }
                case 7: { MonthName = "مهر"; break; }
                case 8: { MonthName = "آبان"; break; }
                case 9: { MonthName = "آذر"; break; }
                case 10: { MonthName = "دی"; break; }
                case 11: { MonthName = "بهمن"; break; }
                case 12: { MonthName = "اسفند"; break; }
                default: { MonthName = "فروردین"; break; }
            }
            return MonthName;
        }
        public static int GetPersianYear(this DateTime date) {
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
            int Year = p.GetYear(date);
            return Year;
        }
        public static int GetPersianMonth(this DateTime date)
        {
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
            int Month = p.GetMonth(date); 
            return Month;
        }
        public static string GetPersianYearAndMounth(this DateTime date)
        {
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
            int Year = p.GetYear(date);
            int Month = p.GetMonth(date);
            int Day = p.GetDayOfMonth(date);
            return Year + "/" + Month;
        }
        public static string GetPersianDateString(DateTime? date)
        {
            try
            {


                string PersianDate = "";
                if (!date.HasValue)
                {
                    return PersianDate;
                }
                System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
                int Year = p.GetYear(date.Value);
                int Month = p.GetMonth(date.Value);
                int Day = p.GetDayOfMonth(date.Value);
                string MonthName = "فروردین";
                switch (Month)
                {
                    case 1: { MonthName = "فروردین"; break; }
                    case 2: { MonthName = "اردیبهشت"; break; }
                    case 3: { MonthName = "خرداد"; break; }
                    case 4: { MonthName = "تیر"; break; }
                    case 5: { MonthName = "مرداد"; break; }
                    case 6: { MonthName = "شهریور"; break; }
                    case 7: { MonthName = "مهر"; break; }
                    case 8: { MonthName = "آبان"; break; }
                    case 9: { MonthName = "آذر"; break; }
                    case 10: { MonthName = "دی"; break; }
                    case 11: { MonthName = "بهمن"; break; }
                    case 12: { MonthName = "اسفند"; break; }
                    default: { MonthName = "فروردین"; break; }
                }
                PersianDate = Day.ToString() + " " + MonthName + " " + Year.ToString();
                return PersianDate;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string GetPersianDateStringSlashed(DateTime? date)
        {
            string PersianDate = "";
            if (!date.HasValue)
            {
                return PersianDate;
            }
            System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
            int Year = p.GetYear(date.Value);
            int Month = p.GetMonth(date.Value);
            int Day = p.GetDayOfMonth(date.Value);

            PersianDate = Day.ToString() + "/" + Month.ToString() + "/" + Year.ToString();
            return PersianDate;
        }
        public static string GetPersianDateTimeString(DateTime? date)
        {
            try
            {


                string PersianDate = "";
                if (!date.HasValue)
                {
                    return PersianDate;
                }
                System.Globalization.PersianCalendar p = new System.Globalization.PersianCalendar();
                int Year = p.GetYear(date.Value);
                int Month = p.GetMonth(date.Value);
                int Day = p.GetDayOfMonth(date.Value);
                int Hour = p.GetHour(date.Value);
                int Minutes = p.GetMinute(date.Value);
                int Second = p.GetSecond(date.Value);
                string MonthName = "فروردین";
                switch (Month)
                {
                    case 1: { MonthName = "فروردین"; break; }
                    case 2: { MonthName = "اردیبهشت"; break; }
                    case 3: { MonthName = "خرداد"; break; }
                    case 4: { MonthName = "تیر"; break; }
                    case 5: { MonthName = "مرداد"; break; }
                    case 6: { MonthName = "شهریور"; break; }
                    case 7: { MonthName = "مهر"; break; }
                    case 8: { MonthName = "آبان"; break; }
                    case 9: { MonthName = "آذر"; break; }
                    case 10: { MonthName = "دی"; break; }
                    case 11: { MonthName = "بهمن"; break; }
                    case 12: { MonthName = "اسفند"; break; }
                    default: { MonthName = "فروردین"; break; }
                }
                PersianDate = Day.ToString() + " " + MonthName + " " + Year.ToString() + " ساعت " + Hour + ":" + Minutes + ":" + Second;
                return PersianDate;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}