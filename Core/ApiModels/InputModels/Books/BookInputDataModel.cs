﻿using Core.ViewModels.Author;
using Core.ViewModels.Genre;

namespace Core.ApiModels.InputModels.Books
{
    public class BookInputDataModel
    {
        public IEnumerable<ListAuthorModel> Authors { get; set; }

        public IEnumerable<ListGenreModel> Genres { get; set; }
    }
}