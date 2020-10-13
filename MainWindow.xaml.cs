using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using Twilio;

namespace WPFNotifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<User> users = User.GetUsers();
            dataGridUsers.ItemsSource = users;
        }

        private void btnSendNotification_Click(object sender, RoutedEventArgs e)
        {
            bool requiredFields = CheckRequiredFields();
            if (requiredFields == true)
            {
                int messagesSent = 0;
                foreach (User user in dataGridUsers.ItemsSource)
                {
                    string phoneNumber = user.phoneNumber;
                    //string phoneNumber = row.Item["phoneNumber"]  .Cells["phoneNumber"].Value.ToString();
                    bool smsOptIn = user.smsOptIn;
                    if (smsOptIn == true && phoneNumber != "")
                    {
                        try
                        {
                            var sid = ConfigurationManager.AppSettings.Get("accountSid");
                            var token = ConfigurationManager.AppSettings.Get("authToken");
                            var from = ConfigurationManager.AppSettings.Get("myTwilioNumber");
                            TwilioClient.Init(sid, token);

                            //Set header text(based on radio btns), body, and footer text.
                            string header = "";
                            string footer = "\n\nThis is an automated text message. Responses will not be seen.";
                            if (newMovieRadBtn.IsChecked == true)
                            {
                                header = "PLEX - New Movie Alert: ";
                            }
                            else if (newTvShowRadBtn.IsChecked == true)
                            {
                                header = "PLEX - New TV Show Alert: ";
                            }
                            else if (maintenanceRadBtn.IsChecked == true)
                            {
                                header = "PLEX - Maintenance Alert: ";
                            }
                            else
                            {
                                header = "PLEX - " + txtBoxOther.Text + ": ";
                            }
                            string body = header + txtboxNotificationBody.Text + footer;
                            SMS.CreateMessage(phoneNumber, from, sid, body);
                            messagesSent += 1;
                        }
                        catch (Exception ex)
                        {

                            lblStatus.Content = "Error: Unable to send texts: " + ex.Message;
                        }

                    }
                }
                lblStatus.Content = "Number of text messages sent: " + messagesSent;
            }
        }

        public bool CheckRequiredFields()
        {
            int usersSelected = 0;
            foreach (User user in dataGridUsers.ItemsSource)
            {
                bool smsOptIn = user.smsOptIn;
                if (smsOptIn == true)
                {
                    usersSelected += 1;
                }
            }

            if (usersSelected == 0)
            {
                lblStatus.Content = "Error: Please select at least one User.";
                return false;
            }
            else if (txtboxNotificationBody.Text == "")
            {
                lblStatus.Content = "Error: Please enter a Notification.";
                return false;
            }
            else if (newMovieRadBtn.IsChecked == false && newTvShowRadBtn.IsChecked == false && maintenanceRadBtn.IsChecked == false && otherRadBtn.IsChecked == false)
            {
                lblStatus.Content = "Error: Please select a Notification Type";
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            if(txtBoxOther.Text != "")
            {
                txtBoxOther.Text = "";
            }
            txtboxNotificationBody.Text = "";
            lblStatus.Content = "Status:";
        }
    }
}
