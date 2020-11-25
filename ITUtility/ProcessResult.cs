using System;
using System.Collections.Generic;
using System.Text;

namespace ITUtility
{
    public class ProcessResult
    {
        public bool Successed { get; set; }

        public string ErrorMessage { get; set; }

        public object Result { get; set; }
    }
    public class ProcessResultObject
    {
        public bool Successed { get; set; }

        public string ErrorMessage { get; set; }

        public string Result { get; set; }
    }
    public class ProcessResultObject<T>
    {
        public bool IsSuccess { get; set; }
        public object ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public T Data { get; set; }
    }
    public class Token<T>
    {
        public string UserId { get; set; }
        public int TokenId { get; set; }
        public object TokenType { get; set; }
        public object UserType { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiredDate { get; set; }
        public T TokemParams { get; set; }
    }
    public class TokenParams
    {
        public string ParamName { get; set; }
        public string Value { get; set; }
    }
    public class ReturnTokenParams
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class Token_Expired
    {
        public bool IsExpired { get; set; }
    }
    
}
