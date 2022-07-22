using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParser
{
    public class Item
    {
        private string title;
        private string link;
        private string description;
        private string pubDate;

        public string Title { get { return title; } set { title = value; } }
        public string Link { get { return link; } set { link = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string PubDate { get { return pubDate; } set { pubDate = value; } }

        public Item()
        {
            title = "";
            link = "";
            description = "";
            pubDate = "";
        }
        public override string ToString()
        {
            return title + "\n" +
                    link + "\n" +
                    description + "\n" +
                    pubDate + "\n";
        }
    }
}
