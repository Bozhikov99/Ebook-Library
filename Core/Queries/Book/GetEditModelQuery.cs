using System;
using Core.ViewModels.Book;
using MediatR;

namespace Core.Queries.Book
{
    public class GetEditModelQuery : IRequest<EditBookModel>
    {
        public GetEditModelQuery(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

