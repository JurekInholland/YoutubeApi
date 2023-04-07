using Domain.Context;
using Microsoft.EntityFrameworkCore;
using Models.DomainModels;

namespace Domain.Repositories.ChannelRepo;

public class YoutubeChannelRepository : RepositoryBase<YoutubeChannel>, IYoutubeChannelRepository
{
    public YoutubeChannelRepository(DbContext context) : base(context)
    {
    }

    private YoutubeAppContext YoutubeContext => (YoutubeAppContext) Context;
}
