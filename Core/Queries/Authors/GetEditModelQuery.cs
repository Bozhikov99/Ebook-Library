using System;
using Core.ViewModels.Author;
using MediatR;

namespace Core.Queries.Author
{
    public class GetEditModelQuery: IRequest<EditAuthorModel>
    {
        public GetEditModelQuery(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

