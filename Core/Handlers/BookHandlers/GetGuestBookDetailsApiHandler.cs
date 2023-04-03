using AutoMapper;
using Common.MessageConstants;
using Core.ApiModels.InputModels.Books;
using Core.Queries.Book;
using Core.ViewModels.Review;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.BookHandlers
{
    public class GetGuestBookDetailsApiHandler : IRequestHandler<GetGuestBookDetailsApiQuery, BookDetailsApiModel>
    {
        private readonly IMapper mapper;
        private readonly IRepository repository;

        public GetGuestBookDetailsApiHandler(IMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<BookDetailsApiModel> Handle(GetGuestBookDetailsApiQuery request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;

            Book book = await repository.AllReadonly<Book>(b => b.Id == bookId)
                .Include(b => b.Genres)
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .FirstAsync();

            if (book is null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }

            BookDetailsApiModel model = mapper.Map<BookDetailsApiModel>(book);

            return model;
        }
    }
}
