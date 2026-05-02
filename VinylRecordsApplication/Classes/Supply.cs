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

        /// <summary> Код поставщика (исправлено!)
        public int IdManufacturer { get; set; }

        /// <summary> Код пластинки
        public int IdRecord { get; set; }

        /// <summary> Дата доставки
        public string DateDelivery { get; set; }

        /// <summary> Кол-во доставки
        public int Count { get; set; }

        /// <summary> Получение данных о всех поставках
        public static IEnumerable<Supply> AllSupplies()
        {
            List<Supply> supplies = new List<Supply>();
            DataTable recordQuery = Classes.DBConnection.Connection("SELECT * FROM [dbo].[Supple]");

            foreach (DataRow row in recordQuery.Rows)
            {
                DateTime dt = new DateTime();
                DateTime.TryParse(row[3].ToString(), out dt);
                string CorrectDate = dt.Year + "-" + dt.Month + "-" + dt.Day;

                supplies.Add(new Supply()
                {
                    Id = Convert.ToInt32(row[0]),
                    // ✅ Исправлено: теперь имя свойства совпадает
                    IdManufacturer = Convert.ToInt32(row[1]),
                    IdRecord = Convert.ToInt32(row[2]),
                    DateDelivery = CorrectDate,
                    Count = Convert.ToInt32(row[4])
                });
            }
            return supplies;
        }

        /// <summary> Сохранения данных
        public void Save(bool Update = false)
        {
            if (Update == false)
            {
                // ✅ Исправлено: [IdManufacturer] вместо [IdManufacruer]
                Classes.DBConnection.Connection(
                    "INSERT INTO [dbo].[Supple]([IdManufacturer], [IdRecord], [DateDelivery], [Count]) " +
                    $"VALUES ({this.IdManufacturer}, {this.IdRecord}, '{this.DateDelivery}', {this.Count});");

                this.Id = Supply.AllSupplies().Where(
                    x => x.IdManufacturer == this.IdManufacturer &&
                    x.IdRecord == this.IdRecord &&
                    x.DateDelivery == this.DateDelivery &&
                    x.Count == this.Count).First().Id;
            }
            else
            {
                Classes.DBConnection.Connection(
                    "UPDATE [dbo].[Supple] " +
                    "SET " +
                    $"[IdManufacturer] = {this.IdManufacturer}, " +
                    $"[IdRecord] = {this.IdRecord}, " +
                    $"[DateDelivery] = '{this.DateDelivery}', " +
                    $"[Count] = {this.Count} " +
                    $"WHERE [Id] = {this.Id};");
            }
        }

        /// <summary> Удаление записи
        public void Delete()
        {
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[Supple] WHERE [Id] = {this.Id};");
        }
    }
}