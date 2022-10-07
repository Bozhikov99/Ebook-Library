using System;
using AutoMapper;
using Core.Helpers;
using Core.Queries.User;
using Core.ViewModels.Book;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.UserHandlers
{
    public class IsBookFavouriteHandler:IRequestHandler<IsBookFavouriteQuery, bool>
    {
        private readonly IRepository repository;
        private readonly UserIdHelper helper;

        public IsBookFavouriteHandler(IRepository repository, UserIdHelper helper)
        {
            this.repository = repository;
            this.helper = helper;
        }

        public async Task<bool> Handle(IsBookFavouriteQuery request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;
            string userId = helper.GetUserId();

            User user = repository.All<User>()
                .Include(u => u.FavouriteBooks)
                .First(u => u.Id == userId);

            bool isBookFavourite = user
                .FavouriteBooks
                .Any(b => b.Id == bookId);

            return isBookFavourite;
        }
    }
}

