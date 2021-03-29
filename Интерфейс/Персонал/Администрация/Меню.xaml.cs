using System.Windows;

namespace Государственный_заказ_Челябинской_области.Интерфейс.Персонал.Администрация
{
    /// <summary>
    /// Меню администратора
    /// </summary>
    public partial class Меню : Window
    {
        public Меню()
        {
            InitializeComponent();
        }
        private void ПоказатьСписокПользователей(object sender, RoutedEventArgs e)
        {
            ПоказПользователей ФормаПоказаПользователей = new ПоказПользователей();
            ФормаПоказаПользователей.Show();
        }

        private void ПоказатьСписокЗаказов(object sender, RoutedEventArgs e)
        {
            ПоказЗаказов ФормаПоказаЗаказов = new ПоказЗаказов();
            ФормаПоказаЗаказов.Show();
        }
    }
}
