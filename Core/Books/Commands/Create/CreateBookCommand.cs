using AutoMapper;
using Core.Validators;
using Core.ViewModels.Book;
using Domain.Entities;
using Infrastructure.Common;

namespace Core.Books.Commands.Create
{
    public class CreateBookCommand : IRequest<bool>
    {
        public CreateBookCommand(CreateBookModel model)
        {
            Model = model;
        }

        public CreateBookModel Model { get; private set; }
    }

    public class CreateBookHandler : IRequestHandler<CreateBookCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly BookValidator validator;

        public CreateBookHandler(IRepository repository, IMapper mapper, BookValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<bool> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            bool isCreated = false;
            CreateBookModel model = request.Model;

            await validator.ValidateTitle(model.Title);

            Book book = mapper.Map<Book>(model);
            List<Genre> genres = new List<Genre>();

            foreach (var g in model.GenreIds)
            {
                Genre currentGenre = await repository.GetByIdAsync<Genre>(g);
                genres.Add(currentGenre);
            }

            book.Genres = genres;

            await repository.AddAsync(book);
            await repository.SaveChangesAsync();

            isCreated = true;

            return isCreated;
        }
    }
}

