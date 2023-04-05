using Domain.Context;
using Microsoft.EntityFrameworkCore;
using Models.DomainModels;

namespace Domain.Repositories.LocalVideoRepo;

public class LocalVideoRepository : RepositoryBase<LocalVideo>, ILocalVideoRepository
{
    public LocalVideoRepository(DbContext context) : base(context)
    {
        Context = context;
    }

    private YoutubeAppContext YoutubeContext => (YoutubeAppContext) Context;
}
