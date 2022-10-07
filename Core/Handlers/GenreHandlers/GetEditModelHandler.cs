using System;
using AutoMapper;
using Common.MessageConstants;
using Core.Queries.Genre;
using Core.ViewModels.Genre;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.GenreHandlers
{
    public class GetEditModelHandler: IRequestHandler<GetEditModelQuery, EditGenreModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetEditModelHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<EditGenreModel> Handle(
            GetEditModelQuery request,
            CancellationToken cancellationToken)
        {
            Genre genre = await repository.GetByIdAsync<Genre>(request.Id);

            if (genre == null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.INVALID_GENRE);
            }

            EditGenreModel model = mapper.Map<EditGenreModel>(genre);

            return model;
        }
    }
}

