using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VinylRecordsApplication.Pages.Records.Elements
{
    /// <summary> Логика взаимодействия для Record.xaml
    public partial class Record : UserControl
    {
        /// <summary> Список состояний
        private IEnumerable<Classes.State> AllState = Classes.State.AllState();
        /// <summary> Данные о пластинке
        private Classes.Record record;
        /// <summary> Ссылка на страницу Маин
        private Pages.Records.Main main;

        public Record(Classes.Record record, Pages.Records.Main main)
        {
            // Инициализируем компоненты для того чтобы с ними работать
            InitializeComponent();
            // получаем данные о поставщиках
            IEnumerable<Classes.Manufacturer> AllManufacturer = Classes.Manufacturer.AllManufacturers();
            // Запоминаем данные о пластинке
            this.record = record;
            // Запоминаем данные о Маин
            this.main = main;
            // Выводим имя
            tbName.Text = record.Name;
            // Выводим год
            tbYear.Text = record.Year.ToString();
            // Выводим формат
            tbFormat.Text = record.Format == 0 ? "Моно" : "Стерео";
            // Выводим размер
            switch (record.Size)
            {
                case 0:
                    tbSize.Text = "7 дюймов";
                    break;
                case 1:
                    tbSize.Text = "10 дюймов";
                    break;
                case 2:
                    tbSize.Text = "12 дюймов";
                    break;
                case 3:
                    tbSize.Text = "Иной";
                    break;
            }
            // Выводим поставщика
            tbManufacturer.Text = AllManufacturer.Where(x => x.Id == record.IdManufacturer).First().Name;
            // Выводим стоимость
            tbPrice.Text = record.Price.ToString();
            // Выбираем состояние
            tbState.Text = AllState.Where(x => x.Id == record.IdState).First().Name;
            // Выводим описание
            tbDescription.Text = record.Description;
        }

        /// <summary> Метод изменения
        private void EditRecord(object sender, RoutedEventArgs e)
        {
            // Обращаемся к главной форме
            // Открываем страницу добавления передавая данные о пластинке => получаем страницу изменения данных
            MainWindow.mainWindow.OpenPage(new Add(this.record));
        }

        /// <summary> Удаление данных
        private void DeleteRecord(object sender, RoutedEventArgs e)
        {
            // Спрашиваем пользователя
            if (MessageBox.Show($"Удалить виниловую пластинку: {this.record.Name}?", "Уведомление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Получаем поставки
                IEnumerable<Classes.Supply> AllSupply = Classes.Supply.AllSupplies();
                // Если среди поставок есть наша пластинка
                if (AllSupply.Where(x => x.IdRecord == record.Id).Count() > 0)
                {
                    // Выводим уведомление о том что нельзя удалить
                    MessageBox.Show($"Виниловую пластинку {this.record.Name} невозможно удалить. Для начала удалите зависимости.", "Уведомление");
                }
                else
                {
                    // Удаляем запись
                    this.record.Delete();
                    // Удаляем элемент с интерфейса
                    main.recordsParent.Children.Remove(this);
                    // Выводим сообщение
                    MessageBox.Show($"Пластинка {this.record.Name} успешно удалена.", "Уведомление");
                }
            }
        }
    }
}