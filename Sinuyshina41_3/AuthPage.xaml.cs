using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sinuyshina41_3
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();

        }

        private void BGuestEntry_Click(object sender, RoutedEventArgs e)
        {
            
            Manager.MainFrame.Navigate(new ProductPage(null));
        }
        private string cap = " ";
        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            
            string login = LoginTB.Text;
            string password = PassTB.Text;
            
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rnd = new Random();
            cap = "";
            if (login == "" || password =="")
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }

            User user = Sinuyshina41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            if(user != null)
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                LoginTB.Text = "";
                PassTB.Text = "";
            }
            else
            {
                MessageBox.Show("Введены неверные данные");
                //рандом
                for (int i = 0; i < 4; i++)
                {
                    cap += chars[rnd.Next(chars.Length)];
                }
                //массив
                TextBlock[] words = { captchaOneWord, captchaTwoWord, captchaThreeWord, captchaFourWord };

                for (int i = 0; i < 4; i++)
                {
                    // Выводим сам символ
                    words[i].Text = cap[i].ToString();

                    // перечёркивание
                    // 50% шанс, что символ будет перечеркнут
                    if (rnd.Next(2) == 0)
                        words[i].TextDecorations = TextDecorations.Strikethrough;
                    else
                        words[i].TextDecorations = null; // без перечеркивания

                    //наложение и сдвиг
                    // Для первого символа наложение не делаем, чтобы не уехал за край
                    if (i > 0 && rnd.Next(2) == 0)
                    {
                        // 50% шанс, что символ наедет на предыдущий (отрицательный отступ)
                        // rnd.Next(-15, 0) дает отступ от -15 до -1 пикселей
                        double marginLeft = rnd.Next(-15, 0);

                        // Также случайно сдвигаем символ вверх или вниз (-10 до +15 пикселей)
                        double marginTop = rnd.Next(-10, 15);

                        words[i].Margin = new Thickness(marginLeft, marginTop, 0, 0);
                    }
                    else
                    {
                        // Если наложения нет, просто ставим в линию с небольшим отступом
                        words[i].Margin = new Thickness(5, 0, 0, 0);
                    }
                }
                if (CaptchaPanel.Visibility == Visibility.Visible)
                {
                    //генерацию + отображение в капче
                    //генерация 


                    if (TBoxCaptcha.Text.ToUpper() != cap)
                    {
                        MessageBox.Show("Неверная капча");
                        LoginBtn.IsEnabled = false;
                        await Task.Delay(10000);//таймер ?
                        LoginBtn.IsEnabled = true;
                        return;
                    }


                }
                CaptchaPanel.Visibility = Visibility.Visible;
                TBoxCaptcha.Visibility = Visibility.Visible;
                TBoxCaptcha.Text = "";
                TBoxCaptcha.Focus();
               
            }
        }
    }
}
