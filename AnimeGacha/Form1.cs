using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace AnimeGacha
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SigleGacha();
        }

        public async void SigleGacha(){
            label1.Text = "loading...";
            label2.Text = "StartDay loding...";
            pictureBox1.Image = (Image)Properties.Resources.loadanim;
            var client = new System.Net.Http.HttpClient();
            var s = await client.GetStringAsync("http://gatya.aokibazooka.net/api");
            dynamic j = JsonConvert.DeserializeObject(s);
            if (j.urls.Count > 0)
            {
                Uri imgurl = j.urls[0];
                try
                {
                    var img = await client.GetByteArrayAsync(imgurl);
                    label1.Text = j.anime.name;
                    label2.Text = "StartDay " + j.anime.startDay;
                    if (img != null) pictureBox1.Image = (Image)new ImageConverter().ConvertFrom(img);
                }
                catch
                {

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var otherwindow = Application.OpenForms.Cast<Form1>().Where(v => v != this).ToArray();
            foreach (var other in otherwindow)
            {
                other.Close();
            }
            this.SigleGacha();
            for (int i = 0; i < 9; i++)
            {
                var f = new Form1();
                f.button1.Visible = false;
                f.button2.Visible = false;
                f.Click += (fs,fe) => f.Close();
                f.pictureBox1.Click += (fs, fe) => f.Close();
                f.Show();
                f.SigleGacha();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {

        }
    }
}
