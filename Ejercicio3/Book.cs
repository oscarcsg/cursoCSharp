namespace Ejercicio3
{
    internal class Book
    {
        public string? _Title { get; set; }
        public bool _IsRead { get; set; }

        public Book() { }

        public Book(string? title)
        {
            _Title = title;
        }

        public Book(string? title, bool isRead) : this(title)
        {
            _IsRead = isRead;
        }
    }
}
