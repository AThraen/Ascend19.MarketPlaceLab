using EPiServer.Core;

namespace AscendMarketPlaceLab.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
