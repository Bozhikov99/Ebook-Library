using AutoMapper;
using Common.MessageConstants;
using Core.ApiModels.InputModels.Books;
using Core.Books.Queries.Details;
using Core.Queries.Book;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.BookHandlers
{
    public class GetGuestBookDetailsApiHandler : IRequestHandler<GetGuestBookDetailsApiQuery, BookDetailsOutputModel>
    {
        private readonly IMapper mapper;
        private readonly IRepository repository;

        public GetGuestBookDetailsApiHandler(IMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<BookDetailsOutputModel> Handle(GetGuestBookDetailsApiQuery request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;

            //TODO: Try to optimize this
            Book book = await repository.AllReadonly<Book>(b => b.Id == bookId)
                .Include(b => b.Genres)
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .FirstAsync();

            if (book is null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }

            BookDetailsOutputModel model = mapper.Map<BookDetailsOutputModel>(book);

            return model;
        }
    }
}
