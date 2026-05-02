using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace VinylRecordsApplication.Classes
{
    public class Supply
    {
        /// <summary> Код поставки
        public int Id { get; set; }
        /// <summary> Код поставщика
        public int IdManufacruer { get; set; }
        /// <summary> Код пластинки
        public int IdRecord { get; set; }
        /// <summary> Дата доставки
        public string DateDelivery { get; set; }
        /// <summary> Кол-во доставки
        public int Count { get; set; }

        /// <summary> Получение данных о всех поставках
        public static IEnumerable<Supply> AllSupplies()
        {
            // Создаём список поставок
            List<Supply> supplies = new List<Supply>();
            // Обращаемся к БД и получаем список
            DataTable recordQuery = Classes.DBConnection.Connection("SELECT * FROM [dbo].[Supple]");
            // Читаем строки
            foreach (DataRow row in recordQuery.Rows)
            {
                // Создаём дату
                DateTime dt = new DateTime();
                // Конвертируем дату из БД
                DateTime.TryParse(row[3].ToString(), out dt);
                // Записываем полученный результат в переменную
                string CorrectDate = dt.Year + "-" + dt.Month + "-" + dt.Day;
                // Заполняем список указывая данные
                supplies.Add(new Supply()
                {
                    Id = Convert.ToInt32(row[0]),
                    IdManufacruer = Convert.ToInt32(row[1]),
                    IdRecord = Convert.ToInt32(row[2]),
                    DateDelivery = CorrectDate,
                    Count = Convert.ToInt32(row[4])
                });
            }
            // Возвращаем список
            return supplies;
        }

        /// <summary> Сохранения данных
        public void Save(bool Update = false)
        {
            // Если создания данных в БД
            if (Update == false)
            {
                // Вызываем SQL запрос, который сохранит данные в БД
                Classes.DBConnection.Connection(
                    "INSERT INTO [dbo].[Supple]([IdManufacruer], [IdRecord], [DateDelivery], [Count]) " +
                    $"VALUES ({this.IdManufacruer}, {this.IdRecord}, '{this.DateDelivery}', {this.Count});");

                // Получаем запись обратно
                // Получаем ID записи
                this.Id = Supply.AllSupplies().Where(
                    x => x.IdManufacruer == this.IdManufacruer &&
                    x.IdRecord == this.IdRecord &&
                    x.DateDelivery == this.DateDelivery &&
                    x.Count == this.Count).First().Id;
            }
            else
            {
                // Если данные необходимо обновить
                // Вызываем SQL запрос на обновление данных
                Classes.DBConnection.Connection(
                    "UPDATE [dbo].[Supple] " +
                    "SET " +
                    $"[IdManufacruer] = {this.IdManufacruer}, " +
                    $"[IdRecord] = {this.IdRecord}, " +
                    $"[DateDelivery] = '{this.DateDelivery}', " +
                    $"[Count] = {this.Count} " +
                    $"WHERE [Id] = {this.Id};");
            }
        }

        /// <summary> Удаление записи о пластинке
        public void Delete()
        {
            // Вызываем SQL запрос на удаление данных
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[Supple] WHERE [Id] = {this.Id};");
        }
    }
}