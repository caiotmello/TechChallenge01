using Microsoft.EntityFrameworkCore;
using ImageUploaderWebMVC.Models;
using ImageUploaderWebMVC.Data;

namespace ImageUploaderWebMVC.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
