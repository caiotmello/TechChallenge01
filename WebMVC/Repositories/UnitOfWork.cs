using System.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;
using ImageUploaderWebMVC.Data;

namespace ImageUploaderWebMVC.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly AppDbContext _context;
        public IImageRepository Images { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Images = new ImageRepository(_context);
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
