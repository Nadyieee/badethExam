using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace badethExam
{
    public partial class StudentPage_Individual : Form
    {
        private string studentId;
        private MySqlConnection Con;
        private MySqlCommand CMD;

        public StudentPage_Individual(string studentId)
        {
            InitializeComponent();
            this.studentId = studentId;
            Con = new MySqlConnection("Server=localhost; Database=studentinfodb; Uid=root; Password=12345");
            CMD = new MySqlCommand();
            CMD.Connection = Con;
        }

        private void StudentPage_Indi_Load(object sender, EventArgs e)
        {
            LoadStudentDetail(studentId);
        }

        private void LoadStudentDetail(string studentId)
        {
            try
            {
                if (Con.State == System.Data.ConnectionState.Closed)
                {
                    Con.Open();
                }

                CMD.CommandType = CommandType.Text;
                CMD.CommandText = "SELECT " +
                    "sr.studentId, " +
                    "CONCAT(sr.firstName, ' ', sr.middleName, ' ', sr.lastName) AS fullName, " +
                    "sr.houseNo," +
                    "sr.brgyName," +
                    "sr.municipality," +
                    "sr.province," +
                    "sr.region," +
                    "sr.country," +
                    "sr.birthdate," +
                    "sr.age," +
                    "sr.studContactNo," +
                    "sr.emailAddress," +
                    "CONCAT(sr.guardianFirstName, ' ', sr.guardianLastName) as guardianName," +
                    "sr.hobbies," +
                    "sr.nickname," +
                    "c.courseName," +
                    "y.yearLvl " +
                    "FROM studentrecordtb sr " +
                    "INNER JOIN coursetb c ON sr.courseId = c.courseId " +
                    "INNER JOIN yeartb y ON sr.yearId = y.yearId " +
                    "WHERE studentId = @studentId";
                CMD.Parameters.AddWithValue("@studentId", studentId);
                MySqlDataReader reader = CMD.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StudentName.Text = reader["fullName"].ToString();
                        Address.Text = reader["houseNo"].ToString() + " " + reader["brgyName"].ToString() + ", " + reader["municipality"].ToString() + ", " + reader["province"].ToString() + ", Region " + reader["region"].ToString() + ", " + reader["country"].ToString();
                        BirthDate.Text = reader["birthdate"].ToString();
                        Age.Text = reader["age"].ToString();
                        StudentNumber.Text = reader["studContactNo"].ToString();
                        EmailAddress.Text = reader["emailAddress"].ToString();
                        GuardianName.Text = reader["guardianName"].ToString();
                        Hobbies.Text = reader["hobbies"].ToString();
                        Nickname.Text = reader["nickname"].ToString();
                        Course.Text = reader["courseName"].ToString();
                        Year.Text = reader["yearLvl"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Student details not found.");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading student details: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }
    }
}
