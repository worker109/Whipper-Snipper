namespace Whipper_Snipper
{
  public class ActionProfile : Action
  {
    public string profileguid = "";

    public override string getDisplayName() => this.getProfile().profilename;

    public Profile getProfile()
    {
      foreach (Profile profile in Program.profilelist.profiles)
      {
        if (profile.guid == this.profileguid)
          return profile;
      }
      return new Profile();
    }

    internal string getDispalyName() => this.getDisplayName();

    public override void run() => this.getProfile().run();

    public override void revert() => this.getProfile().revert();
  }
}
