namespace Catalog.Domain.Entities;

using Catalog.Domain.Common;

public class ProductImage : EntityBase
{
    public string Url { get; private set; }
    public string AltText { get; private set; }
    public bool IsMain { get; private set; }
    public int ProductId { get; private set; }

    public ProductImage(string url, string altText, bool isMain = false)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("Image URL cannot be empty");

        Url = url;
        AltText = altText ?? string.Empty;
        IsMain = isMain;
    }

    public void SetAsMain()
    {
        IsMain = true;
        SetUpdatedAt();
    }

    public void SetAsNonMain()
    {
        IsMain = false;
        SetUpdatedAt();
    }
}
