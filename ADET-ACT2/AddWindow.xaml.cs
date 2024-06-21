using MySql.Data.MySqlClient;
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
using System.Windows.Shapes;
using static ADET_ACT2.MainWindow;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ADET_ACT2
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public Student NewStudent { get; set; }
        private string connectionString = "SERVER=localhost;DATABASE=StudentDB;UID=root;PASSWORD=12345;";
        public AddWindow(Student student = null)
        {

            InitializeComponent();


            if (student != null)
            {
                IDbox.Text = student.ID;
                FirstNbox.Text = student.FirstN;
                MidNbox.Text = student.MiddleN;
                LastNbox.Text = student.LastN;
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsNumeric(IDbox.Text))
            {
                MessageBox.Show("ID should only contain numbers.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (FirstNbox.Text.Length > 100 || MidNbox.Text.Length > 100 || LastNbox.Text.Length > 100)
            {
                MessageBox.Show("First Name, Middle Name, and Last Name can only accept up to 100 characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (YearCbox.SelectedItem == null || SectionCbox.SelectedItem == null)
            {
                MessageBox.Show("Please select level and section.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO studentdb.students (ID, FirstN, MiddleN, LastN, Year, Section) " + "VALUE (@ID, @FirstN, @MiddleN, @LastN, @Year, @Section)";
                    MySqlCommand command = new MySqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@ID", IDbox.Text);
                    command.Parameters.AddWithValue("@FirstName", FirstNbox.Text);
                    command.Parameters.AddWithValue("@MiddleName", MidNbox.Text);
                    command.Parameters.AddWithValue("@LastName", LastNbox.Text);
                    command.Parameters.AddWithValue("@YearLevel", YearCbox);
                    command.Parameters.AddWithValue("@Section", SectionCbox.Text);

                    command.ExecuteNonQuery();
                    connection.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding student record: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            NewStudent = new Student
            {
                ID = IDbox.Text,
                FirstN = FirstNbox.Text,
                MiddleN = MidNbox.Text,
                LastN = LastNbox.Text,
                Year = (YearCbox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Section = (SectionCbox.SelectedItem as ComboBoxItem)?.Content.ToString()
            };
            

            this.DialogResult = true;
            this.Close();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();

        }
        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }
    }
}