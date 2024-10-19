using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository.IRepository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll(string? include = null);

        Category GetById(int categoryId);

        void Create(Category category);

        void Edit(Category category);

        void Delete(Category category);

        void Commit();
    }
}
