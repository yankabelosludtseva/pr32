using System.Collections.Generic;
using System.Windows.Controls;

namespace VinylRecordsApplication.Pages.Manufacturer
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        // Обращаемся к классу Manufacturer, и вызываем метод загрузки данных о поставщиках
        public IEnumerable<Classes.Manufacturer> AllManufacturers = Classes.Manufacturer.AllManufacturers();

        public Main()
        {
            // Инициализируем компоненты на сцене для того чтобы иметь возможность с ними работать
            InitializeComponent();
            // Перебираем поставщиков
            foreach (Classes.Manufacturer manufacturer in AllManufacturers)
                // Добавляем формочки с данными на сцену, передавая в каждую форму данные о поставщике и ссылку на страницу Main
                manufactureParent.Children.Add(new Manufacturer.Elements.Manufacturer(manufacturer, this));
        }
    }
}