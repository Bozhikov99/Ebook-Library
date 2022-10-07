using System;
using AutoMapper;
using Core.Commands.UserCommands;
using Core.Helpers;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.UserHandlers
{
    public class AddBookToFavouritesHandler:IRequestHandler<AddBookToFavouritesCommand, bool>
    {
        private readonly IRepository repository;
        private readonly UserIdHelper helper;

        public AddBookToFavouritesHandler(IRepository repository, UserIdHelper helper)
        {
            this.repository = repository;
            this.helper = helper;
        }

        public async Task<bool> Handle(AddBookToFavouritesCommand request, CancellationToken cancellationToken)
        {
            bool isFavourited = false;

            string bookId = request.BookId;
            string userId = helper.GetUserId();
            User user = await repository.GetByIdAsync<User>(userId);
            Book book = await repository.GetByIdAsync<Book>(bookId);

            user.FavouriteBooks.Add(book);

            repository.Update(user);
            await repository.SaveChangesAsync();

            isFavourited = true;

            return isFavourited;
        }
    }
}

