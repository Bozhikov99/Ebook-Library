﻿using Core.ViewModels.Genre;
using MediatR;

namespace Core.Queries.Genre
{
    public class GetEditModelQuery : IRequest<EditGenreModel>
    {
        public GetEditModelQuery(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}

