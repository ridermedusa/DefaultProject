using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Admin.Models
{
    public class Identification
    {
        public int FirstLevelMenu { get ; set; }
        public string WithinTheChainAddress { get; set; }

        public string Params { get; set; }
        public Identification()
        {
            FirstLevelMenu = 0;
        }

    }
}