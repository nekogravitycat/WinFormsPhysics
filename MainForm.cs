using Timer = System.Windows.Forms.Timer;

namespace WinFormsPhysics {
  public partial class MainForm : Form {
    private readonly Timer UpdateTimer = new();
    private readonly PhysicsEngine Engine = new();
    private int ObjectCount = 0;

    public MainForm() {
      InitializeComponent();
      UpdateTimer.Interval = 5;
      UpdateTimer.Tick += Engine.Update;
      Engine.GravityAcc.X = gravityX.Value / 10.0;
      Engine.GravityAcc.Y = gravityY.Value / 10.0;
      Engine.GravityConst = gravityConst.Value / 10.0;
      gravityXText.Text = Engine.GravityAcc.X.ToString();
      gravityYText.Text = Engine.GravityAcc.Y.ToString();
      gravityConstText.Text = Engine.GravityConst.ToString();
      lengthText.Text = Engine.LengthFactor.ToString();
    }

    private double Clip(double val, double min, double max) {
      if (val < min) return min;
      if (val > max) return max;
      return val;
    }

    private void PhysicsCheckbox_CheckedChanged(object sender, EventArgs e) {
      if (physicsCheckbox.Checked) {
        Engine.ClearVelocity();
        UpdateTimer.Start();
      } else {
        UpdateTimer.Stop();
      }
    }

    private void UpdateObjectCount() {
      ObjectCount = Engine.Objects.Count;
      Text = $"WinFormsPhysics ({ObjectCount})";
    }

    private void SummonButton_Click(object sender, EventArgs e) {
      var win = new ObjectForm();
      var obj = new Object(win);
      win.FormClosed += (sender, e) => {
        Engine.Objects.Remove(obj);
        UpdateObjectCount();
      };
      win.Show();
      Engine.Objects.Add(obj);
      UpdateObjectCount();
    }

    private void clearButton_Click(object sender, EventArgs e) {
      while (Engine.Objects.Count > 0) {
        Engine.Objects[0].BodyForm.Close();
      }
    }

    private void gravityX_Scroll(object sender, EventArgs e) {
      Engine.GravityAcc.X = gravityX.Value / 10.0;
      gravityXText.Text = Engine.GravityAcc.X.ToString();
    }

    private void gravityY_Scroll(object sender, EventArgs e) {
      Engine.GravityAcc.Y = gravityY.Value / 10.0;
      gravityYText.Text = Engine.GravityAcc.Y.ToString();
    }

    private void gravityConst_Scroll(object sender, EventArgs e) {
      Engine.GravityConst = gravityConst.Value / 10.0;
      gravityConstText.Text = Engine.GravityConst.ToString();
    }

    private void gravityXText_TextChanged(object sender, EventArgs e) {
      if (double.TryParse(gravityXText.Text, out var value)) {
        value = Clip(value, -10, 10);
        Engine.GravityAcc.X = value;
        gravityX.Value = (int)value * 10;
      }
      gravityXText.Text = Engine.GravityAcc.X.ToString();
    }

    private void gravityYText_TextChanged(object sender, EventArgs e) {
      if (double.TryParse(gravityYText.Text, out var value)) {
        value = Clip(value, -10, 10);
        Engine.GravityAcc.Y = value;
        gravityY.Value = (int)value * 10;
      }
      gravityYText.Text = Engine.GravityAcc.Y.ToString();
    }

    private void gravityConstText_TextChanged(object sender, EventArgs e) {
      if (double.TryParse(gravityConstText.Text, out var value)) {
        value = Clip(value, 0, 500);
        Engine.GravityConst = value;
        gravityConst.Value = (int)value * 10;
      }
      gravityConst.Text = Engine.GravityConst.ToString();
    }

    private void lengthText_TextChanged(object sender, EventArgs e) {
      if (double.TryParse(lengthText.Text, out var value)) {
        value = Clip(value, 0, 10);
        Engine.LengthFactor = value;
      }
      lengthText.Text = Engine.LengthFactor.ToString();
    }

    private void boundsCheckbox_CheckedChanged(object sender, EventArgs e) {
      Engine.ScreenBounds = boundsCheckbox.Checked;
    }

    private void collisionCheckbox_CheckedChanged(object sender, EventArgs e) {
      Engine.ObjectCollision = collisionCheckbox.Checked;
    }
  }
}
