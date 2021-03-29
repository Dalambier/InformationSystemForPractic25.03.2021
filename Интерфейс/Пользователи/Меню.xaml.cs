using System.Windows;

namespace Государственный_заказ_Челябинской_области.Интерфейс.Пользователи
{
    /// <summary>
    /// Меню пользователя
    /// </summary>
    public partial class Меню : Window
    {
        public Меню()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Кнопка просмотра информации об аккаунте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ПросмотретьИнформацииюОбАккаунте(object sender, RoutedEventArgs e)
        {
            ИнформацияОбАккаунте ИнформацияОбАккаунте = new ИнформацияОбАккаунте();
            ИнформацияОбАккаунте.Show();
        }

        /// <summary>
        /// Форма просмотра заказов пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void МоиЗаказы_ОткрытьФорму(object sender, RoutedEventArgs e)
        {
            ЗаказыПользователя ФормаМоихЗаказов = new ЗаказыПользователя();
            ФормаМоихЗаказов.Show();
        }
        /// <summary>
        /// Кнопка просмотра общего списка заказов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void КнопкаПросмотраСпискаЗаказов(object sender, RoutedEventArgs e)
        {
            Заказы ФормаОбщихЗаказов = new Заказы();
            ФормаОбщихЗаказов.Show();
        }

        /// <summary>
        /// Кнопка просмотра договоров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ПросмотретьДоговоры(object sender, RoutedEventArgs e)
        {
            ДоговорыПользователя ПросмотретьДоговоры = new ДоговорыПользователя();
            ПросмотретьДоговоры.Show();
        }
    }
}
