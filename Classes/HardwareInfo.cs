using System.Collections.Generic;

namespace Whipper_Snipper
{
  public class HardwareInfo
  {
    public string name = "";
    public string deviceid = "";
    public List<string> hardwareids = new List<string>();

    public override string ToString() => this.name;
  }
}
