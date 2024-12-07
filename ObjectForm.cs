using Timer = System.Windows.Forms.Timer;

namespace WinFormsPhysics {
  public partial class ObjectForm : Form {
    public double Mass = 100;
    public bool FixedPos = false;
    public bool BeingDrag = false;
    public Vector DragVelocity = new(0, 0);

    private readonly Timer DragTimer = new();
    private Point DragStartPos = Point.Empty;
    

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
      DragVelocity = new Vector(
        Cursor.Position.X - DragStartPos.X,
        Cursor.Position.Y - DragStartPos.Y
      );
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
