using System;
using System.Collections.Generic;
using System.Text;

namespace Rootedfuture.Models
{
    public class TreeData
    {
        public int id { get; set; }
        public string productName { get; set; }

        public bool error { get; set; }

        public bool treeNotSold { get; set; }

        public string treePhoto { get; set; }

        public string projectName { get; set; }

        public string treePhotoPath
        {
            get { return "http://rootedfutu.re/images/trees/" + treePhoto; }
            set { treePhoto = value; }
        }

        public string treeTypeName { get; set; }
        public string treeTypeDesc { get; set; }
        public IList<TreeDonationList> treeDonationList { get; set; }
    }
}
