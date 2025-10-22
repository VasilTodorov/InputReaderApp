using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputReaderApp.Utils
{
    public enum ErrorCode {InvalidFormat, RowColMismatch, InvalidDimension, InputNotFound}
    /// <summary>
    /// The result of an operation that returns Data.
    /// If Fail it has Code and Message
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
                              
        public static Result<T> Fail(ErrorCode errorCode, string? message=null)
        {
            return new Result<T>(false, errorCode, message, default);
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, null, null, value);
        }
    }
    /// <summary>
    /// The result of an operation that doesn't return Data.
    /// If Fail it has Code and Message.
    /// If Success it has Message.
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public ErrorCode? Code { get; }
        public string? Message { get; }

        private Result(bool isSuccess, ErrorCode? errorCode, string? message)
        {
            IsSuccess = isSuccess;
            Code = errorCode;
            Message = message;
        }

        public static Result Fail(ErrorCode errorCode, string? message = null)
            => new Result(false, errorCode, message);

        public static Result Success(string? message = null)
            => new Result(true, null, message);
    }
}
