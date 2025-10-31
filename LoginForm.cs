using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gallery
{
    public partial class LoginForm : Form
    {
        private string connectionString;

        // Словарь с ролями и паролями 
        private Dictionary<string, string> validUsers = new Dictionary<string, string>
        {
            { "Товаровед", "111" },
            { "Бухгалтер", "222" },
            { "Админ", "333" }
        };

        public LoginForm()
        {
            InitializeComponent();
            //string connectionString = @"Data Source=adc1;Initial Catalog=GalleryDB;Integrated Security=True";string connectionString = @"Data Source=adc1;Initial Catalog=GalleryDB;User ID=ВАШ_ЛОГИН_ОТ_СИСТЕМЫ;Password=ВАШ_ПАРОЛЬ_ОТ_СИСТЕМЫ";
            //connectionString = "Data Source=222909071093-2\\SQLEXPRESS;Initial Catalog=1093Antonova;Integrated Security=True;";
            connectionString = @"Data Source=ADCLG1;Initial Catalog=1093Antonova;Integrated Security=True";
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

            private void btnLogin_Click(object sender, EventArgs e)
            {
                string role = txtUsername.Text.Trim(); // Добавил Trim() для удаления пробелов
                string password = txtPassword.Text;

                if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Пожалуйста, введите роль и пароль.");
                    return;
                }

                // Проверяем логин и пароль 
                if (validUsers.ContainsKey(role) && validUsers[role] == password)
                {
                    this.Hide();

                    Form mainForm = null;
                    switch (role)
                    {
                        case "Админ":
                            mainForm = new AdminForm(connectionString); 
                            break;
                        case "Товаровед":
                            mainForm = new TovarovedForm(connectionString); 
                            break;
                        case "Бухгалтер":
                            mainForm = new BuhgalterForm(connectionString); 
                            break;
                        default:
                            MessageBox.Show("Неизвестная роль пользователя.");
                            this.Show();
                            return;
                    }

                    if (mainForm != null)
                    {
                        mainForm.FormClosed += (s, args) => this.Close();
                        mainForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Неверная роль или пароль. Доступные роли: Товаровед (111), Бухгалтер (222), Админ (333)");
                }
            }

            // Добавляем обработчик нажатия Enter в текстовых полях
            private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    btnLogin_Click(sender, e);
                }
            }

            private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    btnLogin_Click(sender, e);
                }
            }
    }
    }