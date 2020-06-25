using System;
using System.Collections.Generic;
using System.Text;

namespace Rootedfuture.Models
{
    public class TreeDonationList
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public int donate { get; set; }

        public string FullName
        {
            get { return $"{Fname} {Lname}"; }
        }

        public string Donate
        {
            get { return donate.ToString()+"$" ; }
            set { donate = Int32.Parse(value); }
        }
    }
}
