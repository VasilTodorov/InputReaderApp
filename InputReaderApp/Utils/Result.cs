using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Utils
{
    public enum ErrorCode {InvalidFormat, RowColMismatch, InvalidDimension}
    public class Result<T>
    {
        public bool IsSuccess;
        public bool IsFailure => !IsSuccess;
        public ErrorCode? Code { get;}
        public string? Message { get;}
        public T? Data { get;}                

        private Result(bool isSuccess,ErrorCode? errorCode, string? message, T? data)
        {
            IsSuccess = isSuccess;
            Code = errorCode;
            Message = message;
            Data = data;
        }
                              
        public static Result<T> Fail(ErrorCode errorCode, string message="empty message")
        {
            return new Result<T>(false, errorCode, message, default);
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, null, null, value);
        }
    }
}
