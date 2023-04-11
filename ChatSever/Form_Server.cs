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
using static System.Net.WebRequestMethods;
using System.Drawing.Imaging;
using System.Security.Policy;
using System.Security.Cryptography;
using System.Resources;

namespace ChatSever
{
    public partial class Form_Server : Form
    {
        public Form_Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            listView1.View = View.List;    
            Init();


        }
        Socket server,clientt;
        IPAddress ia;
        int port = 4321;
        IPEndPoint ipe;
        List<Socket> ClientList;
        private void Init()
        {
            ClientList = new List<Socket>();
            server = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);
            ia = IPAddress.Any;
            ipe = new IPEndPoint(ia, port);
            server.Bind(ipe);
            Thread tlisten = new Thread(new ThreadStart(() => {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();
                        ClientList.Add(client);

                        Thread receive = new Thread(SeenMess);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch
                {
                    server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                    ia = IPAddress.Any;
                    ipe = new IPEndPoint(ia, port);
                }
            }));

            tlisten.IsBackground = true;
            tlisten.Start();
        }
        
        object MyDeserialize(byte[] data)
        {
            try
            {
                MemoryStream stream = new MemoryStream(data);
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
            catch
            {
                MemoryStream ms = new MemoryStream(data);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }

        }
        private Image byteArrayToImage(byte[] byteArrayIn)
        {
          
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
      
        }
        void MyconvertoImg(object obj1)
        {
            Socket client = obj1 as Socket;
            while (true)
            {
                byte[] data2 = new byte[1024 * 5000];//5mb
                client.Receive(data2);

                Image img =(Image) byteArrayToImage(data2);
                Console.WriteLine("hello hellooooo" +img);
                imageList2.Images.Add(img);
                this.imageList2.ImageSize = new Size(30, 30);
                this.listView1.LargeImageList = this.imageList2;
                int len = this.imageList2.Images.Count;
                this.listView1.Items.Add(" ", len - 1);

            }

        }
     
        void SeenMess(object obj1)
        {
            Socket client = obj1 as Socket;
            bool n;

                while (true)
                {
                    byte[] data = new byte[1024 * 5000];//5mb
                    client.Receive(data);
                try
                {
                    string message = (string)MyDeserialize(data);
                    message = message.Substring(0, message.Length);
                    foreach (Socket item in ClientList)
                    {
                        if (item != null && item != client)
                        {
                            item.Send(Serialize(message));
                        }
                    }
                    AddMessageBcolor(message);
                }
                catch
                {
                    Image img = (Image)byteArrayToImage(data);

                    foreach (Socket item in ClientList)
                    {
                        if (item != null && item != client)
                        {
                            item.Send(CopyImageToByteArray(img));
                        }
                    }
                    imageList2.Images.Add(img);
                    this.imageList2.ImageSize = new Size(90, 60);
                    this.listView1.SmallImageList = this.imageList2;
                    int len = this.imageList2.Images.Count;
                    this.listView1.Items.Add(" ", len - 1);
                }
                
            }
        }
        void MyReceive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                    while (true)
                    {
                    byte[] data = new byte[1024 * 5000];//5mb
                    client.Receive(data);
                    string message = (string)MyDeserialize(data);
                        message = message.Substring(0, message.Length);
                        foreach (Socket item in ClientList)
                        {
                            if (item != null && item != client)
                            {
                                item.Send(Serialize(message));
                            }
                        }
                        AddMessageBcolor(message);                  
                    }
            }
            catch
            {
                ClientList.Remove(client);
                client.Close();
            }
        }
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
            listView1.Items.Add(new ListViewItem() {Text=s+"  :" + hs});
            
        }
        void AddMessageBcolor(string s)
        {
            DateTime d1 = DateTime.Now;
            string h = d1.Hour.ToString();
            string ss = d1.Minute.ToString();
            string hs = h + ":" + ss;
            listView1.Items.Add(new ListViewItem() { Text =s+"  :"+hs}).BackColor = Color.Gray;

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //Start
        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            
        }
        //button receive
        private void button1_Click(object sender, EventArgs e)
        {
          /*  checkStart++;
            MyReceive();
            if (checkStart != 0)
            {
                btStart.Visible = false;
            }*/
        }

        private void Form_Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();    
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Close()
        {
            server.Close();
        }

        private void btsend_Click(object sender, EventArgs e)
        {          
           foreach (Socket item in ClientList)
                {
                MySend(item);
              }
            AddMessage(richTextBox1.Text);
            richTextBox1.Clear();
        }
        int check = 0;
        private void Mysenpic(Socket client)
        {        
            url = getpath;
            Image image = Image.FromFile(url);
            client.Send(CopyImageToByteArray(image));
            if (check == 0)
            {
                imageList2.Images.Add(image);
                this.imageList2.ImageSize = new Size(90, 60);
                this.listView1.SmallImageList = this.imageList2;
                int len = this.imageList2.Images.Count;
                this.listView1.Items.Add(" ", len - 1);
                check = 1;
            }
        }
        private void Mysenicon(Socket client)
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
            Image image = Image.FromFile(url);
            client.Send(CopyImageToByteArray(image));
            if (checksen == 0)
            {
                imageList2.Images.Add(image);
                this.imageList2.ImageSize = new Size(90, 60);
                this.listView1.SmallImageList = this.imageList2;
                int len = this.imageList2.Images.Count;
                this.listView1.Items.Add(" ", len - 1);
                checksen = 1;
            }
            /* url = getpath;
             Image image = Image.FromFile(url);
             client.Send(CopyImageToByteArray(image));
             if (check == 0)
             {
                 imageList2.Images.Add(image);
                 this.imageList2.ImageSize = new Size(90, 60);
                 this.listView1.SmallImageList = this.imageList2;
                 int len = this.imageList2.Images.Count;
                 this.listView1.Items.Add(" ", len - 1);
                 check = 1;
             }*/
        }
        private void MySend(Socket client)
        {
            if (client!=null&& richTextBox1.Text != string.Empty)
            {
                client.Send(Serialize(richTextBox1.Text));
            }         

            /*
            byte[] data = new byte[1024 * 5000];//5mb
            data = Myserialize(s);
            server.Send(data);*/
        }
        private byte[] Myserialize(string s)// mã hóa 
        {
            return Encoding.Unicode.GetBytes(s);
        }
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();

        }

        private void deleteItem_Click(object sender, EventArgs e)
        {
            while (listView1.SelectedIndices.Count != 0)
            {
               listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
            }
        }

        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);

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
        private void icon()
        {

            try
            {

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
                this.imageList1.ImageSize = new Size(25,25);

                this.listView2.LargeImageList = this.imageList1;
                for (int i = 0; i < this.imageList1.Images.Count; i++)
                {
                    this.listView2.Items.Add(" ", i);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("error : " + ex.Message);
            }

        }
        private bool checkk = true;
        private bool checkicon = true;
        private int id = 0;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            listView3.Visible = false;
            if (checkk)
            {
                listView2.Visible = checkk;
                bt_SendEmoji.Visible = checkk;
                checkk = false;
            }
            else
            {
                listView2.Visible = checkk;
                bt_SendEmoji.Visible = checkk;
                checkk = true;
            }
        }
        string path;
        private void Form_Server_Load(object sender, EventArgs e)
        {
            icon1();
            icon();
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (listView2.SelectedIndices.Count <= 0) return;
                if (listView2.FocusedItem == null) return;
                id = listView2.SelectedIndices[0];
                if (id < 0) return;
                Console.WriteLine(id);
                /* if (listView2.SelectedIndices.Count <= 0) return;
                 if (listView2.FocusedItem == null) return;
                 id = listView2.SelectedIndices[0];
                 if (id < 0) return;
                 // nếu mà id = 0 tức là ảnh lỗi hoặc chưa có ảnh nên cho qua
                 Clipboard.SetImage(imageList1.Images[id]); // dán icon vào 
                 richTextBox1.Paste(); // dán vào n*/
            }
            catch
            {

            }
        }
        string mess = "stop";
        string p;
        string url;
        string getpath;

        private void button1_Click_2(object sender, EventArgs e)
        {
            foreach (Socket item in ClientList)
            {
                Mysenpic(item);
            }
            check = 0;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
          
        }
        void AddMessageSendNext(string s)
        {
            DateTime d1 = DateTime.Now;
            string h = d1.Hour.ToString();
            string ss = d1.Minute.ToString();
            string hs = h + ":" + ss;
            listView1.Items.Add(new ListViewItem() { Text = s + "  :" + hs }).BackColor = Color.Pink;

        }
        private void SendNext(Socket client)
        {
            client.Send(Serialize(chuyentiep));
        }
        private void Form_Server_MouseClick(object sender, MouseEventArgs e)
        {

           
        }
        string chuyentiep;
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            string gettn = listView1.SelectedItems[0].Text;
            int len = gettn.Length;
            int lenhead = gettn.Length - 6;
            Console.WriteLine("----:" + len);
            Console.WriteLine("----:" + lenhead);
            chuyentiep = gettn.Substring(0, lenhead);
            chuyentiep+="--tn_ct";//message has been forwarded
            Console.WriteLine(chuyentiep);
        }

        private void toolStrip_Sendnext_Click(object sender, EventArgs e)
        {
            foreach (Socket item in ClientList)
            {
                SendNext(item);
            }
            AddMessageSendNext(chuyentiep);
        }
        int checksen=0;
        private void button4_Click(object sender, EventArgs e)
        {
            foreach (Socket item in ClientList)
            {
                Mysenicon(item);
            }
            listView2.Visible= false;
            bt_SendEmoji.Visible= false;
            checksen = 0;
    
        }

        private void listView3_MouseClick(object sender, MouseEventArgs e)
        {
            string icon=listView3.SelectedItems[0].Text;
            richTextBox1.Text += icon;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {listView2.Visible= false;
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

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            /*OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();

            string directoryPath = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
            openFileDialog1.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                p = openFileDialog1.FileName;

            }

            string resp;
            resp = p.Substring(0, 2);
            var final = p.Substring(2);
            var info = final.Split('\\');
            for (int i = 0; i < info.Length; i++)
            {
                resp += info[i] + "\\";
            }

            for (int i = 0; i < resp.Length - 1; i++)
            {
                getpath += resp[i];
            }
            if (url != string.Empty)
            {
                btSend_picture.Visible= true;
            }
            else
            {
                btSend_picture.Visible = false;
            }*/

            OpenFileDialog openFileImage = new OpenFileDialog();
            openFileImage.Filter = "Images |*.bmp;*.jpg;*.png;*.gif;*.ico";
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
                //Server gởi ảnh đến nhiều client cùng lúc
                foreach (Socket item in ClientList)
                {
                    item.Send(CopyImageToByteArray(img)); //nén chứ ko phải giải nén!!!

                }

                //Image image = Image.FromFile(url);
                // client.Send(nenpic(img));
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
        private void button1_Click_1(object sender, EventArgs e)
        {
         /*   Thread conver = new Thread(MyconvertoImg);
            conver.IsBackground = true;
            conver.Start();*/
        }



    }
}
