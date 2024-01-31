using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public static Selector ParseSelector(string query, char value)
    {
      Selector root = new Selector();
      Selector current = root;
      foreach (var part in query.Split(new[] { ' ' }))
      {
        foreach (var part1 in part.Split(new[] { '#', '.' }))
        {
          if (part.StartsWith('#'))
          {
            current.Id = part.Substring(1);
          }
          else if (part.StartsWith("."))
          {
            current.Classes.Add(part.Substring(1));
          }
          else if (HtmlHelper.Instance.Tags.Contains(part))
          {
            current.TagName = part;
          }
          else
          {
            Console.WriteLine("Error!");
          }
        }
        Selector s = new Selector();
        current.Child = s;
        s.Parent = current;
        current = current.Child;
      }
      current.Child = new Selector();
      return root;
    }
    
  }
}
