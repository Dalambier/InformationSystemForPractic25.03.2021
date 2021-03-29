using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;

namespace Государственный_заказ_Челябинской_области.Интерфейс.Пользователи
{
    /// <summary>
    /// Форма оформления заказа
    /// </summary>
    public partial class ОформлениеНовогоЗаказа : Window
    {
        Модели.ПараметрыСоединенияБазыДанных БазаДанных = new Модели.ПараметрыСоединенияБазыДанных();

        public static string ПутьДоКартинки = string.Empty;

        public ОформлениеНовогоЗаказа()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Кнопка загрузки изображения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ЗагрузитьИзображениеЗаказа(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ДиалогОткрытияКартинки = new OpenFileDialog
            {
                Filter = "Файлы изображений (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png"
            };
            if (ДиалогОткрытияКартинки.ShowDialog() == true)
            {
                ПутьДоКартинки = ДиалогОткрытияКартинки.FileName;
                КнопкаВыбораИзображения.Content = "Загружено";
            }
        }


        private void КнопкаОформленияЗаказа(object sender, RoutedEventArgs e)
        {
            //Проверка полей на заполненность
            if (ПолеДляВвода_НазваниеЗаказа.Text == string.Empty)
            {
                MessageBox.Show("Необходимо ввести название заказа", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (ПолеДляВвода_ОписаниеЗаказа.Text == string.Empty)
            {
                MessageBox.Show("Необходимо ввести описание заказа", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (ПолеДляВвода_Цена.Text == string.Empty)
            {
                MessageBox.Show("Необходимо ввести цену товара", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (ТипЗаказа.SelectedIndex == 0)
            {
                MessageBox.Show("Необходимо Выбрать тип заказа", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (ПутьДоКартинки == string.Empty)
            {
                MessageBox.Show("Необходимо выбрать изображение заказа", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //Проверка полей на валидность
            try
            {
                int.Parse(ПолеДляВвода_Цена.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Цена введена неверно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //Проверка чекбокса
            string ВыбранныйТипЗаказа = string.Empty;
            if (ТипЗаказа.SelectedIndex == 1)
            {
                ВыбранныйТипЗаказа = "Покупка";
            }
            else if (ТипЗаказа.SelectedIndex == 2)
            {
                ВыбранныйТипЗаказа = "Продажа";
            }
            //Проверка на существование названия товара
            try
            {
                БазаДанных.Заказы.First(x => x.Название_заказа == ПолеДляВвода_НазваниеЗаказа.Text);
                MessageBox.Show("Данное название заказа у вас уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            catch
            {

            }
            //Создание строки
            string НастоящееВремя = DateTime.Now.ToString();
            int ЦенаЗаказа = int.Parse(ПолеДляВвода_Цена.Text);
            Модели.Заказы НовыйЗаказ = new Модели.Заказы
            {
                Объявитель = Классы.ХранимаяИнформация.ЛичныйИдентификатор,
                Название_заказа = ПолеДляВвода_НазваниеЗаказа.Text,
                Описание_заказа = ПолеДляВвода_ОписаниеЗаказа.Text,
                Цена = ЦенаЗаказа,
                Тип_заказа = ВыбранныйТипЗаказа,
                Дата_объявления_заказа = НастоящееВремя
            };
            БазаДанных.Заказы.Add(НовыйЗаказ);
            БазаДанных.SaveChanges();
            БазаДанных.Database.ExecuteSqlCommand("UPDATE Заказы SET [Фотография товара] = (SELECT * FROM OPENROWSET(BULK '" + ПутьДоКартинки + "', SINGLE_BLOB) AS image) WHERE [Дата объявления заказа] = '" + НастоящееВремя + "'");
            БазаДанных.SaveChanges();
            this.Close();
        }
    }
}
