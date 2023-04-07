using Proizvodii.Entity;

namespace Proizvodii.Models
{
    public class ProductListView
    {
        public string Filter { get; set; }
        public IEnumerable<CategoryFilter> CategoryFilter { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
    public class CategoryFilter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}
