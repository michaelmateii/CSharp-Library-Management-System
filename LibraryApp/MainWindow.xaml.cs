using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace LibraryApp
{
    public sealed partial class MainWindow : Window
    {
        private LibraryLogic library;

        public MainWindow()
        {
            this.InitializeComponent();
            library = new LibraryLogic();

            library = new LibraryLogic();

            // Add some initial customers
            library.RegisterCustomer("Alice Andersson", "C001");
            library.RegisterCustomer("Bob Berg", "C002");
            library.RegisterCustomer("Clara Carlsson", "C003");

            // Add some initial books
            library.AddBook("The Hobbit", "J.R.R. Tolkien", "B001");
            library.AddBook("1984", "George Orwell", "B002");
            library.AddBook("Clean Code", "Robert C. Martin", "B003");
            library.AddBook("The Pragmatic Programmer", "Andrew Hunt", "B004");

            // Loan a book to demonstrate usage
            library.LoanBook("B002", "C001");

            // Simulate that the loan happened 20 days ago
            var book = library.GetAllBooks().Find(b => b.ISBN == "B002");
            if (book != null)
            {
                book.LoanDate = DateTime.Now.AddDays(-20);
            }


            RefreshBookList();
            RefreshCustomerList();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text;
            string author = AuthorBox.Text;
            string isbn = ISBNBox.Text;

            if (!string.IsNullOrWhiteSpace(title) &&
                !string.IsNullOrWhiteSpace(author) &&
                !string.IsNullOrWhiteSpace(isbn))
            {
                bool added = library.AddBook(title, author, isbn);
                if (added)
                {
                    RefreshBookList();
                    TitleBox.Text = "";
                    AuthorBox.Text = "";
                    ISBNBox.Text = "";
                }
                else
                {
                    ShowMessage("Book with same ISBN already exists.");
                }
            }
            else
            {
                ShowMessage("Please fill in all fields.");
            }
        }

        private void RegisterCustomer_Click(object sender, RoutedEventArgs e)
        {
            string name = CustomerNameBox.Text;
            string id = CustomerIdBox.Text;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(id))
            {
                bool added = library.RegisterCustomer(name, id);
                if (added)
                {
                    ShowMessage("Customer registered.");
                    CustomerNameBox.Text = "";
                    CustomerIdBox.Text = "";
                    RefreshCustomerList();
                }
                else
                {
                    ShowMessage("Customer ID already exists.");
                }
            }
            else
            {
                ShowMessage("Please fill in name and ID.");
            }
        }

        private void LoanBook_Click(object sender, RoutedEventArgs e)
        {
            string customerEntry = CustomerSelectBox.SelectedItem as string;
            string bookEntry = BookSelectBox.SelectedItem as string;

            if (customerEntry != null && bookEntry != null)
            {
                string customerId = customerEntry.Split('(', ')')[1];
                string isbn = bookEntry.Split('[', ']')[1];

                bool success = library.LoanBook(isbn, customerId);
                if (success)
                {
                    ShowMessage("Book loaned.");
                    RefreshBookList();
                }
                else
                {
                    ShowMessage("Book is not available or customer not found.");
                }
            }
            else
            {
                ShowMessage("Please select both a customer and a book.");
            }
        }

        private void ReturnBook_Click(object sender, RoutedEventArgs e)
        {
            string customerEntry = CustomerSelectBox.SelectedItem as string;
            string bookEntry = BookSelectBox.SelectedItem as string;

            if (customerEntry != null && bookEntry != null)
            {
                string customerId = customerEntry.Split('(', ')')[1];
                string isbn = bookEntry.Split('[', ']')[1];

                var (success, fee) = library.ReturnBook(isbn, customerId);
                if (success)
                {
                    if (fee > 0)
                        ShowMessage($"Book returned. Late fee: {fee} kr");
                    else
                        ShowMessage("Book returned. No late fee.");

                    RefreshBookList();
                }
                else
                {
                    ShowMessage("Book was not loaned to this customer.");
                }
            }
            else
            {
                ShowMessage("Please select both a customer and a book.");
            }
        }

        private void ReserveBook_Click(object sender, RoutedEventArgs e)
        {
            string customerEntry = CustomerSelectBox.SelectedItem as string;
            string bookEntry = BookSelectBox.SelectedItem as string;

            if (customerEntry != null && bookEntry != null)
            {
                string customerId = customerEntry.Split('(', ')')[1];
                string isbn = bookEntry.Split('[', ']')[1];

                bool reserved = library.ReserveBook(isbn, customerId);
                if (reserved)
                {
                    ShowMessage("Book reserved.");
                    RefreshBookList();
                }
                else
                {
                    ShowMessage("Reservation failed. Book might be available or already reserved.");
                }
            }
            else
            {
                ShowMessage("Please select a customer and a book.");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchBox.Text.Trim();
            if (!string.IsNullOrEmpty(keyword))
            {
                var results = library.SearchBooks(keyword);
                BookList.Items.Clear();
                BookSelectBox.Items.Clear();
                foreach (var book in results)
                {
                    BookList.Items.Add(book.GetDetails());
                    BookSelectBox.Items.Add($"{book.Title} [{book.ISBN}]");
                }
            }
            else
            {
                ShowMessage("Please enter a keyword.");
            }
        }

        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            RefreshBookList();
        }

        private void RefreshBookList()
        {
            BookList.Items.Clear();
            BookSelectBox.Items.Clear();
            foreach (var book in library.GetAllBooks())
            {
                string details = book.GetDetails();
                BookList.Items.Add(details);
                BookSelectBox.Items.Add($"{book.Title} [{book.ISBN}]");
            }
        }

        private void RefreshCustomerList()
        {
            CustomerSelectBox.Items.Clear();
            foreach (var customer in library.GetAllCustomers())
            {
                CustomerSelectBox.Items.Add($"{customer.Name} ({customer.CustomerID})");
            }
        }

        private void ShowMessage(string message)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Notice",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            _ = dialog.ShowAsync();
        }
    }
}
