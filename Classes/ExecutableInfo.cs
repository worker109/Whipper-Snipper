using System.IO;

namespace Whipper_Snipper
{
  public class ExecutableInfo
  {
    
    public string exepath = "";

    public override string ToString() => Path.GetFileName(this.exepath);
  }
}
