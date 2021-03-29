using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Государственный_заказ_Челябинской_области.Интерфейс.Персонал.Администрация
{
    /// <summary>
    /// Форма показа заказов
    /// </summary>
    public partial class ПоказЗаказов : Window
    {
        public ПоказЗаказов()
        {
            InitializeComponent();
            ПоказатьСписокЗаказов();
        }
        private void ОбновитьСписокЗаказов(object sender, RoutedEventArgs e)
        {
            ПоказатьСписокЗаказов();
        }

        private void ПоказатьЗаказ(object sender, RoutedEventArgs e)
        {
            int ИдентификаторЗаказа = int.Parse((sender as Button).ToolTip.ToString());
            Классы.ХранимаяИнформация.ИдентификаторВыбранногоЗаказа_Администратор = ИдентификаторЗаказа;
            ИнформацияЗаказа ПоказатьИнформациюЗаказа = new ИнформацияЗаказа();
            ПоказатьИнформациюЗаказа.ShowDialog();
            ПоказатьСписокЗаказов();
        }
        void ПоказатьСписокЗаказов()
        {
            Модели.ПараметрыСоединенияБазыДанных БазаДанных = new Модели.ПараметрыСоединенияБазыДанных();
            _Заказы.ItemsSource = БазаДанных.Заказы.ToList();
        }
    }
}
