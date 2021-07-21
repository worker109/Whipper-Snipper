using System;
using System.Collections.Generic;
using System.Linq;

namespace Whipper_Snipper
{
  public class Profile
  {
    public string profilename = "";
    public string guid = Guid.NewGuid().ToString();
    public List<Action> actionitems = new List<Action>();
    private bool expanded;

    public void addActionItem(Action action)
    {
      action.executionorder = this.actionitems.Count + 1;
      this.actionitems.Add(action);
    }

    public override string ToString() => this.profilename;

    public string getDispalyName() => this.profilename;

    public void setDisplayName(string displayname) => this.profilename = displayname;

    public void setExpanded(bool expanded) => this.expanded = expanded;

    public bool getExpanded() => this.expanded;

    public void removeAction(Action action)
    {
      Program.writeLog("Removing action \"" + action.getDisplayName() + "\"");
      this.actionitems.Remove(action);
      this.resetOrder();
    }

    public void resetOrder()
    {
      int num = 0;
      foreach (Action actionitem in this.actionitems)
        actionitem.executionorder = ++num;
    }

    public void run()
    {
      Program.writeLog("Running  profile \"" + this.profilename + "\"");
      foreach (Action actionitem in this.actionitems)
        actionitem.run();
      Program.writeLog("Finished running \"" + this.profilename + "\"");
    }

    public void revert()
    {
      Program.writeLog("Reverting profile \"" + this.profilename + "\"");
      foreach (Action actionitem in this.actionitems)
        actionitem.revert();
      Program.writeLog("Finished reverting \"" + this.profilename + "\"");
    }

    public void moveActionUp(Action action)
    {
      if (action.executionorder == 1)
        return;
      foreach (Action actionitem in this.actionitems)
      {
        if (actionitem.executionorder + 1 == action.executionorder)
        {
          actionitem.executionorder = action.executionorder;
          --action.executionorder;
          this.actionitems.Sort((Comparison<Action>) ((c1, c2) => c1.executionorder.CompareTo(c2.executionorder)));
          break;
        }
      }
    }

    public void moveActionDown(Action action)
    {
      if (action.executionorder == this.actionitems.Count<Action>())
        return;
      foreach (Action actionitem in this.actionitems)
      {
        if (actionitem.executionorder - 1 == action.executionorder)
        {
          actionitem.executionorder = action.executionorder;
          ++action.executionorder;
          this.actionitems.Sort((Comparison<Action>) ((c1, c2) => c1.executionorder.CompareTo(c2.executionorder)));
          break;
        }
      }
    }
  }
}
