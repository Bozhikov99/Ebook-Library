using System;
using Core.Commands.UserCommands;
using Core.Helpers;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.UserHandlers
{
    public class RemoveBookFromFavouritesHandler:IRequestHandler<RemoveBookFromFavouritesCommand, bool>
    {
        private readonly IRepository repository;
        private readonly UserIdHelper helper;

        public RemoveBookFromFavouritesHandler(IRepository repository, UserIdHelper helper)
        {
            this.repository = repository;
            this.helper = helper;
        }

        public async Task<bool> Handle(RemoveBookFromFavouritesCommand request, CancellationToken cancellationToken)
        {
            bool isRemoved = false;

            string bookId = request.BookId;
            string userId = helper.GetUserId();
            User user = repository.All<User>()
                .Include(u => u.FavouriteBooks)
                .First(u => u.Id == userId);

            Book book = await repository.GetByIdAsync<Book>(bookId);

            user.FavouriteBooks.Remove(book);

            repository.Update(user);
            await repository.SaveChangesAsync();

            isRemoved = true;

            return isRemoved;
        }
    }
}

