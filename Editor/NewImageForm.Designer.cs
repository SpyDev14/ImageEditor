namespace Editor
{
	partial class NewImageForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			GroupBox colorGroupBox;
			Button okButton;
			GroupBox sizeGroupBox;
			Panel panel2;
			Panel panel3;
			colorPanel = new Panel();
			heightInput = new TextBox();
			widthInput = new TextBox();
			colorGroupBox = new GroupBox();
			okButton = new Button();
			sizeGroupBox = new GroupBox();
			panel2 = new Panel();
			panel3 = new Panel();
			colorGroupBox.SuspendLayout();
			sizeGroupBox.SuspendLayout();
			panel2.SuspendLayout();
			panel3.SuspendLayout();
			SuspendLayout();
			// 
			// colorGroupBox
			// 
			colorGroupBox.Controls.Add(colorPanel);
			colorGroupBox.Dock = DockStyle.Left;
			colorGroupBox.Location = new Point(10, 10);
			colorGroupBox.Name = "colorGroupBox";
			colorGroupBox.Padding = new Padding(6);
			colorGroupBox.Size = new Size(70, 79);
			colorGroupBox.TabIndex = 0;
			colorGroupBox.TabStop = false;
			colorGroupBox.Text = "Color";
			// 
			// colorPanel
			// 
			colorPanel.BackColor = Color.Red;
			colorPanel.BorderStyle = BorderStyle.Fixed3D;
			colorPanel.Dock = DockStyle.Fill;
			colorPanel.Location = new Point(6, 22);
			colorPanel.Name = "colorPanel";
			colorPanel.Size = new Size(58, 51);
			colorPanel.TabIndex = 0;
			// 
			// okButton
			// 
			okButton.Dock = DockStyle.Right;
			okButton.Location = new Point(98, 0);
			okButton.Margin = new Padding(0);
			okButton.Name = "okButton";
			okButton.Size = new Size(78, 24);
			okButton.TabIndex = 1;
			okButton.Text = "Create";
			okButton.UseVisualStyleBackColor = true;
			okButton.Click += okButton_Click;
			// 
			// sizeGroupBox
			// 
			sizeGroupBox.Controls.Add(heightInput);
			sizeGroupBox.Controls.Add(widthInput);
			sizeGroupBox.Dock = DockStyle.Top;
			sizeGroupBox.Location = new Point(6, 0);
			sizeGroupBox.Name = "sizeGroupBox";
			sizeGroupBox.Padding = new Padding(6, 3, 6, 3);
			sizeGroupBox.Size = new Size(176, 49);
			sizeGroupBox.TabIndex = 2;
			sizeGroupBox.TabStop = false;
			sizeGroupBox.Text = "Size";
			// 
			// heightInput
			// 
			heightInput.Dock = DockStyle.Right;
			heightInput.Location = new Point(90, 19);
			heightInput.Name = "heightInput";
			heightInput.PlaceholderText = "Height";
			heightInput.Size = new Size(80, 23);
			heightInput.TabIndex = 1;
			// 
			// widthInput
			// 
			widthInput.Dock = DockStyle.Left;
			widthInput.Location = new Point(6, 19);
			widthInput.Name = "widthInput";
			widthInput.PlaceholderText = "Width";
			widthInput.Size = new Size(80, 23);
			widthInput.TabIndex = 0;
			// 
			// panel2
			// 
			panel2.Controls.Add(okButton);
			panel2.Dock = DockStyle.Bottom;
			panel2.Location = new Point(6, 55);
			panel2.Name = "panel2";
			panel2.Size = new Size(176, 24);
			panel2.TabIndex = 4;
			// 
			// panel3
			// 
			panel3.Controls.Add(sizeGroupBox);
			panel3.Controls.Add(panel2);
			panel3.Dock = DockStyle.Fill;
			panel3.Location = new Point(80, 10);
			panel3.Name = "panel3";
			panel3.Padding = new Padding(6, 0, 0, 0);
			panel3.Size = new Size(182, 79);
			panel3.TabIndex = 5;
			// 
			// NewImageForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(272, 99);
			Controls.Add(panel3);
			Controls.Add(colorGroupBox);
			Name = "NewImageForm";
			Padding = new Padding(10);
			StartPosition = FormStartPosition.CenterParent;
			Text = "New Image";
			colorGroupBox.ResumeLayout(false);
			sizeGroupBox.ResumeLayout(false);
			sizeGroupBox.PerformLayout();
			panel2.ResumeLayout(false);
			panel3.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private Panel colorPanel;
		private TextBox heightInput;
		private TextBox widthInput;
	}
}