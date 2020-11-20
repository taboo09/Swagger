using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookShop.Api.Entities;

namespace BookShop.Api.Services
{
    public interface IBookRepository : IDisposable
    {
        Task<IEnumerable<Book>> GetBooksAsync(Guid authorId);

        Task<Book> GetBookAsync(Guid authorId, Guid bookId);

        void AddBook(Book bookToAdd);

        Task<bool> SaveChangesAsync();
    }
}