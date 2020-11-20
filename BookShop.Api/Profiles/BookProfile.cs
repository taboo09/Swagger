using AutoMapper;
using BookShop.Api.Entities;
using BookShop.Api.Models;

namespace BookShop.Api.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookWithConcatenatedAuthorName>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src =>
                 $"{src.Author.FirstName} {src.Author.LastName}"));

            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(src =>
                $"{src.Author.FirstName}"))
                .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src =>
                $"{src.Author.LastName}"));

            CreateMap<BookForCreation, Book>();

            CreateMap<BookForCreationWithAmountOfPages, Book>();
        }
    }
}