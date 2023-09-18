using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConfigurationWindow configuration = new ConfigurationWindow();

        Socket socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);        
        IPEndPoint iPEndPoint;
        IPAddress Ip = null;
        byte[] buffer = new byte[1024];
        string Messages = string.Empty;
   
        static string ConfigureDetailsPath = (Environment.CurrentDirectory.ToString()+@"\ConfigureDetails.txt");
        string[] ValueSeprator = { ":" };
        string[] WordSeprator = { "." };
        string[] LineSeprator = { "|" };
        string UserName = "";
        string Password = "";
        int Port = 0;
        string ConfigDetailFormat = "";

        List<string> Details = new List<string>();

        public MainWindow()
        {
            
            InitializeComponent();
        }

        private async Task RecMessgaes()
        {

            try
            {
                while (true)
                {
                    await socketServer.ReceiveAsync(buffer);
                    string msg = Encoding.ASCII.GetString(buffer);

                    listboxMessages.Items.Add(msg);
                    if (msg.Contains("Close"))
                        break;
                    Array.Clear(buffer);
                }
                socketServer.Close();
            }
            catch (Exception ex)
            {
                socketServer.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //read details(username,password,port,ip) from a local txt file,if dont exist read from Configuration wondow
            string ReadConfigureDetails = File.ReadAllText(ConfigureDetailsPath);
            string[] Lines = ReadConfigureDetails.Split(LineSeprator, StringSplitOptions.RemoveEmptyEntries);
            string LastLine = Lines.Last();
            string[] FullDetail = LastLine.Split(WordSeprator, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < FullDetail.Length; i++)
            {
                string[] word = FullDetail[i].Split(ValueSeprator, StringSplitOptions.RemoveEmptyEntries);
                string Detail = word.Last();
                Details.Add(Detail);
            }

            UserName = Details[0];
            Password = Details[1];
            Port = int.Parse(Details[2]);
            try
            {
                iPEndPoint = new IPEndPoint(IPAddress.Loopback, Port);
                socketServer.Bind(iPEndPoint);
                lblStat.Content += $"Listening on {iPEndPoint.Address}:{iPEndPoint.Port}";
                btnStop.Visibility = Visibility.Visible;
                await RecMessgaes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                socketServer.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                socketServer.Close();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            socketServer.Close();
            this.Close();
        }

        private void btnSetConfigure_Click(object sender, RoutedEventArgs e)
        {
            configuration.ShowDialog();
            btnStart.Visibility = Visibility.Visible;
        }
    }
}
