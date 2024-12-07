namespace WinFormsPhysics
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
    private void InitializeComponent() {
      summonButton = new Button();
      physicsCheckbox = new CheckBox();
      gravityX = new TrackBar();
      gravityY = new TrackBar();
      label1 = new Label();
      label2 = new Label();
      gravityConst = new TrackBar();
      label3 = new Label();
      boundsCheckbox = new CheckBox();
      collisionCheckbox = new CheckBox();
      gravityXText = new TextBox();
      gravityYText = new TextBox();
      gravityConstText = new TextBox();
      label4 = new Label();
      lengthText = new TextBox();
      clearButton = new Button();
      ((System.ComponentModel.ISupportInitialize)gravityX).BeginInit();
      ((System.ComponentModel.ISupportInitialize)gravityY).BeginInit();
      ((System.ComponentModel.ISupportInitialize)gravityConst).BeginInit();
      SuspendLayout();
      // 
      // summonButton
      // 
      summonButton.Location = new Point(344, 222);
      summonButton.Margin = new Padding(4);
      summonButton.Name = "summonButton";
      summonButton.Size = new Size(116, 53);
      summonButton.TabIndex = 0;
      summonButton.Text = "New Object";
      summonButton.UseVisualStyleBackColor = true;
      summonButton.Click += SummonButton_Click;
      // 
      // physicsCheckbox
      // 
      physicsCheckbox.AutoSize = true;
      physicsCheckbox.Location = new Point(22, 24);
      physicsCheckbox.Name = "physicsCheckbox";
      physicsCheckbox.Size = new Size(130, 25);
      physicsCheckbox.TabIndex = 1;
      physicsCheckbox.Text = "Enable Physics";
      physicsCheckbox.UseVisualStyleBackColor = true;
      physicsCheckbox.CheckedChanged += PhysicsCheckbox_CheckedChanged;
      // 
      // gravityX
      // 
      gravityX.Location = new Point(101, 68);
      gravityX.Maximum = 100;
      gravityX.Minimum = -100;
      gravityX.Name = "gravityX";
      gravityX.Size = new Size(275, 45);
      gravityX.TabIndex = 2;
      gravityX.Scroll += gravityX_Scroll;
      // 
      // gravityY
      // 
      gravityY.Location = new Point(101, 119);
      gravityY.Maximum = 100;
      gravityY.Minimum = -100;
      gravityY.Name = "gravityY";
      gravityY.Size = new Size(275, 45);
      gravityY.TabIndex = 3;
      gravityY.Scroll += gravityY_Scroll;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(22, 71);
      label1.Name = "label1";
      label1.Size = new Size(73, 21);
      label1.TabIndex = 4;
      label1.Text = "Gravity X";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(22, 122);
      label2.Name = "label2";
      label2.Size = new Size(73, 21);
      label2.TabIndex = 5;
      label2.Text = "Gravity Y";
      // 
      // gravityConst
      // 
      gravityConst.Location = new Point(101, 170);
      gravityConst.Maximum = 5000;
      gravityConst.Name = "gravityConst";
      gravityConst.Size = new Size(275, 45);
      gravityConst.TabIndex = 6;
      gravityConst.Value = 400;
      gravityConst.Scroll += gravityConst_Scroll;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(22, 173);
      label3.Name = "label3";
      label3.Size = new Size(65, 21);
      label3.TabIndex = 7;
      label3.Text = "G Const";
      // 
      // boundsCheckbox
      // 
      boundsCheckbox.AutoSize = true;
      boundsCheckbox.Checked = true;
      boundsCheckbox.CheckState = CheckState.Checked;
      boundsCheckbox.Location = new Point(158, 24);
      boundsCheckbox.Name = "boundsCheckbox";
      boundsCheckbox.Size = new Size(132, 25);
      boundsCheckbox.TabIndex = 8;
      boundsCheckbox.Text = "Screen Bounds";
      boundsCheckbox.UseVisualStyleBackColor = true;
      boundsCheckbox.CheckedChanged += boundsCheckbox_CheckedChanged;
      // 
      // collisionCheckbox
      // 
      collisionCheckbox.AutoSize = true;
      collisionCheckbox.Checked = true;
      collisionCheckbox.CheckState = CheckState.Checked;
      collisionCheckbox.Location = new Point(296, 24);
      collisionCheckbox.Name = "collisionCheckbox";
      collisionCheckbox.Size = new Size(138, 25);
      collisionCheckbox.TabIndex = 9;
      collisionCheckbox.Text = "Object Collision";
      collisionCheckbox.UseVisualStyleBackColor = true;
      collisionCheckbox.CheckedChanged += collisionCheckbox_CheckedChanged;
      // 
      // gravityXText
      // 
      gravityXText.Location = new Point(382, 68);
      gravityXText.Name = "gravityXText";
      gravityXText.Size = new Size(78, 29);
      gravityXText.TabIndex = 10;
      gravityXText.TextAlign = HorizontalAlignment.Center;
      gravityXText.TextChanged += gravityXText_TextChanged;
      // 
      // gravityYText
      // 
      gravityYText.Location = new Point(382, 119);
      gravityYText.Name = "gravityYText";
      gravityYText.Size = new Size(78, 29);
      gravityYText.TabIndex = 11;
      gravityYText.TextAlign = HorizontalAlignment.Center;
      gravityYText.TextChanged += gravityYText_TextChanged;
      // 
      // gravityConstText
      // 
      gravityConstText.Location = new Point(382, 170);
      gravityConstText.Name = "gravityConstText";
      gravityConstText.Size = new Size(78, 29);
      gravityConstText.TabIndex = 12;
      gravityConstText.TextAlign = HorizontalAlignment.Center;
      gravityConstText.TextChanged += gravityConstText_TextChanged;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(22, 238);
      label4.Name = "label4";
      label4.Size = new Size(58, 21);
      label4.TabIndex = 13;
      label4.Text = "Length";
      // 
      // lengthText
      // 
      lengthText.Location = new Point(101, 235);
      lengthText.Name = "lengthText";
      lengthText.Size = new Size(78, 29);
      lengthText.TabIndex = 14;
      lengthText.TextAlign = HorizontalAlignment.Center;
      lengthText.TextChanged += lengthText_TextChanged;
      // 
      // clearButton
      // 
      clearButton.Location = new Point(220, 222);
      clearButton.Margin = new Padding(4);
      clearButton.Name = "clearButton";
      clearButton.Size = new Size(116, 53);
      clearButton.TabIndex = 15;
      clearButton.Text = "Clear Objects";
      clearButton.UseVisualStyleBackColor = true;
      clearButton.Click += clearButton_Click;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(9F, 21F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(480, 298);
      Controls.Add(clearButton);
      Controls.Add(lengthText);
      Controls.Add(label4);
      Controls.Add(gravityConstText);
      Controls.Add(gravityYText);
      Controls.Add(gravityXText);
      Controls.Add(collisionCheckbox);
      Controls.Add(boundsCheckbox);
      Controls.Add(label3);
      Controls.Add(gravityConst);
      Controls.Add(label2);
      Controls.Add(label1);
      Controls.Add(gravityY);
      Controls.Add(gravityX);
      Controls.Add(physicsCheckbox);
      Controls.Add(summonButton);
      Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      Margin = new Padding(4);
      Name = "Form1";
      ShowIcon = false;
      Text = "WinFormsPhysics";
      ((System.ComponentModel.ISupportInitialize)gravityX).EndInit();
      ((System.ComponentModel.ISupportInitialize)gravityY).EndInit();
      ((System.ComponentModel.ISupportInitialize)gravityConst).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Button summonButton;
    private CheckBox physicsCheckbox;
    private TrackBar gravityX;
    private TrackBar gravityY;
    private Label label1;
    private Label label2;
    private TrackBar gravityConst;
    private Label label3;
    private CheckBox boundsCheckbox;
    private CheckBox collisionCheckbox;
    private TextBox gravityXText;
    private TextBox gravityYText;
    private TextBox gravityConstText;
    private Label label4;
    private TextBox lengthText;
    private Button clearButton;
  }
}
