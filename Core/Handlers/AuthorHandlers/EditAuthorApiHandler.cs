using AutoMapper;
using Core.Commands.AuthorCommands;
using Core.Validators;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.AuthorHandlers
{
    public class EditAuthorApiHandler : IRequestHandler<EditAuthorApiCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly AuthorValidator validator;

        public EditAuthorApiHandler(IRepository repository, IMapper mapper, AuthorValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<bool> Handle(EditAuthorApiCommand request, CancellationToken cancellationToken)
        {
            bool isEdited = false;
            string id = request.Id;

            bool isExisting = await repository.AnyAsync<Author>(g => g.Id == id);

            if (!isExisting)
            {
                throw new ArgumentNullException();
            }

            Author author = mapper.Map<Author>(request.Model);
            author.Id = id;
            await validator.ValidateAuthorName(author.FirstName, author.LastName);

            repository.Update(author);
            await repository.SaveChangesAsync();

            isEdited = true;

            return isEdited;
        }
    }
}
