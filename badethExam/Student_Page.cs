using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
namespace badethExam
{
    public partial class Student_Page : Form
    {
        private const string ServerName = "localhost";
        private const string DatabaseName = "studentinfodb";
        private const string UUID = "root";
        private const string Password = "12345";

        private DataTable dt = new DataTable();

        private MySqlConnection con;
        private MySqlCommand cmd;
        private MySqlDataReader? reader;

        public Student_Page()
        {
            InitializeComponent();

            con = new MySqlConnection($"Server={ServerName}; Database={DatabaseName}; Uid={UUID}; Password={Password}");
            cmd = new MySqlCommand();
            cmd.Connection = this.con;
            if (!connect())
            {
                MessageBox.Show("Please configure your connection");
            }
        }

        public bool connect()
        {
            if (this.con.State == ConnectionState.Closed || this.con.State == ConnectionState.Broken)
            {
                try
                {
                    this.con.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Connection failed: " + ex.Message);
                    return false;
                }
            }
            return true;
        }

        public void disconnect()
        {
            if (this.con.State == ConnectionState.Open)
            {
                this.con.Close();
            }
        }

        private void studentPage_load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                load_Students();
            }
            else
            {
                Application.Exit();
            }
        }

        private void load_Students()
        {
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT studentId, CONCAT(firstName, ' ', lastName, ' ', middleName) AS FullName FROM studentrecordtb";
                reader = cmd.ExecuteReader();

                dt.Clear();
                dt.Load(reader);
                dataGridView1.DataSource = dt;

                dataGridView1.Columns["FullName"].HeaderText = "Full Name";  
                dataGridView1.Columns["studentId"].HeaderText = "Student ID"; 

                // Add a button column for the VIEW button if not already added
                if (!dataGridView1.Columns.Contains("ViewDetails"))
                {
                    DataGridViewButtonColumn viewColumn = new DataGridViewButtonColumn();
                    viewColumn.Name = "ViewDetails";
                    viewColumn.HeaderText = "View";
                    viewColumn.Text = "VIEW";
                    viewColumn.UseColumnTextForButtonValue = true;
                    dataGridView1.Columns.Add(viewColumn);
                }

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Student loading error: " + ex.Message);
            }
            finally
            {
                reader?.Close();
            }
        }



        private void dataGridView1_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["ViewDetails"].Index && e.RowIndex >= 0)
            {
                string studentId = dataGridView1.Rows[e.RowIndex].Cells["studentId"].Value.ToString();
                StudentPage_Individual detailsForm = new StudentPage_Individual(studentId);
                detailsForm.Show();
            }
        }
    }
}

