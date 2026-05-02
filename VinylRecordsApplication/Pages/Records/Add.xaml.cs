using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VinylRecordsApplication.Pages.Records
{
    /// <summary> Логика взаимодействия для Add.xaml
    public partial class Add : Page
    {
        // Обращаемся к поставщикам и получаем всех из БД
        public IEnumerable<Classes.Manufacturer> Manufacturers = Classes.Manufacturer.AllManufacturers();

        public IEnumerable<Classes.State> AllState = Classes.State.AllState();
        // Переменная для изменения данных о пластинке
        private Classes.Record changeRecord;

        public Add(Classes.Record changeRecord = null)
        {
            // Инициализируем компоненты для того чтобы с ними работать
            InitializeComponent();
            // Перебираем всех поставщиков
            foreach (var item in Manufacturers)
                // Добавляем поставщика в выпадающий список
                tbManufacturer.Items.Add(item.Name);
            // Если кол-во поставщиков больше 0
            if (Manufacturers.Count() > 0)
                // Выбираем первого поставщика
                tbManufacturer.SelectedIndex = 0;
            // Добавляем состояния в выпадающий список
            foreach (var item in AllState)
                tbState.Items.Add(item.Name);
            // Если у нас есть состояния
            if (AllState.Count() > 0)
                // Выбираем по умолчанию первый
                tbState.SelectedIndex = 0;
            // Если пришли данные о пластинке на изменение
            if (changeRecord != null)
            {
                // Сохраняем данные о пластинке
                this.changeRecord = changeRecord;
                // Выводим наименование
                tbName.Text = changeRecord.Name;
                // Выводим год
                tbYear.Text = changeRecord.Year.ToString();
                // Выводим стоимость
                tbPrice.Text = changeRecord.Price.ToString().Replace(",", ".");
                // Выводим описание
                tbDescription.Text = changeRecord.Description;
                // Выбираем формат
                tbFormat.SelectedIndex = changeRecord.Format;
                // Выбираем поставщика получая его по уникальному коду
                tbManufacturer.SelectedIndex = Manufacturers.ToList().FindIndex(x => x.Id == changeRecord.IdManufacturer);
                // Выбираем размер
                tbSize.SelectedIndex = changeRecord.Size;
                // Выбираем состояние
                tbState.SelectedIndex = AllState.ToList().FindIndex(x => x.Id == changeRecord.IdState);
                // Изменяем название кнопки с "Добавить" на "Изменить"
                addBth.Content = "Изменить";
            }
        }

        /// <summary> Добавление/Изменение пластинки
        private void AddRecord(object sender, RoutedEventArgs e)
        {
            // Проверяем что поле наименования не пустое
            if (!String.IsNullOrEmpty(tbName.Text))
                // Проверяем что поле года не пустое
                if (!String.IsNullOrEmpty(tbYear.Text))
                    // Проверяем что поле цены не пустое
                    if (!String.IsNullOrEmpty(tbPrice.Text))
                        // Проверяем что наименование меньше 250 символов
                        if (tbName.Text.Length <= 250)
                        {
                            // Добавление записи
                            if (changeRecord == null)
                            {
                                // Создаём новый экземпляр класса пластинки, указывая данные
                                Classes.Record newRecord = new Classes.Record()
                                {
                                    // Наименование
                                    Name = tbName.Text,
                                    // Год
                                    Year = Convert.ToInt32(tbYear.Text),
                                    // Формат
                                    Format = tbFormat.SelectedIndex,
                                    // Размер
                                    Size = tbSize.SelectedIndex,
                                    // Производитель
                                    IdManufacturer = Manufacturers.Where(x => x.Name == tbManufacturer.SelectedValue.ToString()).First().Id,
                                    // Стоимость
                                    Price = float.Parse(tbPrice.Text.Replace(".", ",")),
                                    // Состояние
                                    IdState = AllState.Where(x => x.Name == tbState.SelectedItem.ToString()).First().Id,
                                    // Описание
                                    Description = tbDescription.Text
                                };
                                // Сохраняем данные
                                newRecord.Save();
                                // Выводим сообщение
                                MessageBox.Show($"Пластинка {newRecord.Name} успешно добавлена.", "Уведомление");
                                // Открываем пластинку на изменение
                                MainWindow.mainWindow.OpenPage(new Pages.Records.Add(newRecord));
                            }
                            else
                            {
                                // Если изменение
                                // Подхватываем данные с полей
                                changeRecord.Name = tbName.Text;
                                changeRecord.Year = Convert.ToInt32(tbYear.Text);
                                changeRecord.Format = tbFormat.SelectedIndex;
                                changeRecord.Size = tbSize.SelectedIndex;
                                changeRecord.IdManufacturer = Manufacturers.Where(x => x.Name == tbManufacturer.SelectedValue.ToString()).First().Id;
                                changeRecord.Price = float.Parse(tbPrice.Text.Replace(".", ","));
                                changeRecord.IdState = AllState.Where(x => x.Name == tbState.SelectedItem.ToString()).First().Id;
                                changeRecord.Description = tbDescription.Text;
                                // Сохраняем данные
                                changeRecord.Save(true);
                                // Выводим сообщение
                                MessageBox.Show($"Пластинка {changeRecord.Name} успешно изменена.", "Уведомление");
                            }
                        }
                        else
                            // Выводим сообщение об ошибке
                            MessageBox.Show("Наименование пластинки слишком большое.", "Предупреждение");
                    else
                        // Выводим сообщение об ошибке
                        MessageBox.Show("Пожалуйста, укажите стоимость пластинки.", "Предупреждение");
                else
                    // Выводим сообщение об ошибке
                    MessageBox.Show("Пожалуйста, укажите год выпуска пластинки.", "Предупреждение");
            else
                // Выводим сообщение об ошибке
                MessageBox.Show("Пожалуйста, укажите наименование пластинки.", "Предупреждение");
        }

        /// <summary> Функция проверки на цифры
        private void tbPreviewNumber(object sender, TextCompositionEventArgs e)
        {
            // если в строке используется текст, запрещаем ввод
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        /// <summary> Функция проверки на цифры с точкой
        private void tbPreviewFloat(object sender, TextCompositionEventArgs e)
        {
            // если в строке используется текст, запрещаем ввод
            e.Handled = !(Char.IsDigit(e.Text, 0)) && e.Text != ".";
        }
    }
}