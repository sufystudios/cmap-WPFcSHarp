using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        private MainWindow root;
        public Window2(MainWindow root)
        {
            this.root = root;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string nameString = name.Text;
            string addressText = address.Text;
            string fbauthtext = fb_oauth.Text;
            string twitterAuthTxt = twitter_oauth.Text;
            string distance = km.Text;

            Console.WriteLine(nameString + addressText + distance + fbauthtext);

            LogHelper(nameString,addressText,distance,fbauthtext,twitterAuthTxt);


            this.Close();



        }

        private void LogHelper(string nameString, string addressText, string distance, string fbauthtext, string twitterAuthTxt)
        {
            string helperstxt="";
            try
            {
                helperstxt = System.IO.File.ReadAllText(@"helpers.txt");
            } catch(System.IO.FileNotFoundException e) {

            }
           
            helperstxt = Regex.Replace(helperstxt, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            string[] lines = {helperstxt+nameString, addressText, distance, fbauthtext, twitterAuthTxt };
            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
           
            System.IO.File.WriteAllLines(@"helpers.txt",  lines);
            root.updateHelpers();

        }

        public static double calculateDistance(PointLatLng p1, PointLatLng p2)
        {
            return 0;
        }
    }
}
