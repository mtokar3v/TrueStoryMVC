using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Data
{
    interface IDataInfo
    {
        public string Author { get; set; }
        public int Rating { get; set; }
        public DateTime PostTime { get; set; }
    }
}
