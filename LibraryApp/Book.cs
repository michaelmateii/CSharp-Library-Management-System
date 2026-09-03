using System;
using System.Collections.Generic;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public bool IsAvailable { get; set; }
    public string BorrowedByCustomerID { get; set; }
    public DateTime? LoanDate { get; set; }
    public List<string> ReservationQueue { get; set; }

    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        IsAvailable = true;
        BorrowedByCustomerID = null;
        LoanDate = null;
        ReservationQueue = new List<string>();
    }

    public string GetDetails()
    {
        string status = IsAvailable ? "Available" : $"Loaned to {BorrowedByCustomerID}";
        string reservedBy = ReservationQueue.Count > 0
            ? $" | Reserved by: {string.Join(", ", ReservationQueue)}"
            : "";

        return $"{Title} by {Author} (ISBN: {ISBN}) - {status}{reservedBy}";
    }
}
