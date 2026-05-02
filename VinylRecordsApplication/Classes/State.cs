using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace VinylRecordsApplication.Classes
{
    public class State
    {
        /// <summary> Код состояния
        public int Id { get; set; }
        /// <summary> Наименование
        public string Name { get; set; }
        /// <summary> Сокращённое наименование
        public string Subname { get; set; }
        /// <summary> Описание
        public string Description { get; set; }

        /// <summary> Получение всех состояний из базы данных
        public static IEnumerable<State> AllState()
        {
            // Создаём коллекцию состояний
            List<State> allState = new List<State>();
            // Получаем состояния из БД
            DataTable requestStates = DBConnection.Connection("SELECT * FROM [dbo].[State]");
            // Перебираем строки в полученном ответе
            foreach (DataRow state in requestStates.Rows)
                // Добавляем новое состояние в коллекцию
                allState.Add(new State()
                {
                    Id = Convert.ToInt32(state[0]),
                    Name = state[1].ToString(),
                    Subname = state[2].ToString(),
                    Description = state[3].ToString()
                });

            // Возвращаем коллекцию
            return allState;
        }

        /// <summary> Метод сохранения
        public void Save(bool Update = false)
        {
            // Если запись не обновляется
            if (Update == false)
            {
                // Создаём запрос на добавление записи в Базу данных
                Classes.DBConnection.Connection(
                    "INSERT INTO [dbo].[State]([Name], [Subname], [Description]) " +
                    $"VALUES (N'{this.Name}', N'{this.Subname}', N'{this.Description}');");
                // Вытягиваем Код записи по наименованию, сокращённому наименованию, Описанию
                this.Id = AllState().Where(x => x.Name == this.Name &&
                                               x.Subname == this.Subname &&
                                               x.Description == this.Description).First().Id;
            }
            else
            {
                // Если запись обновляется
                // Создаём запрос на обновление записи в Базе данных
                Classes.DBConnection.Connection(
                    "UPDATE [dbo].[State] SET " +
                    $"[Name] = N'{this.Name}', " +
                    $"[Subname] = N'{this.Subname}', " +
                    $"[Description] = N'{this.Description}' " +
                    $"WHERE [Id] = {this.Id};");
            }
        }

        /// <summary> Удаление записи о состоянии
        public void Delete()
        {
            // Выполняем SQL запрос на удаление данных в БД
            Classes.DBConnection.Connection($"DELETE FROM [dbo].[State] WHERE [Id] = {this.Id};");
        }
    }
}