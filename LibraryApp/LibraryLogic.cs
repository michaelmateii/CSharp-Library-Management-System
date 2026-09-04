using System;
using System.Collections.Generic;
using System.Linq;

public class LibraryLogic
{
    private List<Book> books;
    private List<Customer> customers;

    public LibraryLogic()
    {
        books = new List<Book>();
        customers = new List<Customer>();
    }

    public bool AddBook(string title, string author, string isbn)
    {
        if (books.Any(b => b.ISBN == isbn)) return false;
        books.Add(new Book(title, author, isbn));
        return true;
    }

    public bool RegisterCustomer(string name, string id)
    {
        if (customers.Any(c => c.CustomerID == id)) return false;
        customers.Add(new Customer(name, id));
        return true;
    }

    public bool LoanBook(string isbn, string customerId)
    {
        var book = books.FirstOrDefault(b => b.ISBN == isbn && b.IsAvailable);
        var customer = customers.FirstOrDefault(c => c.CustomerID == customerId);

        if (book != null && customer != null)
        {
            book.IsAvailable = false;
            book.BorrowedByCustomerID = customerId;
            book.LoanDate = DateTime.Now;
            customer.BorrowBook(book);
            return true;
        }
        return false;
    }

    public (bool success, int fee) ReturnBook(string isbn, string customerId)
    {
        var book = books.FirstOrDefault(b => b.ISBN == isbn && !b.IsAvailable);
        if (book != null && book.BorrowedByCustomerID == customerId)
        {
            var customer = customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (customer != null)
            {
                customer.ReturnBook(book);
                int fee = 0;

                if (book.LoanDate.HasValue)
                {
                    int daysLate = (DateTime.Now - book.LoanDate.Value).Days - 7;
                    if (daysLate > 0)
                        fee = daysLate * 10;
                }

                book.IsAvailable = true;
                book.BorrowedByCustomerID = null;
                book.LoanDate = null;

                if (book.ReservationQueue.Count > 0)
                {
                    // Auto-loan to next customer in queue
                    string nextCustomerId = book.ReservationQueue[0];
                    var nextCustomer = customers.FirstOrDefault(c => c.CustomerID == nextCustomerId);
                    if (nextCustomer != null)
                    {
                        book.IsAvailable = false;
                        book.BorrowedByCustomerID = nextCustomerId;
                        book.LoanDate = DateTime.Now;
                        nextCustomer.BorrowBook(book);
                    }
                    book.ReservationQueue.RemoveAt(0);
                }

                return (true, fee);
            }
        }
        return (false, 0);
    }
    public bool ReserveBook(string isbn, string customerId)
    {
        var book = books.FirstOrDefault(b => b.ISBN == isbn && !b.IsAvailable);
        var customer = customers.FirstOrDefault(c => c.CustomerID == customerId);

        if (book != null &&
            customer != null &&
            !book.ReservationQueue.Contains(customerId) &&
            book.ReservationQueue.Count < 2)
        {
            book.ReservationQueue.Add(customerId);
            return true;
        }

        return false;
    }


    public List<Book> SearchBooks(string keyword)
    {
        return books
            .Where(b =>
                b.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Book> GetAllBooks()
    {
        return books;
    }

    public List<Customer> GetAllCustomers()
    {
        return customers;
    }
}
