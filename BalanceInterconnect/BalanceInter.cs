using System;
using System.Windows.Forms;
using xNet.Net;
using System.IO;

namespace BalanceInterconnect
{
    public partial class FormMain : System.Windows.Forms.Form,IView
    {
        public event EventHandler<ChangedRememberEventArgs> ChangedRemember;
        public event EventHandler<ChangedWindowStartEventArgs> ChangetWindowStart;
        public event EventHandler<EntranceEventArgs> Entrance;
        public event EventHandler UpdateBalance;
        public event EventHandler LoadWindow;

        bool NotClose;
        public FormMain()
        {
            InitializeComponent();

            NotClose = true;

            checkBoxStartWindows.CheckedChanged += CheckBoxStartWindowsCheckedChanged;
            checkBoxRemember.CheckedChanged += CheckBoxRememberCheckedChanged;
            btnLogin.Click += BtnLoginClick;
            txtUserName.Leave += TxtUserDateChanged;
            txtPassword.Leave += TxtUserDateChanged;
            this.Shown += FormMainShown;


            checkBoxRemember.Checked = Properties.Settings.Default.Remember;
            checkBoxStartWindows.Checked = Properties.Settings.Default.StartWindows;
         }

        private void TxtUserDateChanged(object sender, EventArgs e)
        {
            if (placeholder.GetPlaceholder(txtPassword) != txtPassword.Text &&
               placeholder.GetPlaceholder(txtUserName) != txtUserName.Text)
                btnLogin.Enabled = true;
            else
                btnLogin.Enabled = false;
        }

        public void MessageBalloon(string title, string text)
        {
            if (InvokeRequired)
            {
                Invoke((Action)
                (() =>
                {
                    notifyIcon.BalloonTipTitle = title;
                    notifyIcon.BalloonTipText = text;
                    notifyIcon.Text = title + ": " + text;

                    notifyIcon.ShowBalloonTip(3000);
                }
                ));
            }
            else
            {
                notifyIcon.BalloonTipTitle = title;
                notifyIcon.BalloonTipText = text;
                notifyIcon.Text = title + ": " + text;

                notifyIcon.ShowBalloonTip(3000);
            }
        }

        public void SetNotifyText(string text)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => notifyIcon.Text = text));
            }
            else
                notifyIcon.Text = text;
        }
        public void MessageService(string text, string caption)
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => MessageBox.Show(text, caption)));
            }
            else
                MessageBox.Show(text, caption);
        }

        private void FormMainShown(object sender, EventArgs e)
        {
            if (checkBoxRemember.Checked)
            {
                if (LoadWindow != null)
                    LoadWindow(this, e);
                if (Entrance != null && placeholder.GetPlaceholder(txtPassword) != txtPassword.Text &&
                     placeholder.GetPlaceholder(txtUserName) != txtUserName.Text)
                    Entrance(this, new EntranceEventArgs(UserName, Password));
            }
            checkBoxRemember.Focus();
        }

        private void BtnLoginClick(object sender, EventArgs e)
        {
            timer.Stop();

            if (Entrance != null)
                Entrance(this, new EntranceEventArgs(UserName, Password));

            //timer.Start();
        }

        public string UserName
        {
            get
            {
                return txtUserName.Text;
            }
            set
            {
                txtUserName.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return txtPassword.Text;
            }
            set
            {
                txtPassword.Text = value;
            }
        }
        public bool Remember
        {
            get
            {
                return checkBoxRemember.Checked;
            }
        }

        public new void Hide()
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() =>
                {
                    this.WindowState = FormWindowState.Minimized;
                    base.Hide();
                }
                ));
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                base.Hide();
            }
            
        }

        public new void Show()
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() =>
                {
                    base.Show();
                    this.WindowState = FormWindowState.Normal;
                    TxtUserDateChanged(this, EventArgs.Empty);
                }));
            }
            else
            {
                base.Show();
                this.WindowState = FormWindowState.Normal;
                TxtUserDateChanged(this, EventArgs.Empty);
            }
            
        }

        public void StartTimer()
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => timer.Start()));
            }
            else
                timer.Start();
        }

        public void StopTimer()
        {
            if (InvokeRequired)
            {
                Invoke((Action)(() => timer.Stop()));
            }
            else
                timer.Stop();
        }

        private void CheckBoxRememberCheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Remember = checkBoxRemember.Checked;
            Properties.Settings.Default.Save();
            if (ChangedRemember != null)
                ChangedRemember(this, new ChangedRememberEventArgs(checkBoxRemember.Checked));
        }

        private void CheckBoxStartWindowsCheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartWindows  = checkBoxStartWindows.Checked;
            Properties.Settings.Default.Save();
            if (ChangetWindowStart != null)
                ChangetWindowStart(this, new ChangedWindowStartEventArgs(checkBoxStartWindows.Checked));
        }

        private void exitToolStripMenuItemClick(object sender, EventArgs e)
        {
            NotClose = false;
            Application.Exit();
        }

        private void timerTick(object sender, EventArgs e)
        {
            if (UpdateBalance != null)
                UpdateBalance(this, e);
        }

        private void checkBoxStartWindowsCheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.StartWindows = checkBoxStartWindows.Checked;
            Properties.Settings.Default.Save();
            if (ChangetWindowStart != null)
                ChangetWindowStart(this, new ChangedWindowStartEventArgs(checkBoxStartWindows.Checked));
        }
        
        private void checkBoxRememberCheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Remember = checkBoxRemember.Checked;
            Properties.Settings.Default.Save();
            if (ChangedRemember != null)
                ChangedRemember(this,  new ChangedRememberEventArgs(checkBoxRemember.Checked));
        }

        private void ShowFromNotify(object sender, EventArgs e)
        {
            this.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = NotClose;
            this.Hide();
        }

        
    }
}
