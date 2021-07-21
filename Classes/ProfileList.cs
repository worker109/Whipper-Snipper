using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Whipper_Snipper
{
  [XmlInclude(typeof (ActionHardware))]
  [XmlInclude(typeof (ActionService))]
  [XmlInclude(typeof (ActionProcess))]
  [XmlInclude(typeof (ActionProfile))]
  [XmlInclude(typeof(ActionExecutable))]
  public class ProfileList
  {
    public List<Profile> profiles = new List<Profile>();

    public void addProfile(Profile profile)
    {
      this.profiles.Add(profile);
      this.sortProfiles();
    }

    public void sortProfiles() => this.profiles = this.profiles.OrderBy<Profile, string>((Func<Profile, string>) (o => o.getDispalyName())).ToList<Profile>();

    public void removeProfile(Profile profile)
    {
      Program.writeLog("Deleting profile \"" + profile.getDispalyName() + "\"");
      this.profiles.Remove(profile);
    }
  }
}
