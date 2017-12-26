using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Net;
using System.IO;

namespace image_finder
{
    public partial class Form1 : Form
    {
        string searchText = null;
        string searchID = "009971028505315732222:bk2l2qliuws";
        string key = "AIzaSyAGmbxC2vaACyOA_qZq5Yzt2blPrU-KrOo";
        //ArrayList imageArr = new ArrayList(Image);
        List<Image> imageArr = new List<Image>();
        //Image[] imageArr;
        int imageArrIndex;
        int imageArrSize;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            searchText = t.Text;
            Console.WriteLine(searchText);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            imageArr.Clear();
            imageArrIndex = 0;
            imageArrSize = 0;
            if (!searchText.Equals(""))
            {
                var svc = new Google.Apis.Customsearch.v1.CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer {ApiKey = key });
                Console.WriteLine(searchText);
                //var listRequest = svc.Cse.List(searchText);
                Google.Apis.Customsearch.v1.CseResource.ListRequest listRequest = svc.Cse.List(searchText);
                listRequest.Cx = searchID;
                listRequest.SearchType = Google.Apis.Customsearch.v1.CseResource.ListRequest.SearchTypeEnum.Image;

                for (int i = 0; i < 5; i++)
                {
                    listRequest.Start = 1 + 10*i;
                    Google.Apis.Customsearch.v1.Data.Search search = listRequest.Execute();
                    foreach (var result in search.Items)
                    {
                        imageArrIndex++;
                        imageArr.Add((Image)BitmapFromURL(result.Image.ThumbnailLink));
                    }
                }
                imageArrSize = imageArrIndex;
                imageArrIndex = 0;
                
                pictureBox1.Image = imageArr[imageArrIndex];
                Clipboard.SetImage(pictureBox1.Image);
            }
        }

        public Bitmap BitmapFromURL(string URL)
        {
            try
            {
                WebClient Downloader = new WebClient();
                Stream ImageStream = Downloader.OpenRead(URL);
                Bitmap DownloadImage = Bitmap.FromStream(ImageStream) as Bitmap;
                return DownloadImage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void preButton_Click(object sender, EventArgs e)
        {
            if(imageArrIndex != 0)
            {
                imageArrIndex--;
                pictureBox1.Image = imageArr[imageArrIndex];
                Clipboard.SetImage(pictureBox1.Image);
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (imageArrIndex != imageArrSize-1)
            {
                imageArrIndex++;
                pictureBox1.Image = imageArr[imageArrIndex];
                Clipboard.SetImage(pictureBox1.Image);
            }
        }
    }
}
