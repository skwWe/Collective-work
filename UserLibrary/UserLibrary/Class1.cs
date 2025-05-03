sharp
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

        public static void DeleteUser()
        {
            Console.Write("\nВведите ID пользователя для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Некорректный ID.");
                return;
            }

            UsersLibrary userToDelete = users.FirstOrDefault(u => u.id == id);

            if (userToDelete == null)
            {
                Console.WriteLine("Пользователь с таким ID не найден.");
                return;
            }

            users.Remove(userToDelete);
            SaveUsersToCsv(filePath);
            Console.WriteLine("Пользователь удален успешно.");
        }

        public static void SearchUsers()
        {
            Console.Write("\nВведите строку для поиска (имя, фамилия, страна): ");
            string searchTerm = Console.ReadLine();

            List<UsersLibrary> searchResults = users.Where(u =>
                u.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.surname.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.country.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            if (searchResults.Count > 0)
            {
                Console.WriteLine("\nРезультаты поиска:");
                foreach (var user in searchResults)
                {
                    Console.WriteLine(user.ToString());
                }
            }
            else
            {
                Console.WriteLine("Пользователи не найдены.");
            }
        }

        public static void FilterUsersByAge()
        {
            Console.Write("\nВведите минимальный возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int minAge))
            {
                Console.WriteLine("Некорректный минимальный возраст.");
                return;
            }

            Console.Write("Введите максимальный возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int maxAge))
            {
                Console.WriteLine("Некорректный максимальный возраст.");
                return;
            }

            List<UsersLibrary> filteredUsers = users.Where(u => u.age >= minAge && u.age <= maxAge).ToList();

            if (filteredUsers.Count > 0)
            {
                Console.WriteLine("\nРезультаты фильтрации:");
                foreach (var user in filteredUsers)
                {
                    Console.WriteLine(user.ToString());
                }
            }
            else
            {
                Console.WriteLine("Пользователи не найдены.");
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

        public class Program
        {
            static void Main(string[] args)
            {
                string filePath = "users.csv";
                UserData.LoadUsersFromCsv(filePath); // Load initially

                while (true)
                {
                    Console.WriteLine("\nМеню управления пользователями:");
                    Console.WriteLine("1. Вывести всех пользователей");
                    Console.WriteLine("2. Добавить пользователя");
                    Console.WriteLine("3. Изменить пользователя");
                    Console.WriteLine("4. Удалить пользователя");
                    Console.WriteLine("5. Поиск пользователя");
                    Console.WriteLine("6. Фильтровать пользователей по возрасту");
                    Console.WriteLine("7. Сохранить и выйти");
                    Console.Write("Выберите действие: ");

                    if (!int.TryParse(Console.ReadLine(), out int choice))
                    {
                        Console.WriteLine("Некорректный ввод. Пожалуйста, введите число от 1 до 8.");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            UserData.GetAllUsers();
                            break;
                        case 2:
                            UserData.AddUser();
                            break;
                        case 3:
                            UserData.UpdateUser();
                            break;
                        case 4:
                            UserData.DeleteUser();
                            break;
                        case 5:
                            UserData.SearchUsers();
                            break;
                        case 6:
                            UserData.FilterUsersByAge();
                            break;
                        case 7:
                            UserData.SaveUsersToCsv(filePath);
                            Console.WriteLine("Данные сохранены. Выход.");
                            return;
                        default:
                            Console.WriteLine("Неверный выбор. Пожалуйста, выберите от 1 до 8.");
                            break;
                    }

                    Console.ReadKey(); // Чтобы консоль не закрылась сразу
                }
            }
        }
    }
}
