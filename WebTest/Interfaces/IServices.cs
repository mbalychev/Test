using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebTest.Interfaces
{
    public interface IServices<T> : IDisposable where T : class
    {
        Task CreateAsync(T item);
        Task CreateAsync(string inn, string rating);
        Task<T> ReadAsync(int id);
        Task<List<T>> ReadAllAsync();
        Task UpdateAsync(T item);
        Task DeleteAsync (int id);

    }
}
