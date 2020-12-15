using Assignment1;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsPresentation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Assignment2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 



    public partial class Window1 : Window
    {
        private Window root;
        string filename;
        private PointLatLng pp;

        public Window1( Window root, object sender)
        {
            InitializeComponent();
            if(sender.GetType()== typeof(GMap.NET.WindowsPresentation.GMapMarker))
            {
                Console.WriteLine("clicked marker");
            }
            this.root=root;
        }

        public void addCoords(GMap.NET.PointLatLng latlng)
        {
            pp = latlng;

        }

        private void addEvent_Click()
        {
            MainWindow f = (MainWindow)root;
            f.Show();

            if ((bool)image.IsChecked)
            {
                int id = AddXML.AddToFile("photo", message.Text, pp, filename);
                (f).addMarkerImage(id, pp.Lat, pp.Lng, filename, message.Text);
            }
            else if((bool)twitter.IsChecked)
            {
                Console.WriteLine("test");
                int id = AddXML.AddToFile("tweet", message.Text, pp, filename);
                (f).addMarker(id, pp.Lat, pp.Lng, message.Text,"tweet");
            }

            else if((bool)facebook.IsChecked)
            {
                Console.WriteLine("test");
                int id = AddXML.AddToFile("facebook-status-update", message.Text, pp, filename);
                (f).addMarker(id, pp.Lat, pp.Lng, message.Text, "facebook-status-update");
            }
            this.Close();
            //f.getPanel().Controls.Add(l);
            // int zIndex = f.getPanel().Controls.GetChildIndex(l);
            //  l.BringToFront();
            // Do something...
            // Then send it back again
            //f.showMap();
            //f.getPanel().Controls.SetChildIndex(l, zIndex +1 );
            // Console.WriteLine("testing");
            // Label l=  new Label();
            // l.Text = "HI";
            //  l.Location = new Point(PositionSystem.Instance().X, PositionSystem.Instance().Y);
            //l.MouseClick+=new System.Windows.Forms.MouseEventHandler(f.panel1_MouseClick);
            //  Random rnd = new Random();
            //  Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            //  l.BackColor = randomColor;

            //f.getPanel().Controls.Add(l);
            // int zIndex = f.getPanel().Controls.GetChildIndex(l);
            //  l.BringToFront();
            // Do something...
            // Then send it back again
            //f.showMap();
            //f.getPanel().Controls.SetChildIndex(l, zIndex +1 );


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(filename);
                addEvent_Click();
                this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void pickFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                filename = openFileDialog.FileName;
            Console.WriteLine(filename);
        }
    }
}
