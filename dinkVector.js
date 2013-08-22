(function () {
	if (!window.DinkVector) {

		function getAngle(xz, yz) {
			return Math.atan2(yz, xz) * 180 / Math.PI;
		}

		function getLength(xz, yz) {
			return Math.sqrt(xz * xz + yz * yz);
		}

		function getCoords(angle, length) {
			var rads = angle * Math.PI / 180;
			return {
				h: Math.cos(rads) * length,
				v: Math.sin(rads) * length
			};
		}

		function getAngleStep(h, v) {
			return 45 / getLength(h, v);
		}

		window.DinkVector = function(x, y, z) {

			if (typeof (x) === 'number') {
				// construct x,y,z
				this.x = x;
				this.y = y;
				this.z = z;
			} else if (!x) {
				// construct no params
				this.x = 0;
				this.y = 0;
				this.z = 0;
			} else {
				// construct DinkVector
				this.x = x.x;
				this.y = x.y;
				this.z = x.z;
			}

			this.add = function (dinkVector) {
				return new DinkVector(this.x + dinkVector.x, this.y + dinkVector.y, this.z + dinkVector.z);
			};

			this.subtract = function (dinkVector) {
				return new DinkVector(this.x - dinkVector.x, this.y - dinkVector.y, this.z - dinkVector.z);
			};

			this.multiply = function (dinkVector) {
				return new DinkVector(this.x * dinkVector.x, this.y * dinkVector.y, this.z * dinkVector.z);
			};

			this.divide = function (dinkVector) {
				return new DinkVector(this.x / dinkVector.x, this.y / dinkVector.y, this.z / dinkVector.z);
			};

			this.modulo = function (dinkVector) {
				return new DinkVector(this.x + dinkVector.x, this.y + dinkVector.y, this.z + dinkVector.z);
			};

			this.negate = function () {
				this.x = -this.x;
				this.y = -this.y;
				this.z = -this.z;
			};

			this.normalize = function () {
				var absX = Math.abs(this.x);
				var absY = Math.abs(this.y);
				var absZ = Math.abs(this.z);
				var normalizer = Math.max(absX, Math.max(absY, absZ));
				this.x /= normalizer;
				this.y /= normalizer;
				this.z /= normalizer;
				var scalar = 1 / this.getLength();
				this.x *= scalar;
				this.y *= scalar;
				this.z *= scalar;
			};

			this.getLength = function () {
				return this.getLengthFrom(0, 0, 0);
			};

			this.getLengthFrom = function (lx, ly, lz) {
				var ox = this.x - lx;
				var oy = this.y - ly;
				var oz = this.z - lz;
				return Math.sqrt(ox * ox + oy * oy + oz * oz);
			};

			this.setLength = function (length) {
				this.setLengthFrom(0, 0, 0, length);
			};

			this.setLengthFrom = function (lx, ly, lz, length) {
				var multiplier = length / this.getLengthFrom(lx, ly, lz);
				this.x = (this.x - lx) * multiplier;
				this.y = (this.y - ly) * multiplier;
				this.z = (this.z - lz) * multiplier;
			};

			this.getZAngle = function () {
				return this.getZAngleFrom(0, 0);
			};

			this.getZAngleFrom = function (ax, ay) {
				return getAngle(this.x - ax, this.y - ay);
			};

			this.setZAngle = function (angle) {
				this.setZAngleFrom(0, 0, angle);
			};

			this.setZAngleFrom = function (ax, ay, angle) {
				var length = getLength(this.x - ax, this.y - ay);
				var coords = getCoords(angle, length);
				this.x = coords.h;
				this.y = coords.v;
			};


			this.getXAngle = function () {
				return this.getXAngleFrom(0, 0);
			};

			this.getXAngleFrom = function (az, ay) {
				return getAngle(this.z - az, this.y - ay);
			};

			this.setXAngle = function (angle) {
				this.setXAngleFrom(0, 0, angle);
			};

			this.setXAngleFrom = function (az, ay, angle) {
				var length = getLength(this.z - az, this.y - ay);
				var coords = getCoords(angle, length);
				this.z = coords.h;
				this.y = coords.v;
			};


			this.getYAngle = function () {
				return this.getYAngleFrom(0, 0);
			};

			this.getYAngleFrom = function (ax, az) {
				return getAngle(this.x - ax, this.z - az);
			};

			this.setYAngle = function (angle) {
				this.setYAngleFrom(0, 0, angle);
			};

			this.setYAngleFrom = function (ax, az, angle) {
				var length = getLength(this.x - ax, this.y - az);
				var coords = getCoords(angle, length);
				this.x = coords.h;
				this.z = coords.v;
			};

			this.getXAngleStep = function () {
				return getAngleStep(this.z, this.y);
			};

			this.getYAngleStep = function () {
				return getAngleStep(this.x, this.z);
			};

			this.getZAngleStep = function () {
				return getAngleStep(this.x, this.y);
			};
		}

		DinkVector.FromX = function (xAngle, length) {
			var coords = getCoords(xAngle, length);
			return new DinkVector(0, coords.v, coords.h);
		};

		DinkVector.FromY = function (yAngle, length) {
			var coords = getCoords(yAngle, length);
			return new DinkVector(coords.h, 0, coords.v);
		};

		DinkVector.FromZ = function (zAngle, length) {
			var coords = getCoords(zAngle, length);
			return new DinkVector(coords.h, coords.v, 0);
		};

		DinkVector.FromZX = function (zAngle, xAngle, length) {
			var zcoords = getCoords(zAngle, 1000);
			var vector = new DinkVector(zcoords.h, zcoords.v, 0);
			vector.setXAngle(xAngle);
			vector.x *= vector.y / zcoords.v;
			vector.setLength(length);
			return vector;
		};

		DinkVector.FromZY = function (zAngle, yAngle, length) {
			var zcoords = getCoords(zAngle, 1000);
			var vector = new DinkVector(zcoords.h, zcoords.v, 0);
			vector.setYAngle(yAngle);
			vector.y *= vector.x / zcoords.h;
			vector.setLength(length);
			return vector;
		};

		DinkVector.FromXY = function (xAngle, yAngle, length) {
			var xcoords = getCoords(xAngle, 1000);
			var vector = new DinkVector(0, xcoords.v, xcoords.h);
			var ylen = getLength(vector.z, vector.x);
			var ycoords = getCoords(yAngle, ylen);
			vector.x = ycoords.h;
			vector.z = ycoords.v;
			vector.y *= vector.z / xcoords.h;
			vector.setLength(length);
			return vector;
		};
	}
})();
