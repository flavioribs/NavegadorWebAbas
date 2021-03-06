﻿using System;
using System.IO;
using System.Windows.Forms;

namespace NavegadorWebAbas
{
    public partial class FormNavegador : Form
    {
        private string urlNav;
        private Boolean primeiraVez;
        public FormNavegador(string[] args)
        {
            InitializeComponent();

            primeiraVez = true;

            if (args.Length > 0)
                urlNav = args[0];
            else
                urlNav = "https://www.google.com";
        }

        private void btnNovaGuia_Click(object sender, EventArgs e)
        {
            NovaGuia();
        }

        private void NovaGuia()
        {
            TabPage tab = new TabPage();
            tab.Text = "Nova Guia";
            tabControl1.Controls.Add(tab);
            tabControl1.SelectTab(tabControl1.TabCount - 1);
            WebBrowser browser = new WebBrowser() { ScriptErrorsSuppressed = true };
            browser.Parent = tab;
            browser.Dock = DockStyle.Fill;
            browser.Navigate(urlNav);
            txtUrl.Text = urlNav;
            browser.DocumentCompleted += Browser_DocumentCompleted;
        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (ckeFecharNavegador.Checked)
              timerFechar.Enabled = true;
        }

        private void btnRetorna_Click(object sender, EventArgs e)
        {
            WebBrowser browser = tabControl1.SelectedTab.Controls[0] as WebBrowser;
            if (browser != null)
            {
                browser.Navigate(txtUrl.Text);
            }
        }

        private void btnAvanca_Click(object sender, EventArgs e)
        {
            WebBrowser browser = tabControl1.SelectedTab.Controls[0] as WebBrowser;
            if (browser != null)
            {
                if (browser.CanGoForward)
                    browser.GoForward();
            }
        }

        private void btnIr_Click(object sender, EventArgs e)
        {
            WebBrowser browser = tabControl1.SelectedTab.Controls[0] as WebBrowser;
            if (browser != null)
            {
                browser.Navigate(txtUrl.Text);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NovaGuia();
        }

        private void timerFechar_Tick(object sender, EventArgs e)
        {
            if (primeiraVez)
            {
                primeiraVez = false;
            }
            else
            {
                WebBrowser browser = tabControl1.SelectedTab.Controls[0] as WebBrowser;
                if (browser != null)
                {
                    tabControl1.SelectedTab.Text = browser.DocumentTitle;

                    string mydocpath =
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    var dataHoraAtual = DateTime.Now;

                    if (!Directory.Exists(string.Format(@"{0}\Logs", mydocpath)))
                    {
                        Directory.CreateDirectory(string.Format(@"{0}\Logs", mydocpath));
                    }

                    var filename = string.Format(@"{0}\Logs\LogNavegacao_{1}_{2}.html", mydocpath, dataHoraAtual.ToString("yyyy-MM-dd'_'HH'-'mm'-'ss"), browser.DocumentTitle);

                    using (StreamWriter outputFile = new StreamWriter(filename))
                    {
                        outputFile.WriteLine(browser.DocumentTitle);
                        outputFile.WriteLine(browser.Url);
                        outputFile.WriteLine(browser.DocumentText);
                    }
                }

                Close();
            }
        }
    }
}
