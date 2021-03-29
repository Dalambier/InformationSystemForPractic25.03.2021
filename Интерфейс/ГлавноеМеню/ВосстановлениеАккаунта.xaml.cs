using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace Государственный_заказ_Челябинской_области.Интерфейс.ГлавноеМеню
{
    /// <summary>
    /// Форма для восстановления доступа к аккаунту
    /// </summary>
    public partial class ВосстановлениеАккаунта : Window
    {
        Модели.ПараметрыСоединенияБазыДанных БазаДанных = new Модели.ПараметрыСоединенияБазыДанных();
        public ВосстановлениеАккаунта()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Кнопка для отпраки данных и смены пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ОтправитьДанные_Кнопка(object sender, RoutedEventArgs e)
        {
            if (ПолеДляВвода_АдресЭлектроннойПочты.Text == string.Empty)
            {
                MessageBox.Show("Поле пустое", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                БазаДанных.Пользователи.First(x => x.Электронная_почта == ПолеДляВвода_АдресЭлектроннойПочты.Text);
            }
            catch
            {
                MessageBox.Show("На данный адрес электронной почты ничего не зарегистрировано", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var ПользовательскаяСтрока = БазаДанных.Пользователи.First(x => x.Электронная_почта == ПолеДляВвода_АдресЭлектроннойПочты.Text);
            ПользовательскаяСтрока.Пароль = СгенерироватьПароль();
            БазаДанных.SaveChanges();
            ОтправитьЭлектронноеСообщение(ПользовательскаяСтрока.Логин, ПользовательскаяСтрока.Пароль);
            MessageBox.Show("Данные отправлены на электронную почту", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        /// <summary>
        /// Метод генерации пароля
        /// </summary>
        /// <returns></returns>
        string СгенерироватьПароль()
        {
            Random Рандом = new Random();
            string Пароль = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                int type = Рандом.Next(1, 4);
                if (type == 1)
                    Пароль += (char)Рандом.Next('A', 'Z' + 1);
                else if (type == 2)
                    Пароль += (char)Рандом.Next('a', 'z' + 1);
                else if (type == 3)
                    Пароль += Рандом.Next(0, 9);
            }
            return Пароль;
        }
        /// <summary>
        /// Метод отправки сообщений на электронную почту
        /// </summary>
        /// <param name="Pass"></param>
        void ОтправитьЭлектронноеСообщение(string Логин, string Пароль)
        {
            MailAddress From = new MailAddress(Классы.ПараметрыЭлектроннойПочты.АдресЭлектроннойПочты, Классы.ПараметрыЭлектроннойПочты.ОтображаемоеИмя);

            MailAddress To = new MailAddress(ПолеДляВвода_АдресЭлектроннойПочты.Text);

            MailMessage msg = new MailMessage(From, To)
            {
                Subject = "Восстановление доступа к аккаунту 'Государственный заказ Челябинской области'",

                Body = "Ваш пароль был перегенерирован, можете не беспокоиться<br>" +
                "Ваш логин: " + Логин + "<br>" +
                "Ваш пароль: " + Пароль + "<br>" +
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
    }
}
