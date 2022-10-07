using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Queries.Genre;
using Core.ViewModels.Genre;
using Infrastructure.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.GenreHandlers
{
    public class GetAllGenresHandler : IRequestHandler<GetAllGenresQuery, IEnumerable<ListGenreModel>>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetAllGenresHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ListGenreModel>> Handle(
            GetAllGenresQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<ListGenreModel> genres = await repository.All<Genre>()
                .ProjectTo<ListGenreModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return genres;
        }
    }
}

