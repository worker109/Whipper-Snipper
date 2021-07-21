namespace Whipper_Snipper
{
  public abstract class Action
  {
    public string friendlyname = "";
    public int executionorder;

    public virtual string getDisplayName() => "";

    public virtual void run()
    {
    }

    public virtual void revert()
    {
    }
  }
}
