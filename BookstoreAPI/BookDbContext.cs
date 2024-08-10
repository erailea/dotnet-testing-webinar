using BookstoreAPI.Models;

public class BookDbContext
{
    public BookDbContext(){
        Books = new List<Book>();
    }
    public List<Book>? Books { get; set; }
}