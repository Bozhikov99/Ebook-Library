using AutoMapper;
using Common.MessageConstants;
using Core.ApiModels.Books;
using Core.Queries.Book;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers.BookHandlers
{
    public class GetBookInputModelHandler : IRequestHandler<GetBookInputModelQuery, BookInputModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetBookInputModelHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BookInputModel> Handle(GetBookInputModelQuery request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            bool isExisting = await repository.AnyAsync<Book>(b => b.Id == id);

            if (!isExisting)
            {
                throw new ArgumentNullException(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }

            Book book = await repository.GetByIdAsync<Book>(id);
            BookInputModel model = mapper.Map<BookInputModel>(book);

            return model;
        }
    }
}
