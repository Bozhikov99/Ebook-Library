﻿using Common.MessageConstants;
using Common.ValidationConstants;
using Core.Authors.Queries.Common;
using Core.Authors.Queries.GetAuthors;
using Core.Books.Commands.Create;
using Core.Books.Commands.Delete;
using Core.Books.Commands.Edit;
using Core.Books.Queries.Details;
using Core.Books.Queries.GetBookEditModel;
using Core.Books.Queries.GetBooks;
using Core.Books.Queries.GetContent;
using Core.Extensions;
using Core.Genres.Queries.Common;
using Core.Genres.Queries.GetGenres;
using Core.Helpers;
using Core.Reviews.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GetEditModelQuery = Core.Books.Queries.GetBookEditModel.GetBookEditModelQuery;

namespace Web.Areas.Admin.Controllers
{
    public class BookController : BaseController
    {
        private readonly IMediator mediator;
        private readonly UserIdHelper helper;

        public BookController(IMediator mediator, UserIdHelper helper)
        {
            this.mediator = mediator;
            this.helper = helper;
        }

        public async Task<IActionResult> All(GetBooksQuery query)
        {
            IEnumerable<BookModel> books = await mediator.Send(query);
            ViewBag.PageNo = query.PageNumber;
            ViewBag.PageSize = query.PageSize;
            ViewBag.Genres = query.GenreIds;
            TempData["Search"] = query.Search;

            int starterBook = query.PageNumber * query.PageSize;
            ViewBag.StarterBook = starterBook;

            return View(books);
        }

        public async Task<IActionResult> Read(string id)
        {
            byte[] content = await mediator.Send(new GetContentQuery { BookId = id });

            return File(content, BookConstants.AllowedContentType);
        }

        public async Task<IActionResult> Details(string id)
        {
            BookDetailsOutputModel model = await mediator.Send(new GetBookDetailsQuery { Id = id });

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditBookModel model = await mediator.Send(new GetEditModelQuery { Id = id });
            IEnumerable<AuthorModel> authors = await mediator.Send(new GetAuthorsQuery());
            IEnumerable<GenreModel> genres = await mediator.Send(new GetGenresQuery());
            ViewBag.Authors = authors;
            ViewBag.Genres = genres;

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<AuthorModel> authors = await mediator.Send(new GetAuthorsQuery());
            IEnumerable<GenreModel> genres = await mediator.Send(new GetGenresQuery());
            ViewBag.Authors = authors;
            ViewBag.Genres = genres;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBookCommand command)
        {
            var cover = Request.Form
                .Files
                .FirstOrDefault();

            var content = Request.Form
                .Files
                .LastOrDefault();

            if (cover is not null)
            {
                byte[]? coverBytes = await cover.GetBytesAsync();
                command.Cover = coverBytes;
            }

            if (content is not null)
            {
                byte[]? contentBytes = await content.GetBytesAsync();
                command.Content = contentBytes;
            }

            try
            {
                await mediator.Send(command);
            }
            catch (Exception ex)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ex.Message;

                return View();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EditBookCommand command)
        {
            var cover = Request.Form
                .Files
                .FirstOrDefault();

            var content = Request.Form
                .Files
                .LastOrDefault();

            if (cover is not null)
            {
                byte[]? coverBytes = await cover.GetBytesAsync();
                command.Cover = coverBytes;
            }

            if (content is not null)
            {
                byte[]? contentBytes = await content.GetBytesAsync();
                command.Content = contentBytes;
            }

            try
            {
                await mediator.Send(command);
            }
            catch (Exception ex)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ex.Message;

                return View();
            }

            return RedirectToAction("All");
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await mediator.Send(new DeleteBookCommand { Id = id });
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.DELETE_BOOK_UNEXPECTED;
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(string id)
        {
            try
            {
                await mediator.Send(new DeleteReviewCommand { Id = id });
            }
            catch (Exception)
            {

                throw;
            }

            return Ok();
        }
    }
}
