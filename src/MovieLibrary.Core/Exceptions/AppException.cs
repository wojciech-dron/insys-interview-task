using System;
using System.Net;

namespace MovieLibrary.Core.Exceptions;

public class AppException : Exception
{
    public virtual string AppCode => "undefined_error";
    public virtual HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;
    
    public AppException(string message)
        : base(message)
    { }
}