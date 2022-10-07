using System;
using MediatR;

namespace Core.Queries.Book
{
    public class GetContentQuery : IRequest<byte[]>
    {
        public GetContentQuery(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

