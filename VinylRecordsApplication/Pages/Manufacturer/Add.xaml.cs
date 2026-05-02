using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using VinylRecordsApplication.Classes;

namespace VinylRecordsApplication.Pages.Manufacturer
{
    /// <summary> Логика взаимодействия для Add.xaml
    public partial class Add : Page
    {
        // Обращаемся к классу стран, и выгружаем все из базы данных
        public IEnumerable<Classes.Country> AllCountries = Classes.Country.AllCountries();
        // Создаём переменную которая будет хранить в себе поставщика (которого необходимо изменить)
        Classes.Manufacturer changeManufacturer;

        // Конструктор, который принимает поставщика для изменения
        public Add(Classes.Manufacturer changeManufacturer = null)
        {
            // Инициализируем компоненты для того чтобы с ними работать
            InitializeComponent();
            // Перебираем страны
            foreach (var Country in AllCountries)
                // Добавляем страну в выпадающий список
                tbCountry.Items.Add(Country.Name);
            // Если кол-во стран больше 0
            if (AllCountries.Count() > 0)
                // Выбираем первую страну
                tbCountry.SelectedIndex = 0;

            // Если у нас происходит изменение какого-то поставщика
            if (changeManufacturer != null)
            {
                // Запоминаем поставщика которого мы изменяем
                this.changeManufacturer = changeManufacturer;
                // В поле с именем привязываем значение изменяемого поставщика
                tbName.Text = changeManufacturer.Name;
                // В поле с номером телефона привязываем значение изменяемого поставщика
                tbPhone.Text = changeManufacturer.Phone;
                // В поле почты привязываем значение изменяемого поставщика
                tbEmail.Text = changeManufacturer.Mail;
                // Выбираем страну, которую хранить у изменяемого поставщика, для этого
                // Обращаемся ко всем странам, находим ту которая числиться по ключу у поставщика
                // Получаем её индекс в списке
                // Полю со странами присваиваем новый индекс
                tbCountry.SelectedIndex = AllCountries.ToList().FindIndex(x => x.Id == changeManufacturer.CountryCode);
                // Меняем текст на кнопке с "Добавить" на "Изменить"
                addBth.Content = "Изменить";
            }
        }

        /// <summary> Добавление/Изменение поставщика
        private void AddManufacturer(object sender, RoutedEventArgs e)
        {
            // Проверяем что поле наименование не пустое
            if (!String.IsNullOrEmpty(tbName.Text))
                // Проверяем что поле телефона не пустое
                if (!String.IsNullOrEmpty(tbPhone.Text))
                    // Проверяем что поле почты не пустое
                    if (!String.IsNullOrEmpty(tbEmail.Text))
                        // Проверяем что номер телефона корректен
                        if (CorrectPhone(tbPhone.Text))
                            // Проверяем что поле почты корректно
                            if (CorrectEmail(tbEmail.Text))
                            {
                                // Если у нас нет поставщика на изменение
                                // Значит добавляем
                                if (changeManufacturer == null)
                                {
                                    // Создаём новый класс поставщика, в котором указываем данные
                                    Classes.Manufacturer manufacturer = new Classes.Manufacturer()
                                    {
                                        // Наименование
                                        Name = tbName.Text,
                                        // Номер телефона
                                        Phone = tbPhone.Text,
                                        // Почта
                                        Mail = tbEmail.Text,
                                        // Код страны
                                        // Обращаемся по всем странам и получаем строку у которой наименование совпадает с наименованием
                                        // в выбранном поле
                                        // Запоминаем ID
                                        CountryCode = AllCountries.Where(x => x.Name == tbCountry.SelectedItem.ToString()).First().Id
                                    };
                                    // Вызываем метод сохранения в классе поставщика
                                    manufacturer.Save();
                                    // Выводим сообщение о том что поставщик добавлен
                                    MessageBox.Show($"Поставщик {manufacturer.Name} успешно добавлен.", "Уведомление");
                                    // Открываем страницу добавления, передавая данные о поставщике
                                    // Тем самым откроем изменение поставщика
                                    MainWindow.mainWindow.OpenPage(new Add(manufacturer));
                                }
                                else
                                {
                                    // Если же поставщик на изменение присутствует
                                    // Значит окно открыто в качестве изменения
                                    // Меняем наименование
                                    changeManufacturer.Name = tbName.Text;
                                    // Изменяем телефон
                                    changeManufacturer.Phone = tbPhone.Text;
                                    // Изменяем почту
                                    changeManufacturer.Mail = tbEmail.Text;
                                    // Изменяем код страны
                                    changeManufacturer.CountryCode = AllCountries.Where(x => x.Name == tbCountry.SelectedItem.ToString()).First().Id;
                                    // Вызываем сохранение поставщика
                                    // Указываем ключ TRUE, при таком значении данные в таблице изменятся, а не добавятся вновь
                                    changeManufacturer.Save(true);
                                    // Выводим сообщение
                                    MessageBox.Show($"Поставщик {changeManufacturer.Name} успешно изменён.", "Уведомление");
                                }
                            }
                            else
                                // Выводим сообщение об ошибке
                                MessageBox.Show("Пожалуйста, укажите почту поставщика в формате xx@xx.xx.", "Предупреждение");
                        else
                            // Выводим сообщение об ошибке
                            MessageBox.Show("Пожалуйста, укажите номер поставщика в формате 89000000000.", "Предупреждение");
                    else
                        // Выводим сообщение об ошибке
                        MessageBox.Show("Пожалуйста, укажите почту поставщика.", "Предупреждение");
                else
                    // Выводим сообщение об ошибке
                    MessageBox.Show("Пожалуйста, укажите телефон поставщика.", "Предупреждение");
            else
                // Выводим сообщение об ошибке
                MessageBox.Show("Пожалуйста, укажите наименование поставщика.", "Предупреждение");
        }

        /// <summary>
        /// Метод проверки телефона на корректность
        /// </summary>
        /// <param name="Value">Значение</param>
        public bool CorrectPhone(string Value)
        {
            // Регулярное выражение по которому проверяем
            // Начала строки с цифры 8 и 9, далее цифры от 0-9 в количестве 9 штук
            string sRegex = "^89[0-9]{9}$";
            // Создаём регулярное выражение
            Regex regex = new Regex(sRegex);
            // Получаем совпадения
            MatchCollection matches = regex.Matches(Value);
            // Возвращаем результат, true - если данные подходят, false, если данные не подходят
            return matches.Count > 0;
        }

        /// <summary>
        /// Метод проверки данных на почту
        /// </summary>
        public bool CorrectEmail(string Value)
        {
            // Регулярное выражение
            // Символы от a-z в кол-ве от 2 до 20 символов @ Символы от a-z в кол-ве от 2 до 20 символов . Символы от a-z в кол-ве от 2 до 3 символов
            string sRegex = "^[a-z]{2,20}@[a-z]{2,20}.[a-z]{2,3}$";
            // Создаём регулярное выражение
            Regex regex = new Regex(sRegex);
            // Получаем совпадения
            MatchCollection matches = regex.Matches(Value);
            // Возвращаем результат, true - если данные подходят, false, если данные не подходят
            return matches.Count > 0;
        }

        /// <summary>
        /// Функция проверки на цифры
        /// </summary>
        private void tbPreviewNumber(object sender, TextCompositionEventArgs e)
        {
            // если в строке используется текст, запрещаем ввод
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}