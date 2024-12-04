using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Timer = System.Windows.Forms.Timer;

namespace WinFormsPhysics {
  public partial class Form1 : Form {
    readonly Timer UpdateTimer = new();
    readonly PhysicsEngine Engine = new();

    public Form1() {
      InitializeComponent();
      UpdateTimer.Interval = 5;
      UpdateTimer.Tick += Engine.Update;
      Engine.GravityAcc.X = gravityX.Value / 10.0;
      Engine.GravityAcc.Y = gravityY.Value / 10.0;
      Engine.GravityConst = gravityConst.Value / 10.0;
    }

    private void PhysicsCheckbox_CheckedChanged(object sender, EventArgs e) {
      if (physicsCheckbox.Checked) {
        UpdateTimer.Start();
      } else {
        UpdateTimer.Stop();
      }
    }

    private void SummonButton_Click(object sender, EventArgs e) {
      var win = new ObjectForm();
      var obj = new Object(win);
      win.FormClosed += (sender, e) => Engine.Objects.Remove(obj);
      win.Show();
      Engine.Objects.Add(obj);
    }

    private void gravityX_Scroll(object sender, EventArgs e) {
      Engine.GravityAcc.X = gravityX.Value / 10.0;
    }

    private void gravityY_Scroll(object sender, EventArgs e) {
      Engine.GravityAcc.Y = gravityY.Value / 10.0;
    }

    private void gravityConst_Scroll(object sender, EventArgs e) {
      Engine.GravityConst = gravityConst.Value / 10.0;
    }

    private void boundsCheckbox_CheckedChanged(object sender, EventArgs e) {
      Engine.ScreenBounds = boundsCheckbox.Checked;
    }
  }

  public class Vector {
    public double X;
    public double Y;

    public Vector() {
      X = 0;
      Y = 0;
    }

    public Vector(double x, double y) {
      X = x;
      Y = y;
    }

    public Vector(Point p) {
      X = p.X;
      Y = p.Y;
    }

    public static Vector operator +(Vector a, Vector b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector operator -(Vector a, Vector b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector operator *(Vector a, Vector b) => new(a.X * b.X, a.Y * b.Y);
    public static Vector operator *(Vector a, double c) => new(a.X * c, a.Y * c);
    public static Vector operator /(Vector a, Vector b) => new(a.X / b.X, a.Y / b.Y);
    public static Vector operator /(Vector a, double c) => new(a.X / c, a.Y / c);
    public static Vector operator ~(Vector a) => new(-a.X, -a.Y);

    public static Vector Clip(Vector a, Vector lim) => new(
      Math.Abs(a.X) < Math.Abs(lim.X) ? a.X : lim.X,
      Math.Abs(a.Y) < Math.Abs(lim.Y) ? a.Y : lim.Y
    );

    public static Vector Significant(Vector a) {
      Vector res = a;
      if (0.5 < Math.Abs(a.X) && Math.Abs(a.X) < 1) {
        res.X = res.X > 0 ? 1 : -1;
      }
      if (0.5 < Math.Abs(a.Y) && Math.Abs(a.Y) < 1) {
        res.Y = res.Y > 0 ? 1 : -1;
      }
      return res;
    }

    public Point ToPoint() => new((int)X, (int)Y);
  }

  public class Object(ObjectForm form) {
    public ObjectForm BodyForm = form;
    
    public double Mass => BodyForm.Mass;
    public Vector Velocity = new(0, 0);
    public Vector Position => new(
      (BodyForm.Left + BodyForm.Right) / 2 * 0.5,
      (BodyForm.Top + BodyForm.Bottom) / 2 * 0.5
    );
    public Vector DragStartPos = new(0, 0);
  }

  public class PhysicsEngine {
    public bool ScreenBounds = true;
    public double TimeLapse = 1;
    public double GravityConst = 400;
    public Vector GravityAcc = new(0, 1);
    public List<Object> Objects = [];

    private static double Distance(Vector a, Vector b) {
      double deltaX = a.X - b.X;
      double deltaY = a.Y - b.Y;
      return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    public void ClearVelocity() {
      foreach (Object obj in Objects) {
        obj.Velocity = new(0, 0);
        obj.BodyForm.DragVelocity = Point.Empty;
      }
    }

    public void Update(object? sender, EventArgs e) {
      if (Objects.Count == 0) return;

      var netForces = new Vector[Objects.Count];
      for (int i = 0; i < netForces.Length; i++) {
        netForces[i] = new(0, 0);
      }

      // Compute gravitational force from space
      if (GravityAcc.X != 0 || GravityAcc.Y != 0) {
        for (int i = 0; i < Objects.Count; i++) {
          double gfX = Objects[i].Mass * GravityAcc.X;
          double gfY = Objects[i].Mass * GravityAcc.Y;
          netForces[i] += new Vector(gfX, gfY);
        }
      }
       
      // Compute gravitational force from other objects
      if (GravityConst != 0) {
        for (int i = 0; i < Objects.Count - 1; i++) {
          for (int j = i + 1; j < Objects.Count; j++) {
            Object a = Objects[i], b = Objects[j];
            Vector posA = a.Position, posB = b.Position;

            double distance = Distance(posA, posB);

            // Avoid division by zero
            if (distance == 0) continue;

            double f = (GravityConst * a.Mass * b.Mass) / (distance * distance);

            double deltaX = posB.X - posA.X;
            double deltaY = posB.Y - posA.Y;

            Vector force = new(
              f * deltaX / distance,
              f * deltaY / distance
            );

            netForces[i] += force;
            netForces[j] -= force; // Opposite direction
          }
        }
      }

      // Apply force to objects
      for (int i = 0; i < Objects.Count; i++) {
        Object obj = Objects[i];
        Vector acceleration = netForces[i] / obj.Mass;
        obj.Velocity += acceleration * TimeLapse;
      }

      // Compute collision with borders
      if (ScreenBounds) {
        for (int i = 0; i < Objects.Count; i++) {
          Object obj = Objects[i];
          Form form = obj.BodyForm;
          Vector v = obj.Velocity;
          var bounds = Screen.FromControl(obj.BodyForm).WorkingArea;
          if (form.Left <= bounds.Left || form.Right >= bounds.Right) {
            obj.Velocity = new Vector(-v.X, v.Y) * 0.8;
          }
          if (form.Top <= bounds.Top || form.Bottom >= bounds.Bottom) {
            obj.Velocity = new Vector(v.X, -v.Y) * 0.8;
          }
        }
      }

      // Compute collision with other objects
      for (int i = 0; i < Objects.Count - 1; i++) {
        for (int j = i + 1; j < Objects.Count; j++) {
          Object a = Objects[i], b = Objects[j];

          ObjectForm formA = a.BodyForm, formB = b.BodyForm;

          if (!formA.Bounds.IntersectsWith(formB.Bounds)) continue;

          if (b.BodyForm.FixedPos) {
            a.Velocity = ~a.Velocity * 0.9;
            break;
          }
          if (a.BodyForm.FixedPos) {
            b.Velocity = ~b.Velocity * 0.9;
            break;
          }

          double massSum = a.Mass + b.Mass;
          Vector newVA = (a.Velocity * (a.Mass - b.Mass) + b.Velocity * 2 * b.Mass) / massSum;
          Vector newVB = (a.Velocity * 2 * a.Mass + b.Velocity * (b.Mass - a.Mass)) / massSum;

          a.Velocity = newVA * 0.8;
          b.Velocity = newVB * 0.8;
        }
      }

      // Apply force to objects
      for (int i = 0; i < Objects.Count; i++) {
        Object obj = Objects[i];
        ObjectForm form = obj.BodyForm;

        if (form.FixedPos || form.BeingDrag) {
          obj.Velocity = new(0, 0);
          continue;
        }

        Point dragV = form.DragVelocity;
        if (!form.BeingDrag && dragV != Point.Empty) {
          obj.Velocity = new Vector(dragV) * 0.1;
          form.DragVelocity = Point.Empty;
          continue;
        }

        Vector oldPos = new(form.Location);
        Vector delta = obj.Velocity * TimeLapse;
        Vector newPos = oldPos + Vector.Significant(delta);

        // Hard clip screen bounds
        if (ScreenBounds) {
          var bounds = Screen.FromControl(obj.BodyForm).WorkingArea;
          newPos.X = newPos.X > bounds.Left ? newPos.X : bounds.Left;
          newPos.X = newPos.X < bounds.Right ? newPos.X : bounds.Right;
          newPos.Y = newPos.Y > bounds.Top ? newPos.Y : bounds.Top;
          newPos.Y = newPos.Y < bounds.Bottom ? newPos.Y : bounds.Bottom;
        }

        form.Location = newPos.ToPoint();
      }
    }
  }



  public class Bounce : Control {
    private class Vector2D {
      public double X { get; set; }
      public double Y { get; set; }

      public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y);
      public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
      public static Vector2D operator *(Vector2D a, Vector2D b) => new Vector2D(a.X * b.X, a.Y * b.Y);
      public static Vector2D operator /(Vector2D a, Vector2D b) => new Vector2D(a.X / b.X, a.Y / b.Y);
      public static Vector2D operator ~(Vector2D a) => new Vector2D(-a.X, -a.Y);

      public void clip(Vector2D bounds) {
        clip(bounds, ~bounds);
      }
      public void clip(Vector2D upper, Vector2D lower) {
        if (X > upper.X) X = upper.X;
        if (X < lower.X) X = lower.X;
        if (Y > upper.Y) Y = upper.Y;
        if (Y < lower.Y) Y = lower.Y;
      }

      public Point ToPoint() => new((int)X, (int)Y);

      public Vector2D(double x, double y) {
        X = x;
        Y = y;
      }
      public Vector2D(Point p) {
        X = p.X;
        Y = p.Y;
      }
    }

    Vector2D Gravity { get; set; } = new Vector2D(0, 1f);
    Vector2D Elasticity { get; set; } = new Vector2D(0.5f, 0.6f);
    Vector2D MaxVelocity { get; set; } = new Vector2D(100f, 100f);
    Vector2D Friction { get; set; } = new Vector2D(0.7f, 1f);

    Vector2D mouseDragStart = new Vector2D(0, 0);
    System.Windows.Forms.Timer updateTimer = new System.Windows.Forms.Timer();
    Form parent = null;
    Vector2D velocity = new Vector2D(0, 0);
    bool enabled = true;

    public Bounce() {
      ParentChanged += parentChanged;
    }

    private void parentChanged(object sender, EventArgs e) {
      if (parent != null)
        throw new Exception("Parent form cannot be changed!");

      parent = Parent as Form ?? throw new Exception("Control must be added to a form!");

      parent.ResizeBegin += dragStart;
      parent.ResizeEnd += dragEnd;

      updateTimer.Tick += update;
      updateTimer.Interval = 10;
      updateTimer.Start();
    }

    private void dragStart(object sender, EventArgs e) {
      mouseDragStart = new Vector2D(Cursor.Position);
      // Prevent location updates when user drags form
      enabled = false;
    }
    private void dragEnd(object sender, EventArgs e) {
      velocity = new Vector2D(Cursor.Position) - mouseDragStart;
      enabled = true;
    }

    private void update(object sender, EventArgs e) {
      if (Enabled) {
        if (enabled) {
          velocity += Gravity;
          velocity.clip(MaxVelocity);

          var screenBounds = Screen.GetWorkingArea(parent.Location);

          Vector2D nextLoc = new Vector2D(parent.Location) + velocity;

          if (nextLoc.X <= screenBounds.Left || nextLoc.X >= screenBounds.Right - parent.Size.Width)
            velocity.X *= -Elasticity.X;

          if (nextLoc.Y >= screenBounds.Bottom - parent.Size.Height || nextLoc.Y <= screenBounds.Top) {
            velocity.Y *= -Elasticity.Y;
            velocity *= Friction;
          }

          nextLoc.clip(new Vector2D(screenBounds.Right - parent.Size.Width, screenBounds.Bottom - parent.Size.Height), new Vector2D(screenBounds.Left, screenBounds.Top));

          parent.Location = nextLoc.ToPoint();
        } else {
          mouseDragStart = new Vector2D(Cursor.Position);
        }
      }
    }
  }
}
