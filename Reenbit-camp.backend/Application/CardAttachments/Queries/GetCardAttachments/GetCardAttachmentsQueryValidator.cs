using FluentValidation;

namespace Application.CardAttachments.Queries.GetCardAttachments;

internal class GetCardAttachmentsQueryValidator 
    : AbstractValidator<GetCardAttachmentsQuery> 
{
    public GetCardAttachmentsQueryValidator()
    {
        RuleFor(x => x.CardId)
            .GreaterThan(0)
            .NotEmpty();
    }
}