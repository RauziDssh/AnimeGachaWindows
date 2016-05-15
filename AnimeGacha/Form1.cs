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
            this.Text = "loading...";
            label1.Text = "loading...";
            label2.Text = "StartDay loding...";
            label3.Text = "Searching...";
            pictureBox1.Image = (Image)Properties.Resources.loadanim;
            var anim = await this.GetAnimeAsync();
            try
            {
                this.SetTitle(anim);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            label3.Text = "Finish";
        }
        
        public void SetTitle(Anime anime)
        {
            this.Text = anime.title;
            label1.Text = anime.title;
            label2.Text = anime.startday;
            pictureBox1.Image = anime.image;
        }

        public class Anime
        {
            public string title { get; private set; }
            public string startday { get;private set;}
            public Image image { get;private set; }

            public Anime(string title,string startday,Image img)
            {
                this.title = title;
                this.startday = startday;
                this.image = img;
            }
        }

        public async Task<Anime> GetAnimeAsync()
        {
            var client = new System.Net.Http.HttpClient();
            var s = await client.GetStringAsync("http://gatya.aokibazooka.net/api");
            dynamic j = JsonConvert.DeserializeObject(s);
            if (j.urls.Count == 0) { return new Anime((string)j.anime.name, (string)j.anime.startDay, null); throw new HttpListenerException(); }
            else
            {
                Uri imgurl = j.urls[0];
                try
                {
                    var img = await client.GetByteArrayAsync(imgurl);
                    var imgConvereted = (Image)new ImageConverter().ConvertFrom(img);
                    return new Anime((string)j.anime.name, (string)j.anime.startDay, imgConvereted);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return new Anime("ERROR", "ERROR", null);
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
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
                f.textBox1.Visible = false;
                f.button3.Visible = false;
                f.button1.Visible = false;
                f.button2.Visible = false;
                f.Click += (fs,fe) => f.Close();
                f.pictureBox1.Click += (fs, fe) => f.Close();
                f.Show();
                var a = await f.GetAnimeAsync();
                f.SetTitle(a);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            string s = textBox1.Text;
            EternalGatcha(s,f2,0);
        }

        public async void EternalGatcha(string name,Form2 f2,int count)
        {
            var anim = await this.GetAnimeAsync();
            this.SetTitle(anim);
            if(anim.title != name)
            {
                count++;
                f2.addList(anim.title);
                label3.Text = count.ToString();
                this.EternalGatcha(name, f2, count);
            }
        }
    }
}
