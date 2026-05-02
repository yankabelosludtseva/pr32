using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace VinylRecordsApplication.Classes
{
    public class Manufacturer
    {
        /// <summary> Код поставщика
        public int Id { get; set; }

        /// <summary> Наименование поставщика
        public string Name { get; set; }

        /// <summary> Код страны
        public int CountryCode { get; set; }

        /// <summary> Телефон
        public string Phone { get; set; }

        /// <summary> Почта
        public string Mail { get; set; }

        /// <summary> Список всех поставщиков
        public static IEnumerable<Manufacturer> AllManufacturers()
        {
            // Создаём список поставщиков
            List<Manufacturer> manufacturers = new List<Manufacturer>();
            // Выполняем запрос к базе данных, на получение всех данных
            DataTable recordQuery = Classes.DBConnection.Connection("SELECT * FROM [dbo].[Manufacturer]");
            // Перебираем строки в запросе
            foreach (DataRow row in recordQuery.Rows)
                // Добавляем в список поставщика, присваивая данные на свои места
                manufacturers.Add(new Manufacturer()
                {
                    Id = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    CountryCode = Convert.ToInt32(row[2]),
                    Phone = row[3].ToString(),
                    Mail = row[4].ToString()
                });
            // Возвращаем список поставщиков
            return manufacturers;
        }

        /// <summary> Добавление или обновление записи
        public void Save(bool Update = false)
        {
            // Если добавление записи
            if (Update == false)
            {
                // Создаём запрос на добавление записи, выполняя SQL код
                Classes.DBConnection.Connection(
                    "INSERT INTO [dbo].[Manufacturer]([Name], [CountryCode], [Phone], [Mail]) " +
                    "VALUES (" +
                    $"N'{this.Name}', " +
                    $"{this.CountryCode}, " +
                    $"'{this.Phone}', " +
                    $"'{this.Mail}')"
                );

                // вытаскиваем обратно ID, необходимо для изменения
                // Ищем среди всех поставщиков, поставщика у которого полностью совпадают данные и получаем ID
                this.Id = Manufacturer.AllManufacturers().Where(
                    x => x.Name == this.Name &&
                    x.CountryCode == this.CountryCode &&
                    x.Phone == this.Phone &&
                    x.Mail == this.Mail).First().Id;
            }
            else
                // если у нас выполняется обновление данных о поставщике
                // Создаём запрос на обновление записи, выполняя SQL код
                Classes.DBConnection.Connection(
                    "UPDATE [dbo].[Manufacturer] SET " +
                    $"[Name] = N'{this.Name}', " +
                    $"[CountryCode] = {this.CountryCode}, " +
                    $"[Phone] = '{this.Phone}', " +
                    $"[Mail] = '{this.Mail}' " +
                    $"WHERE [Id] = {this.Id};"
                );
        }

        /// <summary> Удаление записи о поставщике
        public void Delete() =>
            // Выполняем SQL код, который удаляет данные о поставщике по ID
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[Manufacturer] WHERE [Id] = {this.Id};");
    }
}