using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Whipper_Snipper
{
  internal class ServiceController
  {
    private List<ServiceInfo> servicelist = new List<ServiceInfo>();

    public ServiceController() => this.loadServices();

    public List<ServiceInfo> getServiceList(Profile profile = null, ServiceInfo serviceinfo = null)
    {
      if (profile == null)
        return this.servicelist;
      List<ServiceInfo> serviceInfoList = new List<ServiceInfo>();
      foreach (ServiceInfo serviceInfo in this.servicelist)
      {
        bool flag = false;
        foreach (Action actionitem in profile.actionitems)
        {
          if (actionitem.GetType() == typeof (ActionService) && ((ActionService) actionitem).serviceinfo.servicename == serviceInfo.servicename)
            flag = true;
        }
        if (serviceinfo != null && serviceinfo.servicename == serviceInfo.servicename)
          flag = false;
        if (!flag)
          serviceInfoList.Add(serviceInfo);
      }
      return serviceInfoList;
    }

    private void loadServices()
    {
      this.servicelist = new List<ServiceInfo>();
      foreach (System.ServiceProcess.ServiceController service in System.ServiceProcess.ServiceController.GetServices())
        this.servicelist.Add(new ServiceInfo()
        {
          servicename = service.ServiceName,
          displayname = service.DisplayName
        });
      this.servicelist.Sort((Comparison<ServiceInfo>) ((c1, c2) => c1.displayname.CompareTo(c2.displayname)));
    }

    public void changeServiceState(ServiceInfo serviceinfo, bool start)
    {
      string str;
      if (start)
      {
        Program.writeLog("Starting service: " + serviceinfo.displayname);
        str = "start \"" + serviceinfo.servicename + "\"";
      }
      else
      {
        Program.writeLog("Stopping service: " + serviceinfo.displayname);
        str = "stop \"" + serviceinfo.servicename + "\"";
      }
      try
      {
        Process process = new Process()
        {
          StartInfo = new ProcessStartInfo()
          {
            FileName = "sc",
            Arguments = str,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
          }
        };
        process.Start();
        bool flag = false;
        while (!process.StandardOutput.EndOfStream)
        {
          string message = process.StandardOutput.ReadLine();
          if (message.Contains("[SC]") && message.Contains("FAILED"))
            flag = true;
          if (((!message.Contains("[SC]") ? 1 : (!message.Contains("FAILED") ? 1 : 0)) & (flag ? 1 : 0)) != 0 && message.Trim() != "")
            throw new Exception(message);
        }
      }
      catch (Exception ex)
      {
        Program.writeLogError("Error changing service state: " + ex.Message);
      }
    }

    public void changeServiceStartupType(ServiceInfo serviceinfo, string starttype)
    {
      string str1 = "";
      if (!(starttype == "Automatic (Delayed Start)"))
      {
        if (!(starttype == "Automatic"))
        {
          if (!(starttype == "Manual"))
          {
            if (starttype == "Disabled")
              str1 = "disabled";
          }
          else
            str1 = "demand";
        }
        else
          str1 = "auto";
      }
      else
        str1 = "delayed-auto";
      Program.writeLog("Changing service startup type: " + serviceinfo.displayname);
      string str2 = "config \"" + serviceinfo.servicename + "\" start= " + str1;
      try
      {
        Process process = new Process()
        {
          StartInfo = new ProcessStartInfo()
          {
            FileName = "sc",
            Arguments = str2,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
          }
        };
        process.Start();
        bool flag = false;
        while (!process.StandardOutput.EndOfStream)
        {
          string message = process.StandardOutput.ReadLine();
          if (message.Contains("[SC]") && message.Contains("FAILED"))
            flag = true;
          if (((!message.Contains("[SC]") ? 1 : (!message.Contains("FAILED") ? 1 : 0)) & (flag ? 1 : 0)) != 0 && message.Trim() != "")
            throw new Exception(message);
        }
      }
      catch (Exception ex)
      {
        Program.writeLogError("Error changing service state: " + ex.Message);
      }
    }
  }
}
