﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITUtility
{
    public static class ObjectExtensions
    {
        public static bool IsNumeric(this object i)
        {
            if (i == null || i is DateTime)
                return false;

            if (i is Boolean    /* bool */ ||
                i is Byte       /* byte */ ||
                i is Int16      /* short */ ||
                i is Int32      /* int */ ||
                i is Int64      /* long */ ||
                i is Single     /* float */ ||
                i is Double     /* double */  ||
                i is Decimal    /* decimal */)
                return true;

            try
            {
                if (i is string)
                    Decimal.Parse(i as string);
                else
                    Decimal.Parse(i.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }

        public static bool IsNullOrDBNull(this object i)
        {
            if (i == null || i is DBNull)
                return true;
            return false;
        }

        public static bool IsNullOrEmpty(this object i)
        {
            if (i == null || (i is string && string.IsNullOrEmpty(i as string)))
                return true;
            return false;
        }

        public static bool IsNullOrDBNullOrEmpty(this object i)
        {
            if (i.IsNullOrDBNull())
                return true;
            if (i is string)
                return string.IsNullOrEmpty(i as string);
            return false;
        }



        public static T IfNullReturn<T>(this T i, T ReturnValue)
        {
            if (i == null)
                return ReturnValue;
            return i;
        }

        public static T IfNullOrEmptyReturn<T>(this T i, T ReturnValue)
        {
            if (i.IsNullOrEmpty())
                return ReturnValue;
            return i;
        }

        public static T IfNullOrDBNullReturn<T>(this T i, T ReturnValue)
        {
            if (i.IsNullOrDBNull())
                return ReturnValue;
            return i;
        }

        public static T IfNullOrDBNullOrEmptyReturn<T>(this T i, T ReturnValue)
        {
            if (i.IsNullOrDBNullOrEmpty())
                return ReturnValue;
            return i;
        }



        public static string SerializerToJsonString<T>(this T i)
        {
            if (i.IsNullOrDBNullOrEmpty())
                return "{}";
            return JsonConvert.SerializeObject(i);
        }

        public static T DeserializeToObject<T>(this string i)
        {
            if (i.IsNullOrDBNullOrEmpty())
                return default(T);
            return JsonConvert.DeserializeObject<T>(i);
        }
    }
}
