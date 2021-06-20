
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neo4jPSR
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new GraphClient(new Uri("http://localhost:7474/"), "neo4j", "Start.123");

            await client.ConnectAsync();
           
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
                        var books = await client.Cypher
                                     .Match("(m:Book)")
                                     .Return(m => m.As<Book>())
                                     .ResultsAsync;

                        foreach (var record in books)
                            Console.WriteLine($"{record.id} = > Nazwa książki: {record.name}, Autor: {record.author}, Rok wydania: {record.year}");
                        break;
                    case "2":

                        var booksCount = await client.Cypher
                                     .Match("(m:Book)")
                                     .Return(m => m.Count())
                                     .ResultsAsync;
                        foreach (var record in booksCount)
                            Console.WriteLine($"Liczba książek: {record}");
                        break;
                    case "3":

                        Console.WriteLine("podaj nazwe książki ");
                        string name = Console.ReadLine();
                        Console.WriteLine("podaj autora");
                        string author = Console.ReadLine();
                        Console.WriteLine("podaj rok wydania");
                        int year = int.Parse(Console.ReadLine());

                        var newBook = new Book { id = Guid.NewGuid().ToString(), name = name, author = author, year = year };
                        await client.Cypher
                            .Create("(book:Book $newBook)")
                            .WithParam("newBook", newBook)
                            .ExecuteWithoutResultsAsync();
                        break;
                    case "4":
                        Console.WriteLine("wpisz id do edycji");
                        string idToEdit = Console.ReadLine();
                        int endEdit = 0;
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
                                    string name2 = Console.ReadLine();
                                    await client.Cypher
                                            .Match("(book:Book)")
                                            .Where((Book book) => book.id == idToEdit)
                                            .Set("book.name = $name")
                                            .WithParam("name", name2)
                                            .ExecuteWithoutResultsAsync();
                                    break;
                                case "2":
                                    Console.WriteLine("podaj nowego autora");
                                    string author2 = Console.ReadLine();
                                    await client.Cypher
                                            .Match("(book:Book)")
                                            .Where((Book book) => book.id == idToEdit)
                                            .Set("book.author = $author")
                                            .WithParam("author", author2)
                                            .ExecuteWithoutResultsAsync();
                                    break;
                                case "3":
                                    Console.WriteLine("podaj nowy rok wydania");
                                    int year2 = int.Parse(Console.ReadLine());
                                    await client.Cypher
                                            .Match("(book:Book)")
                                            .Where((Book book) => book.id == idToEdit)
                                            .Set("book.year = $year")
                                            .WithParam("year", year2)
                                            .ExecuteWithoutResultsAsync();
                                    break;
                                case "4":
                                    endEdit = 1;
                                    break;
                                default:
                                    Console.WriteLine("zły numer");
                                    break;
                            }

                        }

                        break;
                    case "5":

                        Console.WriteLine("wpisz id do usuniecia");
                        string objToDelete = Console.ReadLine();
                        await client.Cypher
                            .Match("(book:Book)")
                            .Where((Book book) => book.id == objToDelete)
                            .Delete("book")
                            .ExecuteWithoutResultsAsync();
                        break;
                    case "6":
                        Console.WriteLine("podaj autora");
                        string authorA = Console.ReadLine();
                        var booksA = await client.Cypher
                                     .Match("(book:Book)")
                                     .Where((Book book) => book.author == authorA)
                                     .Return(book => book.As<Book>())
                                     .ResultsAsync;

                        foreach (var record in booksA)
                            Console.WriteLine($"{record.id} = > Nazwa książki: {record.name}, Autor: {record.author}, Rok wydania: {record.year}");
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

        }
            
     }
}
