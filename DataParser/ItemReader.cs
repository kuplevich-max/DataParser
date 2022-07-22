using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace DataParser
{
    class ItemReader
    {
        private string filename;
        public ItemReader(string filename)
        {
            this.filename = filename;
        }
        private Item ReadItemModel(XmlElement xnode)
        {
            Item item = new Item();            
            foreach (XmlNode childNode in xnode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "title":
                        item.Title = childNode.InnerText;
                        break;
                    case "link":
                        item.Link = childNode.InnerText;
                        break;
                    case "description":
                        item.Description = childNode.InnerText;
                        break;
                    case "pubDate":
                        item.PubDate = childNode.InnerText;
                        break;
                }
            }
            return item;
        }

        public List<Item> ReadItemsModel()
        {
            List<Item> items = new List<Item>();
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(filename);
            }
            catch(FileNotFoundException)
            {
                throw new Exception($"Файл {filename} не найден!");
            }
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                items.Add(ReadItemModel(xnode));
            }
            return items;
        }

        private Item ReadItemRegular(string data)
        {
            Item item = new Item();
            Regex titleReg = new Regex(@"<title>(.+?)<\/title>");
            Match match = titleReg.Match(data);
            if(match.Success)
            {
                item.Title = match.Groups[1].Value;
            }
            Regex linkReg = new Regex(@"<link>(.+?)<\/link>");
            match = linkReg.Match(data);
            if (match.Success)
            {
                item.Link = match.Groups[1].Value;
            }
            Regex descrReg = new Regex(@"<description>(.+?)<\/description>");
            match = descrReg.Match(data);
            if (match.Success)
            {
                item.Description = match.Groups[1].Value;
            }
            Regex dateReg = new Regex(@"<pubDate>(.+?)<\/pubDate>");
            match = dateReg.Match(data);
            if (match.Success)
            {
                item.PubDate = match.Groups[1].Value;
            }
            return item;
        }

        public List<Item> ReadItemsRegular()
        {
            List<Item> items = new List<Item>();            
            string data;
            using (var sr = new StreamReader(filename))
            {
                data = sr.ReadToEnd();
            }
            Regex itemRegex = new Regex(@"<item>([\s\S]*?)<\/item>");
            MatchCollection matches = itemRegex.Matches(data);
            foreach(Match match in matches)
            {
                items.Add(ReadItemRegular(match.Value));
            }
            return items;
        }
    }
}
