using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsadWebsite.App_Code
{
    public class Link
    {
        private const string index = "Default.aspx";
        private string name;
        private string link;
        private string prefix;
        private string ext;

        public Link(string name, string prefix, string ext) : this(name, name, prefix, ext) { }

        public Link(string name, string link, string prefix, string ext)
        {
            Name = name;
            RawLink = link;
            Prefix = prefix;
            Ext = ext;
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string RawLink
        {
            get
            {
                return link;
            }

            set
            {
                link = value;
            }
        }

        public string Prefix
        {
            get
            {
                return prefix;
            }

            set
            {
                prefix = value;
            }
        }

        public string Ext
        {
            get
            {
                return ext;
            }

            set
            {
                ext = value;
            }
        }

        public string FullLink
        {
            get
            {
                return prefix + link + ext;
            }

            set
            {
                ext = value;
            }
        }

        public string Page
        {
            get { return link + ext; }
        }

        public bool Equals(string page)
        {
            return Name.ToLower() == page.ToLower();
        }
    }
}