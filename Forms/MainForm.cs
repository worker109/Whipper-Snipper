using IWshRuntimeLibrary;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Whipper_Snipper.Properties;

namespace Whipper_Snipper
{
  public class MainForm : Form
  {
    public static ImageList icons;
    private TreeNode contextselectednode;
    private TreeNode toolstripselectednode;
    private IContainer components;
    private SplitContainer splitContainer1;
    private TreeView treeProfiles;
    private RichTextBox txtLog;
    private ContextMenuStrip context_profile;
    private ContextMenuStrip context_action;
    private ContextMenuStrip context_log;
    private ToolStripMenuItem clearLogToolStripMenuItem;
    private ToolStripMenuItem runProfileToolStripMenuItem;
    private ToolStripMenuItem revertProfileToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem addNewActionToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem deleteProfileToolStripMenuItem;
    private ToolStripMenuItem pROFILENAMEToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator3;
    private ContextMenuStrip context_profileadd;
    private ToolStripMenuItem addNewProfileToolStripMenuItem;
    private SplitContainer splitContainer2;
    private ToolStrip toolStrip;
    private ToolStripMenuItem editProfileToolStripMenuItem;
    private ToolStripMenuItem aCTIONNAMEToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripMenuItem editActionToolStripMenuItem;
    private ToolStripMenuItem deleteActionToolStripMenuItem;
    private ToolStripMenuItem moveUpToolStripMenuItem;
    private ToolStripMenuItem moveDownToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator5;
    private ToolStripLabel lblProfile;
    private ToolStripButton btnProfileRun;
    private ToolStripButton btnProfileRevert;
    private ToolStripButton btnProfileEdit;
    private ToolStripButton btnProfileAdd;
    private ToolStripButton btnProfileDelete;
    private ToolStripSeparator toolStripSeparator6;
    private ToolStripLabel lblAction;
    private ToolStripButton lblActionMoveUp;
    private ToolStripButton lblActionMoveDown;
    private ToolStripButton lblActionEdit;
    private ToolStripButton lblActionDelete;
        private ToolStripMenuItem createShortcutRunToolStripMenuItem;
        private ToolStripMenuItem createShortcutRevertToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private SaveFileDialog saveShortcutFileDialog;
        private ToolStripButton lblActionAdd;

    public MainForm() => this.InitializeComponent();

    private void MainForm_Load(object sender, EventArgs e)
    {
      this.Icon = Resources.wsicon;
      Program.loadConfigurationFile();
      Program.instantiateDevcon();
      Program.instantiateServiceController();
      Program.instantiateProcessController();
      Program.instantiateExecutableController();
      this.setupTree();
      this.loadTree();
    }

    public void writeLogTextBox(string message, string textcolor = "Black")
    {
      message += "\n";
      this.txtLog.SelectionStart = this.txtLog.TextLength;
      this.txtLog.SelectionLength = 0;
      this.txtLog.SelectionColor = Color.FromName(textcolor);
      this.txtLog.AppendText(message);
      this.txtLog.SelectionColor = Color.Black;
      this.txtLog.ScrollToCaret();
    }

    private void setupTree()
    {
      MainForm.icons = new ImageList();
      MainForm.icons.Images.Add("icon_add", (Image) Resources.icon_add);
      MainForm.icons.Images.Add("icon_hardware", (Image) Resources.icon_hardware);
      MainForm.icons.Images.Add("icon_process", (Image) Resources.icon_process);
      MainForm.icons.Images.Add("icon_process", (Image)Resources.icon_process);
      MainForm.icons.Images.Add("icon_executable", (Image) Resources.icon_executable);
      MainForm.icons.Images.Add("icon_service", (Image) Resources.icon_service);
      MainForm.icons.Images.Add("icon_add_small", (Image) Resources.icon_add_small);
      MainForm.icons.Images.Add("icon_hardware_small", (Image) Resources.icon_hardware_small);
      MainForm.icons.Images.Add("icon_process_small", (Image) Resources.icon_process_small);
      MainForm.icons.Images.Add("icon_executable_small", (Image)Resources.icon_executable_small);
      MainForm.icons.Images.Add("icon_profile_small", (Image) Resources.icon_profile_small);
      MainForm.icons.Images.Add("icon_service_small", (Image) Resources.icon_service_small);
      MainForm.icons.Images.Add("icon_delete_small", (Image) Resources.icon_delete_small);
      MainForm.icons.Images.Add("icon_moveup_small", (Image) Resources.icon_moveup_small);
      MainForm.icons.Images.Add("icon_movedown_small", (Image) Resources.icon_movedown_small);
      MainForm.icons.Images.Add("icon_run_small", (Image) Resources.icon_run_small);
      MainForm.icons.Images.Add("icon_revert_small", (Image) Resources.icon_revert_small);
      this.treeProfiles.ImageList = MainForm.icons;
    }

    private void loadTree()
    {
      this.treeProfiles.Nodes.Clear();
      foreach (Profile profile in Program.profilelist.profiles)
      {
        TreeNode node1 = new TreeNode()
        {
          Tag = (object) profile,
          Text = profile.getDispalyName()
        };
        node1.ImageKey = node1.SelectedImageKey = "icon_profile_small";
        if (profile.getExpanded())
          node1.Expand();
        foreach (Action actionitem in profile.actionitems)
        {
          TreeNode node2 = new TreeNode();
          node2.Tag = (object) actionitem;
          node2.Text = actionitem.getDisplayName();
          if (actionitem is ActionHardware)
            node2.ImageKey = node2.SelectedImageKey = "icon_hardware_small";
          if (actionitem is ActionService)
            node2.ImageKey = node2.SelectedImageKey = "icon_service_small";
          if (actionitem is ActionProcess)
            node2.ImageKey = node2.SelectedImageKey = "icon_process_small";
          if (actionitem is ActionExecutable)
              node2.ImageKey = node2.SelectedImageKey = "icon_executable_small";
          if (actionitem is ActionProfile)
            node2.ImageKey = node2.SelectedImageKey = "icon_profile_small";
          node1.Nodes.Add(node2);
        }
        this.treeProfiles.Nodes.Add(node1);
      }
    }

    private void treeProfiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Button != MouseButtons.Left || e.Node.Tag != null && e.Node.Tag is Profile || (e.Node.Tag != null && e.Node.Tag is ActionHardware || e.Node.Tag != null && e.Node.Tag is ActionService) || (e.Node.Tag != null && e.Node.Tag is ActionProcess || e.Node.Tag == null))
        return;
      ActionProfile tag = e.Node.Tag as ActionProfile;
    }

    public void loadProfileForm(Profile profile = null)
    {
      int num = (int) (profile != null ? (Form) new ProfileForm(profile) : (Form) new ProfileForm()).ShowDialog();
    }

    public void loadActionPickerForm(Profile profile)
    {
      int num = (int) new ActionPickerForm(profile).ShowDialog();
    }

    public void loadActionHardwareForm(Profile profile, ActionHardware actionhardware = null)
    {
      int num = (int) (actionhardware != null ? (Form) new ActionHardwareForm(profile, actionhardware) : (Form) new ActionHardwareForm(profile)).ShowDialog();
    }

    public void loadActionServiceForm(Profile profile, ActionService actionservice = null)
    {
      int num = (int) (actionservice != null ? (Form) new ActionServiceForm(profile, actionservice) : (Form) new ActionServiceForm(profile)).ShowDialog();
    }

    public void loadActionProcessForm(Profile profile, ActionProcess actionprocess = null)
    {
      int num = (int) (actionprocess != null ? (Form) new ActionProcessForm(profile, actionprocess) : (Form) new ActionProcessForm(profile)).ShowDialog();
    }

    public void loadActionExecutableForm(Profile profile, ActionExecutable actionexecutable = null)
    {
        int num = (int)(actionexecutable != null ? (Form)new ActionExecutableForm(profile, actionexecutable) : (Form)new ActionExecutableForm(profile)).ShowDialog();
    }

        public void loadActionProfileForm(Profile profile, ActionProfile actionprofile = null)
    {
      int num = (int) (actionprofile != null ? (Form) new ActionProfileForm(profile, actionprofile) : (Form) new ActionProfileForm(profile)).ShowDialog();
    }

    public void updateAfterSaveDelete()
    {
      Program.saveConfigurationFile();
      this.loadTree();
    }

    private void treeProfiles_AfterCollapse(object sender, TreeViewEventArgs e)
    {
      if (e.Node.Tag == null || !(e.Node.Tag is Profile))
        return;
      ((Profile) e.Node.Tag).setExpanded(false);
    }

    private void treeProfiles_AfterExpand(object sender, TreeViewEventArgs e)
    {
      if (e.Node.Tag == null || !(e.Node.Tag is Profile))
        return;
      ((Profile) e.Node.Tag).setExpanded(true);
    }

    private void treeProfiles_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Delete)
      {
        if (this.treeProfiles.SelectedNode != null && this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is Profile)
        {
          Profile tag = (Profile) this.treeProfiles.SelectedNode.Tag;
          Program.profilelist.removeProfile(tag);
          Program.mainform.updateAfterSaveDelete();
        }
        if (this.treeProfiles.SelectedNode != null && this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is ActionHardware)
        {
          ((Profile) this.treeProfiles.SelectedNode.Parent.Tag).removeAction((Action) this.treeProfiles.SelectedNode.Tag);
          Program.mainform.updateAfterSaveDelete();
        }
        if (this.treeProfiles.SelectedNode != null && this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is ActionService)
        {
          ((Profile) this.treeProfiles.SelectedNode.Parent.Tag).removeAction((Action) this.treeProfiles.SelectedNode.Tag);
          Program.mainform.updateAfterSaveDelete();
        }
        if (this.treeProfiles.SelectedNode != null && this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is ActionProcess)
        {
          ((Profile) this.treeProfiles.SelectedNode.Parent.Tag).removeAction((Action) this.treeProfiles.SelectedNode.Tag);
          Program.mainform.updateAfterSaveDelete();
        }
        if (this.treeProfiles.SelectedNode != null && this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is ActionExecutable)
        {
            ((Profile)this.treeProfiles.SelectedNode.Parent.Tag).removeAction((Action)this.treeProfiles.SelectedNode.Tag);
            Program.mainform.updateAfterSaveDelete();
        }
        if (this.treeProfiles.SelectedNode != null && this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is ActionProfile)
        {
          ((Profile) this.treeProfiles.SelectedNode.Parent.Tag).removeAction((Action) this.treeProfiles.SelectedNode.Tag);
          Program.mainform.updateAfterSaveDelete();
        }
      }
      if (this.treeProfiles.SelectedNode != null)
        return;
      this.resetToolStrip();
    }

    private void clearLogToolStripMenuItem_Click(object sender, EventArgs e) => this.txtLog.Text = "";

    private void runProfileToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Profile tag = (Profile) this.contextselectednode.Tag;
      this.treeProfiles.SelectedNode = this.contextselectednode;
      tag.run();
    }

    private void revertProfileToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Profile tag = (Profile) this.contextselectednode.Tag;
      this.treeProfiles.SelectedNode = this.contextselectednode;
      tag.revert();
    }

    private void addNewActionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Profile tag = (Profile) this.contextselectednode.Tag;
      this.treeProfiles.SelectedNode = this.contextselectednode;
      this.loadActionPickerForm(tag);
    }

    private void deleteProfileToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (DialogResult.Yes != MessageBox.Show("Delete profile?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        return;
      Profile tag = (Profile) this.contextselectednode.Tag;
      Program.profilelist.removeProfile(tag);
      Program.mainform.updateAfterSaveDelete();
    }

    private void editProfileToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Profile tag = (Profile) this.contextselectednode.Tag;
      this.treeProfiles.SelectedNode = this.contextselectednode;
      this.loadProfileForm(tag);
    }

    private void treeProfiles_MouseUp(object sender, MouseEventArgs e)
    {
      this.treeProfiles.SelectedNode = (TreeNode) null;
      if (e.Button == MouseButtons.Right)
      {
        Point point = new Point(e.X, e.Y);
        this.contextselectednode = this.treeProfiles.GetNodeAt(point);
        if (this.contextselectednode != null)
        {
          if (this.contextselectednode.Tag != null && this.contextselectednode.Tag is Profile)
          {
            this.pROFILENAMEToolStripMenuItem.Text = ((Profile) this.contextselectednode.Tag).getDispalyName();
            this.context_profile.Show((Control) this.treeProfiles, point);
          }
          else if (this.contextselectednode.Tag != null && this.contextselectednode.Tag is Action)
          {
            this.aCTIONNAMEToolStripMenuItem.Text = ((Action) this.contextselectednode.Tag).getDisplayName();
            this.context_action.Show((Control) this.treeProfiles, point);
          }
        }
        else
          this.context_profileadd.Show((Control) this.treeProfiles, point);
      }
      if (this.treeProfiles.SelectedNode != null)
        return;
      this.resetToolStrip();
    }

    private void addNewProfileToolStripMenuItem_Click(object sender, EventArgs e) => this.loadProfileForm();

    private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Action tag1 = (Action) this.contextselectednode.Tag;
      Profile tag2 = (Profile) this.contextselectednode.Parent.Tag;
      tag2.moveActionUp(tag1);
      tag2.resetOrder();
      Program.mainform.updateAfterSaveDelete();
    }

    private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Action tag1 = (Action) this.contextselectednode.Tag;
      Profile tag2 = (Profile) this.contextselectednode.Parent.Tag;
      tag2.moveActionDown(tag1);
      tag2.resetOrder();
      Program.mainform.updateAfterSaveDelete();
    }

    private void editActionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (this.contextselectednode.Tag != null && this.contextselectednode.Tag is ActionHardware)
        this.loadActionHardwareForm((Profile) this.contextselectednode.Parent.Tag, (ActionHardware) this.contextselectednode.Tag);
      else if (this.contextselectednode.Tag != null && this.contextselectednode.Tag is ActionService)
        this.loadActionServiceForm((Profile) this.contextselectednode.Parent.Tag, (ActionService) this.contextselectednode.Tag);
      else if (this.contextselectednode.Tag != null && this.contextselectednode.Tag is ActionProcess)
      {
        this.loadActionProcessForm((Profile) this.contextselectednode.Parent.Tag, (ActionProcess) this.contextselectednode.Tag);
      }
      else if (this.contextselectednode.Tag != null && this.contextselectednode.Tag is ActionExecutable)
      {
        this.loadActionExecutableForm((Profile)this.contextselectednode.Parent.Tag, (ActionExecutable)this.contextselectednode.Tag);
      }
      else
      {
        if (this.contextselectednode.Tag == null || !(this.contextselectednode.Tag is ActionProfile))
          return;
        this.loadActionProfileForm((Profile) this.contextselectednode.Parent.Tag, (ActionProfile) this.contextselectednode.Tag);
      }
    }

    private void deleteActionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (DialogResult.Yes != MessageBox.Show("Delete action?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        return;
      if (this.contextselectednode.Tag != null && this.contextselectednode.Tag is ActionHardware)
      {
        ((Profile) this.contextselectednode.Parent.Tag).removeAction((Action) this.contextselectednode.Tag);
        Program.mainform.updateAfterSaveDelete();
      }
      if (this.contextselectednode.Tag != null && this.contextselectednode.Tag is ActionService)
      {
        ((Profile) this.contextselectednode.Parent.Tag).removeAction((Action) this.contextselectednode.Tag);
        Program.mainform.updateAfterSaveDelete();
      }
      if (this.contextselectednode.Tag != null && this.contextselectednode.Tag is ActionProcess)
      {
        ((Profile) this.contextselectednode.Parent.Tag).removeAction((Action) this.contextselectednode.Tag);
        Program.mainform.updateAfterSaveDelete();
      }
      if (this.contextselectednode.Tag != null && this.contextselectednode.Tag is ActionExecutable)
      {
        ((Profile)this.contextselectednode.Parent.Tag).removeAction((Action)this.contextselectednode.Tag);
        Program.mainform.updateAfterSaveDelete();
      }
      if (this.contextselectednode.Tag == null || !(this.contextselectednode.Tag is ActionProfile))
        return;
      ((Profile) this.contextselectednode.Parent.Tag).removeAction((Action) this.contextselectednode.Tag);
      Program.mainform.updateAfterSaveDelete();
    }

    private void treeProfiles_AfterSelect(object sender, TreeViewEventArgs e)
    {
      this.toolstripselectednode = this.treeProfiles.SelectedNode;
      this.resetToolStrip();
      this.btnProfileAdd.Enabled = false;
      if (this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is Profile)
      {
        this.lblProfile.Enabled = true;
        this.btnProfileRun.Enabled = true;
        this.btnProfileRevert.Enabled = true;
        this.btnProfileEdit.Enabled = true;
        this.btnProfileDelete.Enabled = true;
        this.lblAction.Enabled = true;
        this.lblActionAdd.Enabled = true;
      }
      else
      {
        if (this.treeProfiles.SelectedNode.Tag == null || !(this.treeProfiles.SelectedNode.Tag is Action))
          return;
        this.lblAction.Enabled = true;
        this.lblActionMoveUp.Enabled = true;
        this.lblActionMoveDown.Enabled = true;
        this.lblActionEdit.Enabled = true;
        this.lblActionDelete.Enabled = true;
      }
    }

    private void resetToolStrip()
    {
      this.lblProfile.Enabled = true;
      this.btnProfileAdd.Enabled = true;
      this.btnProfileRun.Enabled = false;
      this.btnProfileRevert.Enabled = false;
      this.btnProfileEdit.Enabled = false;
      this.btnProfileDelete.Enabled = false;
      this.lblAction.Enabled = false;
      this.lblActionAdd.Enabled = false;
      this.lblActionMoveUp.Enabled = false;
      this.lblActionMoveDown.Enabled = false;
      this.lblActionEdit.Enabled = false;
      this.lblActionDelete.Enabled = false;
    }

    private void treeProfiles_Leave(object sender, EventArgs e)
    {
      if (this.treeProfiles.SelectedNode != null)
        return;
      this.resetToolStrip();
    }

    private void treeProfiles_DoubleClick(object sender, EventArgs e)
    {
      if (this.treeProfiles.SelectedNode == null)
        return;
      if (this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is Profile)
        this.loadProfileForm((Profile) this.treeProfiles.SelectedNode.Tag);
      else if (this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is ActionHardware)
        this.loadActionHardwareForm((Profile) this.treeProfiles.SelectedNode.Parent.Tag, (ActionHardware) this.treeProfiles.SelectedNode.Tag);
      else if (this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is ActionService)
        this.loadActionServiceForm((Profile) this.treeProfiles.SelectedNode.Parent.Tag, (ActionService) this.treeProfiles.SelectedNode.Tag);
      else if (this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is ActionProcess)
        this.loadActionProcessForm((Profile) this.treeProfiles.SelectedNode.Parent.Tag, (ActionProcess) this.treeProfiles.SelectedNode.Tag);
      else if (this.treeProfiles.SelectedNode.Tag != null && this.treeProfiles.SelectedNode.Tag is ActionExecutable)
        this.loadActionExecutableForm((Profile)this.treeProfiles.SelectedNode.Parent.Tag, (ActionExecutable)this.treeProfiles.SelectedNode.Tag);
      else
      {
        if (this.treeProfiles.SelectedNode.Tag == null || !(this.treeProfiles.SelectedNode.Tag is ActionProfile))
          return;
        this.loadActionProfileForm((Profile) this.treeProfiles.SelectedNode.Parent.Tag, (ActionProfile) this.treeProfiles.SelectedNode.Tag);
      }
    }

    private void btnProfileAdd_Click(object sender, EventArgs e)
    {
      this.loadProfileForm();
      this.resetToolStrip();
    }

    private void btnProfileRun_Click(object sender, EventArgs e)
    {
      Profile tag = (Profile) this.toolstripselectednode.Tag;
      this.treeProfiles.SelectedNode = this.toolstripselectednode;
      tag.run();
    }

    private void btnProfileRevert_Click(object sender, EventArgs e)
    {
      Profile tag = (Profile) this.toolstripselectednode.Tag;
      this.treeProfiles.SelectedNode = this.toolstripselectednode;
      tag.revert();
    }

    private void btnProfileEdit_Click(object sender, EventArgs e)
    {
      if (this.toolstripselectednode.Tag == null || !(this.toolstripselectednode.Tag is Profile))
        return;
      Profile tag = (Profile) this.toolstripselectednode.Tag;
      this.loadProfileForm(tag);
      this.setTreeSelectedProfile(tag);
    }

    private void btnProfileDelete_Click(object sender, EventArgs e)
    {
      if (DialogResult.Yes != MessageBox.Show("Delete profile?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        return;
      Profile tag = (Profile) this.toolstripselectednode.Tag;
      Program.profilelist.removeProfile(tag);
      Program.mainform.updateAfterSaveDelete();
      this.resetToolStrip();
      this.setTreeSelectedProfile(tag);
    }

    private void lblActionAdd_Click(object sender, EventArgs e)
    {
      if (this.toolstripselectednode.Tag == null || !(this.toolstripselectednode.Tag is Profile))
        return;
      Profile tag = (Profile) this.toolstripselectednode.Tag;
      this.treeProfiles.SelectedNode = this.contextselectednode;
      this.loadActionPickerForm(tag);
    }

    private void lblActionMoveUp_Click(object sender, EventArgs e)
    {
      if (this.toolstripselectednode.Tag == null || !(this.toolstripselectednode.Tag is Action))
        return;
      Action tag1 = (Action) this.toolstripselectednode.Tag;
      Profile tag2 = (Profile) this.toolstripselectednode.Parent.Tag;
      tag2.moveActionUp(tag1);
      tag2.resetOrder();
      Program.mainform.updateAfterSaveDelete();
      this.setTreeSelectedAction(tag1);
    }

    private void lblActionMoveDown_Click(object sender, EventArgs e)
    {
      if (this.toolstripselectednode.Tag == null || !(this.toolstripselectednode.Tag is Action))
        return;
      Action tag1 = (Action) this.toolstripselectednode.Tag;
      Profile tag2 = (Profile) this.toolstripselectednode.Parent.Tag;
      tag2.moveActionDown(tag1);
      tag2.resetOrder();
      Program.mainform.updateAfterSaveDelete();
      this.setTreeSelectedAction(tag1);
    }

    private void lblActionEdit_Click(object sender, EventArgs e)
    {
      if (this.toolstripselectednode.Tag != null && this.toolstripselectednode.Tag is ActionHardware)
      {
        ActionHardware tag = (ActionHardware) this.toolstripselectednode.Tag;
        this.loadActionHardwareForm((Profile) this.toolstripselectednode.Parent.Tag, (ActionHardware) this.toolstripselectednode.Tag);
        this.setTreeSelectedAction((Action) tag);
      }
      else if (this.toolstripselectednode.Tag != null && this.toolstripselectednode.Tag is ActionService)
      {
        ActionService tag = (ActionService) this.toolstripselectednode.Tag;
        this.loadActionServiceForm((Profile) this.toolstripselectednode.Parent.Tag, (ActionService) this.toolstripselectednode.Tag);
        this.setTreeSelectedAction((Action) tag);
      }
      else if (this.toolstripselectednode.Tag != null && this.toolstripselectednode.Tag is ActionProcess)
      {
        ActionProcess tag = (ActionProcess) this.toolstripselectednode.Tag;
        this.loadActionProcessForm((Profile) this.toolstripselectednode.Parent.Tag, (ActionProcess) this.toolstripselectednode.Tag);
        this.setTreeSelectedAction((Action) tag);
      }
      else if (this.toolstripselectednode.Tag != null && this.toolstripselectednode.Tag is ActionExecutable)
      {
        ActionExecutable tag = (ActionExecutable)this.toolstripselectednode.Tag;
        this.loadActionExecutableForm((Profile)this.toolstripselectednode.Parent.Tag, (ActionExecutable)this.toolstripselectednode.Tag);
        this.setTreeSelectedAction((Action)tag);
      }
      else
      {
        if (this.toolstripselectednode.Tag == null || !(this.toolstripselectednode.Tag is ActionProfile))
          return;
        ActionProfile tag = (ActionProfile) this.toolstripselectednode.Tag;
        this.loadActionProfileForm((Profile) this.toolstripselectednode.Parent.Tag, (ActionProfile) this.toolstripselectednode.Tag);
        this.setTreeSelectedAction((Action) tag);
      }
    }

    private void lblActionDelete_Click(object sender, EventArgs e)
    {
      if (DialogResult.Yes != MessageBox.Show("Delete action?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
        return;
      if (this.toolstripselectednode.Tag != null && this.toolstripselectednode.Tag is ActionHardware)
      {
        ((Profile) this.toolstripselectednode.Parent.Tag).removeAction((Action) this.toolstripselectednode.Tag);
        Program.mainform.updateAfterSaveDelete();
      }
      if (this.toolstripselectednode.Tag != null && this.toolstripselectednode.Tag is ActionService)
      {
        ((Profile) this.toolstripselectednode.Parent.Tag).removeAction((Action) this.toolstripselectednode.Tag);
        Program.mainform.updateAfterSaveDelete();
      }
      if (this.toolstripselectednode.Tag != null && this.toolstripselectednode.Tag is ActionProcess)
      {
        ((Profile) this.toolstripselectednode.Parent.Tag).removeAction((Action) this.toolstripselectednode.Tag);
        Program.mainform.updateAfterSaveDelete();
      }
      if (this.toolstripselectednode.Tag != null && this.toolstripselectednode.Tag is ActionExecutable)
      {
         ((Profile)this.toolstripselectednode.Parent.Tag).removeAction((Action)this.toolstripselectednode.Tag);
          Program.mainform.updateAfterSaveDelete();
      }
      if (this.toolstripselectednode.Tag != null && this.toolstripselectednode.Tag is ActionProfile)
      {
        ((Profile) this.toolstripselectednode.Parent.Tag).removeAction((Action) this.toolstripselectednode.Tag);
        Program.mainform.updateAfterSaveDelete();
      }
      this.resetToolStrip();
    }

    private void setTreeSelectedAction(Action action)
    {
      foreach (TreeNode node1 in this.treeProfiles.Nodes)
      {
        foreach (TreeNode node2 in node1.Nodes)
        {
          if (node2.Tag == action)
            this.treeProfiles.SelectedNode = node2;
        }
      }
    }

    public void setTreeSelectedProfile(Profile profile)
    {
      foreach (TreeNode node in this.treeProfiles.Nodes)
      {
        if (node.Tag == profile)
          this.treeProfiles.SelectedNode = node;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeProfiles = new System.Windows.Forms.TreeView();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.context_log = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.context_profile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pROFILENAMEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.runProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revertProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.editProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.context_action = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aCTIONNAMEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.editActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.context_profileadd = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.lblProfile = new System.Windows.Forms.ToolStripLabel();
            this.btnProfileAdd = new System.Windows.Forms.ToolStripButton();
            this.btnProfileRun = new System.Windows.Forms.ToolStripButton();
            this.btnProfileRevert = new System.Windows.Forms.ToolStripButton();
            this.btnProfileEdit = new System.Windows.Forms.ToolStripButton();
            this.btnProfileDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.lblAction = new System.Windows.Forms.ToolStripLabel();
            this.lblActionAdd = new System.Windows.Forms.ToolStripButton();
            this.lblActionMoveUp = new System.Windows.Forms.ToolStripButton();
            this.lblActionMoveDown = new System.Windows.Forms.ToolStripButton();
            this.lblActionEdit = new System.Windows.Forms.ToolStripButton();
            this.lblActionDelete = new System.Windows.Forms.ToolStripButton();
            this.createShortcutRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createShortcutRevertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.saveShortcutFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.context_log.SuspendLayout();
            this.context_profile.SuspendLayout();
            this.context_action.SuspendLayout();
            this.context_profileadd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeProfiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtLog);
            this.splitContainer1.Size = new System.Drawing.Size(1111, 717);
            this.splitContainer1.SplitterDistance = 518;
            this.splitContainer1.TabIndex = 2;
            // 
            // treeProfiles
            // 
            this.treeProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeProfiles.HideSelection = false;
            this.treeProfiles.Location = new System.Drawing.Point(0, 0);
            this.treeProfiles.Name = "treeProfiles";
            this.treeProfiles.Size = new System.Drawing.Size(518, 717);
            this.treeProfiles.TabIndex = 1;
            this.treeProfiles.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeProfiles_AfterCollapse);
            this.treeProfiles.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeProfiles_AfterExpand);
            this.treeProfiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeProfiles_AfterSelect);
            this.treeProfiles.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeProfiles_NodeMouseClick);
            this.treeProfiles.DoubleClick += new System.EventHandler(this.treeProfiles_DoubleClick);
            this.treeProfiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeProfiles_KeyUp);
            this.treeProfiles.Leave += new System.EventHandler(this.treeProfiles_Leave);
            this.treeProfiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeProfiles_MouseUp);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.ContextMenuStrip = this.context_log;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(589, 717);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // context_log
            // 
            this.context_log.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearLogToolStripMenuItem});
            this.context_log.Name = "context_log";
            this.context_log.ShowItemToolTips = false;
            this.context_log.Size = new System.Drawing.Size(122, 26);
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_delete_small;
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            this.clearLogToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.clearLogToolStripMenuItem.Text = "Clear log";
            this.clearLogToolStripMenuItem.Click += new System.EventHandler(this.clearLogToolStripMenuItem_Click);
            // 
            // context_profile
            // 
            this.context_profile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pROFILENAMEToolStripMenuItem,
            this.toolStripSeparator3,
            this.runProfileToolStripMenuItem,
            this.revertProfileToolStripMenuItem,
            this.toolStripSeparator1,
            this.editProfileToolStripMenuItem,
            this.addNewActionToolStripMenuItem,
            this.toolStripSeparator2,
            this.createShortcutRunToolStripMenuItem,
            this.createShortcutRevertToolStripMenuItem,
            this.toolStripSeparator7,
            this.deleteProfileToolStripMenuItem});
            this.context_profile.Name = "context_profile";
            this.context_profile.Size = new System.Drawing.Size(201, 204);
            // 
            // pROFILENAMEToolStripMenuItem
            // 
            this.pROFILENAMEToolStripMenuItem.Enabled = false;
            this.pROFILENAMEToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.pROFILENAMEToolStripMenuItem.Name = "pROFILENAMEToolStripMenuItem";
            this.pROFILENAMEToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.pROFILENAMEToolStripMenuItem.Text = "PROFILENAME";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(197, 6);
            // 
            // runProfileToolStripMenuItem
            // 
            this.runProfileToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_run_small;
            this.runProfileToolStripMenuItem.Name = "runProfileToolStripMenuItem";
            this.runProfileToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.runProfileToolStripMenuItem.Text = "Run profile";
            this.runProfileToolStripMenuItem.Click += new System.EventHandler(this.runProfileToolStripMenuItem_Click);
            // 
            // revertProfileToolStripMenuItem
            // 
            this.revertProfileToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_revert_small;
            this.revertProfileToolStripMenuItem.Name = "revertProfileToolStripMenuItem";
            this.revertProfileToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.revertProfileToolStripMenuItem.Text = "Revert profile";
            this.revertProfileToolStripMenuItem.Click += new System.EventHandler(this.revertProfileToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(197, 6);
            // 
            // editProfileToolStripMenuItem
            // 
            this.editProfileToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_edit_small;
            this.editProfileToolStripMenuItem.Name = "editProfileToolStripMenuItem";
            this.editProfileToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.editProfileToolStripMenuItem.Text = "Edit profile";
            this.editProfileToolStripMenuItem.Click += new System.EventHandler(this.editProfileToolStripMenuItem_Click);
            // 
            // addNewActionToolStripMenuItem
            // 
            this.addNewActionToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_add_small;
            this.addNewActionToolStripMenuItem.Name = "addNewActionToolStripMenuItem";
            this.addNewActionToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.addNewActionToolStripMenuItem.Text = "Add new action";
            this.addNewActionToolStripMenuItem.Click += new System.EventHandler(this.addNewActionToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(197, 6);
            // 
            // deleteProfileToolStripMenuItem
            // 
            this.deleteProfileToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_delete_small;
            this.deleteProfileToolStripMenuItem.Name = "deleteProfileToolStripMenuItem";
            this.deleteProfileToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.deleteProfileToolStripMenuItem.Text = "Delete profile";
            this.deleteProfileToolStripMenuItem.Click += new System.EventHandler(this.deleteProfileToolStripMenuItem_Click);
            // 
            // context_action
            // 
            this.context_action.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aCTIONNAMEToolStripMenuItem,
            this.toolStripSeparator4,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem,
            this.toolStripSeparator5,
            this.editActionToolStripMenuItem,
            this.deleteActionToolStripMenuItem});
            this.context_action.Name = "context_action";
            this.context_action.Size = new System.Drawing.Size(153, 126);
            // 
            // aCTIONNAMEToolStripMenuItem
            // 
            this.aCTIONNAMEToolStripMenuItem.Enabled = false;
            this.aCTIONNAMEToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.aCTIONNAMEToolStripMenuItem.Name = "aCTIONNAMEToolStripMenuItem";
            this.aCTIONNAMEToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aCTIONNAMEToolStripMenuItem.Text = "ACTIONNAME";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(149, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_moveup_small;
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.moveUpToolStripMenuItem.Text = "Move up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_movedown_small;
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.moveDownToolStripMenuItem.Text = "Move down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(149, 6);
            // 
            // editActionToolStripMenuItem
            // 
            this.editActionToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_edit_small;
            this.editActionToolStripMenuItem.Name = "editActionToolStripMenuItem";
            this.editActionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editActionToolStripMenuItem.Text = "Edit action";
            this.editActionToolStripMenuItem.Click += new System.EventHandler(this.editActionToolStripMenuItem_Click);
            // 
            // deleteActionToolStripMenuItem
            // 
            this.deleteActionToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_delete_small;
            this.deleteActionToolStripMenuItem.Name = "deleteActionToolStripMenuItem";
            this.deleteActionToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteActionToolStripMenuItem.Text = "Delete action";
            this.deleteActionToolStripMenuItem.Click += new System.EventHandler(this.deleteActionToolStripMenuItem_Click);
            // 
            // context_profileadd
            // 
            this.context_profileadd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewProfileToolStripMenuItem});
            this.context_profileadd.Name = "context_profileadd";
            this.context_profileadd.Size = new System.Drawing.Size(159, 26);
            // 
            // addNewProfileToolStripMenuItem
            // 
            this.addNewProfileToolStripMenuItem.Image = global::Whipper_Snipper.Properties.Resources.icon_add_small;
            this.addNewProfileToolStripMenuItem.Name = "addNewProfileToolStripMenuItem";
            this.addNewProfileToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.addNewProfileToolStripMenuItem.Text = "Add new profile";
            this.addNewProfileToolStripMenuItem.Click += new System.EventHandler(this.addNewProfileToolStripMenuItem_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip);
            this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.splitContainer2.Panel1MinSize = 20;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(1111, 746);
            this.splitContainer2.SplitterDistance = 25;
            this.splitContainer2.TabIndex = 6;
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblProfile,
            this.btnProfileAdd,
            this.btnProfileRun,
            this.btnProfileRevert,
            this.btnProfileEdit,
            this.btnProfileDelete,
            this.toolStripSeparator6,
            this.lblAction,
            this.lblActionAdd,
            this.lblActionMoveUp,
            this.lblActionMoveDown,
            this.lblActionEdit,
            this.lblActionDelete});
            this.toolStrip.Location = new System.Drawing.Point(0, 3);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1111, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // lblProfile
            // 
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(41, 22);
            this.lblProfile.Text = "Profile";
            // 
            // btnProfileAdd
            // 
            this.btnProfileAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnProfileAdd.Image = global::Whipper_Snipper.Properties.Resources.icon_add;
            this.btnProfileAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProfileAdd.Name = "btnProfileAdd";
            this.btnProfileAdd.Size = new System.Drawing.Size(23, 22);
            this.btnProfileAdd.Text = "Add profile";
            this.btnProfileAdd.Click += new System.EventHandler(this.btnProfileAdd_Click);
            // 
            // btnProfileRun
            // 
            this.btnProfileRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnProfileRun.Enabled = false;
            this.btnProfileRun.Image = global::Whipper_Snipper.Properties.Resources.icon_run_small;
            this.btnProfileRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProfileRun.Name = "btnProfileRun";
            this.btnProfileRun.Size = new System.Drawing.Size(23, 22);
            this.btnProfileRun.Text = "Run profile";
            this.btnProfileRun.Click += new System.EventHandler(this.btnProfileRun_Click);
            // 
            // btnProfileRevert
            // 
            this.btnProfileRevert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnProfileRevert.Enabled = false;
            this.btnProfileRevert.Image = global::Whipper_Snipper.Properties.Resources.icon_revert_small;
            this.btnProfileRevert.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProfileRevert.Name = "btnProfileRevert";
            this.btnProfileRevert.Size = new System.Drawing.Size(23, 22);
            this.btnProfileRevert.Text = "Revert profile";
            this.btnProfileRevert.Click += new System.EventHandler(this.btnProfileRevert_Click);
            // 
            // btnProfileEdit
            // 
            this.btnProfileEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnProfileEdit.Enabled = false;
            this.btnProfileEdit.Image = global::Whipper_Snipper.Properties.Resources.icon_edit_small;
            this.btnProfileEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProfileEdit.Name = "btnProfileEdit";
            this.btnProfileEdit.Size = new System.Drawing.Size(23, 22);
            this.btnProfileEdit.Text = "Edit profile";
            this.btnProfileEdit.Click += new System.EventHandler(this.btnProfileEdit_Click);
            // 
            // btnProfileDelete
            // 
            this.btnProfileDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnProfileDelete.Enabled = false;
            this.btnProfileDelete.Image = global::Whipper_Snipper.Properties.Resources.icon_delete_small;
            this.btnProfileDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProfileDelete.Name = "btnProfileDelete";
            this.btnProfileDelete.Size = new System.Drawing.Size(23, 22);
            this.btnProfileDelete.Text = "Delete profile";
            this.btnProfileDelete.Click += new System.EventHandler(this.btnProfileDelete_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // lblAction
            // 
            this.lblAction.Enabled = false;
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(42, 22);
            this.lblAction.Text = "Action";
            // 
            // lblActionAdd
            // 
            this.lblActionAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lblActionAdd.Enabled = false;
            this.lblActionAdd.Image = global::Whipper_Snipper.Properties.Resources.icon_add_small;
            this.lblActionAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lblActionAdd.Name = "lblActionAdd";
            this.lblActionAdd.Size = new System.Drawing.Size(23, 22);
            this.lblActionAdd.Text = "Add new action";
            this.lblActionAdd.Click += new System.EventHandler(this.lblActionAdd_Click);
            // 
            // lblActionMoveUp
            // 
            this.lblActionMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lblActionMoveUp.Enabled = false;
            this.lblActionMoveUp.Image = global::Whipper_Snipper.Properties.Resources.icon_moveup_small;
            this.lblActionMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lblActionMoveUp.Name = "lblActionMoveUp";
            this.lblActionMoveUp.Size = new System.Drawing.Size(23, 22);
            this.lblActionMoveUp.Text = "Move up";
            this.lblActionMoveUp.Click += new System.EventHandler(this.lblActionMoveUp_Click);
            // 
            // lblActionMoveDown
            // 
            this.lblActionMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lblActionMoveDown.Enabled = false;
            this.lblActionMoveDown.Image = global::Whipper_Snipper.Properties.Resources.icon_movedown_small;
            this.lblActionMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lblActionMoveDown.Name = "lblActionMoveDown";
            this.lblActionMoveDown.Size = new System.Drawing.Size(23, 22);
            this.lblActionMoveDown.Text = "Move down";
            this.lblActionMoveDown.Click += new System.EventHandler(this.lblActionMoveDown_Click);
            // 
            // lblActionEdit
            // 
            this.lblActionEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lblActionEdit.Enabled = false;
            this.lblActionEdit.Image = global::Whipper_Snipper.Properties.Resources.icon_edit_small;
            this.lblActionEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lblActionEdit.Name = "lblActionEdit";
            this.lblActionEdit.Size = new System.Drawing.Size(23, 22);
            this.lblActionEdit.Text = "Edit action";
            this.lblActionEdit.Click += new System.EventHandler(this.lblActionEdit_Click);
            // 
            // lblActionDelete
            // 
            this.lblActionDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lblActionDelete.Enabled = false;
            this.lblActionDelete.Image = global::Whipper_Snipper.Properties.Resources.icon_delete_small;
            this.lblActionDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.lblActionDelete.Name = "lblActionDelete";
            this.lblActionDelete.Size = new System.Drawing.Size(23, 22);
            this.lblActionDelete.Text = "Delete action";
            this.lblActionDelete.Click += new System.EventHandler(this.lblActionDelete_Click);
            // 
            // createShortcutRunToolStripMenuItem
            // 
            this.createShortcutRunToolStripMenuItem.Name = "createShortcutRunToolStripMenuItem";
            this.createShortcutRunToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.createShortcutRunToolStripMenuItem.Text = "Create Shortcut (Run)";
            this.createShortcutRunToolStripMenuItem.Click += new System.EventHandler(this.createShortcutRunToolStripMenuItem_Click);
            // 
            // createShortcutRevertToolStripMenuItem
            // 
            this.createShortcutRevertToolStripMenuItem.Name = "createShortcutRevertToolStripMenuItem";
            this.createShortcutRevertToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.createShortcutRevertToolStripMenuItem.Text = "Create Shortcut (Revert)";
            this.createShortcutRevertToolStripMenuItem.Click += new System.EventHandler(this.createShortcutRevertToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(197, 6);
            // 
            // saveShortcutFileDialog
            // 
            this.saveShortcutFileDialog.Filter = "shortcut files (*.lnk)|*.lnk";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 746);
            this.Controls.Add(this.splitContainer2);
            this.Name = "MainForm";
            this.Text = "Whipper Snipper 1.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.context_log.ResumeLayout(false);
            this.context_profile.ResumeLayout(false);
            this.context_action.ResumeLayout(false);
            this.context_profileadd.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

    }

        private void createShortcutRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Profile tag = (Profile)this.contextselectednode.Tag;
            this.treeProfiles.SelectedNode = this.contextselectednode;

            saveShortcutFileDialog.FileName = tag.getDispalyName() + " (Run).lnk";
            if (saveShortcutFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Program.writeLog("Saving shortcut to " + saveShortcutFileDialog.FileName);
                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(saveShortcutFileDialog.FileName);
                    shortcut.Description = "Run Whipper Snipper Profile: " + tag.getDispalyName();
                    shortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    shortcut.Arguments = "-run " + tag.guid;
                    shortcut.Save();
                }
                catch(Exception ex)
                {
                    Program.writeLogError("Error saving shortcut");
                    Program.writeLogError(ex.Message);
                }              
            }
        }

        private void createShortcutRevertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Profile tag = (Profile)this.contextselectednode.Tag;
            this.treeProfiles.SelectedNode = this.contextselectednode;

            saveShortcutFileDialog.FileName = tag.getDispalyName() + " (Revert).lnk";
            if (saveShortcutFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Program.writeLog("Saving shortcut to " + saveShortcutFileDialog.FileName);
                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(saveShortcutFileDialog.FileName);
                    shortcut.Description = "Run Whipper Snipper Profile: " + tag.getDispalyName();
                    shortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    shortcut.Arguments = "-revert " + tag.guid;
                    shortcut.Save();
                }
                catch (Exception ex)
                {
                    Program.writeLogError("Error saving shortcut");
                    Program.writeLogError(ex.Message);
                }
            }
        }
    }
}
