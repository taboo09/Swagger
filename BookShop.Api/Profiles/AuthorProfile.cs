using AutoMapper;
using BookShop.Api.Entities;
using BookShop.Api.Models;

namespace BookShop.Api.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>();
            
            CreateMap<AuthorForUpdate, Author>();
        }
    }
}