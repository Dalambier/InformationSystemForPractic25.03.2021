using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Государственный_заказ_Челябинской_области.Интерфейс.Пользователи
{
    /// <summary>
    /// Форма для показа заказов пользователей
    /// </summary>
    public partial class ЗаказыПользователя : Window
    {
        Модели.ПараметрыСоединенияБазыДанных БазаДанных = new Модели.ПараметрыСоединенияБазыДанных();
        public ЗаказыПользователя()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Код для показа заказов пользователей
        /// </summary>
        void ПоказатьМоиЗаказы()
        {
            Модели.ПараметрыСоединенияБазыДанных ОбновлённыйСписок = new Модели.ПараметрыСоединенияБазыДанных();
            _СписокМоихЗаказов.ItemsSource = ОбновлённыйСписок.Заказы.Where(x => x.Объявитель == Классы.ХранимаяИнформация.ЛичныйИдентификатор && x.Выполнитель == null).ToList();
        }

        /// <summary>
        /// Обновление списка заказов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ОбновитьСписокМоихЗаказов(object sender, RoutedEventArgs e)
        {
            ПоказатьМоиЗаказы();
        }

        /// <summary>
        /// Открытие формы для создания нового заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ОформитьНовыйЗаказ(object sender, RoutedEventArgs e)
        {
            ОформлениеНовогоЗаказа ФормаСоставленияЗаказа = new ОформлениеНовогоЗаказа();
            ФормаСоставленияЗаказа.ShowDialog();
            ПоказатьМоиЗаказы();
        }

        /// <summary>
        /// Кнопка удаления своего заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void УдалитьСвойЗаказ(object sender, RoutedEventArgs e)
        {
            int ИдентификаторЗаказа = int.Parse((sender as Button).ToolTip.ToString());
            var ВыбранныйЗаказ = БазаДанных.Заказы.First(x => x.id == ИдентификаторЗаказа);
            БазаДанных.Заказы.Remove(ВыбранныйЗаказ);
            БазаДанных.SaveChanges();
            ПоказатьМоиЗаказы();
        }

        /// <summary>
        /// Клавиша просмотра своего заказа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ПросмотретьСвойЗаказ(object sender, RoutedEventArgs e)
        {
            int ИдентификаторЗаказа = int.Parse((sender as Button).ToolTip.ToString());
            Классы.ХранимаяИнформация.ИдентификаторВыбранногоЗаказа = ИдентификаторЗаказа;
            ПросмотрСвоегоЗаказа ОткрытьФормуПросмотраЗаказа = new ПросмотрСвоегоЗаказа();
            ОткрытьФормуПросмотраЗаказа.Show();
        }
    }
}
