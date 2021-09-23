namespace BookWorm.Models.VO
{
    public class Genre
    {
        private string _name;

        public Genre() { }

        public Genre(string name)
        {
            Name = name;
        }

        public string Name { get => _name; set => _name = value; }
    }
}
