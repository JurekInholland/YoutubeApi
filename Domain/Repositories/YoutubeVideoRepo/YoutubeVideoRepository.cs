using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.YoutubeVideoRepo;

public class YoutubeVideoRepository : RepositoryBase<Models.DomainModels.YoutubeVideo>, IYoutubeVideoRepository
{
    public YoutubeVideoRepository(DbContext context) : base(context)
    {
        Context = context;
    }
}
