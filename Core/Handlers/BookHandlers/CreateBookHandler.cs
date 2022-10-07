using System;
using AutoMapper;
using Core.Commands.BookCommands;
using Core.Validators;
using Core.ViewModels.Book;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.BookHandlers
{
    public class CreateBookHandler: IRequestHandler<CreateBookCommand, bool>
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

