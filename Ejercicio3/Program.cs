namespace Ejercicio3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Book> books = CreateBookList();
            Console.WriteLine(IsBookRead(books, "Libro 1")); // false
            Console.WriteLine(IsBookRead(books, "Libro 6")); // true
            Console.WriteLine(IsBookRead(books, "Libro 8")); // false (no existe)
        }

        private static List<Book> CreateBookList()
        {
            List<Book> list = new List<Book>();
            for (int i = 1; i <= 5; i++) list.Add(new Book($"Libro {i}", false));

            list.Add(new Book("Libro 6", true));

            return list;
        }

        private static bool IsBookRead(List<Book> bookList, string? bookTitle)
        {
            foreach (Book b in bookList)
            {
                if (bookTitle.Equals(b._Title)) return b._IsRead;
            }
            // False si el libro no se ha encontrado (no existe)
            return false;
        }
    }
}
