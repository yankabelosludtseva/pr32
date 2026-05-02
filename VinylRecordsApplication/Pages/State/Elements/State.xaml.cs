using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VinylRecordsApplication.Pages.State.Elements
{
    /// <summary> Логика взаимодействия для State.xaml
    public partial class State : UserControl
    {
        /// <summary> Объект State, к которому привязывается интерфейс
        Classes.State state;

        /// <summary> Ссылка на страницу Main
        Pages.State.Main main;

        public State(Classes.State state, Pages.State.Main main)
        {
            InitializeComponent();
            // Запоминаем объект состояния, для того чтобы в последующем с ним взаимодействовать
            this.state = state;
            // Запоминаем главную страницу
            this.main = main;

            // В поле наименования указываем наименование
            tbName.Text = this.state.Name;
            // В поле сокр. наименования указываем сокр. наименование
            tbSubname.Text = this.state.Subname;
            // В поле описания указываем описание
            tbDescription.Text = this.state.Description;
        }

        /// <summary> Изменение состояния
        private void EditState(object sender, RoutedEventArgs e)
        {
            // Обращаемся к главному окну, и вызываем метод открытия страниц
            // Открываем страницу добавления состояний и передаём состояние которое будем изменять
            MainWindow.mainWindow.OpenPage(new Pages.State.Add(state));
        }

        /// <summary> Удаление состояния
        private void DeleteState(object sender, RoutedEventArgs e)
        {
            // Выводим текстовое предупреждение о том что вы собираетесь удалить состояние
            if (MessageBox.Show($"Удалить состояние: {this.state.Name}?", "Уведомление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Получаем все записи пластинок
                IEnumerable<Classes.Record> AllRecord = Classes.Record.AllRecords();

                // Обращаемся к пластинкам и ищем нет ли у нас пластинки с нашим состоянием
                if (AllRecord.Where(x => x.IdState == state.Id).Count() > 0)
                {
                    // Если такая пластинка существует, выводим уведомление о невозможности удалить
                    MessageBox.Show($"Состояние {this.state.Name} невозможно удалить. Для начала удалите зависимости.", "Уведомление");
                }
                else
                {
                    // Если пластинок не существует
                    // Обращаемся к классу состояния и вызываем метод удаления
                    this.state.Delete();

                    // Обращаемся к странице, которая создала элемент и удаляем с неё самого себя
                    main.stateParent.Children.Remove(this);

                    // Выводим сообщение об удалении
                    MessageBox.Show($"Состояние {this.state.Name} успешно удалено.", "Уведомление");
                }
            }
        }
    }
}