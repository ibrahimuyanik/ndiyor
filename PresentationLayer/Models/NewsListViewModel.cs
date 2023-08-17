using EntityLayer.Concrete;

namespace PresentationLayer.Models
{
    public class NewsListViewModel
    {
        public int NewsID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool EditorPickStatus { get; set; }
        public bool HotNewsStatus { get; set; }
        public int CategoryID { get; set; }
        public Category Categories { get; set; }
    }
}
