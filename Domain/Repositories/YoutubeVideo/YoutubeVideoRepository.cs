using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.YoutubeVideo;

public class YoutubeVideoRepository : RepositoryBase<Models.YoutubeVideo>, IYoutubeVideoRepository
{
    public YoutubeVideoRepository(DbContext context) : base(context)
    {
        Context = context;
    }
}
