using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Helpers;
using Core.Queries.User;
using Core.ViewModels.Book;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.UserHandlers
{
    public class GetFavouriteBooksHandler: IRequestHandler<GetFavouriteBooksQuery, IEnumerable<ListBookModel>>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly UserIdHelper helper;

        public GetFavouriteBooksHandler(IRepository repository, IMapper mapper, UserIdHelper helper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.helper = helper;
        }

        public async Task<IEnumerable<ListBookModel>> Handle(GetFavouriteBooksQuery request, CancellationToken cancellationToken)
        {
            string userId = helper.GetUserId();
            User user = await repository.GetByIdAsync<User>(userId);

            IEnumerable<ListBookModel> favouriteBooks = await repository.All<Book>(b => b.UsersFavourited.Contains(user))
                .ProjectTo<ListBookModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return favouriteBooks;
        }
    }
}

