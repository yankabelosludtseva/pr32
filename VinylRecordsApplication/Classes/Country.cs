using System;
using System.Collections.Generic;
using System.Data;

namespace VinylRecordsApplication.Classes
{
    public class Country
    {
        /// <summary> Код страны
        public int Id { get; set; }
        /// <summary> Наименование страны
        public string Name { get; set; }
        /// <summary> Получение всех стран
        public static IEnumerable<Country> AllCountries()
        {
            // Создаём список стран
            List<Country> countries = new List<Country>();
            // Получаем страны из БД
            DataTable requestCountrys = DBConnection.Connection("SELECT * FROM [dbo].[Country]");
            // Перебираем строки и вносим их в список
            foreach (DataRow row in requestCountrys.Rows)
            {
                countries.Add(new Country()
                {
                    Id = Convert.ToInt32(row[0]),
                    Name = row[1].ToString()
                });
            }
            // Возвращаем список
            return countries;
        }
    }
}