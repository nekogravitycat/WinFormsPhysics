namespace WinFormsPhysics {
  partial class ObjectForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      massText = new TextBox();
      label1 = new Label();
      fixPos = new CheckBox();
      SuspendLayout();
      // 
      // massText
      // 
      massText.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      massText.Location = new Point(65, 13);
      massText.Margin = new Padding(4);
      massText.Name = "massText";
      massText.Size = new Size(81, 29);
      massText.TabIndex = 0;
      massText.Text = "100";
      massText.TextChanged += massText_TextChanged;
      // 
      // label1
      // 
      label1.Location = new Point(12, 16);
      label1.Name = "label1";
      label1.Size = new Size(46, 26);
      label1.TabIndex = 1;
      label1.Text = "Mass";
      // 
      // fixPos
      // 
      fixPos.Location = new Point(12, 49);
      fixPos.Name = "fixPos";
      fixPos.Size = new Size(124, 25);
      fixPos.TabIndex = 2;
      fixPos.Text = "Fixed Position";
      fixPos.UseVisualStyleBackColor = true;
      fixPos.CheckedChanged += fixPos_CheckedChanged;
      // 
      // ObjectForm
      // 
      AutoScaleDimensions = new SizeF(9F, 21F);
      AutoScaleMode = AutoScaleMode.Font;
      AutoSize = true;
      ClientSize = new Size(161, 85);
      Controls.Add(label1);
      Controls.Add(fixPos);
      Controls.Add(massText);
      Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
      Margin = new Padding(4);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "ObjectForm";
      ShowIcon = false;
      Text = "ObjectForm";
      ResizeBegin += ObjectForm_ResizeBegin;
      ResizeEnd += ObjectForm_ResizeEnd;
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private TextBox massText;
    private Label label1;
    private CheckBox fixPos;
  }
}