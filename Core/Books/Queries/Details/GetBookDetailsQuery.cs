using AutoMapper;
using Core.ApiModels.OutputModels.Review;
using Core.Helpers;
using Domain.Entities;
using Infrastructure.Common;
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
        private readonly UserIdHelper userIdHelper;

        public GetDetailsHandler(EbookDbContext context, IMapper mapper, UserIdHelper userIdHelper)
        {
            this.context = context;
            this.mapper = mapper;
            this.userIdHelper = userIdHelper;
        }

        public async Task<BookDetailsOutputModel> Handle(GetBookDetailsQuery request, CancellationToken cancellationToken)
        {
            string bookId = request.Id;
            string currentUserId = userIdHelper.GetUserId();

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
                    UsersFavourited = b.UsersFavourited
                })
                .FirstOrDefaultAsync();

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
                         .Sum() / book.Reviews.Count,

            };
            //BookDetailsOutputModel model = mapper.Map<BookDetailsOutputModel>(book);

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

            bool isFavourite = book.UsersFavourited
                .Any(u => u.Id == currentUserId);

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
