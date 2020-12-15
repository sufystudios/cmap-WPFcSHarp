using GMap.NET;

using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Assignment1
{
    class AddXML
    {
        public static void MakeNewXML()
        {


            XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
            // SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace soapenvenc = "http://www.w3.org/2001/12/soap-encoding";//  SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace lle = "http://www.xyz.org/lifelogevents";
            XDocument xDoc = new XDocument(
                        new XDeclaration("1.0", "UTF-16", null),
                           new XElement(soapenv + "Envelope",
                            //  new XAttribute(XNamespace.Xmlns + "SOAP-ENV", "http://www.w3.org/2001/12/soap-envelope\" SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding"),
                            new XAttribute(XNamespace.Xmlns + "SOAP-ENV", "http://www.w3.org/2001/12/soap-envelope"),
                             new XAttribute(soapenv + "encodingStyle", "http://www.w3.org/2001/12/soap-encoding"),



                           new XElement(soapenv + "Body", new XAttribute(XNamespace.Xmlns + "lle", "http://www.xyz.org/lifelogevents")



                                )));

            xDoc.Save("events.xml");
        }
        public static int AddToFile(string eventtype, string message, PointLatLng ll, string filepath)
        {
            try
            {
                if (!File.Exists("events.xml"))
                {
                    MakeNewXML();
                }
                // new XAttribute(XNamespace.Xmlns + "SOAP-ENV", "http://www.w3.org/2001/12/soap-envelope\" SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding"
                XNamespace lle = "http://www.xyz.org/lifelogevents";
                XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
                XDocument file = XDocument.Load(@"events.xml");

                // Console.WriteLine(file.ToString());

                int id = 0;
                // int id = (int?) Int32.Parse(file.Elements(soapenv + "Envelope").Elements(soapenv + "Body").Elements(lle + "Event").LastOrDefault().Element(lle + "eventid").Value) ?? 0;
                //Console.WriteLine(file.Elements(soapenv + "Envelope").Elements(soapenv + "Body").Elements(lle + "Event").LastOrDefault().Element(lle+"eventid").Value);
                if (file.Elements(soapenv + "Envelope").Elements(soapenv + "Body").Elements(lle + "Event").LastOrDefault() != null)
                    id = Int32.Parse(file.Elements(soapenv + "Envelope").Elements(soapenv + "Body").Elements(lle + "Event").LastOrDefault().Element(lle + "eventid").Value) + 1;
                else
                    id = 0;
                XElement ev;
                if (eventtype == "tweet")
                {
                    ev = new XElement(lle + "Event",

                 new XElement(lle + "eventid", id), new XElement(lle + eventtype.ToLower(), new XElement(lle + "text", message), new XElement(lle + "location", new XElement(lle + "lat", ll.Lat.ToString()), new XElement(lle + "lon", ll.Lng.ToString())), new XElement(lle + "datetimestamp", new DateTime().ToString())), new XElement(lle + "connections"));
                    file.Element(soapenv + "Envelope").Element(soapenv + "Body").Add(ev);
                }
                else if (eventtype == "facebook-status-update")
                {
                    ev = new XElement(lle + "Event",

                 new XElement(lle + "eventid", id), new XElement(lle + eventtype, new XElement(lle + "text", message), new XElement(lle + "location", new XElement(lle + "lat", ll.Lat.ToString()), new XElement(lle + "lon", ll.Lng.ToString())), new XElement(lle + "datetimestamp", new DateTime().ToString())), new XElement(lle + "connections"));
                    file.Element(soapenv + "Envelope").Element(soapenv + "Body").Add(ev);
                }
                else if (eventtype == "photo")
                {
                    ev = new XElement(lle + "Event",

                 new XElement(lle + "eventid", id), new XElement(lle + eventtype, new XElement(lle + "filepath", filepath), new XElement(lle + "location", new XElement(lle + "lat", ll.Lat.ToString()), new XElement(lle + "lon", ll.Lng.ToString()))), new XElement(lle + "connections"));
                    file.Element(soapenv + "Envelope").Element(soapenv + "Body").Add(ev);
                }
                else if (eventtype == "video")
                {
                    ev = new XElement(lle + "Event",

                 new XElement(lle + "eventid", id), new XElement(lle + eventtype, new XElement(lle + "filepath", filepath), new XElement(lle + "location", new XElement(lle + "lat", ll.Lat.ToString()), new XElement(lle + "lon", ll.Lng.ToString())), new XElement(lle + "start-time", new XElement(lle + "datetimestamp", new DateTime().ToString())), new XElement(lle + "end-time", new XElement(lle + "datetimestamp", new DateTime().ToString()))), new XElement(lle + "connections"));
                    file.Element(soapenv + "Envelope").Element(soapenv + "Body").Add(ev);
                }
                else if (eventtype == "tracklog")
                {
                    ev = new XElement(lle + "Event",
                    new XElement(lle + "eventid", id), new XElement(lle + eventtype, new XElement(lle + "filepath", filepath), new XElement(lle + "data", LoadGPXTracks(filepath) + LoadGPXWaypoints(filepath)), new XElement(lle + "start-time", new XElement(lle + "datetimestamp", new DateTime().ToString())), new XElement(lle + "end-time", new XElement(lle + "datetimestamp", new DateTime().ToString()))), new XElement(lle + "connections"));
                    file.Element(soapenv + "Envelope").Element(soapenv + "Body").Add(ev);
                }

                // Console.WriteLine("null");
                //file.Element(soapenv +"Body").Add(ev);
                file.Save("events.xml");
                // Console.WriteLine(file);
                //Console.WriteLine(sw);
                return id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        private static XNamespace GetGpxNameSpace()
        {
            XNamespace gpx = XNamespace.Get("http://www.topografix.com/GPX/1/1");
            return gpx;
        }
        public static List<double[]> getWaypoints(string data)
        {
            Console.WriteLine(data);
            List<double[]> waypoints = new List<double[]>();
            string[] split = data.Split('\n');

            for (int i = 0; i < split.Length - 1; i++)
            {
                Console.WriteLine(split[i].Split(' ')[0].ToString());
                double[] d = new double[2];
                Console.WriteLine(split[i]);
                d[0] = double.Parse(split[i].Split(' ')[1].Split(':')[1]);
                d[1] = double.Parse(split[i].Split(' ')[2].Split(':')[1]);
                waypoints.Add(d);
            }
            return waypoints;

        }
        public static IEnumerable<Tracklog> getTracklogs()
        {

            XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
            // SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace soapenvenc = "http://www.w3.org/2001/12/soap-encoding";//  SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace lle = "http://www.xyz.org/lifelogevents";

            XDocument document = XDocument.Load("events.xml");
            var events = from r in document.Descendants(lle + "Event")
                         from x in r.Elements().First().ElementsAfterSelf()
                         where x.Name.LocalName == "tracklog"
                         select new Tracklog(
                              Int32.Parse(r.Element(lle + "eventid").Value),
                             x.Element(lle + "data").Value,

                             x.Element(lle + "filepath").Value,
                            x.Name.LocalName.ToString()
                             );


            return events;
            ;
        }
        public static string LoadGPXTracks(string sFile)
        {
            XDocument gpxDoc = GetGpxDoc(sFile);
            XNamespace gpx = GetGpxNameSpace();
            var tracks = from track in gpxDoc.Descendants(gpx + "trk")
                         select new
                         {
                             Name = track.Element(gpx + "name") != null ?
                            track.Element(gpx + "name").Value : null,
                             Segs = (
                                from trackpoint in track.Descendants(gpx + "trkpt")
                                select new
                                {
                                    Latitude = trackpoint.Attribute("lat").Value,
                                    Longitude = trackpoint.Attribute("lon").Value,
                                    Elevation = trackpoint.Element(gpx + "ele") != null ?
                                    trackpoint.Element(gpx + "ele").Value : null,
                                    Time = trackpoint.Element(gpx + "time") != null ?
                                    trackpoint.Element(gpx + "time").Value : null
                                }
                              )
                         };

            StringBuilder sb = new StringBuilder();
            foreach (var trk in tracks)
            {
                // Populate track data objects. 
                foreach (var trkSeg in trk.Segs)
                {
                    // Populate detailed track segments 
                    // in the object model here. 
                    sb.Append(
                      string.Format("Date:{4} Latitude:{1} Longitude:{2} " +
                                   "Elevation:{3} Date:{4}\n",
                      trk.Name, trkSeg.Latitude,
                      trkSeg.Longitude, trkSeg.Elevation,
                      trkSeg.Time));
                }
            }
            return sb.ToString();
        }


        public static string LoadGPXWaypoints(string sFile)
        {
            try
            {
                XDocument gpxDoc = GetGpxDoc(sFile);
                XNamespace gpx = GetGpxNameSpace();

                var waypoints = from waypoint in gpxDoc.Descendants(gpx + "wpt")
                                select new
                                {
                                    Latitude = waypoint.Attribute("lat").Value,
                                    Longitude = waypoint.Attribute("lon").Value,
                                    Elevation = waypoint.Element(gpx + "ele") != null ?
                                      waypoint.Element(gpx + "ele").Value : null,
                                    Name = waypoint.Element(gpx + "name") != null ?
                                      waypoint.Element(gpx + "name").Value : null,
                                    Dt = waypoint.Element(gpx + "cmt") != null ?
                                      waypoint.Element(gpx + "cmt").Value : null
                                };

                StringBuilder sb = new StringBuilder();
                foreach (var wpt in waypoints)
                {

                    sb.Append(
                      string.Format("Date:{4} Latitude:{1} Longitude:{2} Elevation:{3} Date:{4}\n",
                      wpt.Name, wpt.Latitude, wpt.Longitude,
                      wpt.Elevation, wpt.Dt));
                }

                //sb.Append(LoadGPXTracks(sFile));
                Console.WriteLine(sb);
                return sb.ToString();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private static XDocument GetGpxDoc(string sFile)
        {
            try
            {
                XDocument gpxDoc;

                gpxDoc = XDocument.Load(sFile);
                return gpxDoc;
            }
            catch (Exception e)
            {
                throw;
            }

        }
        public static IEnumerable<Event> getXMLEvents()
        {

            XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
            // SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace soapenvenc = "http://www.w3.org/2001/12/soap-encoding";//  SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace lle = "http://www.xyz.org/lifelogevents";

            XDocument document = XDocument.Load("events.xml");
            var events = from r in document.Descendants(lle + "Event")
                         from x in r.Elements().First().ElementsAfterSelf()
                         where x.Name.LocalName != "photo" && x.Name.LocalName != "video" && isTextEvent(x.Name.LocalName)

                         select new Event(
                              Int32.Parse(r.Element(lle + "eventid").Value),
                             double.Parse(x.Element(lle + "location").Element(lle + "lat").Value),
                             double.Parse(x.Element(lle + "location").Element(lle + "lon").Value),
                             x.Element(lle + "text").Value,
                            x.Name.LocalName.ToString()
                             );


            return events;
            ;
        }
        public static IEnumerable<Video> getXMLVideos()
        {

            XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
            // SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace soapenvenc = "http://www.w3.org/2001/12/soap-encoding";//  SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace lle = "http://www.xyz.org/lifelogevents";

            XDocument document = XDocument.Load("events.xml");
            var events = from r in document.Descendants(lle + "Event")
                         from x in r.Elements().First().ElementsAfterSelf()
                         where x.Name.LocalName == "video"
                         select new Video(
                              Int32.Parse(r.Element(lle + "eventid").Value),
                             double.Parse(x.Element(lle + "location").Element(lle + "lat").Value),
                             double.Parse(x.Element(lle + "location").Element(lle + "lon").Value),
                             x.Element(lle + "filepath").Value,
                            x.Name.LocalName.ToString()
                             );


            return events;
            ;
        }
        public static IEnumerable<Photo> getXMLPhotos()
        {

            XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
            // SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace soapenvenc = "http://www.w3.org/2001/12/soap-encoding";//  SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace lle = "http://www.xyz.org/lifelogevents";

            XDocument document = XDocument.Load("events.xml");
            var events = from r in document.Descendants(lle + "Event")
                         from x in r.Elements().First().ElementsAfterSelf()
                         where x.Name.LocalName == "photo"
                         select new Photo(
                              Int32.Parse(r.Element(lle + "eventid").Value),
                             double.Parse(x.Element(lle + "location").Element(lle + "lat").Value),
                             double.Parse(x.Element(lle + "location").Element(lle + "lon").Value),
                             x.Element(lle + "filepath").Value,
                            x.Name.LocalName.ToString()
                             );


            return events;
            ;
        }

        public static IEnumerable<Connection> GetConnections(int id)
        {

            XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
            // SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace soapenvenc = "http://www.w3.org/2001/12/soap-encoding";//  SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace lle = "http://www.xyz.org/lifelogevents";

            XDocument document = XDocument.Load("events.xml");
            var connections = from r in document.Descendants(lle + "Event")
                              from x in r.Descendants(lle + "connection")
                              where x != null && r.Element(lle + "eventid").Value.Equals(id.ToString())
                              select new Connection(
                           Int32.Parse(x.Element(lle + "from").Value),
                           Int32.Parse(x.Element(lle + "to").Value)
                         );


            return connections;
            ;
        }
        public static void removeEvent(int id)
        {
            XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
            // SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace soapenvenc = "http://www.w3.org/2001/12/soap-encoding";//  SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace lle = "http://www.xyz.org/lifelogevents";
            XDocument file = XDocument.Load("events.xml");

            //XDocument xmlDoc = XDocument.Load("d:/data.xml");
            var elementsToDelete = from ele in file.Elements(soapenv + "Envelope").Elements(soapenv + "Body").Elements(lle + "Event")
                                   where ele != null && ele.Element(lle + "eventid").Value.Equals(id.ToString())
                                   select ele;
            Console.WriteLine(id);
            foreach (var e in elementsToDelete)
            {
                e.Remove();

            }
            file.Save("events.xml");
        }
        public static void removeEvent(double lat, double lon)
        {
            XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
            // SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace soapenvenc = "http://www.w3.org/2001/12/soap-encoding";//  SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace lle = "http://www.xyz.org/lifelogevents";
            XDocument file = XDocument.Load("events.xml");

            //XDocument xmlDoc = XDocument.Load("d:/data.xml");
            var elementsToDelete = from ele in file.Elements(soapenv + "Envelope").Elements(soapenv + "Body").Elements(lle + "Event")
                                   where ele != null && ele.Element(lle + "lat").Value.Equals(lat.ToString()) && ele.Element(lle + "lon").Value.Equals(lon.ToString())
                                   select ele;

            foreach (var e in elementsToDelete)
            {
                e.Remove();

            }
            file.Save("events.xml");

        }
        public static void addConnection(int start, int to)
        {
            XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
            // SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace soapenvenc = "http://www.w3.org/2001/12/soap-encoding";//  SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace lle = "http://www.xyz.org/lifelogevents";
            XDocument file = XDocument.Load("events.xml");
            XElement conn = new XElement(lle + "connection",

              new XElement(lle + "from", start), new XElement(lle + "to", to));

            var elementsToUpdate = from ele in file.Elements(soapenv + "Envelope").Elements(soapenv + "Body").Elements(lle + "Event")
                                   where ele != null && ele.Element(lle + "eventid").Value.Equals(start.ToString())
                                   select ele;
            int id = 0;

            foreach (var e in elementsToUpdate)
            {
                e.Element(lle + "connections").Add(conn);
                //id = Int32.Parse(e.Element(lle + "eventid").Value);
            }
            // file.Element(soapenv + "Envelope").Element(soapenv + "Body").Add(ev);
            file.Save("events.xml");
        }
        public static void updateEvent(GMapMarker mark, string text)
        {

            XNamespace soapenv = "http://www.w3.org/2001/12/soap-envelope";
            // SOAPENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace soapenvenc = "http://www.w3.org/2001/12/soap-encoding";//  SOAP-ENV:encodingStyle=\"http://www.w3.org/2001/12/soap-encoding";
            XNamespace lle = "http://www.xyz.org/lifelogevents";
            XDocument file = XDocument.Load("events.xml");

            //XDocument xmlDoc = XDocument.Load("d:/data.xml");
            try
            {
                var elementsToUpdate = from ele in file.Elements(soapenv + "Envelope").Elements(soapenv + "Body").Elements(lle + "Event")
                                       from x in ele.Elements().First().ElementsAfterSelf()
                                       where ele != null && ele.Element(lle + "eventid").Value.Equals(Event.getIndex(mark).ToString()) && isTextEvent(x.Name.LocalName)
                                       select ele;
                int id = 0;

                foreach (var e in elementsToUpdate)
                {

                    e.Elements().First().ElementsAfterSelf().Elements(lle + "text").First().Value = text;
                    id = Int32.Parse(e.Element(lle + "eventid").Value);
                }
                if (isTextEvent(elementsToUpdate.Elements().First().ElementsAfterSelf().First().Name.LocalName))
                {
                    file.Save("events.xml");

                    mark.Tag = id + ":" + elementsToUpdate.Elements().First().ElementsAfterSelf().First().Name.LocalName + ": " + text;

                }
            }
            catch (Exception e)
            {

            }
        }
        public static void addToTracklog(double lat, double lon, int id)
        {

        }
        public static bool isTextEvent(string name)
        {
            if (name == "facebook-status-update")
                return true;
            if (name == "tweet")
                return true;
            return false;
        }
    }
}
