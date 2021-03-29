using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Государственный_заказ_Челябинской_области
{
    /// <summary>
    /// Форма входа в аккаунт
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вход в форму технической поддержки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ТехническаяПоддержка_Кнопка(object sender, MouseButtonEventArgs e)
        {
            Интерфейс.ГлавноеМеню.ТехническаяПоддержкаСвязь ФормаДляСвязисТехническойПоддержкой = new Интерфейс.ГлавноеМеню.ТехническаяПоддержкаСвязь();
            this.Hide();
            ФормаДляСвязисТехническойПоддержкой.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// Вход в форму регистрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Регистрация_Кнопка(object sender, MouseButtonEventArgs e)
        {
            Интерфейс.ГлавноеМеню.Регистрация ФормаРегистрации = new Интерфейс.ГлавноеМеню.Регистрация();
            this.Hide();
            ФормаРегистрации.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// Вход в форму восстановления аккаунта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ВосстановлениеДоступа_Кнопка(object sender, MouseButtonEventArgs e)
        {
            Интерфейс.ГлавноеМеню.ВосстановлениеАккаунта ФормаВосстановленияАккаунта = new Интерфейс.ГлавноеМеню.ВосстановлениеАккаунта();
            this.Hide();
            ФормаВосстановленияАккаунта.ShowDialog();
            this.Show();
        }

        /// <summary>
        /// Вход в аккаунт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Вход_Кнопка(object sender, RoutedEventArgs e)
        {
            Модели.ПараметрыСоединенияБазыДанных БазаДанных = new Модели.ПараметрыСоединенияБазыДанных();
            try
            {
                var ПользовательскаяСтрока = БазаДанных.Персонал.First(x => x.Логин == ПолеДляВвода_Логин.Text && x.Пароль == ПолеДляВвода_Пароль.Password);
                if (ПользовательскаяСтрока.Тип_аккаунта == "Техническая поддержка")
                {
                    Интерфейс.Персонал.Техническая_поддержка.ПросмотрПисем ФормаТехническойПоддержки = new Интерфейс.Персонал.Техническая_поддержка.ПросмотрПисем();
                    this.Hide();
                    ФормаТехническойПоддержки.ShowDialog();
                    Application.Current.Shutdown();
                }
                else if (ПользовательскаяСтрока.Тип_аккаунта == "Администратор")
                {
                    Интерфейс.Персонал.Администрация.Меню ФормаАдминистратора = new Интерфейс.Персонал.Администрация.Меню();
                    this.Hide();
                    ФормаАдминистратора.ShowDialog();
                    Application.Current.Shutdown();
                }
            }
            catch
            {
                try
                {
                    var ПользовательскаяСтрока = БазаДанных.Пользователи.First(x => x.Логин == ПолеДляВвода_Логин.Text && x.Пароль == ПолеДляВвода_Пароль.Password);
                    Классы.ХранимаяИнформация.ЛичныйИдентификатор = ПользовательскаяСтрока.id;
                    Интерфейс.Пользователи.Меню МенюПользователя = new Интерфейс.Пользователи.Меню();
                    this.Hide();
                    МенюПользователя.ShowDialog();
                    Application.Current.Shutdown();
                }
                catch
                {
                    MessageBox.Show("Проверьте введённые данные", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
