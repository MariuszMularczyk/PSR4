using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orient.Client;
using Orient.Client.API.Query.Interfaces;

namespace OrientDBPSR
{
    class Program
    {
        static void Main(string[] args)
        {
            ODatabase database = new ODatabase("localhost", 2424, "PSR", ODatabaseType.Graph, "root", "Start.123");
            //database.Create.Class<Book>().Run();
            int quit = 0;
            while (quit == 0)
            {

                Console.WriteLine("wybierz akcje");
                Console.WriteLine("-------------------------");
                Console.WriteLine("1 - Wyswietl wszystko");
                Console.WriteLine("2 - Wyswietl liczbe książek");
                Console.WriteLine("3 - Dodaj");
                Console.WriteLine("4 - edytuj");
                Console.WriteLine("5 - usun");
                Console.WriteLine("6 - wyświetl książki danego autora");
                Console.WriteLine("7 - Quit");


                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":

                        Console.WriteLine("Wszystkie książki: ");
                        List<ODocument> books = database.Select()
                                                            .From("Book")
                                                            .ToList();

                        foreach (ODocument record in books)
                        {
                            Book book = record.To<Book>();
                            Console.WriteLine($"{record.ORID.ToString()} = > Nazwa książki: {book.Name}, Autor: {book.Author}, Rok wydania: {book.Year}");
                        }
                        break;
                    case "2":

                        int booksCount = database.Select()
                                                            .From("Book")
                                                            .ToList().Count();
                            Console.WriteLine($"Liczba książek: {booksCount}");
                        break;
                    case "3":

                        Console.WriteLine("podaj nazwe książki ");
                        string name = Console.ReadLine();
                        Console.WriteLine("podaj autora");
                        string author = Console.ReadLine();
                        Console.WriteLine("podaj rok wydania");
                        int year = int.Parse(Console.ReadLine());
                        Book bookToInsertt = new Book()
                        {
                            Name = name,
                            Author = author,
                            Year = year
                        };
                        database.Insert(bookToInsertt).Into<Book>().Run();
                        break;
                    case "4":
                        Console.WriteLine("wpisz id do edycji");
                        string idToEdit = Console.ReadLine();


                        List<ODocument> objectsToEdit = database.Select()
                                                            .From("Book").Limit(1)
                                                             .Where("@rid").Equals<string>(idToEdit).ToList();
                        int endEdit = 0;
                        Book objectToEdit = objectsToEdit[0].To<Book>();
                        while (endEdit == 0)
                        {

                            Console.WriteLine("co chcesz edytować");
                            Console.WriteLine("-------------------------");
                            Console.WriteLine("1 - nazwa");
                            Console.WriteLine("2 - autor");
                            Console.WriteLine("3 - rok wydania");
                            Console.WriteLine("4 - zakończ edycje");

                            string choice2 = Console.ReadLine();

                            switch (choice2)
                            {
                                case "1":

                                    Console.WriteLine("podaj nowa nazwe");
                                    objectToEdit.Name = Console.ReadLine();
                                    
                                    break;
                                case "2":
                                    Console.WriteLine("podaj nowego autora");
                                    objectToEdit.Author = Console.ReadLine();
                                   
                                    break;
                                case "3":
                                    Console.WriteLine("podaj nowy rok wydania");
                                    objectToEdit.Year = int.Parse(Console.ReadLine());
                                    
                                    break;
                                case "4":
                                    endEdit = 1;
                                    break;
                                default:
                                    Console.WriteLine("zły numer");
                                    break;
                            }

                        }
                        database.Update<Book>(objectToEdit).Run();
                        break;
                    case "5":

                        Console.WriteLine("wpisz id do usuniecia");
                        string objToDelete = Console.ReadLine();
                        List<ODocument> objectsToDelete = database.Select()
                                                           .From("Book").Limit(1)
                                                            .Where("@rid").Equals<string>(objToDelete).ToList();

                        Book objectToDelete = objectsToDelete[0].To<Book>();
                        database.Delete.Document<Book>(objectToDelete).Run();
                        break;
                    case "6":
                        Console.WriteLine("podaj autora");
                        string authorA = Console.ReadLine();

                        List<ODocument> booksA = database.Select()
                                    .From("Book")
                                    .Where("Author").Equals<string>(authorA)
                                    .ToList();

                        foreach (ODocument record in booksA)
                        {
                            Book book = record.To<Book>();
                            Console.WriteLine($"{record.ORID.ToString()} = > Nazwa książki: {book.Name}, Autor: {book.Author}, Rok wydania: {book.Year}");
                        }
                        break;
                    case "7":
                        quit = 1;
                        Console.WriteLine("Wyjscie");
                        break;
                    default:
                        Console.WriteLine("zły numer");
                        break;
                }
            }
            database.Close();

        }

    }
}
