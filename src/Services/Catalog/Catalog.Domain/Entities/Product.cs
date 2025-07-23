using Catalog.Domain.Common;
using Catalog.Domain.ValueObjects;

namespace Catalog.Domain.Entities
{
    public class Product : EntityBase, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Money Price { get; private set; }
        public int Stock { get; private set; }
        public string Sku { get; private set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }

        private readonly List<ProductImage> _images = new();
        public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

        protected Product() { }

        public Product(string name, string description, decimal price, string currency,
            int stock, string sku, int categoryId, string imageUrl = null)
        {
            SetName(name);
            SetDescription(description);
            SetPrice(price, currency);
            SetStock(stock);
            SetSku(sku);
            CategoryId = categoryId;
            ImageUrl = imageUrl;
            IsActive = true;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty");

            if (name.Length > 200)
                throw new DomainException("Product name cannot exceed 200 characters");

            Name = name;
            SetUpdatedAt();
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Product description cannot be empty");

            Description = description;
            SetUpdatedAt();
        }

        public void SetPrice(decimal amount, string currency)
        {
            Price = new Money(amount, currency);
            SetUpdatedAt();
        }

        public void SetStock(int quantity)
        {
            if (quantity < 0)
                throw new DomainException("Stock quantity cannot be negative");

            Stock = quantity;
            SetUpdatedAt();
        }

        public void SetSku(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
                throw new DomainException("SKU cannot be empty");

            if (sku.Length > 50)
                throw new DomainException("SKU cannot exceed 50 characters");

            Sku = sku.ToUpperInvariant();
            SetUpdatedAt();
        }

        public void AddStock(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity to add must be positive");

            Stock += quantity;
            SetUpdatedAt();
        }

        public void RemoveStock(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException("Quantity to remove must be positive");

            if (Stock - quantity < 0)
                throw new DomainException($"Insufficient stock. Available: {Stock}, Requested: {quantity}");

            Stock -= quantity;
            SetUpdatedAt();
        }

        public bool IsInStock() => Stock > 0;

        public void Activate()
        {
            IsActive = true;
            SetUpdatedAt();
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdatedAt();
        }

        public void AddImage(string url, string altText, bool isMain = false)
        {
            if (_images.Count >= 10)
                throw new DomainException("Cannot add more than 10 images per product");

            if (isMain)
            {
                // Set all existing images as non-main
                foreach (var img in _images)
                {
                    img.SetAsNonMain();
                }
            }

            var image = new ProductImage(url, altText, isMain);
            _images.Add(image);
            SetUpdatedAt();
        }

        public void RemoveImage(int imageId)
        {
            var image = _images.FirstOrDefault(i => i.Id == imageId);
            if (image != null)
            {
                _images.Remove(image);
                SetUpdatedAt();
            }
        }
    }

}
