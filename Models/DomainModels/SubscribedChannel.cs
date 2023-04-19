namespace Models.DomainModels;

public class SubscribedChannel : BaseEntity
{
    public virtual YoutubeChannel Channel { get; set; }
    public Enums.ChannelSubscriptionType SubscriptionType { get; set; }
}
