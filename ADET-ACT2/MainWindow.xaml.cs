using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
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

namespace ADET_ACT2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Student> Students { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            //initializing database connection
            string connectionString = "SERVER=localhost;DATABASE=StudentDB;UID=root;PASSWORD=12345;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand("select * from studentdb.students", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            Students = new ObservableCollection<Student>();
            dataGrid.DataContext = dt;
            dataGrid.ItemsSource = Students;

        }

        public class Student
        {
            public string ID { get; set; }
            public string FirstN { get; set; }
            public string MiddleN { get; set; }
            public string LastN { get; set; }
            public string Year { get; set; }
            public string Section { get; set; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddWindow AddWindow = new AddWindow();
            if (AddWindow.ShowDialog() == true)
            {
                Students.Add(AddWindow.NewStudent);
            }

        }

        private void btnUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Modify(dataGrid.SelectedItem as Student);
            }
            else
            {
                MessageBox.Show("Please select a student to Modify.", "Update Student", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Students.Remove(dataGrid.SelectedItem as Student);
            }
            else
            {
                MessageBox.Show("Please select a student to Delete.", "Delete Student", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnRef_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.Items.Refresh();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Modify(Student student)
        {
            AddWindow addWindow = new AddWindow(student);
            if (addWindow.ShowDialog() == true)
            {
                if (student == null)
                {

                    Students.Add(addWindow.NewStudent);
                }
                else
                {

                    var existingStudent = Students.FirstOrDefault(s => s.ID == student.ID);
                    if (existingStudent != null)
                    {
                        existingStudent.FirstN = addWindow.NewStudent.FirstN;
                        existingStudent.MiddleN = addWindow.NewStudent.MiddleN;
                        existingStudent.LastN = addWindow.NewStudent.LastN;
                        existingStudent.Year = addWindow.NewStudent.Year;
                        existingStudent.Section = addWindow.NewStudent.Section;
                    }
                }
            }
        }
    }
}