using System.Collections.Generic;
using System.Windows.Controls;

namespace VinylRecordsApplication.Pages.Supply
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        // Обращаемся к классу поставок, и получаем все поставки из базы данных
        IEnumerable<Classes.Supply> AllSupplies = Classes.Supply.AllSupplies();

        public Main()
        {
            // Инициализируем компоненты на сцене для того чтобы с ними работать
            InitializeComponent();
            // Перебираем все данные о поставках
            foreach (var supply in AllSupplies)
                // Создаём элемент в списке, передавая данные о поставке и ссылку на форму Main, для последующего взаимодействия
                supplyParent.Children.Add(new Pages.Supply.Elements.Supply(supply, this));
        }
    }
}