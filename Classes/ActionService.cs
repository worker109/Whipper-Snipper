namespace Whipper_Snipper
{
  public class ActionService : Action
  {
    public ServiceInfo serviceinfo = new ServiceInfo();
    public string action_enable = "Stop";
    public string action_revert = "Start";
    public string action_startup_enable = "(no action)";
    public string action_startup_revert = "(no action)";

    public override string getDisplayName()
    {
      if (this.friendlyname.Trim() != "")
        return this.friendlyname;
      string str = this.serviceinfo.displayname;
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
      if (this.action_startup_enable != "(no action)")
        Program.servicecontroller.changeServiceStartupType(this.serviceinfo, this.action_startup_enable);
      string actionEnable = this.action_enable;
      if (!(actionEnable == "Start"))
      {
        if (!(actionEnable == "Stop"))
          return;
        Program.servicecontroller.changeServiceState(this.serviceinfo, false);
      }
      else
        Program.servicecontroller.changeServiceState(this.serviceinfo, true);
    }

    public override void revert()
    {
      if (this.action_startup_revert != "(no action)")
        Program.servicecontroller.changeServiceStartupType(this.serviceinfo, this.action_startup_revert);
      string actionRevert = this.action_revert;
      if (!(actionRevert == "Start"))
      {
        if (!(actionRevert == "Stop"))
          return;
        Program.servicecontroller.changeServiceState(this.serviceinfo, false);
      }
      else
        Program.servicecontroller.changeServiceState(this.serviceinfo, true);
    }
  }
}
