using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practi_code2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public static Selector ParseSelector(string query)
        {
            Selector root = new Selector();
            Selector current = root;

            foreach (var part in query.Split(' '))
            {
                string[] selectors = new Regex("(?=[#\\.])").Split(part).Where(s => s.Length > 0).ToArray();

                foreach (var sel in selectors)
                {
                    if (sel.StartsWith('#'))
                        current.Id = sel.Substring(1);
                    else if (sel.StartsWith("."))
                        current.Classes.Add(sel.Substring(1));
                    else if (HtmlHelper.Instance.Tags.Contains(sel))
                        current.TagName = sel;
                    else
                        Console.WriteLine("Error!");
                }
                Selector s = new Selector();
                current.Child = s;
                s.Parent = current;
                current = current.Child;
            }
            current.Parent.Child = null;
            return root;
        }

    }
}
