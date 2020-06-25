using System;
using System.Collections.Generic;
using System.Text;

namespace Rootedfuture.Models
{
    public class GalleryItem
    {
        public string file_name { get; set; }

        public string treePhotoPath
       {
           get { return "http://rootedfutu.re/images/trees/" + file_name; }
           set { file_name = value; }
       }

        public string created_at { get; set; }
        public int daysAgo { get; set; }

        public string DaysAgo
        {
            get {
                string daysCounter = string.Empty;
                switch (daysAgo)
                {
                    case 0:
                        daysCounter = "Today";
                    break;
                    case 1:
                        daysCounter = "1 Day ago";
                        break;
                    default:
                        daysCounter = daysAgo.ToString() + " Days ago";
                        break;

                }
                return daysCounter;
            
            }
            set { daysAgo = Int32.Parse(value); }
        }
    }
}
