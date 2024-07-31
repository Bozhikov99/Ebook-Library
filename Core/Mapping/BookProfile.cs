using AutoMapper;

namespace Core.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            //CreateMap<Book, BookModel>()
            //   .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
            //   .ForMember(d => d.Author, s => s.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"))
            //   .ForMember(d => d.Rating,
            //       s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
            //            .Select(r => r.Value)
            //            .Sum() / b.Reviews.Count));

            //CreateMap<Book, BookDetailsModel>()
            //    .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
            //    .ForMember(d => d.Author, s => s.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"))
            //    .ForMember(d => d.Rating,
            //        s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
            //             .Select(r => r.Value)
            //             .Sum() / b.Reviews.Count));

            //CreateMap<Book, BookDetailsOutputModel>()
            //    .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
            //    .ForMember(d => d.Author, s => s.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"))
            //    .ForMember(d => d.Reviews, s => s.MapFrom(b => b.Reviews))
            //    .ForMember(d => d.Rating,
            //        s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
            //             .Select(r => r.Value)
            //             .Sum() / b.Reviews.Count));
        }
    }
}
