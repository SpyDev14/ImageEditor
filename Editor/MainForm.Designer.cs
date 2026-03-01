namespace Editor
{
	partial class MainForm
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Panel toolsPanel;
			GroupBox colorGroupBox;
			GroupBox penWidthGroupBox;
			selectedColorPanel = new Panel();
			penWidthTrackBar = new TrackBar();
			workspacePanel = new Panel();
			openedPicture = new PictureBox();
			menuStrip = new MenuStrip();
			fileToolStripMenuItem = new ToolStripMenuItem();
			openToolStripMenuItem = new ToolStripMenuItem();
			newToolStripMenuItem = new ToolStripMenuItem();
			saveToolStripMenuItem = new ToolStripMenuItem();
			saveAsToolStripMenuItem = new ToolStripMenuItem();
			editToolStripMenuItem = new ToolStripMenuItem();
			toolsPanel = new Panel();
			colorGroupBox = new GroupBox();
			penWidthGroupBox = new GroupBox();
			toolsPanel.SuspendLayout();
			colorGroupBox.SuspendLayout();
			penWidthGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)penWidthTrackBar).BeginInit();
			workspacePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)openedPicture).BeginInit();
			menuStrip.SuspendLayout();
			SuspendLayout();
			// 
			// toolsPanel
			// 
			toolsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			toolsPanel.BackColor = SystemColors.ControlLightLight;
			toolsPanel.Controls.Add(colorGroupBox);
			toolsPanel.Controls.Add(penWidthGroupBox);
			toolsPanel.Location = new Point(506, 24);
			toolsPanel.Name = "toolsPanel";
			toolsPanel.Padding = new Padding(8, 6, 6, 6);
			toolsPanel.Size = new Size(130, 317);
			toolsPanel.TabIndex = 3;
			// 
			// colorGroupBox
			// 
			colorGroupBox.Controls.Add(selectedColorPanel);
			colorGroupBox.Dock = DockStyle.Top;
			colorGroupBox.Location = new Point(8, 61);
			colorGroupBox.Name = "colorGroupBox";
			colorGroupBox.Padding = new Padding(5, 3, 5, 5);
			colorGroupBox.Size = new Size(116, 48);
			colorGroupBox.TabIndex = 4;
			colorGroupBox.TabStop = false;
			colorGroupBox.Text = "Color";
			// 
			// selectedColorPanel
			// 
			selectedColorPanel.BackColor = Color.Red;
			selectedColorPanel.BorderStyle = BorderStyle.Fixed3D;
			selectedColorPanel.Cursor = Cursors.Hand;
			selectedColorPanel.Dock = DockStyle.Fill;
			selectedColorPanel.Location = new Point(5, 19);
			selectedColorPanel.Name = "selectedColorPanel";
			selectedColorPanel.Size = new Size(106, 24);
			selectedColorPanel.TabIndex = 0;
			selectedColorPanel.Click += selectedColorPanel_Click;
			// 
			// penWidthGroupBox
			// 
			penWidthGroupBox.Controls.Add(penWidthTrackBar);
			penWidthGroupBox.Dock = DockStyle.Top;
			penWidthGroupBox.Location = new Point(8, 6);
			penWidthGroupBox.Name = "penWidthGroupBox";
			penWidthGroupBox.Size = new Size(116, 55);
			penWidthGroupBox.TabIndex = 3;
			penWidthGroupBox.TabStop = false;
			penWidthGroupBox.Text = "Pen width";
			// 
			// penWidthTrackBar
			// 
			penWidthTrackBar.Cursor = Cursors.Hand;
			penWidthTrackBar.Dock = DockStyle.Fill;
			penWidthTrackBar.LargeChange = 2;
			penWidthTrackBar.Location = new Point(3, 19);
			penWidthTrackBar.Minimum = 1;
			penWidthTrackBar.Name = "penWidthTrackBar";
			penWidthTrackBar.Size = new Size(110, 33);
			penWidthTrackBar.TabIndex = 0;
			penWidthTrackBar.Value = 1;
			penWidthTrackBar.ValueChanged += penWidthTrackBar_ValueChanged;
			// 
			// workspacePanel
			// 
			workspacePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			workspacePanel.BorderStyle = BorderStyle.FixedSingle;
			workspacePanel.Controls.Add(openedPicture);
			workspacePanel.Location = new Point(1, 24);
			workspacePanel.Margin = new Padding(0);
			workspacePanel.Name = "workspacePanel";
			workspacePanel.Size = new Size(505, 316);
			workspacePanel.TabIndex = 2;
			workspacePanel.MouseDown += workspacePanel_MouseDown;
			workspacePanel.MouseMove += workspacePanel_MouseMove;
			workspacePanel.MouseUp += workspacePanel_MouseUp;
			// 
			// openedPicture
			// 
			openedPicture.Anchor = AnchorStyles.None;
			openedPicture.Cursor = Cursors.Cross;
			openedPicture.Enabled = false;
			openedPicture.Location = new Point(125, 70);
			openedPicture.Name = "openedPicture";
			openedPicture.Size = new Size(247, 168);
			openedPicture.TabIndex = 1;
			openedPicture.TabStop = false;
			// 
			// menuStrip
			// 
			menuStrip.BackColor = SystemColors.ControlLightLight;
			menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
			menuStrip.Location = new Point(0, 0);
			menuStrip.Name = "menuStrip";
			menuStrip.Size = new Size(636, 24);
			menuStrip.TabIndex = 0;
			menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, newToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new Size(37, 20);
			fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			openToolStripMenuItem.Name = "openToolStripMenuItem";
			openToolStripMenuItem.Size = new Size(114, 22);
			openToolStripMenuItem.Text = "Open";
			openToolStripMenuItem.Click += openToolStripMenuItem_Click;
			// 
			// newToolStripMenuItem
			// 
			newToolStripMenuItem.Name = "newToolStripMenuItem";
			newToolStripMenuItem.Size = new Size(114, 22);
			newToolStripMenuItem.Text = "New";
			newToolStripMenuItem.Click += newToolStripMenuItem_Click;
			// 
			// saveToolStripMenuItem
			// 
			saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			saveToolStripMenuItem.Size = new Size(114, 22);
			saveToolStripMenuItem.Text = "Save";
			saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
			// 
			// saveAsToolStripMenuItem
			// 
			saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			saveAsToolStripMenuItem.Size = new Size(114, 22);
			saveAsToolStripMenuItem.Text = "Save As";
			saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
			// 
			// editToolStripMenuItem
			// 
			editToolStripMenuItem.Name = "editToolStripMenuItem";
			editToolStripMenuItem.Size = new Size(39, 20);
			editToolStripMenuItem.Text = "Edit";
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(636, 341);
			Controls.Add(toolsPanel);
			Controls.Add(workspacePanel);
			Controls.Add(menuStrip);
			MainMenuStrip = menuStrip;
			MinimumSize = new Size(224, 192);
			Name = "MainForm";
			RightToLeft = RightToLeft.No;
			Text = "Image Editor";
			toolsPanel.ResumeLayout(false);
			colorGroupBox.ResumeLayout(false);
			penWidthGroupBox.ResumeLayout(false);
			penWidthGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)penWidthTrackBar).EndInit();
			workspacePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)openedPicture).EndInit();
			menuStrip.ResumeLayout(false);
			menuStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private MenuStrip menuStrip;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem openToolStripMenuItem;
		private ToolStripMenuItem newToolStripMenuItem;
		private ToolStripMenuItem saveToolStripMenuItem;
		private ToolStripMenuItem saveAsToolStripMenuItem;
		private PictureBox openedPicture;
		private Panel workspacePanel;
		private ToolStripMenuItem editToolStripMenuItem;
		private TrackBar penWidthTrackBar;
		private Panel selectedColorPanel;
	}
}
