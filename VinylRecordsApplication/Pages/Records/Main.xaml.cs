using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace VinylRecordsApplication.Pages.Records
{
    /// <summary> Логика взаимодействия для Main.xaml
    public partial class Main : Page
    {
        // Обращаемся к классу состояний и получаем все состояния
        public IEnumerable<Classes.State> AllState = Classes.State.AllState();
        // Обращаемся к классу пластинок и получаем все пластинки
        public IEnumerable<Classes.Record> AllRecords = Classes.Record.AllRecords();
        // Обращаемся к классу поставщиков и получаем всех поставщиков
        public IEnumerable<Classes.Manufacturer> AllManufacturers = Classes.Manufacturer.AllManufacturers();
        // Переменная отвечающая за создание UI
        private bool CreateUI = false;

        public List<Classes.Record> searchRecords;

        public Main()
        {
            // Инициализируем компоненты на сцене для того чтобы с ними работать
            InitializeComponent();
            // Получаем из перечисления список
            searchRecords = AllRecords.ToList();
            // Говорим что интерфейс создан
            CreateUI = true;
            // вызываем создание записей, по всем
            LoadAllRecord(AllRecords.ToList());
            // Вызываем загрузку поставщиков
            LoadAllManufacture();
            // Вызываем загрузку состояний
            LoadAllState();
        }

        /// <summary> Загрузка всех пластинок
        public void LoadRecord()
        {
            // Обращаемся к классу пластинок и получаем все пластинки
            AllRecords = Classes.Record.AllRecords();
            // вызываем создание записей, по всем
            LoadAllRecord(AllRecords.ToList());
        }

        /// <summary> Создание записей о пластинках
        public void LoadAllRecord(List<Classes.Record> AllRecords)
        {
            // Чистим холст, удаляя старые данные
            recordsParent.Children.Clear();
            // Перебираем пластинки
            foreach (var record in AllRecords)
                // Создаём элементы передавая данные о пластинке и ссылку на Маин
                recordsParent.Children.Add(new Pages.Records.Elements.Record(record, this));
        }

        /// <summary> Загрузка данных о поставщиках
        public void LoadAllManufacture()
        {
            // Чистим список о поставщиках
            tbManufacturer.Items.Clear();
            // Перебираем поставщиков
            foreach (var manufacturer in AllManufacturers)
                // Добавляем данные в выпадающий список
                tbManufacturer.Items.Add(manufacturer.Name);
            // Добавляем отдельно данные "Выберите"
            tbManufacturer.Items.Add("Выберите ...");
            // Устанавливаем первоначальное положение как выберите
            tbManufacturer.SelectedIndex = tbManufacturer.Items.Count - 1;
        }

        /// <summary> Загрузка данных о состояниях
        public void LoadAllState()
        {
            // Чистим список состояний
            tbState.Items.Clear();
            // Перебираем состояния
            foreach (var state in AllState)
                // Добавляем данные в выпадающий список
                tbState.Items.Add(state.Name);
            // Добавляем отдельно данные "Выберите"
            tbState.Items.Add("Выберите ...");
            // Устанавливаем первоначальное положение как выберите
            tbState.SelectedIndex = tbState.Items.Count - 1;
        }

        /// <summary> Фильтрация записей
        public void RecordsFilter()
        {
            // Создаём список отсортированных данных
            List<Classes.Record> FilterRecords = new List<Classes.Record>();
            // если выбран поставщик
            if (tbManufacturer.SelectedIndex != tbManufacturer.Items.Count - 1)
                // Получаем данные из всех пластинок по выбранному поставщику
                FilterRecords = AllRecords.Where(x => x.IdManufacturer ==
                    // Получаем код поставщика из всех поставщиков по выбранному наименованию поставщика
                    AllManufacturers.Where(y => y.Name == tbManufacturer.SelectedItem.ToString()).First().Id).ToList();
            else
                // Если поставщик не выбран, получаем весь список пластинок
                FilterRecords = AllRecords.ToList();

            // Если выбрано состояние
            if (tbState.SelectedIndex != tbState.Items.Count - 1)
                // Получаем пластинки с выбранным состоянием
                FilterRecords = FilterRecords.FindAll(x => x.IdState ==
                    AllState.Where(y => y.Name == tbState.SelectedItem.ToString()).First().Id);

            // Если наименование не пустое
            if (tbName.Text != "")
            {
                // Проверяем написано ли в поле моно
                if ("моно".Contains(tbName.Text.ToLower()))
                {
                    // Если написано моно, ищем только моно
                    FilterRecords = FilterRecords.FindAll(x => x.Format == 0);
                }
                // Если в наименование написано стерео
                else if ("стерео".Contains(tbName.Text.ToLower()))
                {
                    // Ищем только стерео
                    FilterRecords = FilterRecords.FindAll(x => x.Format == 1);
                }
                else
                {
                    // Получаем список пластинок, который совпадают по наименованию с введённым значением
                    FilterRecords = FilterRecords.FindAll(
                        x =>
                            x.Name.ToLower().Contains(tbName.Text.ToLower()) ||
                            x.Year.ToString().Contains(tbName.Text) ||
                            x.Price.ToString().Contains(tbName.Text) ||
                            x.Description.ToLower().Contains(tbName.Text.ToLower()));
                }
            }
            // Очищаем список поиска
            searchRecords.Clear();
            // Присваиваем новый
            searchRecords = FilterRecords;
            // Выводим список в интерфейс
            LoadAllRecord(FilterRecords);
        }

        private void FilterRecords(object sender, SelectionChangedEventArgs e)
        {
            // Если интерфейс создан
            if (CreateUI)
                // Активируем фильтр
                RecordsFilter();
        }

        private void SearchRecords(object sender, KeyEventArgs e) =>
            // При вводе текста, активируем фильтр
            RecordsFilter();
    }
}