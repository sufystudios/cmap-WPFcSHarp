
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;

namespace Assignment1
{
    class Event
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public string message { get; set; }
        public int id { get; set; }
        public int[] destinations { get; set; }
        public string type { get; set; }
        public IEnumerable<Connection> conns;
        public Event(int id, double lat, double lon, string message, string type)
        {
            this.id = id;
            this.lat = lat;
            this.lon = lon;
            this.message = message;
            this.type = type;
        }
        public static int getIndex(GMapMarker e)
        {
            int index = e.Tag.ToString().IndexOf(":");
            string sub = e.Tag.ToString().Substring(0, index);
            return Int32.Parse(sub);

        }
    }
    class Tracklog : Event
    {
        public string data;
        public string filename;
        public Tracklog(int id, string data, string filename, string type) : base(id, 0, 0, "", "tracklog")
        {
            this.data = data;
            this.filename = filename;
        }
    }
    class Video : Photo
    {
        public Video(int id, double lat, double lon, string filename, string type) : base(id, lat, lon, filename, type)
        {

        }
    }
    class Photo : Event
    {
        public string filename;
        public Photo(int id, double lat, double lon, string filename, string type) : base(id, lat, lon, "", type)
        {
            this.filename = filename;
        }
    }
    class Connection
    {
        public int from { get; set; }
        public int to { get; set; }
        public Connection(int from, int to)
        {
            this.from = from;
            this.to = to;
        }
    }
}
