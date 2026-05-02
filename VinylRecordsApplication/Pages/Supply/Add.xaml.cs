using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VinylRecordsApplication.Pages.Supply
{
    /// <summary> Логика взаимодействия для Add.xaml
    public partial class Add : Page
    {
        /// <summary> Обращаемся к классу поставщиков и получаем всех поставщиков из БД
        IEnumerable<Classes.Manufacturer> AllManufacturers = Classes.Manufacturer.AllManufacturers();
        /// <summary> Обращаемся к классу пластинок и получаем все пластинки из БД
        IEnumerable<Classes.Record> AllRecords = Classes.Record.AllRecords();
        /// <summary> Переменная для данных о изменяемой поставке для изменения
        Classes.Supply changeSupply;

        /// <summary> Конструктор принимаемый данные о поставке для изменения
        public Add(Classes.Supply changeSupply = null)
        {
            // Инициализируем компоненты для последующего взаимодействия
            InitializeComponent();
            // Перебираем всех поставщиков и выводим в выпадающий список
            foreach (var manufacturer in AllManufacturers)
                // Выводим имя поставщика в выпадающий список
                tbManufacturer.Items.Add(manufacturer.Name);
            // Если поставщиков больше 0, выбираем первого поставщика
            if (tbManufacturer.Items.Count > 0)
                // Выбираем первого поставщика
                tbManufacturer.SelectedIndex = 0;
            // Перебираем пластинки
            foreach (var record in AllRecords)
                // Выводим наименование пластинки в выпадающий список
                tbRecord.Items.Add(record.Name);
            // Если пластинок больше 0
            if (tbRecord.Items.Count > 0)
                // Выбираем первую пластинку
                tbRecord.SelectedIndex = 0;
            // Если пришли данные на изменение
            if (changeSupply != null)
            {
                // Запоминаем данные для изменения для последующего взаимодействия
                this.changeSupply = changeSupply;
                // Из всех поставщиков выбираем того поставщика, который числится за поставкой
                // Для этого из всех поставщиков находим по ключу, и находим его индекс в списке
                tbManufacturer.SelectedIndex = AllManufacturers.ToList().FindIndex(x => x.Id == changeSupply.IdManufacturer);
                // Из всех пластинок выбираем пластинку, которая числится в поставке
                // Для этого из всех пластинок, находим по ключу и находим её индекс
                tbRecord.SelectedIndex = AllRecords.ToList().FindIndex(x => x.Id == changeSupply.IdRecord);
                // В поле количества вставляем данные из поставки
                tbCount.Text = changeSupply.Count.ToString();
                // Создаём дату
                DateTime dt = new DateTime();
                // Конвертируем дату из базы данных в созданную дату
                DateTime.TryParse(changeSupply.DateDelivery, out dt);
                // Указываем в поле для выбора даты, дату из данных поставки
                tbDateDelivery.SelectedDate = dt;
                // Изменяем текст на кнопке с "Добавить" на "Изменить"
                addBth.Content = "Изменить";
            }
        }

        /// <summary> Метод добавления/изменения данных
        private void AddSupply(object sender, RoutedEventArgs e)
        {
            // Создаём дату
            DateTime dt = new DateTime();
            // Пытаемся сконвертировать выбранную дату в полю
            // Если дата конвертируется, значит она выбрана
            if (DateTime.TryParse(tbDateDelivery.SelectedDate.ToString(), out dt))
                // Проверяем что кол-во указано
                if (!String.IsNullOrEmpty(tbCount.Text))
                {
                    // Если данных на изменения нет
                    // Значит добавляем запись
                    if (changeSupply == null)
                    {
                        // Создаём экземпляр класса поставки и указываем данные
                        Classes.Supply newSupply = new Classes.Supply()
                        {
                            // В поставщика указываем того поставщика что выбран в поле
                            // Для этого ищем поставщика по наименованию и получаем его Код
                            IdManufacturer = AllManufacturers.Where(x => x.Name == tbManufacturer.SelectedItem.ToString()).First().Id,
                            // В пластинку указываем пластинку которая выбрана в поле
                            // Для этого находим пластинку по наименованию и указываем её Код
                            IdRecord = AllRecords.Where(x => x.Name == tbRecord.SelectedItem.ToString()).First().Id,
                            // Конвертируем значение из поля в число
                            Count = Convert.ToInt32(tbCount.Text),
                            // Указываем дату, предварительно её подкорректировав
                            DateDelivery = CorrectDate(tbDateDelivery.SelectedDate.ToString())
                        };
                        // Вызываем сохранение данных
                        newSupply.Save();
                        // Выводим надпись о том что поставка добавлена
                        MessageBox.Show($"Поставка №{newSupply.Id} успешно добавлена.", "Уведомление");
                        // Обращаемся к главному окну и вызываем метод открытия страниц
                        // Указываем страницу добавления, передавая в неё данные
                        // Тем самым страница добавления станет страницей изменения
                        MainWindow.mainWindow.OpenPage(new Pages.Supply.Add(newSupply));
                    }
                    else
                    {
                        // Если данные существуют, значит необходимо изменить
                        // В поставщика указываем того поставщика что выбран в поле
                        // Для этого ищем поставщика по наименованию и получаем его Код
                        changeSupply.IdManufacturer = AllManufacturers.Where(x => x.Name == tbManufacturer.SelectedItem.ToString()).First().Id;
                        // В пластинку указываем пластинку которая выбрана в поле
                        // Для этого находим пластинку по наименованию и указываем её Код
                        changeSupply.IdRecord = AllRecords.Where(x => x.Name == tbRecord.SelectedItem.ToString()).First().Id;
                        // Конвертируем значение из поля в число
                        changeSupply.Count = Convert.ToInt32(tbCount.Text);
                        // Указываем дату, предварительно её подкорректировав
                        changeSupply.DateDelivery = CorrectDate(tbDateDelivery.SelectedDate.ToString());
                        // Вызываем сохранение данных
                        changeSupply.Save(true);
                        // Выводим надпись о том что поставка добавлена
                        MessageBox.Show($"Поставка №{changeSupply.Id} успешно изменена.", "Уведомление");
                    }
                }
                else
                    // Выводим сообщение об ошибке
                    MessageBox.Show("Пожалуйста, укажите количество поставки.", "Предупреждение");
            else
                // Выводим сообщение об ошибке
                MessageBox.Show("Пожалуйста, укажите дату поставки.", "Предупреждение");
        }

        /// <summary> Функция проверки на цифры
        private void tbPreviewNumber(object sender, TextCompositionEventArgs e)
        {
            // если в строке используется текст, запрещаем ввод
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        /// <summary> Корректировка даты для БД
        public string CorrectDate(string Value)
        {
            // Создаём дату
            DateTime dt = new DateTime();
            // Конвертируем дату в созданную дату
            DateTime.TryParse(Value, out dt);
            // Выводим Год-Месяц-День, а не День.Месяц.Год
            return dt.Year + "-" + dt.Month + "-" + dt.Day;
        }
    }
}