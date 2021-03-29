using Microsoft.Win32;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows;

namespace Государственный_заказ_Челябинской_области.Интерфейс.ГлавноеМеню
{
    /// <summary>
    /// Форма для регистрации новых пользователей
    /// </summary>
    public partial class Регистрация : Window
    {
        public static string ПутьДоКартинки = string.Empty;
        public Регистрация()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Кнопка выбора аватара пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ВыбратьАватар_Кнопка(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ДиалогОткрытияКартинки = new OpenFileDialog
            {
                Filter = "Файлы изображений (*.bmp, *.jpg, *.jpeg, *.png)|*.bmp;*.jpg;*.jpeg;*.png"
            };
            if (ДиалогОткрытияКартинки.ShowDialog() == true)
            {
                ПутьДоКартинки = ДиалогОткрытияКартинки.FileName;
                КнопкаВыбораАватара.Content = "Аватар выбран";
            }
        }
        /// <summary>
        /// Клавиша регистрации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Регистрация_Кнопка(object sender, RoutedEventArgs e)
        {
            Модели.ПараметрыСоединенияБазыДанных БазаДанных = new Модели.ПараметрыСоединенияБазыДанных();
            //Проверка полей на заполненность
            if (ПолеДляВвода_ЭлектронныйАдрес.Text == string.Empty)
                MessageBox.Show("Необходимо ввести адрес электронной почты", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (ПолеДляВвода_Имя.Text == string.Empty)
                MessageBox.Show("Необходимо ввести имя", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (ПолеДляВвода_ИНН.Text == string.Empty)
                MessageBox.Show("Необходимо ввести ИНН почты", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (ПолеДляВвода_Фамилия.Text == string.Empty)
                MessageBox.Show("Необходимо ввести фамилию", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (ПолеДляВвода_Логин.Text == string.Empty)
                MessageBox.Show("Необходимо ввести логин", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (ПолеДляВвода_НомерПаспорта.Text == string.Empty)
                MessageBox.Show("Необходимо ввести номер паспорта", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (ПолеДляВвода_СерияПаспорта.Text == string.Empty)
                MessageBox.Show("Необходимо ввести серию паспорта", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (ПутьДоКартинки == string.Empty)
            {
                MessageBox.Show("Необходимо выбрать аватар", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //Проверка на существование логина
                try
                {
                    БазаДанных.Пользователи.First(x => x.Логин == ПолеДляВвода_Логин.Text);
                    MessageBox.Show("Данный логин уже существует", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch
                {
                    //Проверка на существование адреса электронной почты
                    try
                    {
                        БазаДанных.Пользователи.First(x => x.Электронная_почта == ПолеДляВвода_Логин.Text);
                        MessageBox.Show("Данная электронаня почта уже зарегистрирована", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    catch
                    {
                        string СгенерированныйПароль = СгенерироватьПароль();
                        Модели.Пользователи Новый_пользователь = new Модели.Пользователи
                        {
                            Имя = ПолеДляВвода_Имя.Text,
                            Фамилия = ПолеДляВвода_Фамилия.Text,
                            Электронная_почта = ПолеДляВвода_ЭлектронныйАдрес.Text
                        };
                        try
                        {
                            Новый_пользователь.ИНН = ПолеДляВвода_ИНН.Text;
                            Новый_пользователь.Логин = ПолеДляВвода_Логин.Text;
                            Новый_пользователь.Номер_паспорта = ПолеДляВвода_НомерПаспорта.Text;
                            Новый_пользователь.Серия_паспорта = ПолеДляВвода_СерияПаспорта.Text;
                            Новый_пользователь.Пароль = СгенерированныйПароль;
                            Новый_пользователь.Дата_регистрации = DateTime.Now.ToString();
                            БазаДанных.Пользователи.Add(Новый_пользователь);
                            try
                            {
                                ОтправитьЭлектронноеСообщение(СгенерированныйПароль);
                                БазаДанных.SaveChanges();
                                БазаДанных.Database.ExecuteSqlCommand("UPDATE Пользователи SET Аватар = (SELECT * FROM OPENROWSET(BULK '" + ПутьДоКартинки + "', SINGLE_BLOB) AS image) WHERE Логин = '" + ПолеДляВвода_Логин.Text + "'");
                                БазаДанных.SaveChanges();
                                MessageBox.Show("Аккаунт зарегистрирован, данные для входа высланы на Вашу электронную почту", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
                            }
                            catch
                            {
                                MessageBox.Show("Проверьте свой адрес электронной почты", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Неверно введены данные", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
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
        /// <param name="Пароль"></param>
        void ОтправитьЭлектронноеСообщение(string Пароль)
        {
            MailAddress From = new MailAddress(Классы.ПараметрыЭлектроннойПочты.АдресЭлектроннойПочты, Классы.ПараметрыЭлектроннойПочты.ОтображаемоеИмя);
            MailAddress To = new MailAddress(ПолеДляВвода_ЭлектронныйАдрес.Text);

            MailMessage msg = new MailMessage(From, To)
            {
                Subject = "Регистрация на портале 'Государственный заказ Челябинской области'",

                Body = "Приветствуем Вас на нашем замечательном портале 'Государственный заказ Челябинской области'<br>" +
                "Ваш логин: " + ПолеДляВвода_Логин.Text + "<br>" +
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