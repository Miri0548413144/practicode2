
using practi_code2;
using System.Text.RegularExpressions;

var html = await Load("https://learn.malkabruk.co.il/practicode/projects/pract-2/#_5");
var cleanHtml = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
//new Regex("\\s").Replace(html, "");
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToArray();
//var htmlElement = "<div id=\"my-id\" class=\"my-class-1 my-class-2\" width=\"100%\">text</div>";
//var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);

HtmlElement rootElement = NewChild(htmlLines[1].Split(' ')[0], null, htmlLines[1]);
BuildTree(rootElement, htmlLines);
Console.WriteLine("htmlTree: ");
PrintTree(rootElement,0);
var list = rootElement.FindElements(Selector.ParseSelector("footer.md-footer nav"));


static void BuildTree(HtmlElement root,string[] htmlLines)
{
  HtmlElement current = root;
  foreach(var line in htmlLines)
  {
    string start = line.Split(" ")[0];
    if (start == "/html")
      break;
    if (start.StartsWith( "/"))
    {
      current=current.Parent;
      continue;
    }
    if (!HtmlHelper.Instance.Tags.Contains(start)){ 
      current.InnerHtml = start;
      continue;
    }
    HtmlElement newH = NewChild(start, current, line);
    current.Children.Add(newH);
    if(!HtmlHelper.Instance.VoidTags.Contains(start)||line.EndsWith('/'))
    {
      current = newH;
    }
  }
}
static HtmlElement NewChild(string tagNames,HtmlElement root, string line)
{HtmlElement child=new HtmlElement() {Name = tagNames,Parent=root};
  var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
  foreach (var x in attributes)
  {
    string nameAttribute = x.ToString().Split("=")[0];
    string ValueAttribute = x.ToString().Split("=")[1].Replace("\"","");
    if (nameAttribute == "id")
      child.Id = ValueAttribute;
    else if (nameAttribute == "class")
    {
      string[] classArray = ValueAttribute.Split(' ');
      List<string> wordsList = new List<string>(classArray);
      child.Classes = wordsList;
    }
    else child.Attributes.Add( nameAttribute,ValueAttribute);
  }

  return child;
}

static void PrintTree(HtmlElement node, int depth)
{
    if (node != null)
    {
        // Print current node
        Console.WriteLine($"{new string(' ', depth * 2)}<{node.Name}>" + node);

        // Print children
        foreach (var child in node.Children)
        {
            PrintTree(child, depth + 1);
        }

        // Print closing tag
        Console.WriteLine($"{new string(' ', depth * 2)}</{node.Name}>");
    }
}



Console.ReadLine();

async Task<string> Load(string url)
{
  HttpClient client = new HttpClient();
  var response = await client.GetAsync(url);
  var html = await response.Content.ReadAsStringAsync();
  return html;
}

