using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ITUtility
{
    public static partial class Utility
    {
        public static string ConditionInOrEqual(string whereColumn, object[] datas, List<DBParameter> param)
        {
            string condition, param_name = whereColumn;
            whereColumn = whereColumn.Trim();
            if (whereColumn.IndexOf(' ') > -1)
            {
                throw new Exception("ConditionInOrEqual : column " + whereColumn + " ไม่สามารถมีช่องว่างได้");
            }
            if (whereColumn.IndexOf(".") > -1)
            {
                param_name = whereColumn.Replace('.', '_');
            }
            if (datas.Count() > 1)
            {
                string sIds = "";
                foreach (object id in datas)
                {
                    if (sIds != "")
                        sIds = sIds + ", ";
                    sIds = sIds + Utility.SQLValueString(id);
                }

                condition = whereColumn + " IN (" + sIds + ")\n";
            }
            else
            {

                condition = whereColumn + " = :v" + param_name;

                Utility.SQLValueString(param, "v" + param_name, datas.FirstOrDefault());
            }

            return condition;
        }
        public static string ConditionInOrEqual(string whereColumn, long?[] datas, List<DBParameter> param)
        {
            string condition, param_name = whereColumn;
            whereColumn = whereColumn.Trim();
            if (whereColumn.IndexOf(' ') > -1)
            {
                throw new Exception("ConditionInOrEqual : column " + whereColumn + " ไม่สามารถมีช่องว่างได้");
            }
            if (whereColumn.IndexOf(".") > -1)
            {
                param_name = whereColumn.Replace('.', '_');
            }
            if (datas.Count() > 1)
            {
                string sIds = "";
                foreach (object id in datas)
                {
                    if (sIds != "")
                        sIds = sIds + ", ";
                    sIds = sIds + Utility.SQLValueString(id);
                }

                condition = whereColumn + " IN (" + sIds + ")\n";
            }
            else
            {

                condition = whereColumn + " = :v" + param_name;

                Utility.SQLValueString(param, "v" + param_name, datas.FirstOrDefault());
            }

            return condition;
        }

        public static long? GetDBInt64Value(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!input.IsNumeric())
                return null;
            return Convert.ToInt64(input);
        }

        public static long? GetDBInt64Value(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBInt64Value(dr[fieldName]);
        }

        public static string GetDBStringValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!(input is string))
                return null;
            if (!string.IsNullOrEmpty((string)input))
                return ((string)input).Trim();
            return (string)input;
        }

        public static string GetDBStringValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBStringValue(dr[fieldName]);
        }

        public static DateTime? GetDBDateTimeValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!(input is DateTime))
                return null;
            return (DateTime)input;
        }        

        public static DateTime? GetDBDateTimeValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBDateTimeValue(dr[fieldName]);
        }

        public static decimal? GetDBDecimalValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!input.IsNumeric())
                return null;
            return Decimal.Parse(input.ToString(), CultureInfo.CreateSpecificCulture("th-TH"));
        }

        public static char? GetDBCharValue(object input)
        {
            if (input.IsNullOrDBNull())
                return null;
            if (!(input is string))
                return null;
            if (string.IsNullOrEmpty((string)input))
                return null;
            return ((string)input).ToCharArray()[0];
        }

        public static String SQLValueString(Object input)
        {
            String returnStr = "";
            if (input == null)
            {
                returnStr = "Null";
            }
            else if (input is DBNull)
            {
                returnStr = "Null";
            }
            else
            {
                if (input is String)
                {

                    if ((String)input == "")
                    {
                        returnStr = "Null";
                    }
                    else
                    {
                        returnStr = "'" + ((String)input).Replace("'", "''") + "'";
                    }
                }
                else if (input is Char)
                {
                    Char rtnChr = (Char)input;
                    String rtnStr = rtnChr.ToString();
                    rtnStr = rtnStr.Trim();
                    if (rtnStr == "")
                    {
                        returnStr = "Null";
                    }
                    else
                    {
                        returnStr = "'" + rtnStr.Replace("'", "''") + "'";
                    }
                }
                else if (input is Int16)
                {
                    returnStr = ((Int16)input).ToString();
                }
                else if (input is Int32)
                {
                    returnStr = ((Int32)input).ToString();
                }
                else if (input is Int64)
                {
                    returnStr = ((Int64)input).ToString();
                }
                else if (input is Double)
                {
                    returnStr = ((Double)input).ToString();
                }
                else if (input is Decimal)
                {
                    returnStr = ((Decimal)input).ToString();
                }
                else if (input is Single)
                {
                    returnStr = ((Single)input).ToString();
                }
                else if (input is DateTime)
                {
                    returnStr = SQLDate((DateTime)input);
                }
                else
                {
                    returnStr = input.ToString();
                }
            }
            return returnStr;
        }

        public static String SQLValueStringN(Object input)
        {
            String returnStr = "";
            if (input == null)
            {
                returnStr = "Null";
            }
            else if (input is DBNull)
            {
                returnStr = "Null";
            }
            else
            {
                if (input is String)
                {

                    if ((String)input == "")
                    {
                        returnStr = "Null";
                    }
                    else
                    {
                        var asciiBytesCount = Encoding.ASCII.GetByteCount((String)input);
                        var unicodBytesCount = Encoding.UTF8.GetByteCount((String)input);
                        returnStr = "'" + ((String)input).Replace("'", "''") + "'";
                        if (asciiBytesCount == unicodBytesCount)
                        {
                            returnStr = "'" + ((String)input).Replace("'", "''") + "'";
                        }
                        else
                        {
                            returnStr = "N'" + ((String)input).Replace("'", "''") + "'";
                        }
                    }
                }
                else if (input is Char)
                {
                    Char rtnChr = (Char)input;
                    String rtnStr = rtnChr.ToString();
                    rtnStr = rtnStr.Trim();
                    if (rtnStr == "")
                    {
                        returnStr = "Null";
                    }
                    else
                    {

                        returnStr = "N'" + rtnStr.Replace("'", "''") + "'";
                        var asciiBytesCount = Encoding.ASCII.GetByteCount(rtnStr);
                        var unicodBytesCount = Encoding.UTF8.GetByteCount(rtnStr);

                        if (asciiBytesCount == unicodBytesCount)
                        {
                            returnStr = "'" + rtnStr.Replace("'", "''") + "'";
                        }
                        else
                        {
                            returnStr = "N'" + rtnStr.Replace("'", "''") + "'";
                        }


                    }
                }
                else if (input is Int16)
                {
                    returnStr = ((Int16)input).ToString();
                }
                else if (input is Int32)
                {
                    returnStr = ((Int32)input).ToString();
                }
                else if (input is Int64)
                {
                    returnStr = ((Int64)input).ToString();
                }
                else if (input is Double)
                {
                    returnStr = ((Double)input).ToString();
                }
                else if (input is Decimal)
                {
                    returnStr = ((Decimal)input).ToString();
                }
                else if (input is Single)
                {
                    returnStr = ((Single)input).ToString();
                }
                else if (input is DateTime)
                {
                    returnStr = SQLDate((DateTime)input);
                }
                else
                {
                    returnStr = input.ToString();
                }
            }
            return returnStr;
        }

        public static void SQLValueString(List<DBParameter> paramList, string name, object input)
        {
            string returnStr = "";
            if (input == null)
            {
                returnStr = "Null";
                paramList.Add(new DBParameter(name, DBNull.Value, OracleDbType.Varchar2));
            }
            else if (input is DBNull)
            {
                returnStr = "Null";
                paramList.Add(new DBParameter(name, DBNull.Value, OracleDbType.Varchar2));

            }
            else
            {
                if (input is string)
                {

                    if ((string)input == "")
                    {
                        returnStr = "Null";
                        paramList.Add(new DBParameter(name, DBNull.Value, OracleDbType.Varchar2));
                    }
                    else
                    {
                        returnStr = "'" + ((string)input).Replace("'", "''") + "'";

                        paramList.Add(new DBParameter(name, input, OracleDbType.Varchar2));
                    }
                }
                else if (input is Char)
                {
                    Char rtnChr = (Char)input;
                    string rtnStr = rtnChr.ToString();
                    rtnStr = rtnStr.Trim();
                    if (rtnStr == "")
                    {
                        returnStr = "Null";
                    }
                    else
                    {
                        returnStr = "'" + rtnStr.Replace("'", "''") + "'";
                        paramList.Add(new DBParameter(name, input, OracleDbType.Char));
                    }
                }
                else if (input is Int16)
                {
                    returnStr = ((Int16)input).ToString();

                    paramList.Add(new DBParameter(name, input, OracleDbType.Int16));
                }
                else if (input is Int32)
                {
                    returnStr = ((Int32)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Int32));
                }
                else if (input is Int64)
                {
                    returnStr = ((Int64)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Int64));
                }
                else if (input is Double)
                {
                    returnStr = ((Double)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Double));
                }
                else if (input is Decimal)
                {
                    returnStr = ((Decimal)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Decimal));
                }
                else if (input is Single)
                {
                    returnStr = ((Single)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Single));
                }
                else if (input is DateTime)
                {
                    returnStr = SQLDate((DateTime)input);
                    paramList.Add(new DBParameter(name, input, OracleDbType.Date));
                }
                else
                {
                    returnStr = input.ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Varchar2));
                }
            }
        }

        public static void SQLValueString(List<DBParameter> paramList, string name, object input, int size = 50)
        {
            string returnStr = "";
            if (input == null)
            {
                returnStr = "Null";
                paramList.Add(new DBParameter(name, DBNull.Value, OracleDbType.Varchar2));
            }
            else if (input is DBNull)
            {
                returnStr = "Null";
                paramList.Add(new DBParameter(name, DBNull.Value, OracleDbType.Varchar2));

            }
            else
            {
                if (input is string)
                {

                    if ((string)input == "")
                    {
                        returnStr = "Null";
                        paramList.Add(new DBParameter(name, DBNull.Value, OracleDbType.Varchar2));
                    }
                    else
                    {
                        returnStr = "'" + ((string)input).Replace("'", "''") + "'";

                        paramList.Add(new DBParameter(name, input, OracleDbType.Varchar2, size));
                    }
                }
                else if (input is Char)
                {
                    Char rtnChr = (Char)input;
                    string rtnStr = rtnChr.ToString();
                    rtnStr = rtnStr.Trim();
                    if (rtnStr == "")
                    {
                        returnStr = "Null";
                    }
                    else
                    {
                        returnStr = "'" + rtnStr.Replace("'", "''") + "'";
                        paramList.Add(new DBParameter(name, input, OracleDbType.Char, size));
                    }
                }
                else if (input is Int16)
                {
                    returnStr = ((Int16)input).ToString();

                    paramList.Add(new DBParameter(name, input, OracleDbType.Int16, size));
                }
                else if (input is Int32)
                {
                    returnStr = ((Int32)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Int32, size));
                }
                else if (input is Int64)
                {
                    returnStr = ((Int64)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Int64, size));
                }
                else if (input is Double)
                {
                    returnStr = ((Double)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Double, size));
                }
                else if (input is Decimal)
                {
                    returnStr = ((Decimal)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Decimal, size));
                }
                else if (input is Single)
                {
                    returnStr = ((Single)input).ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Single, size));
                }
                else if (input is DateTime)
                {
                    returnStr = SQLDate((DateTime)input);
                    paramList.Add(new DBParameter(name, input, OracleDbType.Date, size));
                }
                else
                {
                    returnStr = input.ToString();
                    paramList.Add(new DBParameter(name, input, OracleDbType.Varchar2, size));
                }
            }

        }

        public static string SQLValueInString(Array inputs)
        {
            if (inputs == null || inputs.Length == 0)
                return "('')";
            string result = "(";
            foreach (object input in inputs)
                if (result.Length == 1)
                    result = result + SQLValueString(input);
                else
                    result = result + "," + SQLValueString(input);

            result = result + ")";

            return result;
        }

        public static string SQLDate(DateTime refDate)
        {
            if (refDate == new DateTime())
                return "Null";

            return string.Format("TO_DATE('{0}-{1}-{2} {3}:{4}:{5}','RRRR-MM-DD HH24:MI:SS', 'NLS_CALENDAR=''GREGORIAN''')"
                , refDate.Year.ToString()
                , refDate.Month.ToString().PadLeft(2, '0')
                , refDate.Day.ToString().PadLeft(2, '0')
                , refDate.Hour.ToString().PadLeft(2, '0')
                , refDate.Minute.ToString().PadLeft(2, '0')
                , refDate.Second.ToString().PadLeft(2, '0')
                );
        }

        public static string SQLDate(DateTime? refDate)
        {
            if (refDate != null)
                return SQLDate(refDate.Value);
            else
                return "Null";
        }

        public static char? GetDBCharValue(DataRow dr, string fieldName)
        {
            if (!DataRowHasValue(dr, fieldName))
                return null;
            return GetDBCharValue(dr[fieldName]);
        }

        public static bool DataRowHasValue(DataRow dr, string columnName)
        {
            if (dr == null || !dr.Table.Columns.Contains(columnName) || dr.IsNull(columnName))
                return false;
            return true;
        }

        public static DataTable FillDataTable(OracleCommand oCmd)
        {
            using (OracleDataReader oReader = oCmd.ExecuteReader())
            {
                using (DataTable dataTable = new DataTable())
                {
                    dataTable.Load(oReader);
                    oReader.Close();
                    return dataTable;
                }
            }
        }
        public static async Task<DataTable> FillDataTableAsync(OracleCommand oCmd)
        {
            using (var oReader =await oCmd.ExecuteReaderAsync())
            {
                using (DataTable dataTable = new DataTable())
                {
                    dataTable.Load(oReader);
                    oReader.Close();
                    return dataTable;
                }
            }
        }

        public static DataTable FillDataTable(string sqlStr, OracleConnection connection)
        {
            OracleDataAdapter oAdpt = new OracleDataAdapter(sqlStr, connection);
            DataTable dt = new DataTable();
            oAdpt.Fill(dt);
            oAdpt.Dispose();
            return dt;
        }

        public static DataTable FillDataTable(string sqlStr, OracleConnection connection, List<DBParameter> param = null)
        {
            OracleCommand cmd = new OracleCommand(sqlStr, connection);

            DataTable dt = new DataTable();


            cmd.Parameters.Clear();
            cmd.BindByName = true;
            if (param != null && param.Any())
            {
                foreach (DBParameter item in param)
                {
                    cmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
                }
            }

            using (OracleDataReader reader = cmd.ExecuteReader())
            {

                dt.Load(reader);

            }


            cmd.Dispose();


            return dt;
        }
        public static async Task<DataTable> FillDataTableAsync(string sqlStr, OracleConnection connection, List<DBParameter> param = null)
        {
            OracleCommand cmd = new OracleCommand(sqlStr, connection);

            DataTable dt = new DataTable();


            cmd.Parameters.Clear();
            cmd.BindByName = true;
            if (param != null && param.Any())
            {
                foreach (DBParameter item in param)
                {
                    cmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
                }
            }

            using (var reader =await cmd.ExecuteReaderAsync())
            {

                dt.Load(reader);

            }


            cmd.Dispose();


            return dt;
        }
        public static T[] FillDataTable<T>(string sqlStr, OracleConnection connection) where T : new()
        {
            OracleDataAdapter oAdpt = new OracleDataAdapter(sqlStr, connection);
            DataTable dt = new DataTable();
            IEnumerable<T> result;
            oAdpt.Fill(dt);
            oAdpt.Dispose();
            result = dt.AsEnumerable<T>();
            dt.Dispose();
            return result.ToArray();
        }

        public static T[] FillDataTable<T>(string sqlStr, OracleConnection connection, List<DBParameter> param) where T : new()
        {
            OracleCommand cmd = new OracleCommand(sqlStr, connection);

            DataTable dt = new DataTable();
            IEnumerable<T> result;

            if (param != null && param.Any())
            {
                cmd.Parameters.Clear();
                cmd.BindByName = true;
                foreach (DBParameter item in param)
                {
                    cmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;

                }
            }
            //  cmd.Parameters.AddRange(param);

            using (OracleDataReader reader = cmd.ExecuteReader())
            {

                dt.Load(reader);

            }

            cmd.Dispose();

            result = dt.AsEnumerable<T>();

            dt.Dispose();

            return result.ToArray();
        }

        public static async Task<T[]> FillDataTableAsync<T>(string sqlStr, OracleConnection connection, List<DBParameter> param) where T : new()
        {
            OracleCommand cmd = new OracleCommand(sqlStr, connection);

            DataTable dt = new DataTable();
            IEnumerable<T> result;

            if (param != null && param.Any())
            {
                cmd.Parameters.Clear();
                cmd.BindByName = true;
                foreach (DBParameter item in param)
                {
                    cmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;

                }
            }
            //  cmd.Parameters.AddRange(param);

            using (var reader =await cmd.ExecuteReaderAsync())
            {

                dt.Load(reader);

            }

            cmd.Dispose();

            result = dt.AsEnumerable<T>();

            dt.Dispose();

            return result.ToArray();
        }

        public static async Task<object> ExecuteScalarAsync(OracleConnection conn, string sqlStr)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);

            object returnValue =await oCmd.ExecuteScalarAsync();
            oCmd.Dispose();
            return returnValue;
        }

        public static async Task<object> ExecuteScalarAsync(OracleConnection conn, string sqlStr, List<DBParameter> DBParams)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);

            if (DBParams != null && DBParams.Any())
            {
                oCmd.Parameters.Clear();
                oCmd.BindByName = true;
                foreach (DBParameter item in DBParams)
                {
                    oCmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
                }
            }
            object returnValue =await oCmd.ExecuteScalarAsync();
            oCmd.Dispose();
            return returnValue;
        }

        public static async Task<int> ExecuteNonQueryAsync(OracleConnection conn, string sqlStr)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);
            int i =await oCmd.ExecuteNonQueryAsync();
            oCmd.Dispose();
            return i;
        }

        public static async Task<int> ExecuteNonQueryAsync(string sqlStr, OracleConnection conn, List<DBParameter> DBParams)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);

            if (DBParams != null && DBParams.Any())
            {
                oCmd.Parameters.Clear();
                oCmd.BindByName = true;
                foreach (DBParameter item in DBParams)
                {
                    oCmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
                }
            }
            int i = await oCmd.ExecuteNonQueryAsync();
            oCmd.Dispose();
            return i;
        }
        public static object ExecuteScalar(OracleConnection conn, string sqlStr)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);

            object returnValue = oCmd.ExecuteScalar();
            oCmd.Dispose();
            return returnValue;
        }

        public static object ExecuteScalar(OracleConnection conn, string sqlStr, List<DBParameter> DBParams)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);

            if (DBParams != null && DBParams.Any())
            {
                oCmd.Parameters.Clear();
                oCmd.BindByName = true;
                foreach (DBParameter item in DBParams)
                {
                    oCmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
                }
            }
            object returnValue = oCmd.ExecuteScalar();
            oCmd.Dispose();
            return returnValue;
        }

        public static int ExecuteNonQuery(OracleConnection conn, string sqlStr)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);
            int i = oCmd.ExecuteNonQuery();
            oCmd.Dispose();
            return i;
        }

        public static int ExecuteNonQuery(string sqlStr, OracleConnection conn, List<DBParameter> DBParams)
        {
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);

            if (DBParams != null && DBParams.Any())
            {
                oCmd.Parameters.Clear();
                oCmd.BindByName = true;
                foreach (DBParameter item in DBParams)
                {
                    oCmd.Parameters.Add(new OracleParameter(item.NAME, item.TYPE)).Value = item.VALUE;
                }
            }
            int i = oCmd.ExecuteNonQuery();
            oCmd.Dispose();
            return i;
        }

        public static string StripComments(string code)
        {
            var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
            return System.Text.RegularExpressions.Regex.Replace(code, re, "$1");
        }

        public static string dateTimeToString(DateTime input)
        {
            String returnStr = "";
            returnStr = input.Day.ToString() + "/" + input.Month.ToString() + "/" + input.Year.ToString() + " " + input.Hour.ToString() + ":" + input.Minute.ToString() + ":" + input.Second.ToString() + "." + input.Millisecond.ToString();
            return returnStr;
        }

        public static string dateTimeToString(DateTime? inputDate, string format, string era)
        {
            if (inputDate == null) return "";
            else return dateTimeToString(inputDate.Value, format, era);
        }

        public static string dateTimeToString(DateTime inputDate, string format, string era)
        {
            DateTime dateIn = inputDate;

            List<string> innerLs = new List<string>();
            char[] cs = format.ToCharArray();
            List<char> lc = new List<char>();
            List<char> innerLc = null;

            bool inner = false;

            foreach (char c in cs)
            {
                if (!inner)
                {
                    if (c != '[')
                    {
                        lc.Add(c);
                    }
                    else
                    { //inner start
                        inner = true;
                        innerLc = new List<char>();
                        lc.AddRange(("[" + innerLs.Count().ToString() + "]").ToCharArray());
                    }
                }
                else
                { //inner zone
                    if (c == ']')
                    {//inner end
                        innerLs.Add(new string(innerLc.ToArray()));
                        inner = false;
                        innerLc = null;
                    }
                    else
                    {
                        innerLc.Add(c);
                    }
                }
            }
            if (innerLc != null)
            {
                innerLs.Add(new string(innerLc.ToArray()));
            }

            string s1 = new string(lc.ToArray());
            string s2 = oldDateTimeToString(dateIn, s1, era);
            foreach (string innerS in innerLs)
            {
                s2 = s2.Replace("[" + innerLs.IndexOf(innerS).ToString() + "]", innerS);
            }
            return s2;
        }

        private static string oldDateTimeToString(DateTime inputDate, string format, string era)
        {
            string returnStr = "";

            if (format == "o")
                return inputDate.ToString("o");

            if (era == null) era = "";
            bool AD_era = true;
            string ERA = era.ToUpper();
            if (ERA == "BU" || ERA == "B.U." || ERA == "B.U")
                AD_era = false;


            string sDay = inputDate.Day.ToString().PadLeft(2, '0');
            string sMonth = inputDate.Month.ToString().PadLeft(2, '0');
            string sYear;
            if (!AD_era)
                sYear = (inputDate.Year + 543).ToString().PadLeft(4, '0');
            else
                sYear = inputDate.Year.ToString().PadLeft(4, '0');
            string sHour = inputDate.Hour.ToString().PadLeft(2, '0');
            string sMinute = inputDate.Minute.ToString().PadLeft(2, '0');
            string sSecond = inputDate.Second.ToString().PadLeft(2, '0');
            string sMilliSecond = inputDate.Millisecond.ToString().PadLeft(2, '0');
            #region :: Full Thai Month ::
            string tMonthName = "";
            switch (inputDate.Month)
            {
                case 1:
                    tMonthName = "มกราคม";
                    break;
                case 2:
                    tMonthName = "กุมภาพันธ์";
                    break;
                case 3:
                    tMonthName = "มีนาคม";
                    break;
                case 4:
                    tMonthName = "เมษายน";
                    break;
                case 5:
                    tMonthName = "พฤษภาคม";
                    break;
                case 6:
                    tMonthName = "มิถุนายน";
                    break;
                case 7:
                    tMonthName = "กรกฎาคม";
                    break;
                case 8:
                    tMonthName = "สิงหาคม";
                    break;
                case 9:
                    tMonthName = "กันยายน";
                    break;
                case 10:
                    tMonthName = "ตุลาคม";
                    break;
                case 11:
                    tMonthName = "พฤศจิกายน";
                    break;
                case 12:
                    tMonthName = "ธันวาคม";
                    break;
            }
            #endregion
            #region :: Short Thai Month ::
            string tShortMonthName = "";
            switch (inputDate.Month)
            {
                case 1:
                    tShortMonthName = "ม.ค.";
                    break;
                case 2:
                    tShortMonthName = "ก.พ.";
                    break;
                case 3:
                    tShortMonthName = "มี.ค.";
                    break;
                case 4:
                    tShortMonthName = "เม.ย.";
                    break;
                case 5:
                    tShortMonthName = "พ.ค.";
                    break;
                case 6:
                    tShortMonthName = "มิ.ย.";
                    break;
                case 7:
                    tShortMonthName = "ก.ค.";
                    break;
                case 8:
                    tShortMonthName = "ส.ค.";
                    break;
                case 9:
                    tShortMonthName = "ก.ย.";
                    break;
                case 10:
                    tShortMonthName = "ต.ค.";
                    break;
                case 11:
                    tShortMonthName = "พ.ย.";
                    break;
                case 12:
                    tShortMonthName = "ธ.ค.";
                    break;
            }
            #endregion

            #region :: Set Format ::
            int index = 0;
            List<string> listDateTimeValue = new List<string>();
            #region :: Day ::

            if (format.IndexOf("dddd") > -1)
            {
                format = format.Replace("dddd", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("dddd", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("dddd", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            if (format.IndexOf("ddd") > -1)
            {
                format = format.Replace("ddd", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("ddd", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("ddd", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            if (format.IndexOf("dd") > -1)
            {
                format = format.Replace("dd", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sDay);
                index++;
            }
            if (format.IndexOf("d") > -1)
            {
                format = format.Replace("d", "{" + index.ToString() + "}");
                listDateTimeValue.Add(inputDate.Day.ToString());
                index++;
            }
            #endregion
            #region :: Month ::
            if (format.IndexOf("MMMM") > -1)
            {
                format = format.Replace("MMMM", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("MMMM", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("MMMM", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            if (format.IndexOf("MMM") > -1)
            {
                format = format.Replace("MMM", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("MMM", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("MMM", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            if (format.IndexOf("MM") > -1)
            {
                format = format.Replace("MM", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sMonth);
                index++;
            }
            if (format.IndexOf("Month") > -1)
            {
                format = format.Replace("Month", "{" + index.ToString() + "}");
                listDateTimeValue.Add(tMonthName);
                index++;
            }
            if (format.IndexOf("M") > -1)
            {
                format = format.Replace("M", "{" + index.ToString() + "}");
                listDateTimeValue.Add(inputDate.Month.ToString());
                index++;
            }
            if (format.IndexOf("m.m.") > -1)
            {
                format = format.Replace("m.m.", "{" + index.ToString() + "}");
                listDateTimeValue.Add(tShortMonthName);
                index++;
            }
            #endregion
            #region :: Year ::
            if (format.IndexOf("yyyy") > -1)
            {
                format = format.Replace("yyyy", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("yyyy", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("yyyy", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            if (format.IndexOf("yyy") > -1)
            {

            }
            if (format.IndexOf("yy") > -1)
            {
                format = format.Replace("yy", "{" + index.ToString() + "}");
                if (!AD_era)
                {
                    listDateTimeValue.Add(inputDate.ToString("yy", CultureInfo.GetCultureInfo("th-TH")));
                }
                else
                {
                    listDateTimeValue.Add(inputDate.ToString("yy", CultureInfo.GetCultureInfo("en-US")));
                }
                index++;
            }
            #endregion
            #region :: Hour ::
            if (format.IndexOf("hh") > -1)
            {
                format = format.Replace("hh", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sHour);
                index++;
            }
            #endregion
            #region :: Minute ::
            if (format.IndexOf("mi") > -1)
            {
                format = format.Replace("mi", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sMinute);
                index++;
            }
            #endregion
            #region :: Second ::
            if (format.IndexOf("ss") > -1)
            {
                format = format.Replace("ss", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sSecond);
                index++;
            }
            #endregion
            #region :: Millisecond ::
            if (format.IndexOf("ms") > -1)
            {
                format = format.Replace("ms", "{" + index.ToString() + "}");
                listDateTimeValue.Add(sMilliSecond);
                index++;
            }
            #endregion
            #region :: Replace Format ::
            returnStr = format;
            if (listDateTimeValue != null && listDateTimeValue.Any())
            {
                for (int i = 0; i < listDateTimeValue.Count; i++)
                {
                    returnStr = returnStr.Replace("{" + i.ToString() + "}", listDateTimeValue[i]);
                }
            }
            #endregion
            #endregion
            return returnStr;
        }

        public static DateTime? ConvertStringToDateTime(string strDatetime, string outFormat = "dd/MM/yyyy HH.mm.ss")
        {
            string[] formats = getFormatDateTime();
            if (strDatetime.IsNotNull())
            {
                if (string.IsNullOrEmpty(outFormat))
                    outFormat = "dd/MM/yyyy HH.mm.ss";

                DateTime newDT;
                if (DateTime.TryParseExact(strDatetime, formats,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out newDT))
                {
                    newDT.ToString(outFormat, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                }
                else
                {
                    newDT = new DateTime();
                }
                return newDT;
            }
            else
            {
                return null;
            }
        }

        public static DateTime? ConvertStringToDate(string strDatetime, string outFormat = "dd/MM/yyyy")
        {
            string[] formats = getFormatDateTime();

            if (strDatetime.IsNotNull())
            {
                if (string.IsNullOrEmpty(outFormat))
                    outFormat = "dd/MM/yyyy";
                DateTime newDT;
                if (DateTime.TryParseExact(strDatetime, formats,
                                       System.Globalization.CultureInfo.InvariantCulture,
                                       System.Globalization.DateTimeStyles.None,
                                       out newDT))
                {
                    newDT.ToString(outFormat, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                }
                else
                {
                    newDT = new DateTime();
                }

                return newDT;
            }
            else
            {
                return null;
            }
        }
        public static DateTime? ConvertDateStrToDateTime(string data_str, CultureInfo currentCulture, string format = "dd/MM/yyyy")
        {


            //CultureInfo currentCulture = new System.Globalization.CultureInfo("th-th");
            DateTime dateValue;
            if (DateTime.TryParseExact(data_str, format, currentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
            {

            }
            else
            {

                return null;
            }

            return dateValue;
        }
        
        public static DateTime? ConvertDateStrToDateTimeTHA(string data_str, string format = "dd/MM/yyyy")
        {
            CultureInfo currentCulture = new System.Globalization.CultureInfo("th-th");
            //CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            DateTime? dateValue = ConvertDateStrToDateTime(data_str, format);
            DateTime dateValueTH;

            if (dateValue == null) return dateValue;

            if (DateTime.TryParseExact(dateValue.Value.ToString(format, currentCulture), format, currentCulture, System.Globalization.DateTimeStyles.None, out dateValueTH))
            {

            }
            else
            {

                return null;
            }


            return dateValueTH;
        }
        public static DateTime? ConvertDateStrToDateTime(string data_str, string format = "dd/MM/yyyy")
        {

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            //CultureInfo currentCulture = new System.Globalization.CultureInfo("th-th");
            DateTime dateValue;
            if (DateTime.TryParseExact(data_str, format, currentCulture, System.Globalization.DateTimeStyles.None, out dateValue))
            {

            }
            else
            {

                return null;
            }

            return dateValue;
        }
        public static decimal? GetOraParamDecimal(OracleParameter input)
        {
            if (input == null)
                return null;
            if (input.Value == null)
                return null;
            if (input.Value is decimal)
                return (decimal)input.Value;
            if (!(input.Value is OracleDecimal))
                return null;
            if (((OracleDecimal)input.Value).IsNull)
                return null;
            return ((OracleDecimal)input.Value).Value;
        }

        public static DateTime? GetOraParamDate(OracleParameter input)
        {
            if (input == null)
                return null;
            if (input.Value == null)
                return null;
            if (input.Value is DateTime)
                return (DateTime)input.Value;
            if (!(input.Value is OracleDate))
                return null;
            if (((OracleDate)input.Value).IsNull)
                return null;
            return ((OracleDate)input.Value).Value;
        }

        public static string GetOraParamString(OracleParameter input)
        {
            if (input == null)
                return null;
            if (input.Value == null)
                return null;
            if (input.Value is string)
                return (string)input.Value;
            if (input.Value is OracleString)
            {
                if (((OracleString)input.Value).IsNull)
                    return null;
                else
                    return ((OracleString)input.Value).Value;
            }

            return null;
        }

        public static int? GetOraParamInt32(OracleParameter input)
        {
            decimal? dInput = GetOraParamDecimal(input);
            if (dInput == null)
                return null;
            return Convert.ToInt32(dInput);
        }

        public static long? GetOraParamInt64(OracleParameter input)
        {
            decimal? dInput = GetOraParamDecimal(input);
            if (dInput == null)
                return null;
            return Convert.ToInt64(dInput);
        }

        public static char? GetOraParamChar(OracleParameter input)
        {
            string dInput = GetOraParamString(input);
            if (dInput == null)
                return null;
            if (dInput.Length == 0)
                return null;
            return dInput.ToCharArray()[0];
        }

        public static string maskAccountNo(string AccNo)
        {
            if (!string.IsNullOrEmpty(AccNo))
            {
                AccNo = AccNo.Replace("-", "").Trim();
                int acc_no_length = AccNo.Length;
                int front_length = 0, back_length = 0;

                if (acc_no_length == 10)
                {
                    front_length = 4;
                    back_length = 3;
                    AccNo = maskAccountDynamix(AccNo, acc_no_length, front_length, back_length);
                }
                else if (acc_no_length == 12)
                {
                    front_length = 4;
                    back_length = 4;
                    AccNo = maskAccountDynamix(AccNo, acc_no_length, front_length, back_length);
                }
                else if (acc_no_length == 15)
                {
                    front_length = 5;
                    back_length = 5;
                    AccNo = maskAccountDynamix(AccNo, acc_no_length, front_length, back_length);
                }

            }
            return AccNo;
        }

        private static string maskAccountDynamix(string AccNo, int acc_no_length, int front_length, int back_length)
        {
            int open_length = front_length + back_length;

            if (acc_no_length > open_length)
            {
                int count_remain = acc_no_length - open_length;
                string temp_accno_start = AccNo.Substring(0, front_length);
                string temp_accno_end = AccNo.Substring(AccNo.Length - back_length, back_length);
                AccNo = temp_accno_start + temp_accno_end.PadLeft(count_remain + back_length, 'X');
            }
            else
            {
                throw new Exception("Account No. Length > " + open_length + " Charector");
            }

            return AccNo;
        }

        public static string Account_No_Format(string acc_no)
        {

            string acc;
            string acc1 = "", acc2 = "", acc3 = "", acc4 = "", acc5 = "";
            if (string.IsNullOrEmpty(acc_no)) return acc_no;

            acc1 = acc_no.Substring(0, 3);
            acc2 = acc_no.Substring(3, 1);
            acc3 = acc_no.Substring(4, 5);
            acc4 = acc_no.Substring(9, 1);

            if (acc_no.Count() > 10)
            {
                acc5 = acc_no.Substring(10, acc_no.Count() - 10);
                if (acc2.Count() > 5) acc2 = acc2.Substring(0, 5);
                acc = string.Format("{0}{1}{2}{3}{4}", acc1, acc2, acc3, acc4, acc5);
            }
            else
            {

                acc = string.Format("{0}-{1}-{2}-{3}", acc1, acc2, acc3, acc4);
            }

            return acc;
        }

        /// <summary>
        /// จัด format เลขบัตรเครดิต
        /// </summary>
        /// <param name="CardNo">เลขบัตรเครดิต</param>
        /// <returns>รูปแบบ 0000-0000-0000-0000</returns>
        public static string CreditCardNumberFormat(string CardNo)
        {
            string new_cardno = "";

            if (!string.IsNullOrEmpty(CardNo))
            {
                if (CardNo.Length < 16) throw new Exception("Incorrect Credit Card No.");
                for (int n = 4; n < CardNo.Length; n += 4)
                {
                    new_cardno = CardNo.Insert(n, "-");
                    n += 1;
                    CardNo = new_cardno;
                }
            }
            return new_cardno;
        }

        /// <summary>
        /// เปิดหน้า 6 หลัง 4
        /// </summary>
        /// <param name="CardNo">เลขบัตรเครดิต</param>
        /// <returns>เลขบัตรเครดิต mask X เปิดหน้า 6 หลัง 4</returns>
        public static string maskCreditCard(string CardNo)
        {
            if (!string.IsNullOrEmpty(CardNo))
            {
                if (CardNo.Length < 16) throw new Exception("Incorrect Credit Card No.");
                int card_no_length = CardNo.Length;
                if (card_no_length > 10)
                {
                    int count_remain = card_no_length - 10;
                    string temp_cardno_start = CardNo.Substring(0, 6);
                    string temp_cardno_end = CardNo.Substring(CardNo.Length - 4, 4);
                    CardNo = temp_cardno_start + temp_cardno_end.PadLeft(count_remain + 4, 'X');
                }
            }
            return CardNo;
        }

        private static string[] getFormatDateTime()
        {
            string[] formats = {"dd/MM/yyyy", "dd-MMM-yyyy", "yyyy-MM-dd","yyyy-M-d",
                                "dd-MM-yyyy", "M/d/yyyy", "dd MMM yyyy",
                                "yyyy-MM-ddTHH:mm:ss","yyyy-MMM-ddTHH:mm:ss",
                                "yyyy-M-dTHH:mm:ss","dd/MM/yyyy HH.mm.ss","dd/MM/yyyy HH:mm:ss",
                                "yyyy-MM-ddTHH:mm","yyyy-MMM-ddTHH:mm",
                                "yyyy-M-dTHH:mm","dd/MM/yyyy HH.mm","dd/MM/yyyy HH:mm",
                                "yyyy-MM-ddTH:mm:ss","yyyy-MMM-ddTH:mm:ss",
                                "yyyy-M-dTH:mm:ss","dd/MM/yyyy H.mm.ss","dd/MM/yyyy H:mm:ss",
                                "yyyy-MM-ddTH:mm","yyyy-MMM-ddTH:mm",
                                "yyyy-M-dTH:mm","dd/MM/yyyy H.mm","dd/MM/yyyy H:mm",
                                "d/MM/yyyy H:mm:ss","d/MM/yyyy H:mm","d/MM/yyyyTH:mm:ss",
                                "d/MM/yyyyTHH:mm:ss","d/MM/yyyyTHH:mm",
                                "d/MM/yyyyTHH.mm.ss","d/MM/yyyyTHH.mm",
                                "d/MM/yyyy HH.mm.ss","d/MM/yyyy HH.mm",

                                "dd/MM/yy", "dd-MMM-yy", "yy-MM-dd","yy-M-d",
                                "dd-MM-yy", "M/d/yy", "dd MMM yy",
                                "yy-MM-ddTHH:mm:ss","yy-MMM-ddTHH:mm:ss",
                                "yy-M-dTHH:mm:ss","dd/MM/yy HH.mm.ss","dd/MM/yy HH:mm:ss",
                                "yy-MM-ddTHH:mm","yy-MMM-ddTHH:mm",
                                "yy-M-dTHH:mm","dd/MM/yy HH.mm","dd/MM/yy HH:mm",
                                "yy-MM-ddTH:mm:ss","yy-MMM-ddTH:mm:ss",
                                "yy-M-dTH:mm:ss","dd/MM/yy H.mm.ss","dd/MM/yy H:mm:ss",
                                "yy-MM-ddTH:mm","yy-MMM-ddTH:mm",
                                "yy-M-dTH:mm","dd/MM/yy H.mm","dd/MM/yy H:mm",
                                "d/MM/yy H:mm:ss","d/MM/yy H:mm","d/MM/yyTH:mm:ss",
                                "d/MM/yyTHH:mm:ss","d/MM/yyTHH:mm",
                                "d/MM/yyTHH.mm.ss","d/MM/yyTHH.mm",
                                "d/MM/yy HH.mm.ss","d/MM/yy HH.mm"
            };

            return formats;
        }

        public static DateTime beginOfDay(DateTime refDateTime)
        {
            return new DateTime(refDateTime.Year, refDateTime.Month, refDateTime.Day, 0, 0, 0);
        }

        public static DateTime endOfDay(DateTime refDateTime)
        {
            return new DateTime(refDateTime.Year, refDateTime.Month, refDateTime.Day, 23, 59, 59, 999);
        }

        public static DateTime? StringToDateTime(String input, string era)
        {
            era = era.Replace(".", "");
            int eraDif = 0;
            try
            {
                if (
                    era.ToUpper().Trim() == "BU" ||
                    era.ToUpper().Trim() == "BC")
                {
                    eraDif = 543;
                }

                string sDate = input.Split(" ".ToCharArray())[0];
                string sTime = "";
                if (input.Split(" ".ToCharArray()).Length > 1)
                {
                    sTime = input.Split(" ".ToCharArray())[1];
                }


                string stDay, stMonth, stYear;
                int istDay, istMonth, istYear;
                stDay = sDate.Split("/".ToCharArray())[0];
                stMonth = sDate.Split("/".ToCharArray())[1];
                stYear = sDate.Split("/".ToCharArray())[2];
                istYear = int.Parse(stYear) - eraDif;
                istMonth = int.Parse(stMonth);
                istDay = int.Parse(stDay);

                if (sTime == "")
                {
                    return new DateTime(istYear, istMonth, istDay);
                }
                else
                {
                    string stHour, stMinute, stSecond;
                    int istHour, istMinute, istSecond;
                    stHour = sTime.Split(":".ToCharArray())[0];
                    stMinute = sTime.Split(":".ToCharArray())[1];
                    stSecond = sTime.Split(":".ToCharArray())[2];
                    istSecond = int.Parse(stSecond);
                    istMinute = int.Parse(stMinute);
                    istHour = int.Parse(stHour);

                    return new DateTime(istYear, istMonth, istDay, istHour, istMinute, istSecond);

                }
            }
            catch
            {
                return null;
            }
        }

        public static DateTime StringToDate(String input)
        {
            string stDay, stMonth, stYear;
            int istDay, istMonth, istYear;
            stDay = input.Split("/".ToCharArray())[0];
            stMonth = input.Split("/".ToCharArray())[1];
            stYear = input.Split("/".ToCharArray())[2];
            istYear = int.Parse(stYear) - 543;
            istMonth = int.Parse(stMonth);
            istDay = int.Parse(stDay);
            return new DateTime(istYear, istMonth, istDay);
        }

        public static long GetSeqNextVal(OracleConnection conn, string sequenceName)
        {
            string sqlStr =
                "select " + sequenceName + ".nextval from dual";
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);
            long returnValue = Convert.ToInt64(oCmd.ExecuteScalar());
            oCmd.Dispose();
            return returnValue;
        }
        public static async Task<long> GetSeqNextValAsync(OracleConnection conn, string sequenceName)
        {
            string sqlStr =
                "select " + sequenceName + ".nextval from dual";
            OracleCommand oCmd = new OracleCommand(sqlStr, conn);
            long returnValue = Convert.ToInt64(await oCmd.ExecuteScalarAsync());
            oCmd.Dispose();
            return returnValue;
        }

        public static decimal? ConvertToNullableDecimal(object input)
        {
            if (input == null)
                return (decimal?)null;
            else
                return Convert.ToDecimal(input);
        }

    }
    public class DBParameter
    {
        public string NAME { get; set; }
        public object VALUE { get; set; }
        public OracleDbType TYPE { get; set; }
        public int SIZE { get; set; }

        public DBParameter(string IN_NAME, object IN_VALUE, OracleDbType IN_TYPE = OracleDbType.Varchar2, int IN_SIZE = 20)
        {
            this.NAME = IN_NAME;
            if (IN_VALUE == null)
            {
                this.VALUE = DBNull.Value;
            }
            else if (IN_VALUE is DBNull)
            {
                this.VALUE = DBNull.Value;

            }
            else

            {
                this.VALUE = IN_VALUE;
            }

            this.TYPE = IN_TYPE;
            this.SIZE = IN_SIZE;
        }

        public DBParameter(string IN_NAME, object IN_VALUE, OracleDbType IN_TYPE = OracleDbType.Varchar2)
        {
            this.NAME = IN_NAME;
            if (IN_VALUE == null)
            {
                this.VALUE = DBNull.Value;
            }
            else if (IN_VALUE is DBNull)
            {
                this.VALUE = DBNull.Value;

            }
            else

            {
                this.VALUE = IN_VALUE;
            }

            this.TYPE = IN_TYPE;
        }

        public DBParameter(string IN_NAME, object IN_VALUE)
        {
            this.NAME = IN_NAME;
            if (IN_VALUE == null)
            {
                this.VALUE = DBNull.Value;
            }
            else if (IN_VALUE is DBNull)
            {
                this.VALUE = DBNull.Value;

            }
            else

            {
                this.VALUE = IN_VALUE;
            }

            this.TYPE = OracleDbType.Varchar2;
        }

    }

    internal static class DataTableExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            //if (table.Rows.Count==0)
            //{

            //    yield return new T();
            //}
            //else
            //{
            foreach (var row in table.Rows)
            {
                yield return CreateItemFromRow<T>(row as DataRow, properties);
            }
            //}
        }
        public static T AsEnumerable<T>(this DataRow row) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            return CreateItemFromRow<T>(row as DataRow, properties);
        }
        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (PropertyInfo property in properties)
            {
                if (!property.CanWrite) continue;
                if (!row.Table.Columns.Contains(property.Name)) continue;

                if (DBNull.Value != row[property.Name])
                {
                    property.SetValue(item, TypeExtension.ChangeType(row[property.Name], property.PropertyType), null);
                }
            }

            return item;
        }
    }

    internal static class TypeExtension
    {
        public static bool IsNullable(this Type type)
        {
            return ((type.IsGenericType) && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        public static IEnumerable<string> AppendAll(this IEnumerable<string> text, string prefix, string suffix)
        {
            foreach (string s in text)
            {
                yield return string.Format("{0}{1}{2}", prefix, s, suffix);
            }
        }

        public static void ApplyAll(this IEnumerable items, string propertyName, object data)
        {
            foreach (var item in items)
            {
                PropertyInfo property = item.GetType().GetProperty(propertyName);
                if (property == null) throw new NullReferenceException();
                if (!property.CanWrite) throw new InvalidOperationException(string.Format("Property or indexer '{0}' cannot be assign to, it is read only.", property.Name));

                property.SetValue(item, TypeExtension.ChangeType(data, property.PropertyType), null);
                //yield return item;
            }
        }
        public static IEnumerable<TResult> ConvertAll<TResult>(this IEnumerable items)
        {
            foreach (var item in items)
            {
                yield return (TResult)TypeExtension.ChangeType(item, typeof(TResult));
            }
        }
        public static object ChangeType(object value, Type conversionType)
        {
            // Note: This if block was taken from Convert.ChangeType as is, and is needed here since we're
            // checking properties on conversionType below.
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            }
            // end if

            if (conversionType.IsEnum)
            {
                return ConvertToEnumType(value, conversionType);
            }

            // If it's not a nullable type, just pass through the parameters to Convert.ChangeType
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                // It's a nullable type, so instead of calling Convert.ChangeType directly which would throw a
                // InvalidCastException (per http://weblogs.asp.net/pjohnson/archive/2006/02/07/437631.aspx),
                // determine what the underlying type is
                // If it's null, it won't convert to the underlying type, but that's fine since nulls don't really
                // have a type--so just return null
                // Note: We only do this check if we're converting to a nullable type, since doing it outside
                // would diverge from Convert.ChangeType's behavior, which throws an InvalidCastException if
                // value is null and conversionType is a value type.
                if (value == null)
                {
                    return null;
                } // end if

                // It's a nullable type, and not null, so that means it can be converted to its underlying type,
                // so overwrite the passed-in conversion type with this underlying type
                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            } // end if

            // Now that we've guaranteed conversionType is something Convert.ChangeType can handle (i.e. not a
            // nullable type), pass the call on to Convert.ChangeType
            return Convert.ChangeType(value, conversionType);
        }

        private static object ConvertToEnumType(object value, Type type)
        {
            if (value is string)
            {
                return Enum.Parse(type, value as string);
            }
            else
            {
                if (!Enum.IsDefined(type, value))
                {
                    throw new FormatException("Undefined value for enum type");
                }

                return Enum.ToObject(type, value);
            }
        }
        
    }
}
