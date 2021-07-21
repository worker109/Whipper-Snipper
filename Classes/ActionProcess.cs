namespace Whipper_Snipper
{
  public class ActionProcess : Action
  {
    public ProcessInfo processinfo = new ProcessInfo();
    public string action_enable = "Kill";
    public string action_revert = "Start";

    public override string getDisplayName()
    {
      if (this.friendlyname.Trim() != "")
        return this.friendlyname;
      string str = this.processinfo.exename;
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
      if (!(actionEnable == "Kill"))
      {
        if (!(actionEnable == "Start"))
          return;
        Program.processcontroller.startProcess(this.processinfo);
      }
      else
        Program.processcontroller.killProcess(this.processinfo);
    }

    public override void revert()
    {
      string actionRevert = this.action_revert;
      if (!(actionRevert == "Kill"))
      {
        if (!(actionRevert == "Start"))
          return;
        Program.processcontroller.startProcess(this.processinfo);
      }
      else
        Program.processcontroller.killProcess(this.processinfo);
    }
  }
}
