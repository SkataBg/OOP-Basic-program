using Spectre.Console;
using System.Reflection;
using TCSA.OOP.LibraryManagmentSystem;
using TCSA.OOP.LibraryManagmentSystem.Models;

namespace TCSA.OOP.LibraryManagmentSystem.Controllers;

internal class BooksController : BaseController ,IBaseController
{
    public void ViewItems()
    {
        var table = new Table();
        table.Border(TableBorder.Rounded);

        table.AddColumn("[yellow]ID[/]");
        table.AddColumn("[yellow]Title[/]");
        table.AddColumn("[yellow]Author[/]");
        table.AddColumn("[yellow]Category[/]");
        table.AddColumn("[yellow]Location[/]");
        table.AddColumn("[yellow]Pages[/]");

        var books = MockDatabase.LibraryItems.OfType<Book>();

        foreach (var book in books)
        {
            table.AddRow(
                book.Id.ToString(),
                $"[cyan]{book.Name}[/]",
                $"[cyan]{book.Author}[/]",
                $"[green]{book.Category}[/]",
                $"[blue]{book.Location}[/]",
                book.Pages.ToString()
                );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("Press Any Key To Continue");
        Console.ReadKey(true);
    }
    public void AddItem()
    {
        var title = AnsiConsole.Ask<string>("Enter the [green]title[/] of the book to add:");
        var author = AnsiConsole.Ask<string>("Enter the [green]author[/] of the book:");
        var category = AnsiConsole.Ask<string>("Enter the [green]category[/] of the book:");
        var location = AnsiConsole.Ask<string>("Enter the [green]location[/] of the book:");
        var pages = AnsiConsole.Ask<int>("Enter the [green]number of pages[/] in the book:");

        if (MockDatabase.LibraryItems.OfType<Book>().Any(b => b.Name.Equals(title, StringComparison.OrdinalIgnoreCase)))
        {
            AnsiConsole.MarkupLine("[red]This book already exists[/]");
        }
        else
        {
            var newBook = new Book(MockDatabase.LibraryItems.Count + 1, title, author, category, location, pages);
            MockDatabase.LibraryItems.Add(newBook);
            DisplayMessage("Book added successfuly!", "green");
        }
        DisplayMessage("Press Any Key to Continue!", "green");
        Console.ReadKey(true);
    }
    public void DeleteItem()
    {
        var books = MockDatabase.LibraryItems.OfType<Book>().ToList();
        if (books.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]There are no books available to delete[/]");
            Console.ReadKey(true);
            return;
        }
        var bookToDelete = AnsiConsole.Prompt(
            new SelectionPrompt<Book>()
            .Title("Select a [red]book[/] to delete:")
            .UseConverter(b => $"{b.Name}")
            .AddChoices(books));
        if (ConfirmDeleteion(bookToDelete.Name))
        {
            if (MockDatabase.LibraryItems.Remove(bookToDelete))
            {
                DisplayMessage("Book deleted successfuly!", "red");
            }
            else
            {
                DisplayMessage("Book not found.", "red");
            }
        }
        else
        {
            DisplayMessage("Deletion canceled.", "yellow");
        }

            DisplayMessage("Press Any Key To Continue.", "green");
        Console.ReadKey(true);
    }
}
