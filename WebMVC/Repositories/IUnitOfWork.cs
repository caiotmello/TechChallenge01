namespace ImageUploaderWebMVC.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IImageRepository Images { get; }

        int Complete();
    }
}
