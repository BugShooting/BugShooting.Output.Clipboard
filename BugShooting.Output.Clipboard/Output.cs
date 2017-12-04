using BS.Plugin.V3.Output;

namespace BugShooting.Output.Clipboard
{

  public class Output: IOutput 
  {
 
    public string Name
    {
      get { return "Copy to Clipboard"; }
    }

    public string Information
    {
      get { return string.Empty; }
    }

  }
}
