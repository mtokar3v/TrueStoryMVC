﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueStoryMVC.Data
{
    interface IPicture
    {
        public int Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public byte[] Data { get; set; }
        public int PostId { get; set; }
    }
}