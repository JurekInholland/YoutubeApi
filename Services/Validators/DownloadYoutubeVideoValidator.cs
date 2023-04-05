using FluentValidation;
using Models.DomainModels;

namespace Services.Validators;

public abstract class DownloadYoutubeVideoValidator : AbstractValidator<YoutubeVideo>
{
    protected DownloadYoutubeVideoValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Duration).LessThanOrEqualTo(TimeSpan.FromHours(1));
    }
}
