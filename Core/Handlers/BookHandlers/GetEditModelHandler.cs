using System;
using AutoMapper;
using Core.Queries.Book;
using Core.ViewModels.Book;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.BookHandlers
{
    public class GetEditModelHandler : IRequestHandler<GetEditModelQuery, EditBookModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetEditModelHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<EditBookModel> Handle(GetEditModelQuery request, CancellationToken cancellationToken)
        {
            Book book = await repository.GetByIdAsync<Book>(request.Id);
            EditBookModel model = mapper.Map<EditBookModel>(book);

            return model;
        }
    }
}

