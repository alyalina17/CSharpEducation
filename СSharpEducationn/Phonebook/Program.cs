using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Abonent
{
    public string PhoneNumber { get; set; }
    public string Name { get; set; }

    public Abonent(string phoneNumber, string name)
    {
        PhoneNumber = phoneNumber;
        Name = name;
    }
}

public class Phonebook
{
    private static Phonebook _instance;
    private List<Abonent> _abonents;
    private const string FilePath = "phonebook.txt";


    private Phonebook()
    {
        _abonents = new List<Abonent>();
        LoadFromFile();
    }


    public static Phonebook Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Phonebook();
            }
            return _instance;
        }
    }


    public bool AddAbonent(string phoneNumber, string name)
    {

        if (_abonents.Any(a => a.PhoneNumber == phoneNumber || a.Name == name))
        {
            return false;
        }

        _abonents.Add(new Abonent(phoneNumber, name));
        SaveToFile();
        return true;
    }


    public bool RemoveAbonentByPhone(string phoneNumber)
    {
        var abonent = _abonents.FirstOrDefault(a => a.PhoneNumber == phoneNumber);
        if (abonent != null)
        {
            _abonents.Remove(abonent);
            SaveToFile();
            return true;
        }
        return false;
    }

    public string GetNameByPhone(string phoneNumber)
    {
        var abonent = _abonents.FirstOrDefault(a => a.PhoneNumber == phoneNumber);
        return abonent?.Name;
    }


    public string GetPhoneByName(string name)
    {
        var abonent = _abonents.FirstOrDefault(a => a.Name == name);
        return abonent?.PhoneNumber;
    }


    public List<Abonent> GetAllAbonents()
    {
        return new List<Abonent>(_abonents);
    }

    private void SaveToFile()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                foreach (var abonent in _abonents)
                {
                    writer.WriteLine($"{abonent.PhoneNumber}|{abonent.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
        }
    }


    private void LoadFromFile()
    {
        if (!File.Exists(FilePath)) return;

        try
        {
            _abonents.Clear();
            var lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    _abonents.Add(new Abonent(parts[0], parts[1]));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке: {ex.Message}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var phonebook = Phonebook.Instance;

        while (true)
        {
            Console.WriteLine("\nТелефонная книга");
            Console.WriteLine("1. Добавить абонента");
            Console.WriteLine("2. Удалить абонента по номеру");
            Console.WriteLine("3. Найти имя по номеру");
            Console.WriteLine("4. Найти номер по имени");
            Console.WriteLine("5. Показать всех абонентов");
            Console.WriteLine("6. Выйти");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите номер телефона: ");
                    var phone = Console.ReadLine();
                    Console.Write("Введите имя: ");
                    var name = Console.ReadLine();

                    if (phonebook.AddAbonent(phone, name))
                    {
                        Console.WriteLine("Абонент добавлен");
                    }
                    else
                    {
                        Console.WriteLine("Абонент с таким номером или именем уже существует");
                    }
                    break;

                case "2":
                    Console.Write("Введите номер для удаления: ");
                    var phoneToRemove = Console.ReadLine();

                    if (phonebook.RemoveAbonentByPhone(phoneToRemove))
                    {
                        Console.WriteLine("Абонент удален");
                    }
                    else
                    {
                        Console.WriteLine("Абонент не найден");
                    }
                    break;

                case "3":
                    Console.Write("Введите номер для поиска: ");
                    var phoneToFind = Console.ReadLine();
                    var foundName = phonebook.GetNameByPhone(phoneToFind);

                    if (foundName != null)
                    {
                        Console.WriteLine($"Имя абонента: {foundName}");
                    }
                    else
                    {
                        Console.WriteLine("Абонент не найден");
                    }
                    break;

                case "4":
                    Console.Write("Введите имя для поиска: ");
                    var nameToFind = Console.ReadLine();
                    var foundPhone = phonebook.GetPhoneByName(nameToFind);

                    if (foundPhone != null)
                    {
                        Console.WriteLine($"Номер телефона: {foundPhone}");
                    }
                    else
                    {
                        Console.WriteLine("Абонент не найден");
                    }
                    break;

                case "5":
                    var allAbonents = phonebook.GetAllAbonents();
                    Console.WriteLine("\nСписок всех абонентов:");
                    foreach (var abonent in allAbonents)
                    {
                        Console.WriteLine($"{abonent.Name}: {abonent.PhoneNumber}");
                    }
                    break;

                case "6":
                    return;

                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
        }
    }
}