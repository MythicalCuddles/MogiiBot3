using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordBot.Common;
using Google.Authenticator;
using MelissaNet;

namespace DiscordBot
{
    public partial class frmAuth : Form
    {
        public frmAuth()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.txtAccountTitle.Text = "MogiiBot3";
            string key = Guid.NewGuid().ToString().Replace("-", "");
            this.txtSecretKey.Text = key;
            Configuration.UpdateConfiguration(secretKey: Cryptography.EncryptString(key));

            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var setupCode = tfA.GenerateSetupCode(this.txtAccountTitle.Text, this.txtSecretKey.Text, pbQR.Width, pbQR.Height);

            WebClient wc = new WebClient();
            MemoryStream ms = new MemoryStream(wc.DownloadData(setupCode.QrCodeSetupImageUrl));
            this.pbQR.Image = Image.FromStream(ms);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            TwoFactorAuthenticator tfA = new TwoFactorAuthenticator();
            var result = tfA.ValidateTwoFactorPIN(txtSecretKey.Text, this.txtCode.Text);

            MessageBox.Show(result ? "Code Valid! TwoAuth setup successful." : "Incorrect", "Result");

            if (result)
            {
                Close();
            }
        }
    }
}