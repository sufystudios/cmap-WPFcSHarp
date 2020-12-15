using Assignment1;
using GMap.NET;

using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure.MappingViews;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Tweetinvi;

namespace Assignment2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<int, GMapMarker> markers;
        private FacebookService facebookService;
        bool init = true;
        private List<Account> facebookHelpers;
        TwitterClient userClient;
        public class helper
        {
            public string name;
            public string address;
            public string km;
            public string fbauth;
            public string twitterauth;
        }

        public List<helper> helperlist;


        public MainWindow()
        {
            InitializeComponent();
            facebookHelpers = new List<Account>();
            facebookService = new FacebookService(new FacebookClient());
            markers = new Dictionary<int, GMapMarker>();
            userClient = new TwitterClient("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            updateHelpers();
            

        }

        public void postFacebookMessage(PointLatLng latlng,string message)
        {
            if (!init)
            {
                foreach (helper i in helperlist)
                {
                    Console.WriteLine(i.address == null);
                    Console.WriteLine("name " + i.name);
                    Console.WriteLine("address " + i.address);
                    Console.WriteLine("km " + i.km);
                    Console.WriteLine("fbauth " + i.fbauth);
                    Console.WriteLine(helperlist.Count());
                    Console.WriteLine(mapView.GetPositionByKeywords(i.address));
                    try
                    {
                        var route = GMap.NET.MapProviders.OpenStreetMapProvider.Instance
                             .GetRoute(mapView.GetPositionByKeywords(i.address), latlng, false, false, 14);

                        var distance = route.Distance;
                        Console.WriteLine("Distance" + distance);

                        if (distance <= Convert.ToDouble(i.km) && distance != 0)
                        {
                            Console.WriteLine("Near the point");

                            Task.Run(() =>
                            {

                                var post = facebookService.PostOnWallAsync(i.fbauth, message);
                                Task.WaitAll(post);
                                Console.WriteLine("Post Complete");
                            }
                                );
                        }
                        else
                        {
                            Console.WriteLine("not near the point");
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }

          
        }

        private void mapView_Loaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose your provider here
            mapView.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            mapView.MinZoom = 2;
            mapView.MaxZoom = 17;
            mapView.BringIntoView();
            
            // whole world zoom
            mapView.Zoom = 2;
            // lets the map use the mousewheel to zoom
            mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            // lets the user drag the map
            mapView.CanDragMap = true;
            // lets the user drag the map with the left mouse button
            mapView.DragButton = MouseButton.Left;
            mapView.SetPositionByKeywords("Sydney, Australia");
            mapView.Zoom = 10;

            updateHelpers();
            showMap();
            init = false;


        }

        private void rightMouse(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(mapView);

          
           
            PointLatLng pp = mapView.FromLocalToLatLng((int)p.X,(int)p.Y);
            
             
            Console.WriteLine(pp.ToString());

            Window1 window = new Window1(this,sender);
            window.addCoords(pp);
            window.Show();
            
            

        }

        internal void updateHelpers()
        {
            string helpers = "";
            helperlist = new List<helper>();
            try
            {
               helpers = System.IO.File.ReadAllText(@"helpers.txt");
            } catch( Exception e)
            {

            }
              //  Console.WriteLine(helpers);
            string aLine;
            StringReader strReader = new StringReader(helpers);
            bool eof = false;
                while (!eof)
                {
               
                    helper helpertmp = new helper();
                    for (int i = 0; i < 5; i++)
                    {
                        aLine = strReader.ReadLine();
                    Console.WriteLine(aLine);
                    if (aLine == null )
                        {
                            eof = true;
                            break;
                        }
                    if(aLine.Equals("")) {
                        eof = true;
                        break;
                    }
                       
                        switch (i)
                        {
                            case 0:
                                helpertmp.name = aLine;
                            Console.WriteLine(aLine);
                                break;
                            case 1:
                                helpertmp.address = aLine;
                            Console.WriteLine(aLine);
                            break;
                            case 2:
                                helpertmp.km = aLine;
                            Console.WriteLine(aLine);
                            break;
                            case 3:
                                helpertmp.fbauth = aLine;
                            Console.WriteLine(aLine);
                            break;
                            case 4:
                                helpertmp.twitterauth = aLine;
                            Console.WriteLine(aLine);
                            break;

                        }


                    }
                    helperlist.Add(helpertmp);
            
            }
            helperlist.RemoveAt(helperlist.Count() - 1);
            foreach (helper h in helperlist)
            {
                Console.WriteLine(h.name);

            }
          

            
         

        }

        public GMapControl GetGMapControl()
        {
            return mapView;
        }

        public async void showMap()
        {
            try
            {
      

                var events = AddXML.getXMLEvents();


                //markers = new List<GMapMarker>();
                //labels = new List<Label>();


                foreach (var item in events)
                {
                    // Point p = getPositionOnScreen(item.lon, item.lat);
                    addMarker(item.id, item.lat, item.lon, item.message, item.type);
                    /* PointLatLng point = new PointLatLng(item.lat,item.lon);
                     GMapMarker marker = new GMarkerGoogle(point,GMarkerGoogleType.pink);
                     overlay.Markers.Add(marker);
                     marker.ToolTipText =item.id+ item.message;
                     marker.ToolTipMode = MarkerTooltipMode.Always;
                     */


                }
                var tracklogs = AddXML.getTracklogs();
              
                var photos = AddXML.getXMLPhotos();
                foreach (var item in photos)
                {
                    Console.WriteLine("test");
                    // Point p = getPositionOnScreen(item.lon, item.lat);
                    addMarkerImage(item.id, item.lat, item.lon, item.filename, item.type);
                    /* PointLatLng point = new PointLatLng(item.lat,item.lon);
                     GMapMarker marker = new GMarkerGoogle(point,GMarkerGoogleType.pink);
                     overlay.Markers.Add(marker);
                     marker.ToolTipText =item.id+ item.message;
                     marker.ToolTipMode = MarkerTooltipMode.Always;
                     */


                }
                var videos = AddXML.getXMLVideos();
                foreach (var item in videos)
                {
                    Console.WriteLine(item.type);

                    // Point p = getPositionOnScreen(item.lon, item.lat);
                 
                    /* PointLatLng point = new PointLatLng(item.lat,item.lon);
                     GMapMarker marker = new GMarkerGoogle(point,GMarkerGoogleType.pink);
                     overlay.Markers.Add(marker);
                     marker.ToolTipText =item.id+ item.message;
                     marker.ToolTipMode = MarkerTooltipMode.Always;
                     */


                }
             

            }
            catch (Exception e)
            {
                Console.WriteLine("couldn't download" + e);
            }
        }



        public void addMarker(int id, double lat, double lon, string message, string type)
        {
            PointLatLng point = new PointLatLng(lat, lon);
            var marker = new GMapMarker(point);
            if (type == "tweet")
            {
                System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapImage.BeginInit();
                var uri = new Uri("pack://application:,,,/bird.png");
                bitmapImage.UriSource = uri;
                
                bitmapImage.EndInit();
             
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = bitmapImage;
                image.Width = 25;
                image.Height = 25;


                marker = new GMapMarker(point);
                marker.Shape = image;
                Task.Run(async () =>
                {
                    var tweet = await userClient.Tweets.PublishTweetAsync(message);
                    Console.WriteLine("You published the tweet : " + tweet);
                });
            }
            else if (type == "facebook-status-update")
            {

                System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("pack://application:,,,/fb.png");
                bitmapImage.EndInit();
                GMap.NET.WindowsPresentation.GMapMarker marker2 = new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng(35.6960617168288, 51.4005661010742));
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = bitmapImage;
                image.Width = 25;
                image.Height =25;
             
                marker = new GMapMarker(point);
                marker.Shape = image;
                postFacebookMessage(point, message);
            }
            else
            {
                marker = new GMapMarker(point);
                marker.ZIndex = 1;
                marker.Shape = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.5
                };
            }
            mapView.Markers.Add(marker);
            markers[id] = marker;

            marker.Tag = id.ToString() + ":" + type + " :" + message;
            var textBlock = new TextBlock(new Run(message));
            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            textBlock.Arrange(new Rect(textBlock.DesiredSize));
            mapView.Markers.Add(new GMapMarker(new PointLatLng(lat, lon))
            {
                Offset = new Point(-textBlock.ActualWidth / 2 ,-15),
                Shape = textBlock
            }) ;



        }
        public void addMarkerImage(int id, double lat, double lon, string filename, string type)
        {
            PointLatLng point = new PointLatLng(lat, lon);
            try
            {
             
                System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(filename);
                bitmapImage.EndInit();
               System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.Source = bitmapImage;
                GMapMarker marker = new GMapMarker(point);
                image.Width = 50;
                image.Height = 50;
                markers[id] = marker;
                
                marker.Tag = id.ToString() + ":" + type + " :";
                marker.Shape = image;
                mapView.Markers.Add(marker);
            }
            catch (Exception e)
            {

            }

        }

        private void AddHelper_Click(object sender, RoutedEventArgs e)
        {
          
            Window2 helperWindow = new Window2(this);

            helperWindow.Show();
          
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            mapView.SetPositionByKeywords(Address.Text);
            mapView.Zoom=Convert.ToDouble(Zoom.Text);

        }
    }


}
