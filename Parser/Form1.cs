using Parser.ParserDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser
{
    public partial class Form1 : Form
    {
        private List<SaveReferences> ListSR = new List<SaveReferences>();
        private bool isWorking = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void MakesChoicePages(List<SaveReferences> pages)
        {
            CheckedListBox checkedLB = new CheckedListBox();
            checkedLB.BorderStyle = BorderStyle.None;
            checkedLB.Location = new System.Drawing.Point(215, 25);
            checkedLB.BackColor = Color.FromKnownColor(KnownColor.Control);
            checkedLB.Width = 200;
            checkedLB.Height = 289;
            for (int i = 0; i < pages.Count; i++)
            {
                CheckBox temp = new CheckBox();
                temp.Name = pages[i].Name;
                temp.Text = pages[i].Name;
                checkedLB.Items.Add(temp);
            }
            checkedLB.DisplayMember = "Name";
            checkedLB.ItemCheck += new ItemCheckEventHandler(this.ChangedCheckBox);
            this.Controls.Add(checkedLB);
        }

        private void ChangedCheckBox(object sender, EventArgs e)
        {
            CheckedListBox checkedLb = (CheckedListBox)sender;
            ListSR[checkedLb.SelectedIndex].IsDownload = !ListSR[checkedLb.SelectedIndex].IsDownload;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (ParserHtml.ValidateUri(Url.Text))
            {
                log.Text = "";
                Parser ViewPages = new Parser(Url.Text);
                ListSR = ViewPages.StartGetPages();
                MakesChoicePages(ListSR);
                Guna.UI2.WinForms.Guna2Button parserB = new Guna.UI2.WinForms.Guna2Button();
                parserB.BorderRadius = 15;
                parserB.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
                parserB.Size = new System.Drawing.Size(141, 27);
                parserB.Text = "Спарсить";
                parserB.Location = new Point(239, 321);
                parserB.Click += new System.EventHandler(this.Start);
                parserB.ForeColor = Color.FromKnownColor(KnownColor.Black);
                this.Controls.Add(parserB);
            }
            else
            {
                log.Text = "Не валидный Url";
            }
        }
        /* http://www.css-tricks.ru/articles/css/attribute-selectors */
        /*https://megabit32.ru/ */
        private async void Start(object sender, EventArgs e)
        {   
            if (isWorking) { log.Text = "Программа запущена"; return; }
            if (!ParserHtml.ValidateUri(Url.Text)) { log.Text = "Не валидный Url"; return; }
            if(Path.Text == "") { log.Text = "Заполните поле :\n'Путь для сохранения' "; return; }
            Parser main = new Parser(Url.Text, Path.Text, "index.html");
            List<SaveReferences> UsefullList = new List<SaveReferences> (main.Preparation(ListSR));
            if(UsefullList.Count == 0) { log.Text = "Не выбрана \nни одна страница"; return; }
            log.Text = "";
            isWorking = true;
            log.Text = await Task.Run(() => main.MainPage(UsefullList, this));
            isWorking = false;
        }
        public int ProgressValue
        {
            set { guna2CircleProgressBar1.Value = value; }
            get { return guna2CircleProgressBar1.Value; }
        }
        public int ProgressMaxValue
        {
            set { guna2CircleProgressBar1.Maximum = value; }
            get { return guna2CircleProgressBar1.Maximum; }
        }
        private void Path_DoubleClick(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                Path.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
