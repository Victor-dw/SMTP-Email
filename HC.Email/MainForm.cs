using System;
using System.Windows.Forms;

namespace HC.Email
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var config = ConfigHelper.GetConfig<EmailConfig>();
            txtHost.Text = config.Host;
            txtPort.Text = config.Port;
            txtMailFrom.Text = config.MailFrom;
            txtUserName.Text = config.UserName;
            txtPassword.Text = config.Password;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var config = ConfigHelper.GetConfig<EmailConfig>();
                config.Host = txtHost.Text;
                config.Port = txtPort.Text;
                config.MailFrom = txtMailFrom.Text;
                config.UserName = txtUserName.Text;
                config.Password = txtPassword.Text;
                ConfigHelper.UpdateConfig(config);
                WriteSucessMsg("保存成功！");
            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
        }


        private void btnTest_Click(object sender, EventArgs e)
        {
            if (txtTestEmail.Text.Length > 0)
            {
                if (EmailService.Send(txtTestEmail.Text, "测试邮件",
                                      "<span style='color:green'>这是一封测试邮件，您能收到此邮件，说明您的邮箱参数设置正确，请勿回复此邮件。</span>"))
                {
                    WriteSucessMsg("发送成功!");
                }
                else
                {
                    WriteErrMsg("发送失败!");
                }
            }
            else
            {
                txtTestEmail.Focus();
            }
        }

        #region 提示信息

        /// <summary>
        ///     提示成功信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteSucessMsg(string msg)
        {
            MessageBox.Show(msg, "成功提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        ///     提示失败信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteErrMsg(string msg)
        {
            MessageBox.Show(msg, "错误提示信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        ///     提示信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteMsg(string msg)
        {
            MessageBox.Show(msg, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        ///     异常信息
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteException(Exception ex)
        {
            MessageBox.Show(ex.Message, "异常信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion

        #region 帮助链接

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Help.ShowHelp(this, "http://help.163.com/09/1223/14/5R7P3QI100753VB8.html");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Help.ShowHelp(this, "http://kf.qq.com/faq/120322fu63YV130422nqIrqu.html");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Help.ShowHelp(this, "http://service.exmail.qq.com/cgi-bin/help?subtype=1&&id=26&&no=308");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Help.ShowHelp(this, "http://help.163.com/10/0312/13/61J0LI3200752CLQ.html");
        }

        #endregion
    }
}