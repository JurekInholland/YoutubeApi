using Domain.Context;
using Microsoft.EntityFrameworkCore;
using Models.DomainModels;

namespace Domain.Repositories.SubscribedChannelRepo;

public class SubscribedChannelRepository : RepositoryBase<SubscribedChannel>, ISubscribedChannelsRepository
{
    public SubscribedChannelRepository(DbContext context) : base(context)
    {
    }
    private YoutubeAppContext YoutubeContext => (YoutubeAppContext) Context;

}
