using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AspWpfClientSignaRApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection hubConnection;

        public MainWindow()
        {
            InitializeComponent();

            hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7101/chat")
                .Build();

            hubConnection.On<string, string>("Receive", (message, userName) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var messageFull = $"{userName}: {message}";
                    chatRoom.Items.Insert(0, messageFull);
                });
            });
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await hubConnection.StartAsync();
                chatRoom.Items.Add("You comming to chat");
                btnSend.IsEnabled = true;
            }
            catch (Exception ex)
            {
                chatRoom.Items.Add(ex.Message);
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await hubConnection.InvokeAsync("Send", message.Text, userName.Text);
            }
            catch (Exception ex)
            {
                chatRoom.Items.Add(ex.Message);
            }
        }
    }
}