using AutoMapper;
using Core.ApiModels.OutputModels.Review;
using Core.Common.Services;
using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Books.Queries.Details
{
    public class GetBookDetailsQuery : IRequest<BookDetailsOutputModel>
    {
        public string Id { get; set; } = null!;
    }

    public class GetDetailsHandler : IRequestHandler<GetBookDetailsQuery, BookDetailsOutputModel>
    {
        private readonly EbookDbContext context;
        private readonly IMapper mapper;
        private readonly CurrentUserService userService;

        public GetDetailsHandler(EbookDbContext context, IMapper mapper, CurrentUserService userService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
        }

        public async Task<BookDetailsOutputModel> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            string bookId = request.Id;
            string currentUserId = userService.UserId!;

            Book? book = await context.Books
                .Select(b => new Book
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Cover = b.Cover,
                    ReleaseYear = b.ReleaseYear,
                    Pages = b.Pages,
                    Author = b.Author,
                    BookGenres = b.BookGenres
                        .Select(bg => new BookGenre
                        {
                            BookId = bg.BookId,
                            Genre = bg.Genre,
                            GenreId = bg.GenreId,
                            Book = bg.Book
                        })
                        .ToArray(),
                    Reviews = b.Reviews,
                    BookUsers = b.BookUsers
                })
                .FirstOrDefaultAsync(b => string.Equals(b.Id, bookId));

            BookDetailsOutputModel model = new BookDetailsOutputModel
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Cover = book.Cover,
                ReleaseYear = book.ReleaseYear,
                Pages = book.Pages,
                Author = $"{book.Author.FirstName} {book.Author.LastName}",
                Genres = book.BookGenres.Select(bg => bg.Genre.Name),
                Rating = book.Reviews.Count == 0 ? 0 :
                         book.Reviews.Select(r => r.Value)
                         .Sum() / book.Reviews.Count
            };

            if (string.IsNullOrEmpty(currentUserId))
            {
                return model;
            }

            User? user = await context.Users
                .FirstOrDefaultAsync(u => string.Equals(u.Id, currentUserId));

            if (user is null)
            {
                throw new ArgumentException();
            }

            model.UserId = user.Id;

            bool isFavourite = book.BookUsers
                .Any(bu => string.Equals(bu.UserId, currentUserId));

            Review? userReview = await context.Reviews
                .FirstOrDefaultAsync(r => string.Equals(r.UserId, currentUserId));

            UserReviewOutputModel userReviewModel = mapper.Map<UserReviewOutputModel>(userReview);

            model.UserReview = userReviewModel;
            model.IsFavourite = isFavourite;
            model.Reviews = model.Reviews
                .Where(r => r.UserName != user.UserName);

            return model;
        }
    }
}
