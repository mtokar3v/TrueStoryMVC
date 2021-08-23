using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Models.ViewModels
{
    //enum PostBlockType : byte
    //{ 
    //    HOT,
    //    BEST,
    //    NEW
    //}

    public class PostBlockInfo
    {
        public byte PostBlockType { get; set; } //HOT - 0, BEST - 1, NEW - 2
        public int Number { get; set; }
    }
}
