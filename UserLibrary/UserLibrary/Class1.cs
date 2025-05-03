
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UserLibrary
{
    public class UsersLibrary
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string country { get; set; }
        public int age { get; set; }

        public override string ToString()
        {
            return $"ID: {id}, Имя: {name}, Фамилия: {surname}, Страна: {country}, Возраст: {age}";
        }
    }

    public static class UserData
    {
        private static List<UsersLibrary> users = new List<UsersLibrary>();
        private static string filePath = "users.csv";

        public static void LoadUsersFromCsv(string filePath)
        {
            users.Clear();
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    if (reader.ReadLine() == null)
                    {
                        Console.WriteLine("Файл пуст или не содержит заголовок");
                        return;
                    }

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] values = line.Split(',');
                        if (values.Length == 5)
                        {
                            UsersLibrary user = new UsersLibrary();
                            if (int.TryParse(values[0], out int id)) user.id = id;
                            user.name = values[1];
                            user.surname = values[2];
                            user.country = values[3];
                            if (int.TryParse(values[4], out int age)) user.age = age;
                            users.Add(user);
                        }
                        else
                        {
                            Console.WriteLine($"Некорректная строка в CSV файле: {line}");
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл не найден: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            }
        }

        public static void SaveUsersToCsv(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("id,name,surname,country,age");

                    foreach (var user in users)
                    {
                        writer.WriteLine($"{user.id},{user.name},{user.surname},{user.country},{user.age}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи файла: {ex.Message}");
            }
        }

        public static void UpdateUser()
        {
            Console.Write("\nВведите ID пользователя для изменения: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            UsersLibrary userToUpdate = users.FirstOrDefault(u => u.id == id);

            if (userToUpdate == null)
            {
                Console.WriteLine("Пользователь с таким ID не найден.");
                return;
            }

            Console.WriteLine($"Текущие данные пользователя: {userToUpdate.ToString()}");

            Console.Write("Введите новое имя (оставьте пустым, чтобы не изменять): ");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                userToUpdate.name = newName;
            }

            Console.Write("Введите новую фамилию (оставьте пустым, чтобы не изменять): ");
            string newSurname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newSurname))
            {
                userToUpdate.surname = newSurname;
            }

            Console.Write("Введите новую страну (оставьте пустым, чтобы не изменять): ");
            string newCountry = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newCountry))
            {
                userToUpdate.country = newCountry;
            }

            Console.Write("Введите новый возраст (оставьте пустым, чтобы не изменять): ");
            if (int.TryParse(Console.ReadLine(), out int newAge))
            {
                userToUpdate.age = newAge;
            }

            SaveUsersToCsv(filePath);
            Console.WriteLine("Данные пользователя успешно обновлены.");
        }

        public static void AddUser()
        {
            Console.WriteLine("\nДобавление нового пользователя:");

            UsersLibrary newUser = new UsersLibrary();

            Console.Write("Введите ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID. Пользователь не добавлен.");
                return;
            }
            newUser.id = id;

            Console.Write("Введите имя: ");
            newUser.name = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            newUser.surname = Console.ReadLine();

            Console.Write("Введите страну: ");
            newUser.country = Console.ReadLine();

            Console.Write("Введите возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Некорректный возраст. Пользователь не добавлен.");
                return;
            }
            newUser.age = age;

            users.Add(newUser);
            SaveUsersToCsv(filePath);

            Console.WriteLine("Пользователь добавлен успешно.");
        }




        public static void FindUsers()
        {
            Console.WriteLine("\nПоиск пользователей. Оставьте поле пустым, чтобы не учитывать его в фильтре.");

            Console.Write("Имя: ");
            string name = Console.ReadLine()?.Trim();

            Console.Write("Фамилия: ");
            string surname = Console.ReadLine()?.Trim();

            Console.Write("Страна: ");
            string country = Console.ReadLine()?.Trim();

            Console.Write("Возраст: ");
            string ageInput = Console.ReadLine()?.Trim();
            bool ageParsed = int.TryParse(ageInput, out int age);

            var filteredUsers = users.Where(u =>
                (string.IsNullOrEmpty(name) || u.name.Equals(name, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(surname) || u.surname.Equals(surname, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(country) || u.country.Equals(country, StringComparison.OrdinalIgnoreCase)) &&
                (!ageParsed || u.age == age)
            ).ToList();

            if (filteredUsers.Count > 0)
            {
                Console.WriteLine("\nНайденные пользователи:");
                foreach (var user in filteredUsers)
                {
                    Console.WriteLine(user.ToString());
                }
            }
            else
            {
                Console.WriteLine("Пользователи с указанными параметрами не найдены.");
            }
        }






        public static void GetAllUsers()
        {
            if (users.Count > 0)
            {
                Console.WriteLine("\nСписок всех пользователей:");
                foreach (var user in users)
                {
                    Console.WriteLine(user.ToString());
                }
            }
            else
            {
                Console.WriteLine("Список пользователей пуст.");
            }
        }


    }
}


