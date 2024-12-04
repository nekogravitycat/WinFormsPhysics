using Timer = System.Windows.Forms.Timer;

namespace WinFormsPhysics {
  public partial class ObjectForm : Form {
    readonly Timer DragTimer = new();
    public double Mass = 100;
    public bool FixedPos = false;
    public bool BeingDrag = false;
    private Point DragStartPos = Point.Empty;
    public Point DragVelocity = Point.Empty;

    public ObjectForm() {
      InitializeComponent();
      _ = double.TryParse(massText.Text, out Mass);
      DragTimer.Interval = 200;
      DragTimer.Tick += DragTimerUpdate;
    }

    private void fixPos_CheckedChanged(object sender, EventArgs e) {
      FixedPos = fixPos.Checked;
    }

    private void ObjectForm_ResizeBegin(object sender, EventArgs e) {
      DragTimer.Start();
      BeingDrag = true;
    }

    private void ObjectForm_ResizeEnd(object sender, EventArgs e) {
      DragVelocity = new(Cursor.Position.X - DragStartPos.X, Cursor.Position.Y - DragStartPos.Y);
      BeingDrag = false;
    }

    private void DragTimerUpdate(object? sender, EventArgs e) {
      DragStartPos = Cursor.Position;
    }

    private void massText_TextChanged(object sender, EventArgs e) {
      if (double.TryParse(massText.Text, out double newMass)) {
        Mass = newMass;
      } else {
        massText.Text = Mass.ToString();
      }
    }
  }
}
