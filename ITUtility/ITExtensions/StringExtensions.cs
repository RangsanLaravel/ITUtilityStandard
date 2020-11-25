﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ITUtility
{
    public static class StringExtensions
    {
        #region " IsNullOrEmpty "
        public static bool IsNullOrEmpty(this string i)
        {
            return string.IsNullOrEmpty(i);
        }

        public static bool IsNotNullOrEmpty(this string i)
        {
            return !string.IsNullOrEmpty(i);
        }
        #endregion

        #region " ToNullableString "
        public static string ToNullableString(this long? i)
        {
            if (i == null || i == (long?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this int? i)
        {
            if (i == null || i == (int?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this short? i)
        {
            if (i == null || i == (short?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this decimal? i)
        {
            if (i == null || i == (decimal?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this double? i)
        {
            if (i == null || i == (double?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this DateTime? i)
        {
            if (i == null) //|| i == (DateTime?)null)
                return null;
            return i.ToString();
        }

        public static string ToNullableString(this DateTime? i, IFormatProvider provider)
        {
            if (i == null) //|| i == (DateTime?)null)
                return null;
            return (i ?? DateTime.Now).ToString(provider);
        }

        public static string ToNullableString(this DateTime? i, string format)
        {
            if (i == null) //|| i == (DateTime?)null)
                return null;
            return (i ?? DateTime.Now).ToString(format);
        }

        public static string ToNullableString(this DateTime? i, string format, IFormatProvider provider)
        {
            if (i == null) //|| i == (DateTime?)null)
                return null;
            return (i ?? DateTime.Now).ToString(format, provider);
        }

        public static string ToNullableString(this char? i)
        {
            if (i == null || i == (char?)null)
                return null;
            return i.ToString();
        }
        #endregion

        public static string TrimComments(this string i)
        {
            if (string.IsNullOrEmpty(i))
                return null;
            return Utility.StripComments(i);
        }

        public static bool IsNotNull(this string Input)
        {
            if (Input != null)
            {
                Input = Input.Trim();
                if (Input != "")
                {
                    if (!string.IsNullOrEmpty(Input) && !string.IsNullOrWhiteSpace(Input))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        
    }
}
