using AutoMapper;
using Core.Commands.GenreCommands;
using Core.Validators;
using Core.ViewModels.Genre;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.GenreHandlers
{
    public class CreateGenreApiHandler : IRequestHandler<CreateGenreApiCommand, ListGenreModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly GenreValidator validator;

        public CreateGenreApiHandler(IRepository repository, IMapper mapper, GenreValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<ListGenreModel> Handle(CreateGenreApiCommand request, CancellationToken cancellationToken)
        {
            Genre genre = mapper.Map<Genre>(request.Model);

            await validator.ValidateGenreName(genre.Name);
            await repository.AddAsync(genre);
            await repository.SaveChangesAsync();

            Genre createdGenre = await repository
                .FirstAsync<Genre>(g => g.Name == genre.Name);

            ListGenreModel result = mapper.Map<ListGenreModel>(createdGenre);

            return result;
        }
    }
}
