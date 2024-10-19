using E_Commerce.Data;
using E_Commerce.Models;
using E_Commerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        //ApplicationDbContext dbContext = new ApplicationDbContext();

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // CRUD
        public List<Category> GetAll(string? include = null)
        {
            if(include == null)
                return dbContext.Categories.ToList();
            else
                return dbContext.Categories.Include(include).ToList();
        }

        public Category GetById(int categoryId)
        {
            return dbContext.Categories.Find(categoryId);
        }

        public void Create(Category category)
        {
            dbContext.Categories.Add(category);
        }

        public void Edit(Category category)
        {
            dbContext.Categories.Update(category);
        }

        public void Delete(Category category)
        {
            dbContext.Categories.Remove(category);
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }
    }
}
