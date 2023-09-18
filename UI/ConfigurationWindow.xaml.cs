using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        static string ConfigureDetailsPath = (Environment.CurrentDirectory.ToString() + @"\ConfigureDetails.txt");
        string UserName = "";
        string Password = "";
        int Port = 0;

        string ConfigDetailFormat = "";

        public ConfigurationWindow()
        {
            InitializeComponent();
        }


        private async void btnSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //save the detail in a local txt file 
                UserName = txtUserName.Text;
                Password = txtPassword.Text;
                Port = int.Parse(txtPort.Text);
                ConfigDetailFormat = $"username:{UserName}.password:{Password}.port:{Port}|";
                File.AppendAllText(ConfigureDetailsPath, ConfigDetailFormat);
                this.Close();
            }
            catch
            {
                MessageBox.Show("Can Not Save Details");
                this.Close();
            }

        }
    }
}
