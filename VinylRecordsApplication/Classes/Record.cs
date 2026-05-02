using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace VinylRecordsApplication.Classes
{
    public class Record
    {
        /// <summary> Код пластинки
        public int Id { get; set; }
        /// <summary> Наименование пластинки
        public string Name { get; set; }
        /// <summary> Год выпуска пластинки
        public int Year { get; set; }
        /// <summary> Формат записи пластинки 0 - МОНО 1 - СТЕРЕО
        public int Format { get; set; }
        /// <summary> Размер пластинки 0:7 дюймов, 1:10 дюймов, 2:12 дюймов, 3:Иное
        public int Size { get; set; }
        /// <summary> Код производителя
        public int IdManufacturer { get; set; }
        /// <summary> Стоимость
        public float Price { get; set; }
        /// <summary> Состояние 0:SS, 1:EX, 2:M, 3:NM, 4:VG, 5:G, 6:F, 7:P, 8:B
        public int IdState { get; set; }
        /// <summary> Заметки
        public string Description { get; set; }

        /// <summary> Получение всех записей из БД
        public static IEnumerable<Record> AllRecords()
        {
            // Создаём список с записями о пластинке
            List<Record> records = new List<Record>();
            // Получаем список из базы данных
            DataTable recordQuery = Classes.DBConnection.Connection("SELECT * FROM [dbo].[Record]");
            // Читаем строки заполняя данные в классе
            foreach (DataRow row in recordQuery.Rows)
                records.Add(new Record()
                {
                    Id = Convert.ToInt32(row[0]),
                    Name = row[1].ToString(),
                    Year = Convert.ToInt32(row[2]),
                    Format = Convert.ToInt32(row[3]),
                    Size = Convert.ToInt32(row[4]),
                    IdManufacturer = Convert.ToInt32(row[5]),
                    Price = float.Parse(row[6].ToString()),
                    IdState = Convert.ToInt32(row[7]),
                    Description = row[8].ToString()
                });
            // Возвращаем список
            return records;
        }

        /// <summary> Добавление или обновление записи
        public void Save(bool Update = false)
        {
            // Корректируем цену, заменяя запятую на точку
            string CorrectPrice = this.Price.ToString().Replace(",", ".");
            // Если данные необходимо внести
            if (Update == false)
            {
                // Выполняем SQL запрос на добавление записи
                Classes.DBConnection.Connection(
                    "INSERT INTO " +
                    "[dbo].[Record](" +
                    "[Name], " +
                    "[Year], " +
                    "[Format], " +
                    "[Size], " +
                    "[IdManufacturer], " +
                    "[Price], " +
                    "[IdState], " +
                    "[Description]) " +
                    "VALUES(" +
                    $"N'{this.Name}', " +
                    $"{this.Year}, " +
                    $"{this.Format}, " +
                    $"{this.Size}, " +
                    $"{this.IdManufacturer}, " +
                    $"{CorrectPrice}, " +
                    $"{this.IdState}, " +
                    $"N'{this.Description}');");

                // Получаем запись обратно
                // Получаем ID записи
                this.Id = Record.AllRecords().Where(
                    x => x.Name == this.Name &&
                    x.Year == this.Year &&
                    x.Format == this.Format &&
                    x.Size == this.Size &&
                    x.IdManufacturer == this.IdManufacturer &&
                    x.IdState == this.IdState &&
                    x.Description == this.Description).First().Id;
            }
            else
                // Если данные необходимо изменить
                // Выполняем SQL запрос на обновление данных в БД
                Classes.DBConnection.Connection(
                    "UPDATE [dbo].[Record] " +
                    $"SET [Name] = N'{this.Name}', " +
                    $"[Year] = {this.Year}, " +
                    $"[Format] = {this.Format}, " +
                    $"[Size] = {this.Size}, " +
                    $"[IdManufacturer] = {this.IdManufacturer}, " +
                    $"[Price] = {CorrectPrice}, " +
                    $"[IdState] = {this.IdState}, " +
                    $"[Description] = N'{this.Description}' " +
                    $"WHERE [Id] = {this.Id}");
        }

        /// <summary> Удаление записи о пластинке
        public void Delete()
        {
            // Выполняем SQL запрос на удаление данных в БД
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[Record] WHERE [Id] = {this.Id};");
        }
    }
}