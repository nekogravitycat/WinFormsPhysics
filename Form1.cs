using Timer = System.Windows.Forms.Timer;

namespace WinFormsPhysics {
  public partial class Form1 : Form {
    readonly Timer UpdateTimer = new();
    readonly PhysicsEngine Engine = new();
    private int ObjectCount = 0;

    public Form1() {
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
    public bool ObjectCollision = true;
    public double TimeLapse = 1;
    public double LengthFactor = 1;
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
      for (int i = 0; i < Objects.Count; i++) {
        netForces[i] = new(0, 0);
      }

      // Compute gravitational forces
      for (int i = 0; i < Objects.Count; i++) {
        // From space
        if (GravityAcc.X != 0 || GravityAcc.Y != 0) {
          double gfX = Objects[i].Mass * GravityAcc.X;
          double gfY = Objects[i].Mass * GravityAcc.Y;
          netForces[i] += new Vector(gfX, gfY);
        }

        // From other objects
        if (GravityConst != 0) {
          for (int j = i + 1; j < Objects.Count; j++) {
            Object a = Objects[i], b = Objects[j];
            Vector posA = a.Position, posB = b.Position;

            double distance = Distance(posA, posB) * LengthFactor;
            if (distance == 0) continue;

            double f = (GravityConst * a.Mass * b.Mass) / (distance * distance);
            double deltaX = posB.X - posA.X;
            double deltaY = posB.Y - posA.Y;

            Vector force = new(f * deltaX / distance, f * deltaY / distance);

            netForces[i] += force;
            netForces[j] -= force;
          }
        }
      }

      // Apply force to objects
      for (int i = 0; i < Objects.Count; i++) {
        Object obj = Objects[i];
        Vector acceleration = netForces[i] / obj.Mass;
        obj.Velocity += acceleration * TimeLapse;
      }

      // Compute collisions
      for (int i = 0; i < Objects.Count; i++) {
        // With borders
        if (ScreenBounds) {
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

        // With other objects
        if (ObjectCollision) {
          for (int j = i + 1; j < Objects.Count; j++) {
            Object a = Objects[i], b = Objects[j];
            ObjectForm formA = a.BodyForm, formB = b.BodyForm;
            Rectangle overlap = Rectangle.Intersect(formA.Bounds, formB.Bounds);

            if (overlap == Rectangle.Empty) continue;

            bool horizontalCollision = overlap.Height > overlap.Width;

            if (formB.FixedPos) {
              if (horizontalCollision) {
                a.Velocity.X *= -1;
                a.Velocity *= 0.9;
              } else {
                a.Velocity.Y *= -1;
                a.Velocity *= 0.9;
              }
              break;
            }

            if (formA.FixedPos) {
              if (horizontalCollision) {
                b.Velocity.X *= -1;
                b.Velocity *= 0.9;
              } else {
                b.Velocity.Y *= -1;
                b.Velocity *= 0.9;
              }
              break;
            }

            double massSum = a.Mass + b.Mass;
            Vector newVA = (a.Velocity * (a.Mass - b.Mass) + b.Velocity * 2 * b.Mass) / massSum;
            Vector newVB = (a.Velocity * 2 * a.Mass + b.Velocity * (b.Mass - a.Mass)) / massSum;

            if (horizontalCollision) {
              a.Velocity.X = newVA.X * 0.8;
              b.Velocity.X = newVB.X * 0.8;
            } else {
              a.Velocity.Y = newVA.Y * 0.8;
              b.Velocity.Y = newVB.Y * 0.8;
            }
          }
        }
      }

      // Apply velocity to objects
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
}
