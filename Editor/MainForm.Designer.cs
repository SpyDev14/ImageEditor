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
			MenuStrip menuStrip;
			ToolStripMenuItem fileToolStripMenuItem;
			ToolStripMenuItem openToolStripMenuItem;
			ToolStripMenuItem newToolStripMenuItem;
			ToolStripMenuItem withParamsToolStripMenuItem;
			ToolStripMenuItem saveToolStripMenuItem;
			ToolStripMenuItem saveAsToolStripMenuItem;
			ToolStripMenuItem editToolStripMenuItem;
			ToolStripMenuItem effectsToolStripMenuItem;
			ToolStripMenuItem turnToGrayEffectToolStripMenuItem;
			ToolStripMenuItem viewToolStripMenuItem;
			ToolStripMenuItem centerCameraToolStripMenuItem;
			zoomLabel = new Label();
			selectedColorPanel = new Panel();
			penWidthTrackBar = new TrackBar();
			resetZoomToolStripMenuItem = new ToolStripMenuItem();
			workspacePanel = new Panel();
			picture = new PictureBox();
			toolsPanel = new Panel();
			colorGroupBox = new GroupBox();
			penWidthGroupBox = new GroupBox();
			menuStrip = new MenuStrip();
			fileToolStripMenuItem = new ToolStripMenuItem();
			openToolStripMenuItem = new ToolStripMenuItem();
			newToolStripMenuItem = new ToolStripMenuItem();
			withParamsToolStripMenuItem = new ToolStripMenuItem();
			saveToolStripMenuItem = new ToolStripMenuItem();
			saveAsToolStripMenuItem = new ToolStripMenuItem();
			editToolStripMenuItem = new ToolStripMenuItem();
			effectsToolStripMenuItem = new ToolStripMenuItem();
			turnToGrayEffectToolStripMenuItem = new ToolStripMenuItem();
			viewToolStripMenuItem = new ToolStripMenuItem();
			centerCameraToolStripMenuItem = new ToolStripMenuItem();
			toolsPanel.SuspendLayout();
			colorGroupBox.SuspendLayout();
			penWidthGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)penWidthTrackBar).BeginInit();
			menuStrip.SuspendLayout();
			workspacePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)picture).BeginInit();
			SuspendLayout();
			// 
			// toolsPanel
			// 
			toolsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			toolsPanel.BackColor = SystemColors.ControlLightLight;
			toolsPanel.Controls.Add(zoomLabel);
			toolsPanel.Controls.Add(colorGroupBox);
			toolsPanel.Controls.Add(penWidthGroupBox);
			toolsPanel.Location = new Point(578, 32);
			toolsPanel.Margin = new Padding(3, 4, 3, 4);
			toolsPanel.Name = "toolsPanel";
			toolsPanel.Padding = new Padding(9, 8, 7, 8);
			toolsPanel.Size = new Size(149, 423);
			toolsPanel.TabIndex = 3;
			// 
			// zoomLabel
			// 
			zoomLabel.AutoSize = true;
			zoomLabel.Dock = DockStyle.Bottom;
			zoomLabel.ForeColor = SystemColors.GrayText;
			zoomLabel.Location = new Point(9, 395);
			zoomLabel.Margin = new Padding(0);
			zoomLabel.Name = "zoomLabel";
			zoomLabel.Size = new Size(89, 20);
			zoomLabel.TabIndex = 5;
			zoomLabel.Text = "Zoom 100%";
			zoomLabel.TextAlign = ContentAlignment.BottomLeft;
			// 
			// colorGroupBox
			// 
			colorGroupBox.Controls.Add(selectedColorPanel);
			colorGroupBox.Dock = DockStyle.Top;
			colorGroupBox.Location = new Point(9, 81);
			colorGroupBox.Margin = new Padding(3, 4, 3, 4);
			colorGroupBox.Name = "colorGroupBox";
			colorGroupBox.Padding = new Padding(6, 4, 6, 7);
			colorGroupBox.Size = new Size(133, 64);
			colorGroupBox.TabIndex = 4;
			colorGroupBox.TabStop = false;
			colorGroupBox.Text = "Color";
			// 
			// selectedColorPanel
			// 
			selectedColorPanel.BackColor = Color.Red;
			selectedColorPanel.BorderStyle = BorderStyle.FixedSingle;
			selectedColorPanel.Cursor = Cursors.Hand;
			selectedColorPanel.Dock = DockStyle.Fill;
			selectedColorPanel.Location = new Point(6, 24);
			selectedColorPanel.Margin = new Padding(3, 4, 3, 4);
			selectedColorPanel.Name = "selectedColorPanel";
			selectedColorPanel.Size = new Size(121, 33);
			selectedColorPanel.TabIndex = 0;
			selectedColorPanel.Click += selectedColorPanel_Click;
			// 
			// penWidthGroupBox
			// 
			penWidthGroupBox.Controls.Add(penWidthTrackBar);
			penWidthGroupBox.Dock = DockStyle.Top;
			penWidthGroupBox.Location = new Point(9, 8);
			penWidthGroupBox.Margin = new Padding(3, 4, 3, 4);
			penWidthGroupBox.Name = "penWidthGroupBox";
			penWidthGroupBox.Padding = new Padding(3, 4, 3, 4);
			penWidthGroupBox.Size = new Size(133, 73);
			penWidthGroupBox.TabIndex = 3;
			penWidthGroupBox.TabStop = false;
			penWidthGroupBox.Text = "Pen width";
			// 
			// penWidthTrackBar
			// 
			penWidthTrackBar.Cursor = Cursors.Hand;
			penWidthTrackBar.Dock = DockStyle.Fill;
			penWidthTrackBar.LargeChange = 2;
			penWidthTrackBar.Location = new Point(3, 24);
			penWidthTrackBar.Margin = new Padding(3, 4, 3, 4);
			penWidthTrackBar.Minimum = 1;
			penWidthTrackBar.Name = "penWidthTrackBar";
			penWidthTrackBar.Size = new Size(127, 45);
			penWidthTrackBar.TabIndex = 0;
			penWidthTrackBar.Value = 1;
			penWidthTrackBar.ValueChanged += penWidthTrackBar_ValueChanged;
			// 
			// menuStrip
			// 
			menuStrip.BackColor = SystemColors.ControlLightLight;
			menuStrip.ImageScalingSize = new Size(20, 20);
			menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, viewToolStripMenuItem });
			menuStrip.Location = new Point(0, 0);
			menuStrip.Name = "menuStrip";
			menuStrip.Padding = new Padding(7, 3, 0, 3);
			menuStrip.Size = new Size(727, 30);
			menuStrip.TabIndex = 0;
			menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, newToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new Size(46, 24);
			fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			openToolStripMenuItem.Name = "openToolStripMenuItem";
			openToolStripMenuItem.Size = new Size(143, 26);
			openToolStripMenuItem.Text = "Open";
			openToolStripMenuItem.Click += openToolStripMenuItem_Click;
			// 
			// newToolStripMenuItem
			// 
			newToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { withParamsToolStripMenuItem });
			newToolStripMenuItem.Name = "newToolStripMenuItem";
			newToolStripMenuItem.Size = new Size(143, 26);
			newToolStripMenuItem.Text = "New";
			newToolStripMenuItem.Click += newToolStripMenuItem_Click;
			// 
			// withParamsToolStripMenuItem
			// 
			withParamsToolStripMenuItem.Name = "withParamsToolStripMenuItem";
			withParamsToolStripMenuItem.Size = new Size(176, 26);
			withParamsToolStripMenuItem.Text = "With params";
			withParamsToolStripMenuItem.Visible = false;
			withParamsToolStripMenuItem.Click += withParamsToolStripMenuItem_Click;
			// 
			// saveToolStripMenuItem
			// 
			saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			saveToolStripMenuItem.Size = new Size(143, 26);
			saveToolStripMenuItem.Text = "Save";
			saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
			// 
			// saveAsToolStripMenuItem
			// 
			saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			saveAsToolStripMenuItem.Size = new Size(143, 26);
			saveAsToolStripMenuItem.Text = "Save As";
			saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
			// 
			// editToolStripMenuItem
			// 
			editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { effectsToolStripMenuItem });
			editToolStripMenuItem.Name = "editToolStripMenuItem";
			editToolStripMenuItem.Size = new Size(49, 24);
			editToolStripMenuItem.Text = "Edit";
			// 
			// effectsToolStripMenuItem
			// 
			effectsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { turnToGrayEffectToolStripMenuItem });
			effectsToolStripMenuItem.Name = "effectsToolStripMenuItem";
			effectsToolStripMenuItem.Size = new Size(136, 26);
			effectsToolStripMenuItem.Text = "Effects";
			// 
			// turnToGrayEffectToolStripMenuItem
			// 
			turnToGrayEffectToolStripMenuItem.Name = "turnToGrayEffectToolStripMenuItem";
			turnToGrayEffectToolStripMenuItem.Size = new Size(172, 26);
			turnToGrayEffectToolStripMenuItem.Text = "Turn to gray";
			turnToGrayEffectToolStripMenuItem.Click += turnToGrayEffectToolStripMenuItem_Click;
			// 
			// viewToolStripMenuItem
			// 
			viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { centerCameraToolStripMenuItem, resetZoomToolStripMenuItem });
			viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			viewToolStripMenuItem.Size = new Size(55, 24);
			viewToolStripMenuItem.Text = "View";
			// 
			// centerCameraToolStripMenuItem
			// 
			centerCameraToolStripMenuItem.Name = "centerCameraToolStripMenuItem";
			centerCameraToolStripMenuItem.Size = new Size(224, 26);
			centerCameraToolStripMenuItem.Text = "Center camera";
			centerCameraToolStripMenuItem.Click += centerCameraToolStripMenuItem_Click;
			// 
			// resetZoomToolStripMenuItem
			// 
			resetZoomToolStripMenuItem.Name = "resetZoomToolStripMenuItem";
			resetZoomToolStripMenuItem.Size = new Size(224, 26);
			resetZoomToolStripMenuItem.Text = "Reset zoom";
			resetZoomToolStripMenuItem.Click += resetZoomToolStripMenuItem_Click;
			// 
			// workspacePanel
			// 
			workspacePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			workspacePanel.BackColor = SystemColors.Control;
			workspacePanel.BorderStyle = BorderStyle.FixedSingle;
			workspacePanel.Controls.Add(picture);
			workspacePanel.Location = new Point(-1, 32);
			workspacePanel.Margin = new Padding(0);
			workspacePanel.Name = "workspacePanel";
			workspacePanel.Size = new Size(579, 423);
			workspacePanel.TabIndex = 2;
			workspacePanel.MouseDown += workspacePanel_MouseDown;
			workspacePanel.MouseMove += workspacePanel_MouseMove;
			workspacePanel.MouseUp += workspacePanel_MouseUp;
			// 
			// picture
			// 
			picture.Anchor = AnchorStyles.None;
			picture.Enabled = false;
			picture.Location = new Point(144, 95);
			picture.Margin = new Padding(3, 4, 3, 4);
			picture.Name = "picture";
			picture.Size = new Size(282, 224);
			picture.TabIndex = 1;
			picture.TabStop = false;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(727, 455);
			Controls.Add(toolsPanel);
			Controls.Add(workspacePanel);
			Controls.Add(menuStrip);
			KeyPreview = true;
			MainMenuStrip = menuStrip;
			Margin = new Padding(3, 4, 3, 4);
			MinimumSize = new Size(253, 240);
			Name = "MainForm";
			RightToLeft = RightToLeft.No;
			Text = "Image Editor";
			KeyDown += MainForm_KeyDown;
			Resize += MainForm_Resize;
			toolsPanel.ResumeLayout(false);
			toolsPanel.PerformLayout();
			colorGroupBox.ResumeLayout(false);
			penWidthGroupBox.ResumeLayout(false);
			penWidthGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)penWidthTrackBar).EndInit();
			menuStrip.ResumeLayout(false);
			menuStrip.PerformLayout();
			workspacePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)picture).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private PictureBox picture;
		private Panel workspacePanel;
		private TrackBar penWidthTrackBar;
		private Panel selectedColorPanel;
		private Label zoomLabel;
		private ToolStripMenuItem resetZoomToolStripMenuItem;
	}
}
