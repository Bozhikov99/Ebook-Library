using AutoMapper;
using Core.ViewModels.Book;
using Infrastructure.Common;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public class BookService : IBookService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        public BookService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task Create(CreateBookModel model)
        {
            Book book = mapper.Map<Book>(model);

            await repository.AddAsync(book);
            await repository.SaveChangesAsync();
        }
    }
}
