namespace Whipper_Snipper
{
  public class ActionHardware : Action
  {
    public HardwareInfo hardwareinfo = new HardwareInfo();
    public string action_enable = "Disable";
    public string action_revert = "Enable";

    public override string getDisplayName()
    {
      if (this.friendlyname.Trim() != "")
        return this.friendlyname;
      string str = this.hardwareinfo.name;
      if (!(this.action_enable == "(no action)") || !(this.action_revert == "(no action)"))
      {
        if (this.action_enable != "(no action)" && this.action_revert != "(no action)")
          str = str + " (" + this.action_enable + "/" + this.action_revert + ")";
        else if (this.action_revert != "(no action)")
          str = str + " (Revert: " + this.action_revert + ")";
        else if (this.action_enable != "(no action)")
          str = str + " (" + this.action_enable + ")";
      }
      return str;
    }

    internal string getDispalyName() => this.getDisplayName();

    public override void run()
    {
      string actionEnable = this.action_enable;
      if (!(actionEnable == "Disable"))
      {
        if (!(actionEnable == "Enable"))
          return;
        Program.devcon.changeHardwareState(this.hardwareinfo, true);
      }
      else
        Program.devcon.changeHardwareState(this.hardwareinfo, false);
    }

    public override void revert()
    {
      string actionRevert = this.action_revert;
      if (!(actionRevert == "Disable"))
      {
        if (!(actionRevert == "Enable"))
          return;
        Program.devcon.changeHardwareState(this.hardwareinfo, true);
      }
      else
        Program.devcon.changeHardwareState(this.hardwareinfo, false);
    }
  }
}
