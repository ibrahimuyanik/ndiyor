namespace PresentationLayer.Models
{
    public class CommentListViewModel
    {
        public int CommentID { get; set; }
        public string Commentt { get; set; }
        public int NewsID { get; set; }
        public int EatID { get; set; }
        public int AppUserID { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string NameSurname { get; set; }
        public string UserName { get; set; }
    }
}
