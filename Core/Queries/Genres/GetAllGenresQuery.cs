using System;
using Core.ViewModels.Genre;
using MediatR;

namespace Core.Queries.Genre
{
    public class GetAllGenresQuery : IRequest<IEnumerable<ListGenreModel>>
    {
        public GetAllGenresQuery()
        {
        }
    }
}

