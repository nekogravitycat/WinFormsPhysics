namespace WinFormsPhysics {
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
    public double MaxForce = 1000;
    public Vector GravityAcc = new(0, 1);
    public List<Object> Objects = [];

    private static double Distance(Vector a, Vector b) {
      double deltaX = a.X - b.X;
      double deltaY = a.Y - b.Y;
      return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    public void ClearVelocity() {
      foreach (Object obj in Objects) {
        obj.Velocity = new Vector(0, 0);
        obj.BodyForm.DragVelocity = new Vector(0, 0);
      }
    }

    public void Update(object? sender, EventArgs e) {
      if (Objects.Count == 0) return;

      Vector[] netForces = new Vector[Objects.Count];
      for (int i = 0; i < Objects.Count; i++) {
        netForces[i] = new Vector(0, 0);
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
        Vector force = netForces[i];
        if (MaxForce < 0) {
          force = Vector.Clip(force, new Vector(MaxForce, MaxForce));
        }
        Vector acceleration = force / obj.Mass;
        obj.Velocity += acceleration * TimeLapse;
      }

      // Compute collisions
      for (int i = 0; i < Objects.Count; i++) {
        // With borders
        if (ScreenBounds) {
          Object obj = Objects[i];
          Form form = obj.BodyForm;
          Vector v = obj.Velocity;
          Rectangle bounds = Screen.FromControl(obj.BodyForm).WorkingArea;
          double newX = v.X, newY = v.Y;
          if (form.Left <= bounds.Left) {
            newX = v.X > 0 ? v.X : -v.X;
            newX *= 0.8;
          }
          if (form.Right >= bounds.Right) {
            newX = v.X < 0 ? v.X : -v.X;
            newX *= 0.8;
          }
          if (form.Top <= bounds.Top) {
            newY = v.Y > 0 ? v.Y : -v.Y;
            newY *= 0.8;
          }
          if (form.Bottom >= bounds.Bottom) {
            newY = v.Y < 0 ? v.Y : -v.Y;
            newY *= 0.8;
          }
          obj.Velocity = new Vector(newX, newY);
        }

        // With other objects
        if (ObjectCollision) {
          for (int j = i + 1; j < Objects.Count; j++) {
            Object a = Objects[i], b = Objects[j];
            ObjectForm formA = a.BodyForm, formB = b.BodyForm;
            Rectangle overlap = Rectangle.Intersect(formA.Bounds, formB.Bounds);

            if (overlap == Rectangle.Empty) continue;

            bool horizontalCollision = overlap.Height > overlap.Width;

            if (formA.FixedPos || formB.FixedPos) {
              Object movingObj = formA.FixedPos ? b : a;
              if (horizontalCollision) {
                movingObj.Velocity.X *= -1;
              } else {
                movingObj.Velocity.Y *= -1;
              }
              movingObj.Velocity *= 0.9;
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
      foreach (Object obj in Objects) {
        ObjectForm form = obj.BodyForm;

        if (form.FixedPos || form.BeingDrag) {
          obj.Velocity = new Vector(0, 0);
          continue;
        }

        Vector dragV = form.DragVelocity;
        if (!form.BeingDrag && dragV.X != 0 && dragV.Y != 0) {
          obj.Velocity = dragV * 0.1;
          form.DragVelocity = new Vector(0, 0);
          continue;
        }

        Vector delta = obj.Velocity * TimeLapse;
        Vector newPos = new Vector(form.Location) + Vector.Significant(delta);

        // Hard clip screen bounds
        if (ScreenBounds) {
          Rectangle bounds = Screen.FromControl(obj.BodyForm).WorkingArea;
          newPos.X = form.Left + 50 > bounds.Left ? newPos.X : bounds.Left;
          newPos.X = form.Right - 50 < bounds.Right ? newPos.X : bounds.Right - form.Width;
          newPos.Y = form.Top + 50 > bounds.Top ? newPos.Y : bounds.Top;
          newPos.Y = form.Bottom - 50 < bounds.Bottom ? newPos.Y : bounds.Bottom - form.Height;
        }

        form.Location = newPos.ToPoint();
      }
    }
  }
}
