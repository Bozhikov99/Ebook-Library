using System;
using Core.ViewModels.Author;
using MediatR;

namespace Core.Queries.Author
{
    public class GetAllAuthorsQuery : IRequest<IEnumerable<ListAuthorModel>>
    {
        public GetAllAuthorsQuery()
        {
        }
    }
}

