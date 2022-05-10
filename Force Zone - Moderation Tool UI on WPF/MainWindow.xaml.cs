using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Force_Zone___Moderation_Tool_UI_on_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Dat : IDataErrorInfo
    {
        public string Login { get; set; }
        public string Error { get => throw new NotImplementedException(); }
        public string this[string name]
        {
            get
            {
                switch (name)
                {
                    case "Login":

                        if (Login != null && Regex.IsMatch(Login, "[а-яА-ЯеЁ]")) return "Логин должен быть написан\n на английском языке!";
                        if (Login != null && Login.Length < 3) return "Логин меньше 3 букв!";
                        return String.Empty;
                    default:
                        return String.Empty;
                }
            }
        }
    }

    public partial class MainWindow : Window
    {
        private int failed = 0;
        private static bool timeOut;

        public Dat dat;
        public MainWindow()
        {
            dat = new Dat();
            this.DataContext = dat;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region
            if (failed > 3)
            {
                if(!timeOut)
                {
                    rez.Text = "Попробуйте войти позже!";
                    return;
                } else SetTimer();
            }
            if (login.Text == String.Empty)
            {
                rez.Text = "Введите логин!";
                return;
            }
            if (password.Password == String.Empty)
            {
                rez.Text = "Введите пароль!";
                return;
            }
            if (login.Text.Length < 3)
            {
                rez.Text = "Логин меньше 3 букв!";
                return;
            }
            if (Regex.IsMatch(login.Text, "[а-яА-ЯеЁ]"))
            {
                rez.Text = "Логин должен быть на английском!";
                return;
            }
                if (!Login(login.Text, password.Password))
            {
                failed++;
                rez.Text = "Неверный пароль!";
                return;
            }
            #endregion

            else rez.Text = "Успешный вход!";
        }

        private bool Login(string login, string password)
        {
            var wb = new WebClient();
            return wb.DownloadString($"https://moderation-tool.forcezone.ru/api/login/get?login={login}&password={password}&modtool=true") switch
            {
                "true" => true,
                _ => false
            };
        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            var aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            timeOut = true;
        }

        private void SiteRedirect_Click(object sender, RoutedEventArgs e)
        {
            var destinationurl = "https://moderation-tool.forcezone.ru/";
            var sInfo = new System.Diagnostics.ProcessStartInfo(destinationurl)
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }
    }
}
