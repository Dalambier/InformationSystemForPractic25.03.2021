using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;

namespace Государственный_заказ_Челябинской_области.Интерфейс.Персонал.Техническая_поддержка
{
    /// <summary>
    /// Форма просмотра писем
    /// </summary>
    public partial class ПросмотрПисем : Window
    {
        Модели.ПараметрыСоединенияБазыДанных БазаДанных = new Модели.ПараметрыСоединенияБазыДанных();

        public static string ЭлектронныйАдресПользователя;
        public static string СообщениеПользователя;
        public static int ИдентификаторСообщения;
        public ПросмотрПисем()
        {
            InitializeComponent();
            _Письма.ItemsSource = БазаДанных.Письма_технической_поддержки.Where(x => x.Статус == "Проверяется").ToList();
        }

        private void ВыборПисьма(object sender, RoutedEventArgs e)
        {
            string ВыбранныйЭлемент = (sender as Button).Content.ToString();
            int ВыбранныйИдентификатор = int.Parse(ВыбранныйЭлемент);
            var ВыбраннаяСтрока = БазаДанных.Письма_технической_поддержки.First(x => x.id == ВыбранныйИдентификатор);
            _СодержаниеПисьма.Text = ВыбраннаяСтрока.Письмо;
            ОтветитьНаПисьмо.IsEnabled = true;
            ПолеДляВводаОтветногоПисьма.IsEnabled = true;
            ЭлектронныйАдресПользователя = ВыбраннаяСтрока.Электронная_почта;
            СообщениеПользователя = ВыбраннаяСтрока.Письмо;
            ИдентификаторСообщения = ВыбраннаяСтрока.id;
        }

        private void ОтветитьНаПисьмо_Кнопка(object sender, RoutedEventArgs e)
        {
            if (ПолеДляВводаОтветногоПисьма.Text == string.Empty)
            {
                MessageBox.Show("Поле не заполнено!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ОтправитьЭлектронноеСообщение(ЭлектронныйАдресПользователя);
            ПометитьПисьмо.IsEnabled = true;
        }

        /// <summary>
        /// Метод отправки сообщений на электронную почту
        /// </summary>
        /// <param name="Пароль"></param>
        void ОтправитьЭлектронноеСообщение(string ЭлектронныйАдрес)
        {
            MailAddress From = new MailAddress(Классы.ПараметрыЭлектроннойПочты.АдресЭлектроннойПочты, Классы.ПараметрыЭлектроннойПочты.ОтображаемоеИмя);

            MailAddress To = new MailAddress(ЭлектронныйАдрес);

            MailMessage msg = new MailMessage(From, To)
            {
                Subject = "Ответ от технической поддержки 'Государственный заказ Челябинской области'",

                Body = "Техническая поддержка ответила на ваше сообщение: " + СообщениеПользователя + "<br>" +
                "Ответ: " + ПолеДляВводаОтветногоПисьма.Text + "<br>" +
                "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~<br>" +
                "Счастья вам",

                IsBodyHtml = true
            };

            SmtpClient smtp = new SmtpClient(Классы.ПараметрыЭлектроннойПочты.SMTP_Адрес, Классы.ПараметрыЭлектроннойПочты.SMTP_Порт)
            {
                Credentials = new NetworkCredential(Классы.ПараметрыЭлектроннойПочты.АдресЭлектроннойПочты, Классы.ПараметрыЭлектроннойПочты.ПарольЭлектроннойПочты),
                EnableSsl = true
            };
            smtp.Send(msg);
        }

        /// <summary>
        /// Кнопка для помечивания письма как "решённое"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ПометитьКакРешено_Кнопка(object sender, RoutedEventArgs e)
        {
            var ВыбраннаяСтрока = БазаДанных.Письма_технической_поддержки.First(x => x.id == ИдентификаторСообщения);
            ВыбраннаяСтрока.Статус = "Решено";
            БазаДанных.SaveChanges();
            ПолеДляВводаОтветногоПисьма.Text = string.Empty;
            ПолеДляВводаОтветногоПисьма.IsEnabled = false;
            ОтветитьНаПисьмо.IsEnabled = false;
            ПометитьПисьмо.IsEnabled = false;
            _Письма.ItemsSource = БазаДанных.Письма_технической_поддержки.Where(x => x.Статус == "Проверяется").ToList();
            _СодержаниеПисьма.Text = "Текст для технической поддержки";
        }
    }
}
