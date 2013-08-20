using System;

namespace System.Media
{
    public class DinkVector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public DinkVector() {}

        public DinkVector(double x, double y, double z) { X = x; Y = y; Z = z; }

        public DinkVector(DinkVector dinkVector) { X = dinkVector.X; Y = dinkVector.Y; Z = dinkVector.Z; }

        public static DinkVector FromX(double xAngle, double length)
        {
            var xcoords = GetCoords(xAngle, length);
            return new DinkVector(0, xcoords.Item2, xcoords.Item1);
        }

        public static DinkVector FromY(double yAngle, double length)
        {
            var ycoords = GetCoords(yAngle, length);
            return new DinkVector(ycoords.Item2, 0, ycoords.Item1);
        }

        public static DinkVector FromZ(double zAngle, double length)
        {
            var zcoords = GetCoords(zAngle, length);
            return new DinkVector(zcoords.Item1, zcoords.Item2, 0);
        }

        public static DinkVector FromZX(double zAngle, double xAngle, double length)
        {
            var zcoords = GetCoords(zAngle, 1000);
            var vector = new DinkVector(zcoords.Item1, zcoords.Item2, 0) {XAngle = xAngle};
            vector.X *= vector.Y / zcoords.Item2;
            vector.Length = length;
            return vector;
        }

        public static DinkVector FromZY(double zAngle, double yAngle, double length)
        {
            var zcoords = GetCoords(zAngle, 1000);
            var vector = new DinkVector(zcoords.Item1, zcoords.Item2, 0) {YAngle = yAngle};
            vector.Y *= vector.X / zcoords.Item1;
            vector.Length = length;
            return vector;
        }

        public static DinkVector FromXY(double xAngle, double yAngle, double length)
        {
            var xcoords = GetCoords(xAngle, 1000);
            var vector = new DinkVector(0, xcoords.Item2, xcoords.Item1);
            var ylen = GetLength(vector.Z, vector.X);
            var ycoords = GetCoords(yAngle, ylen);
            vector.X = ycoords.Item1;
            vector.Z = ycoords.Item2;
            vector.Y *= vector.Z / xcoords.Item1;
            vector.Length = length;
            return vector;
        }

        public DinkVector Add(DinkVector dinkVector)
        {
            return Add(dinkVector.X, dinkVector.Y, dinkVector.Z);
        }

        public DinkVector Add(double x, double y, double z)
        {
            return new DinkVector(X + x, Y + y, Z + z);
        }

        public static DinkVector operator +(DinkVector left, DinkVector right)
        {
            return left.Add(right);
        }

        public DinkVector Subtract(DinkVector dinkVector)
        {
            return Subtract(dinkVector.X, dinkVector.Y, dinkVector.Z);
        }

        public DinkVector Subtract(double x, double y, double z)
        {
            return new DinkVector(X - x, Y - y, Z - z);
        }

        public static DinkVector operator -(DinkVector left, DinkVector right)
        {
            return left.Subtract(right);
        }

        public DinkVector Multiply(DinkVector dinkVector)
        {
            return Multiply(dinkVector.X, dinkVector.Y, dinkVector.Z);
        }

        public DinkVector Multiply(double x, double y, double z)
        {
            return new DinkVector(X * x, Y * y, Z * z);
        }

        public static DinkVector operator *(DinkVector left, DinkVector right)
        {
            return left.Multiply(right);
        }

        public DinkVector Divide(DinkVector dinkVector)
        {
            return Divide(dinkVector.X, dinkVector.Y, dinkVector.Z);
        }

        public DinkVector Divide(double x, double y, double z)
        {
            return new DinkVector(X / x, Y / y, Z / z);
        }

        public static DinkVector operator /(DinkVector left, DinkVector right)
        {
            return left.Divide(right);
        }

        public DinkVector Modulo(DinkVector dinkVector)
        {
            return Modulo(dinkVector.X, dinkVector.Y, dinkVector.Z);
        }

        public DinkVector Modulo(double x, double y, double z)
        {
            return new DinkVector(X % x, Y % y, Z % z);
        }

        public static DinkVector operator %(DinkVector left, DinkVector right)
        {
            return left.Modulo(right);
        }

        public void Negate()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        public void Normalize()
        {
            var absX = Math.Abs(X);
            var absY = Math.Abs(Y);
            var absZ = Math.Abs(Z);
            var normalizer = Math.Max(absX, Math.Max(absY, absZ));
            X /= normalizer;
            Y /= normalizer;
            Z /= normalizer;
            var scalar = 1d / Length;
            X *= scalar;
            Y *= scalar;
            Y *= scalar;
        }

        protected static double GetAngle(double xz, double yz)
        {
            return Math.Atan2(yz, xz) * 180d / Math.PI;
        }

        protected static double GetLength(double xz, double yz)
        {
            return Math.Sqrt(Math.Pow(xz, 2) + Math.Pow(yz, 2));
        }

        protected static Tuple<double, double> GetCoords(double angle, double length)
        {
            var rads = angle * Math.PI / 180d;
            return new Tuple<double, double>(
                Math.Cos(rads) * length,
                Math.Sin(rads) * length);
        }

        public double Length 
        {
            get { return GetLengthFrom(0, 0, 0); }
            set { SetLengthFrom(0, 0, 0, value); }
        }

        public double GetLengthFrom(DinkVector dinkVector)
        {
            return GetLengthFrom(dinkVector.X, dinkVector.Y, dinkVector.Z);
        }

        public double GetLengthFrom(double x, double y, double z)
        {
            return Math.Sqrt(Math.Pow(X - x, 2) + Math.Pow(Y - y, 2) + Math.Pow(Z - z, 2));
        }

        public void SetLengthFrom(DinkVector dinkVector, double length)
        {
            SetLengthFrom(dinkVector.X, dinkVector.Y, dinkVector.Z, length);
        }

        public void SetLengthFrom(double x, double y, double z, double length)
        {
            var multiplier = length / Length;
            X = (X - x) * multiplier;
            Y = (Y - y) * multiplier;
            Z = (Z - z) * multiplier;
        }

        public double ZAngle
        {
            get { return GetZAngleFrom(0, 0); }
            set { SetZAngleFrom(0, 0, value); }
        }

        public double GetZAngleFrom(DinkVector dinkVector)
        {
            return GetZAngleFrom(dinkVector.X, dinkVector.Y);
        }

        public double GetZAngleFrom(double x, double y)
        {
            return GetAngle(X - x, Y - y);
        }

        public void SetZAngleFrom(DinkVector dinkVector, double angle)
        {
            SetZAngleFrom(dinkVector.X, dinkVector.Y, angle);
        }

        public void SetZAngleFrom(double x, double y, double angle)
        {
            var length = GetLength(X - x, Y - y);
            var coords = GetCoords(angle, length);
            X = coords.Item1;
            Y = coords.Item2;
        }

        public double XAngle
        {
            get { return GetXAngleFrom(0, 0); }
            set { SetXAngleFrom(0, 0, value); }
        }

        public double GetXAngleFrom(DinkVector dinkVector)
        {
            return GetXAngleFrom(dinkVector.Z, dinkVector.Y);
        }

        public double GetXAngleFrom(double z, double y)
        {
            return GetAngle(Z - z, Y - y);
        }

        public void SetXAngleFrom(DinkVector dinkVector, double angle)
        {
            SetXAngleFrom(dinkVector.Z, dinkVector.Y, angle);
        }

        public void SetXAngleFrom(double z, double y, double angle)
        {
            var length = GetLength(Z - z, Y - y);
            var coords = GetCoords(angle, length);
            Z = coords.Item1;
            Y = coords.Item2;
        }

        public double YAngle
        {
            get { return GetYAngleFrom(0, 0); }
            set { SetYAngleFrom(0, 0, value); }
        }

        public double GetYAngleFrom(DinkVector dinkVector)
        {
            return GetYAngleFrom(dinkVector.X, dinkVector.Z);
        }

        public double GetYAngleFrom(double x, double z)
        {
            return GetAngle(X - x, Z - z);
        }

        public void SetYAngleFrom(DinkVector dinkVector, double angle)
        {
            SetYAngleFrom(dinkVector.X, dinkVector.Z, angle);
        }

        public void SetYAngleFrom(double x, double z, double angle)
        {
            var length = GetLength(X - x, Z - z);
            var coords = GetCoords(angle, length);
            X = coords.Item1;
            Z = coords.Item2;
        }

        protected static double GetAngleStep(double xz, double yz)
        {
            return 45 / GetLength(xz, yz);
        }

        public double ZAngleStep
        {
            get { return GetAngleStep(X, Y); }
        }

        public double XAngleStep
        {
            get { return GetAngleStep(Z, Y); }
        }

        public double YAngleStep
        {
            get { return GetAngleStep(X, Z); }
        }
    }
}
