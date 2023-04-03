using AutoMapper;
using Common.MessageConstants;
using Core.ApiModels.InputModels.Genre;
using Core.Queries.Genre;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.GenreHandlers
{
    public class GetUpsertModelHandler : IRequestHandler<GetUpsertModelQuery, UpsertGenreModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetUpsertModelHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<UpsertGenreModel> Handle(GetUpsertModelQuery request, CancellationToken cancellationToken)
        {
            Genre genre = await repository.GetByIdAsync<Genre>(request.Id);

            if (genre is null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.INVALID_GENRE);
            }

            UpsertGenreModel model = mapper.Map<UpsertGenreModel>(genre);

            return model;
        }
    }
}
