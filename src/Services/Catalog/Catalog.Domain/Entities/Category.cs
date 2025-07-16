using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Category : EntityBase, IAgreegateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public bool IsActive { get; set; }

        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        protected Category() { }

        public Category(string name, string description, string imageUrl = null)
        {
            SetName(name);
            SetDescription(description);
            ImageUrl = imageUrl;
            IsActive = true;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Category description cannot be empty");

            if (description.Length > 500)
                throw new DomainException("Category description cannot exceed 500 characters");

            Description = description;
            SetUpdatedAt();
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category name cannot be empty");

            if (name.Length > 100)
                throw new DomainException("Category name cannot exceed 100 characters");

            Name = name;
            SetUpdatedAt();
        }

        public void SetImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            SetUpdatedAt();
        }

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
    }
}



