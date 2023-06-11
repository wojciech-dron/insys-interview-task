using System.Collections.Generic;

namespace MovieLibrary.Api.Models;

public class ErrorResponse
{
    public string Message { get; set; } = "";
    public string Code { get; set; } = "";
    public string StackTrace { get; set; }

    public IDictionary<string, string[]> Errors { get; set; }
}