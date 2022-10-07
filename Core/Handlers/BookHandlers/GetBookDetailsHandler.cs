using System;
using AutoMapper;
using Core.Queries.Book;
using Core.ViewModels.Book;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.BookHandlers
{
    public class GetBookDetailsHandler: IRequestHandler<GetBookDetailsQuery, BookDetailsModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetBookDetailsHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BookDetailsModel> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            Book book = repository.All<Book>(b => b.Id == request.Id)
                .Include(b => b.Genres)
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .First();

            BookDetailsModel model = mapper.Map<BookDetailsModel>(book);

            return model;
        }
    }
}

