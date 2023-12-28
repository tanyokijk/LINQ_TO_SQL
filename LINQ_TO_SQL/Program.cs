using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Linq.SqlClient;

namespace LINQ_TO_SQL
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string connectionString = "Data Source=countries.sqlite;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                CreateTable(connection);

                InsertValues(connection);

                var countries = GetAllCountries(connection);

                PrintAllCountries("Вся інформація про країни:", countries);

                Console.WriteLine("Назви країн:\n");
                foreach (var country in countries)
                {
                    Console.WriteLine(country.NameCountry);
                }

                Console.WriteLine("\nНазви столиць:\n");
                foreach (var country in countries)
                {
                    Console.WriteLine(country.NameCapital);
                }

                Console.WriteLine("\nНазви європейських країн:\n");
                var europeCountries = countries.Where(c => c.Part == "Europe");
                foreach (var country in europeCountries)
                {
                    Console.WriteLine(country.NameCountry);
                }

                Console.WriteLine("\nНазви країн, площа яких більше 1000000:\n");
                var largeCountries = countries.Where(c => c.Area > 1000000);
                foreach (var country in largeCountries)
                {
                    Console.WriteLine(country.NameCountry);
                }

                var auCountries = countries.Where(c => c.NameCountry.ToLower().Contains("a") && c.NameCountry.ToLower().Contains("u")).ToList();
                PrintAllCountries("\nКраїни, в назвах яких є літери ‘a’ та ‘u’:", auCountries);

                var aCountries = countries.Where(c => c.NameCountry.ToLower().Contains("a")).ToList();
                PrintAllCountries("Країни, в назвах яких є літери ‘a’", aCountries);

                Console.WriteLine("Назви країн, площа яких в діапазоні 50 тис - 1 млн:\n");
                Console.WriteLine("{0,-25} {1,-20}\n", "Назва країни", "Площа");
                var midleCountries = countries.Where(c => c.Area < 1000000 && c.Area > 500000);
                foreach (var country in midleCountries)
                {
                    Console.WriteLine("{0,-25} {1,-20}", country.NameCountry, country.Area);
                }

                Console.WriteLine("\nНазви країн, в яких населення більше 1 мільярда:\n");
                Console.WriteLine("{0,-25} {1,-20}\n", "Назва країни", "Населення");
                var PopularCountries = countries.Where(c => c.Number > 1000000000);
                foreach (var country in PopularCountries)
                {
                    Console.WriteLine("{0,-28} {1}", country.NameCountry, country.Number);
                }

                Console.WriteLine("\nТоп-5 країни за площею:\n");
                Console.WriteLine("{0,-28} {1}\n", "   Назва країни", "Площа");
                var theLargestCountries = countries.OrderByDescending(c => c.Area).Take(5).ToList();
                foreach (var country in theLargestCountries)
                {
                    Console.WriteLine("{0,-28} {1}", country.NameCountry, country.Area);
                }

                Console.WriteLine("\nТоп-5 країни за населенням:\n");
                Console.WriteLine("{0,-25} {1,-20}\n", "   Назва країни", "Населення");
                var theMostPopularCountries = countries.OrderByDescending(c => c.Number).Take(5).ToList();
                foreach (var country in theMostPopularCountries)
                {
                    Console.WriteLine("{0,-25} {1,-20}", country.NameCountry, country.Number);
                }

                Console.WriteLine("\nНайбільша країна за площею:\n");
                Console.WriteLine("{0,-25} {1,-20}\n", "Назва країни", "Площа");
                Console.WriteLine("{0,-25} {1,-20}\n", theLargestCountries[0].NameCountry, theLargestCountries[0].Area);

                Console.WriteLine("Найбільша країна за населенням:\n");
                Console.WriteLine("{0,-25} {1,-20}\n", "Назва країни", "Населення");
                Console.WriteLine("{0,-25} {1,-20}\n", theMostPopularCountries[0].NameCountry, theMostPopularCountries[0].Number);

                Console.WriteLine("Країна з найменшою площею в Африці:\n");
                Console.WriteLine("{0,-25} {1,-20}\n", "Назва країни", "Площа");

                var theSmallestCountryInAfrika = countries.OrderBy(c => c.Area).Where(c => c.Part == "Africa").ToList();
                Console.WriteLine("{0,-25} {1,-20}\n", theSmallestCountryInAfrika[0].NameCountry, theSmallestCountryInAfrika[0].Area);

                var averageAreaInAsia = countries.Where(c => c.Part == "Asia").Average(c => c.Area);
                Console.WriteLine("Cередня площа країн в Азії: " + averageAreaInAsia);

                Console.ReadKey();
            }
        }

        static void CreateTable(SQLiteConnection connection)
        {
            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Countries (
                    id INTEGER PRIMARY KEY,
                    name_country TEXT,
                    name_capital TEXT,
                    number INTEGER,
                    area REAL,
                    part TEXT
                );";

            SQLiteCommand commandCreate = new SQLiteCommand(createTableQuery, connection);
            commandCreate.ExecuteNonQuery();
        }

        static void InsertValues(SQLiteConnection connection)
        {
            Country[] countries =
                {
                    new Country("Ukraine", "Kyiv", 41167300, 603628, "Europe"),
                    new Country("Germany", "Berlin", 83190556, 357022, "Europe"),
                    new Country("Brazil", "Brasilia", 213993437, 8515767, "South America"),
                    new Country("India", "New Delhi", 1393409038, 3287263, "Asia"),
                    new Country("Australia", "Canberra", 25788221, 7692024, "Oceania"),
                    new Country("Canada", "Ottawa", 38067906, 9984670, "North America"),
                    new Country("Japan", "Tokyo", 125584838, 377975, "Asia"),
                    new Country("France", "Paris", 67406000, 551695, "Europe"),
                    new Country("Mexico", "Mexico City", 130262216, 1964375, "North America"),
                    new Country("Egypt", "Cairo", 104258327, 1002450, "Africa"),
                    new Country("South Africa", "Pretoria", 60041935, 1221037, "Africa"),
                };

            string insertQuery = "INSERT INTO Countries (name_country, name_capital, number, area, part) VALUES (@NameCountry, @NameCapital, @Number, @Area, @Part);";
            using (SQLiteCommand commandInsert = new SQLiteCommand(insertQuery, connection))
            {
                foreach (Country country in countries)
                {
                    AddCountryParameters(commandInsert, country.NameCountry, country.NameCapital, country.Number, country.Area, country.Part);
                    commandInsert.ExecuteNonQuery();
                }
            }
        }

        static void AddCountryParameters(SQLiteCommand command, string nameCountry, string nameCapital, int number, float area, string part)
        {
            command.Parameters.AddWithValue("@NameCountry", nameCountry);
            command.Parameters.AddWithValue("@NameCapital", nameCapital);
            command.Parameters.AddWithValue("@Number", number);
            command.Parameters.AddWithValue("@Area", area);
            command.Parameters.AddWithValue("@Part", part);
        }

        static List<Country> GetAllCountries(SQLiteConnection connection)
        {
            List<Country> countries = new List<Country>();

            string selectQuery = "SELECT * FROM Countries;";

            using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string nameCountry = Convert.ToString(reader["name_country"]);
                        string nameCapital = Convert.ToString(reader["name_capital"]);
                        int number = Convert.ToInt32(reader["number"]);
                        float area = Convert.ToSingle(reader["area"]);
                        string part = Convert.ToString(reader["part"]);

                        Country country = new Country(nameCountry, nameCapital, number, area, part);
                        countries.Add(country);
                    }
                }
            }

            return countries;
        }

        static void PrintAllCountries(string text, List<Country> countries)
        {
            Console.WriteLine(text);
            Console.WriteLine();
            Console.WriteLine("{0,-25} {1,-20} {2,-15} {3,-15} {4,-15}\n", "Назва країни", "Назва столиці", "Населення", "Площа", "Частина світу");
            foreach (var country in countries)
            {
                Console.WriteLine("{0,-25} {1,-20} {2,-15} {3,-15} {4,-15}", country.NameCountry, country.NameCapital, country.Number, country.Area, country.Part);
            }

            Console.WriteLine();
        }
    }
}
