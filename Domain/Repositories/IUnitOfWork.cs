using Domain.Repositories.YoutubeVideoRepository;

namespace Domain.Repositories;

public interface IUnitOfWork
    // : IDisposable
{
    IYoutubeVideoRepository YoutubeVideos { get; }

    Task Save();
}
