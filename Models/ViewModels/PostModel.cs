using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;


namespace TrueStoryMVC.Models.ViewModels
{
    public class PostModel
    {
        public string Header { get; set; }
        public List<string> Texts { get; set; }
        public List<string> Images { get; set; }
        public string TagsLine { get; set; }
        public List<string> Scheme { get; set; }
        
    }
}
