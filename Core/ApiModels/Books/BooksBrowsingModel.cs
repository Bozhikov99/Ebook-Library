﻿using Core.ViewModels.Book;

namespace Core.ApiModels.Books
{
    public class BooksBrowsingModel
    {
        public IEnumerable<ListBookModel> Books { get; set; } = new List<ListBookModel>();

        public string[] Genres { get; set; }
    }
}