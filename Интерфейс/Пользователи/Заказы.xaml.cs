using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Государственный_заказ_Челябинской_области.Интерфейс.Пользователи
{
    /// <summary>
    /// Форма показа заказов
    /// </summary>
    public partial class Заказы : Window
    {
        public Заказы()
        {
            InitializeComponent();
            ПоказатьМоиЗаказы();
        }
        void ПоказатьМоиЗаказы()
        {
            Модели.ПараметрыСоединенияБазыДанных ОбновлённыйСписок = new Модели.ПараметрыСоединенияБазыДанных();
            _СписокЗаказов.ItemsSource = ОбновлённыйСписок.Заказы.Where(x => x.Объявитель != Классы.ХранимаяИнформация.ЛичныйИдентификатор && x.Выполнитель == null).ToList();
        }

        private void ОбновитьСписокЗаказов(object sender, RoutedEventArgs e)
        {
            ПоказатьМоиЗаказы();
        }

        private void ПоказатьЗаказ(object sender, RoutedEventArgs e)
        {
            int ВыбранныйИдентификатор = int.Parse((sender as Button).ToolTip.ToString());
            Классы.ХранимаяИнформация.ИдентификаторВыбранногоЗаказа = ВыбранныйИдентификатор;
            ПросмотрЗаказа ФормаПросмотраЗаказа = new ПросмотрЗаказа();
            ФормаПросмотраЗаказа.ShowDialog();
            ПоказатьМоиЗаказы();
        }
    }
}
