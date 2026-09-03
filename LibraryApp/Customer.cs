using System.Collections.Generic;

public class Customer
{
    public string Name { get; set; }
    public string CustomerID { get; set; }
    public List<Book> LoanedBooks { get; private set; }

    public Customer(string name, string id)
    {
        Name = name;
        CustomerID = id;
        LoanedBooks = new List<Book>();
    }

    public void BorrowBook(Book book)
    {
        LoanedBooks.Add(book);
    }

    public void ReturnBook(Book book)
    {
        LoanedBooks.Remove(book);
    }

    public List<Book> GetLoanedBooks()
    {
        return LoanedBooks;
    }
}
