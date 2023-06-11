using System;
using System.Net;

namespace MovieLibrary.Core.Exceptions;

public class NotFoundException : AppException
{
    public int Id { get; set; }

    public NotFoundException(Type type, int id)
        : base($"Entity {type.Name} with id {id} was not found.")
    {
        Id = id;
    }

    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    public override string AppCode => "not_found";
}