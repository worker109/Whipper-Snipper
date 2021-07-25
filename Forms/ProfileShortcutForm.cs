using IWshRuntimeLibrary;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Whipper_Snipper.Properties;

namespace Whipper_Snipper
{
    public class ProfileShortcutForm : Form
    {
        private Profile profile;
        private bool run;   
        private IContainer components;
        private Label label2;
        private Button btnSave;
        private Button btnCancel;
        private SaveFileDialog saveShortcutDialog;
        private ComboBox cboIcon;
        private string defaultnonetext = "(none)";

        public ProfileShortcutForm(Profile profile, bool run)
        {
            this.InitializeComponent();
            this.profile = profile;
            this.run = run;            
            this.Text = "Create Shortcut " + (run?"(Run)":"(Revert)");
        }

        private void ProfileShortcutForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.icon_profile_icon;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.cboIcon.Items.Clear();
            foreach (Action action in profile.actionitems)
            {
                if (action.GetType().Name == "ActionExecutable")
                {
                    var actionexecutable = (ActionExecutable)action;
                    this.cboIcon.Items.Add(actionexecutable.executableinfo);
                }
                if (action.GetType().Name == "ActionProcess")
                {
                    var actionprocess = (ActionProcess)action;
                    this.cboIcon.Items.Add(actionprocess.processinfo);
                }
            }           
            this.cboIcon.Items.Add(defaultnonetext);
            if(this.cboIcon.Items.Count == 1)
            {
                this.cboIcon.SelectedIndex = this.cboIcon.Items.Count - 1;
            }
            else
            {
                this.cboIcon.SelectedIndex = this.cboIcon.Items.Count - 2;
            }
            //this.cboProfile.SelectedItem = (object) this.actionprofile.getProfile();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Pen darkGray = Pens.DarkGray;
            Rectangle clipRectangle = e.ClipRectangle;
            int left = clipRectangle.Left;
            clipRectangle = e.ClipRectangle;
            int y1 = clipRectangle.Bottom - 1;
            clipRectangle = e.ClipRectangle;
            int right = clipRectangle.Right;
            clipRectangle = e.ClipRectangle;
            int y2 = clipRectangle.Bottom - 1;
            graphics.DrawLine(darkGray, left, y1, right, y2);
            this.OnPaint(e);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            string iconpath = "";
            if(cboIcon.SelectedItem.ToString() != defaultnonetext)
            {
                if (cboIcon.SelectedItem.GetType().Name == "ExecutableInfo")
                {
                    var executableinfo = (ExecutableInfo)cboIcon.SelectedItem;
                    iconpath = executableinfo.exepath;
                }
                if (cboIcon.SelectedItem.GetType().Name == "ProcessInfo")
                {
                    var processinfo = (ProcessInfo)cboIcon.SelectedItem;
                    iconpath = processinfo.exepath;
                }                
            }

            saveShortcutDialog.FileName = profile.getDispalyName() + (run ? " (Run).lnk" : " (Revert).lnk");
            if (saveShortcutDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Program.writeLog("Saving shortcut to " + saveShortcutDialog.FileName);
                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(saveShortcutDialog.FileName);
                    shortcut.Description = "Run Whipper Snipper Profile: " + profile.getDispalyName();
                    shortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    shortcut.Arguments = (run ? "-run " : "-revert ") + profile.guid;
                    if (iconpath != "")
                        shortcut.IconLocation = iconpath;
                    shortcut.Save();
                }
                catch (Exception ex)
                {
                    Program.writeLogError("Error saving shortcut");
                    Program.writeLogError(ex.Message);                    
                }
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboIcon = new System.Windows.Forms.ComboBox();
            this.saveShortcutDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Use icon from:";
            // 
            // btnSave
            // 
            this.btnSave.ImageKey = "(none)";
            this.btnSave.Location = new System.Drawing.Point(15, 69);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(96, 69);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cboIcon
            // 
            this.cboIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIcon.FormattingEnabled = true;
            this.cboIcon.Location = new System.Drawing.Point(15, 25);
            this.cboIcon.Name = "cboIcon";
            this.cboIcon.Size = new System.Drawing.Size(237, 21);
            this.cboIcon.TabIndex = 7;
            // 
            // saveShortcutDialog
            // 
            this.saveShortcutDialog.Filter = "shortcut files (*.lnk)|*.lnk";
            // 
            // ProfileShortcutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 107);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cboIcon);
            this.Controls.Add(this.btnCancel);
            this.Name = "ProfileShortcutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Profile Shortcut";
            this.Load += new System.EventHandler(this.ProfileShortcutForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
