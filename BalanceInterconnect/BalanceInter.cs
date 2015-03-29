using System;
using System.Windows.Forms;
using xNet.Net;
using System.IO;

namespace BalanceInterconnect
{
    public partial class FormMain : System.Windows.Forms.Form
    {
   
        private Login login;
        private Double balance;
        private bool showoBalance, balanceMin;
        private int day;
        private bool remember;
        private string pathKey;

        public FormMain()
        {
            InitializeComponent();
            
            Reg();
            balance = 0;
            pathKey ="remember.ibk";
            try
            {
                remember = File.Exists(pathKey);
                day = DateTime.Now.Day;

                if (remember)
                {
                    var userDate = File.ReadAllText(pathKey).Split(';');
                    if (userDate.Length == 2)
                    {
                        txtUserName.Text = userDate[0];
                        txtPassword.Text = userDate[1];
                    }
                }

                checkBoxRemember.Checked = remember;
            }
            catch (Exception err)
            {
                Application.Exit();
            }
            
        }

        private void Reg()
        {

            Microsoft.Win32.RegistryKey key =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);

            //добавляем первый параметр - название ключа  
            // Второй параметр - это путь к   
            // исполняемому файлу нашей программы.  
           /* if (key.GetValue("BalanceInter", Environment.CurrentDirectory) == null)
            {
                key.De("BalanceInter", Environment.CurrentDirectory);
            }
            */
            key.SetValue("BalanceInter", System.Reflection.Assembly.GetExecutingAssembly().Location);
            key.Close();
        }

        private void btnLogun_Click(object sender, EventArgs e)
        {
            try
            {
                balanceMin = false;
                showoBalance = false;
                string userDate,
                       user = txtUserName.Text,
                       log = txtPassword.Text;

                if (checkBoxRemember.Checked)
                {
                    userDate = String.Format("{0};{1}", user, log);
                    File.WriteAllText(pathKey, userDate);
                }
                else
                    File.Delete(pathKey);

                
                this.WindowState = FormWindowState.Minimized;
                this.Hide();

                login = new Login(user, log);

                UpdateBalance();
                timer.Start();
            }
            catch (Exception  err)
            {
                Application.Exit();
            }
        }

        private void UpdateBalance()
        {
            try
            {
                balance = login.GetBalance();
                if (balance <= 25 && !balanceMin)
                {
                    balanceMin = true;
                    MessageBox.Show(String.Format("Пополни баланс, осталось {0} руб.",balance));
                }
                if (balance > 25 && balanceMin)
                {
                    balanceMin = false;
                }

                notifyIcon.Text =String.Format("Баланс {0} руб.", balance.ToString());

                if (day != DateTime.Now.Day || !showoBalance)
                {
                    showoBalance = true;
                    balanceMin = false;
                    notifyIcon.BalloonTipText = balance.ToString();
                    notifyIcon.ShowBalloonTip(3000);
                }
                
            }
            catch (LoginException err)
            {
                MessageBox.Show(err.Message);

                timer.Stop();

                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            catch (HttpException err)
            { }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                Application.Exit();
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void userDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
       }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if(remember)
                btnLogun_Click(sender, e);
            
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            if (remember)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateBalance();
        }
    }
}
