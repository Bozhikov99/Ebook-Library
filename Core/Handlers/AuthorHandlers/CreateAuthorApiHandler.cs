using AutoMapper;
using Core.ApiModels.Author;
using Core.Commands.AuthorCommands;
using Core.Validators;
using Core.ViewModels.Author;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.AuthorHandlers
{
    public class CreateAuthorApiHandler : IRequestHandler<CreateAuthorApiCommand, ListAuthorModel>
    {
        private readonly AuthorValidator validator;
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public CreateAuthorApiHandler(IRepository repository, IMapper mapper, AuthorValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<ListAuthorModel> Handle(CreateAuthorApiCommand request, CancellationToken cancellationToken)
        {
            UpsertAuthorModel model = request.Model;
            Author author = mapper.Map<Author>(model);
            await validator.ValidateAuthorName(author.FirstName, author.LastName);

            await repository.AddAsync(author);
            await repository.SaveChangesAsync();

            Author createdAuthor = await repository
                .FirstAsync<Author>(a => a.FirstName == model.FirstName && a.LastName == model.LastName);

            ListAuthorModel result = mapper.Map<ListAuthorModel>(createdAuthor);

            return result;
        }
    }
}
