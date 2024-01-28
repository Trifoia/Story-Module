using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trifoia.Module.Story.Models
{
    public class InkChoice
    {
        public string Text { get; set; }
        public List<string> Tags { get; set; } = new();
        public int Index { get; set; }
        public string PathStringOnChoice { get; set; }
    }
}
