using AutoMapper;
using Core.Commands.GenreCommands;
using Core.Validators;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.GenreHandlers
{
    public class EditGenreApiHandler : IRequestHandler<EditGenreApiCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly GenreValidator validator;

        public EditGenreApiHandler(IRepository repository, IMapper mapper, GenreValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<bool> Handle(EditGenreApiCommand request, CancellationToken cancellationToken)
        {
            bool isEdited = false;

            string id = request.Id;
            bool isExisting = await repository.AnyAsync<Genre>(g => g.Id == id);

            if (!isExisting)
            {
                throw new ArgumentNullException();
            }

            Genre genre = mapper.Map<Genre>(request.Model);
            genre.Id = id;

            await validator.ValidateGenreName(genre.Name);
            repository.Update(genre);
            await repository.SaveChangesAsync();

            isEdited = true;

            return isEdited;
        }
    }
}
