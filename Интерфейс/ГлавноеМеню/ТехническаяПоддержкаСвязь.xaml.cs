using System.Net;
using System.Net.Mail;
using System.Windows;

namespace Государственный_заказ_Челябинской_области.Интерфейс.ГлавноеМеню
{
    /// <summary>
    /// Форма для связи с технической поддержкой
    /// </summary>
    public partial class ТехническаяПоддержкаСвязь : Window
    {
        Модели.ПараметрыСоединенияБазыДанных БазаДанных = new Модели.ПараметрыСоединенияБазыДанных();
        public ТехническаяПоддержкаСвязь()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Проверка полей на заполненность, проверка валидности почтового адреса и непосредственно отправка письма
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ОтправитьПисьмо_Кнопка(object sender, RoutedEventArgs e)
        {
            if (ПолеДляВвода_ЭлектронныйАдрес.Text == string.Empty)
            {
                MessageBox.Show("Введите свой электронный адрес", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (ПолеДляВвода_Письмо.Text == string.Empty)
            {
                MessageBox.Show("Введите текст сообщения", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                ОтправитьЭлектронноеСообщение();
            }
            catch
            {
                MessageBox.Show("Неправильный почтовый адрес", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ОтправитьСообщениеТехничекойПоддержке();
            MessageBox.Show("Письмо отправлено!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        /// <summary>
        /// Отправка письма технической поддержке
        /// </summary>
        void ОтправитьСообщениеТехничекойПоддержке()
        {
            Модели.Письма_технической_поддержки Новое_письмо = new Модели.Письма_технической_поддержки
            {
                Статус = "Проверяется",
                Письмо = ПолеДляВвода_Письмо.Text,
                Электронная_почта = ПолеДляВвода_ЭлектронныйАдрес.Text
            };
            БазаДанных.Письма_технической_поддержки.Add(Новое_письмо);
            БазаДанных.SaveChanges();
        }

        /// <summary>
        /// Отправка тестового электронного письма пользователю
        /// </summary>
        void ОтправитьЭлектронноеСообщение()
        {
            MailAddress From = new MailAddress(Классы.ПараметрыЭлектроннойПочты.АдресЭлектроннойПочты, Классы.ПараметрыЭлектроннойПочты.ОтображаемоеИмя);
            MailAddress To = new MailAddress(ПолеДляВвода_ЭлектронныйАдрес.Text);

            MailMessage msg = new MailMessage(From, To)
            {
                Subject = "Техническая поддержка",

                Body = "Вы успешно отправили сообщение технической поддержке, пожалуйста, ожидайте ответа.",

                IsBodyHtml = true
            };
            SmtpClient smtp = new SmtpClient(Классы.ПараметрыЭлектроннойПочты.SMTP_Адрес, Классы.ПараметрыЭлектроннойПочты.SMTP_Порт)
            {
                Credentials = new NetworkCredential(Классы.ПараметрыЭлектроннойПочты.АдресЭлектроннойПочты, Классы.ПараметрыЭлектроннойПочты.ПарольЭлектроннойПочты),
                EnableSsl = true
            };
            smtp.Send(msg);
        }
    }
}
