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
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Versioning;
using System.Xml.Linq;
using System.Drawing.Imaging;
using System.Security.Policy;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MiniChat_NguyenVanTuan
{
    public partial class Form_Client : Form
    {
        public Form_Client()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            listView2.View = View.List;
            MyConect();
        }
        int checkconect=0;
        Socket client;
        IPAddress ia;
        int port = 4321;
        IPEndPoint ipe;
        private void MyConect()
        {
            client = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            ia = IPAddress.Parse("127.0.1");
            ipe = new IPEndPoint(ia , port);
            try
            {
                client.Connect(ipe);
            }
            catch { MessageBox.Show("khoong the connect den server", " Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread listen= new Thread(SeenMess);
            listen.IsBackground = true;
            listen.Start();
        }
        private void Close()
        {
            client.Close();
        }
        void SeenMess()//nhận 
        {

            while (true)
            {
                byte[] data = new byte[1024 * 5000];//5mb
                client.Receive(data);
                try
                {
                    string message = (string)Deserialize(data);
                    AddMessageBcolor(message);
                }
                catch
                {
                    Image img = (Image)byteArrayToImage(data);

                    /*   foreach (Socket item in ClientList)
                       {
                           if (item != null && item != client)
                           {
                               item.Send(CopyImageToByteArray(img));
                           }
                       }*/
                    imageList2.Images.Add(img);
                    this.imageList2.ImageSize = new Size(90, 60);
                    this.listView1.SmallImageList = this.imageList2;
                    int len = this.imageList2.Images.Count;
                    this.listView1.Items.Add(" ", len - 1);
                }

            }
        }
        private void MyReceive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];//5mb
                    client.Receive(data);
                    string message = (string)Deserialize(data);
                    AddMessageBcolor(message);
                }
            }
            catch
            {
                Close();
            }
        }
        
        private void MySend(string s)
        {
            if (richTextBox1.Text != string.Empty)
            {
                client.Send(Serialize(richTextBox1.Text));
            }
        }

        string p;
        string url;
        string getpath;
        private void button3_Click_1(object sender, EventArgs e)
        {
         
        }
        private static readonly ImageConverter _imageConverter = new ImageConverter();
        private byte[] CopyImageToByteArray(Image theImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                theImage.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
        }
        void AddMessage(string s)
        {
            DateTime d1 = DateTime.Now;
            string h = d1.Hour.ToString();
            string ss = d1.Minute.ToString();
            string hs = h + ":" + ss;
            listView1.Items.Add(new ListViewItem() { Text =s+"  :"+hs });         
            richTextBox1.Clear();
        }
        void AddMessageBcolor(string s)
        {
            DateTime d1 = DateTime.Now;
            string h = d1.Hour.ToString();
            string ss = d1.Minute.ToString();
            string hs = h + ":" + ss;
            listView1.Items.Add(new ListViewItem() {Text =s+"  :"+hs}).BackColor = Color.Gray;

        }
        void AddMessageSendNext(string s)
        {
            DateTime d1 = DateTime.Now;
            string h = d1.Hour.ToString();
            string ss = d1.Minute.ToString();
            string hs = h + ":" + ss;
            listView1.Items.Add(new ListViewItem() { Text =s + "  :" + hs }).BackColor = Color.Pink;

        }
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
                 
        }
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
         
        }
        private Image byteArrayToImage(byte[] byteArrayIn)
        {

            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;

        }
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
          /*  checkconect++;
            MyConect();
            if (checkconect != 0)
            {
                btConnect.Visible = false;
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {        
                MySend(richTextBox1.Text);
                AddMessage(richTextBox1.Text);
        }
        private void Form_Client_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }
        private void deleteItem_Click(object sender, EventArgs e)
        {
            while (listView1.SelectedIndices.Count != 0)
            {
                listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
            }
        }
        private void icon()
        {

            try
            {
               /* byte[] bytes = { (byte)0xF0, (byte)0x9F, (byte)0x98, (byte)0x81 };
                Image img = (Image)byteArrayToImage(bytes);
                imageList2.Images.Add(img);
                this.imageList2.ImageSize = new Size(90, 60);
                this.listView1.SmallImageList = this.imageList2;
                int len = this.imageList2.Images.Count;
                this.listView1.Items.Add(" ", len - 1);*/
                string[] file;
                file = new string[] { "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon1.png",
                    "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon2.png",
                    "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon3.png",
                    "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon4.png",
                    "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon5.png",
                    "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon6.png"
                    ,"C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon7.png"
                    ,"C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon8.png"
                    ,"C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon9.png",
                    "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon10.png",
                    "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon11.png"
                };
                foreach (String f in file)
                {
                    Image img = Image.FromFile(f);
                    imageList1.Images.Add(img);
                }
                this.listView2.View = View.LargeIcon;
                this.imageList1.ImageSize = new Size(25, 25);

                this.listView2.LargeImageList = this.imageList1;
                for (int i = 0; i < this.imageList1.Images.Count; i++)
                {
                    this.listView2.Items.Add(" ", i);
                }
              /*  listView2.Items.Add("😅");
                listView2.Items.Add("😃");
                listView2.Items.Add("😁");*/

            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.Message);
            }

        }
        private bool checkk = true;
        private void toolStripIcon_Click(object sender, EventArgs e)
        {
            listView3.Visible = false;
            if (checkk)
            {
                listView2.Visible = checkk;
                bt_senemoji.Visible = checkk;
                checkk = false;
            }
            else
            {
                listView2.Visible = checkk;
                bt_senemoji.Visible = checkk;
                checkk = true;
            }
        }
        private int id = 0;
        private byte[] SeriaUnicodde(string s)
        {
            return Encoding.Unicode.GetBytes(s);
        }
        private string DeseriaUnicode(byte[] data)
        {
            return Encoding.Unicode.GetString(data);
        }
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listView2.SelectedIndices.Count <= 0) return;
                if (listView2.FocusedItem == null) return;
                id = listView2.SelectedIndices[0];
                if (id < 0) return;
                Console.WriteLine("--------------"+id);
            }
            catch
            {

            }
        }
        private void Mysenicon()
        {
            switch (id)
            {
                case 0:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon1.png";
                    break;
                case 1:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon2.png";
                    break;
                case 2:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon3.png";
                    break;
                case 3:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon4.png";
                    break;
                case 4:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon5.png";
                    break;
                case 5:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon6.png";
                    break;
                case 6:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon7.png";
                    break;
                case 7:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon8.png";
                    break;
                case 8:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon9.png";
                    break;
                case 9:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon10.png";
                    break;
                case 10:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon11.png";
                    break;
                default:
                    url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon11.png";
                    break;
            }
           /* if (id == 0)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon1.png";
            }
            if (id == 1)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon2.png";
            }
            if (id == 2)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon3.png";
            }
            if (id == 3)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon4.png";
            }
            if (id == 4)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon5.png";
            }
            if (id == 5)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon6.png";
            }
            if (id == 6)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon7.png";
            }
            if (id == 7)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon8.png";
            }
            if (id == 8)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon9.png";
            }
            if (id == 9)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon10.png";
            }
            if (id == 10)
            {
                url = "C:\\Users\\HP\\Documents\\HK_I_N42022_2023\\Lập trinfhh Window\\icon\\icon11.png";
            }*/
            Image image = Image.FromFile(url);
            client.Send(CopyImageToByteArray(image));
            imageList2.Images.Add(image);
            this.imageList2.ImageSize = new Size(90, 60);
            this.listView1.SmallImageList = this.imageList2;
            int len = this.imageList2.Images.Count;
            this.listView1.Items.Add(" ", len - 1);
        }
        private void Getemoji()
        {
            byte[] bytes = { (byte)0xF0, (byte)0x9F, (byte)0x98, (byte)0x81 }; // A byte array contains non-ASCII (or non-readable) characters

            listView2.Items.Add(DeseriaUnicode(bytes));
        }

        private void Form_Client_Load(object sender, EventArgs e)
        {
            icon();
            icon1();


        }
        private void SendNext(string s)
        {
            client.Send(Serialize(chuyentiep));
        }
        private void button4_Click(object sender, EventArgs e)
        {
          
        }
        string chuyentiep;
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
           
            string gettn = listView1.SelectedItems[0].Text;
            int len = gettn.Length;
            int lenhead = gettn.Length - 6;
            Console.WriteLine("----:"+len);
            Console.WriteLine("----:"+lenhead);
            chuyentiep = gettn.Substring(0,lenhead);
            chuyentiep += "--tn_ct";
            Console.WriteLine(chuyentiep);
        }

        private void tooltrip_SendNext_Click(object sender, EventArgs e)
        {
            SendNext(chuyentiep);
            AddMessageSendNext(chuyentiep);
        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            string emoji = listView2.SelectedItems[0].Text;
            richTextBox1.Text+=emoji;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Mysenicon();
            listView2.Visible = false;
            bt_senemoji.Visible = false;
        }
        private void icon1()
        {
            listView3.Items.Add("😁");
            listView3.Items.Add("😂");
            listView3.Items.Add("😍");
            listView3.Items.Add("😏");
            listView3.Items.Add("😓");
            listView3.Items.Add("😋");
            listView3.Items.Add("😘");
            listView3.Items.Add("😜");
            listView3.Items.Add("😜");
            listView3.Items.Add("😢");
            listView3.Items.Add("😤");
        }
        private bool checkicon = true;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            listView2.Visible = false;
            if (checkicon)
            {
                listView3.Visible = checkicon;
                checkicon = false;
            }
            else
            {
                listView3.Visible = checkicon;
                checkicon = true;
            }
        }

        private void listView3_MouseClick(object sender, MouseEventArgs e)
        {
            string icon = listView3.SelectedItems[0].Text;
            richTextBox1.Text += icon;
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileImage = new OpenFileDialog();
            openFileImage.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileImage.Multiselect = false;
            openFileImage.FileName = "";
            DialogResult result = openFileImage.ShowDialog();


            if (result == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(openFileImage.FileName.ToString());
                if (fi.Length > 1024 * 5000)
                {
                    MessageBox.Show("Size ảnh không được lớn hơn 5MB");
                    return;
                }
                Image img = Image.FromFile(openFileImage.FileName);

                string time = DateTime.Now.ToString("HH:mm");
                client.Send(CopyImageToByteArray(img));
                imageList2.Images.Add(img);
                this.imageList2.ImageSize = new Size(90, 60);
                this.listView1.SmallImageList = this.imageList2;
                int len = this.imageList2.Images.Count;
                this.listView1.Items.Add(" ", len - 1);

            }
            else
            {
                return;
            }
        }

    }
}
