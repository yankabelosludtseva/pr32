using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace VinylRecordsApplication
{
    /// <summary> Логика взаимодействия для MainWindow.xaml
    public partial class MainWindow : Window
    {
        /// <summary> Переменная которая хранит в себе класс MainWindows, для того чтобы ...
        public static MainWindow mainWindow;
        public Pages.Records.Main mainRecords = new Pages.Records.Main();

        public MainWindow()
        {
            // Инициализируем компоненты на сцене, для того чтобы обращаться к ним
            InitializeComponent();
            // Запоминаем MainWindows, указывая себя
            MainWindow.mainWindow = this;
            // Вызываем метод открытия страниц, и говорим что необходимо открыть страницу Main
            OpenPage(mainRecords);
        }

        /// <summary> Метод открытия страниц
        public void OpenPage(Page pages)
        {
            // Обращаемся к элементу Frame, и открываем указанную страницу
            frame.Navigate(pages);
        }

        /// <summary> Метод открытия страницы с виниловыми пластинками
        private void OpenRecordList(object sender, RoutedEventArgs e)
        {
            // Вызываем метод открытия страницы с виниловыми пластинками
            OpenPage(mainRecords);
            mainRecords.LoadRecord();
        }

        /// <summary> Метод открытия страницы для добавления виниловых пластинок
        private void OpenRecordAdd(object sender, RoutedEventArgs e) =>
            // Вызываем метод открытия страницы для добавления виниловых пластинок
            OpenPage(new Pages.Records.Add());

        /// <summary> Метод открытия страницы с поставщиками
        private void OpenManufacturersList(object sender, RoutedEventArgs e) =>
            // Вызываем метод открытия страницы с поставщиками
            OpenPage(new Pages.Manufacturer.Main());

        /// <summary> Метод открытия страницы для добавления поставщиков
        private void OpenManufacturersAdd(object sender, RoutedEventArgs e) =>
            // Вызываем метод открытия страницы для добавления поставщиков
            OpenPage(new Pages.Manufacturer.Add());

        /// <summary> Метод открытия страницы для с поступлениями
        private void OpenSupplyList(object sender, RoutedEventArgs e) =>
            // Вызываем метод открытия страницы для отображения поставок
            OpenPage(new Pages.Supply.Main());

        /// <summary> Метод открытия страницы для добавления поставок
        private void OpenSupplyAdd(object sender, RoutedEventArgs e) =>
            // Вызываем метод открытия страницы для добавления поставок
            OpenPage(new Pages.Supply.Add());

        /// <summary> Метод открытия страницы со списком состояний
        private void OpenStateList(object sender, RoutedEventArgs e) =>
            OpenPage(new Pages.State.Main());

        /// <summary> Метод открытия страницы добавления состояния
        private void OpenStateAdd(object sender, RoutedEventArgs e) =>
            OpenPage(new Pages.State.Add());

        /// <summary> Метод экспорта виниловых пластинок
        private void ExportRecord(object sender, RoutedEventArgs e)
        {
            // Создаём диалог сохранения
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            // Указываем какой формат будем сохранять
            saveFileDialog.Filter = "Excel (*.xlsx)|*.xlsx";
            // Указываем что необходимо восстановить путь до файла при повторном открытии
            saveFileDialog.RestoreDirectory = true;
            // Открываем файловый диалог
            saveFileDialog.ShowDialog();
            // Если имя файла не равно пустоте
            if (saveFileDialog.FileName != "")
            {
                // Вызываем метод создания Excel документа
                Classes.Record.Export(saveFileDialog.FileName, mainRecords.searchRecords);
            }
        }
    }
}