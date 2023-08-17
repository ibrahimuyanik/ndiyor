using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PresentationLayer.Models
{
    public class MyViewModel
    {
        public List<TestCategory> TestCategories { get; set; }
        public List<Category> Categories { get; set; }
        public List<Eat> Eats { get; set; }
    }
}
