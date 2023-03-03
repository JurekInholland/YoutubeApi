using Microsoft.EntityFrameworkCore;
using Models;

namespace Domain.Repositories.YoutubeVideoRepository;

public class YoutubeVideoRepository : RepositoryBase<YoutubeVideo>, IYoutubeVideoRepository
{
    public YoutubeVideoRepository(DbContext context) : base(context)
    {
        Context = context;
    }
}
