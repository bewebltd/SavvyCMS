using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

// from Rich Newman
// http://richnewman.wordpress.com/about/code-listings-and-diagrams/hslcolor-class/
// modified by Mike Nelson
// example:
// 


namespace Beweb {
	public class Colour {
		// Private data members below are on scale 0-1
		// They are scaled for use externally based on scale
		private double hue = 1.0;
		private double saturation = 1.0;
		private double luminosity = 1.0;

		private const double scale = 240.0;

		public double Hue {
			get { return hue * scale; }
			set { hue = CheckRange(value / scale); }
		}

		public double Saturation {
			get { return saturation * scale; }
			set { saturation = CheckRange(value / scale); }
		}

		public double Luminosity {
			get { return luminosity * scale; }
			set { luminosity = CheckRange(value / scale); }
		}

		private double CheckRange(double value) {
			if (value < 0.0)
				value = 0.0;
			else if (value > 1.0)
				value = 1.0;
			return value;
		}

		public override string ToString() {
			return ColorTranslator.ToHtml(this);
		}

		public string ToHSLString() {
			return String.Format("H: {0:#0.##} S: {1:#0.##} L: {2:#0.##}", Hue, Saturation, Luminosity);
		}

		public string ToRGBString() {
			Color color = (Color)this;
			return String.Format("R: {0:#0.##} G: {1:#0.##} B: {2:#0.##}", color.R, color.G, color.B);
		}

		#region Casts to/from System.Drawing.Color

		public static implicit operator Color(Colour colour) {
			double r = 0, g = 0, b = 0;
			if (colour.luminosity != 0) {
				if (colour.saturation == 0)
					r = g = b = colour.luminosity;
				else {
					double temp2 = GetTemp2(colour);
					double temp1 = 2.0 * colour.luminosity - temp2;

					r = GetColorComponent(temp1, temp2, colour.hue + 1.0 / 3.0);
					g = GetColorComponent(temp1, temp2, colour.hue);
					b = GetColorComponent(temp1, temp2, colour.hue - 1.0 / 3.0);
				}
			}
			return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
		}

		private static double GetColorComponent(double temp1, double temp2, double temp3) {
			temp3 = MoveIntoRange(temp3);
			if (temp3 < 1.0 / 6.0)
				return temp1 + (temp2 - temp1) * 6.0 * temp3;
			else if (temp3 < 0.5)
				return temp2;
			else if (temp3 < 2.0 / 3.0)
				return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
			else
				return temp1;
		}

		private static double MoveIntoRange(double temp3) {
			if (temp3 < 0.0)
				temp3 += 1.0;
			else if (temp3 > 1.0)
				temp3 -= 1.0;
			return temp3;
		}

		private static double GetTemp2(Colour colour) {
			double temp2;
			if (colour.luminosity < 0.5) //<=??
				temp2 = colour.luminosity * (1.0 + colour.saturation);
			else
				temp2 = colour.luminosity + colour.saturation - (colour.luminosity * colour.saturation);
			return temp2;
		}

		public static implicit operator Colour(Color color) {
			Colour colour = new Colour();
			colour.hue = color.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360 
			colour.luminosity = color.GetBrightness();
			colour.saturation = color.GetSaturation();
			return colour;
		}

		#endregion

		public void SetRGB(int red, int green, int blue) {
			Colour colour = (Colour)Color.FromArgb(red, green, blue);
			this.hue = colour.hue;
			this.saturation = colour.saturation;
			this.luminosity = colour.luminosity;
		}

		public void SetHex(string rbgHexString) {
			Colour colour = (Colour)ColorTranslator.FromHtml(rbgHexString);
			this.hue = colour.hue;
			this.saturation = colour.saturation;
			this.luminosity = colour.luminosity;
		}

		public Colour() {
		}

		public Colour(string rbgHexString) {
			var color = ColorTranslator.FromHtml(rbgHexString);
			SetRGB(color.R, color.G, color.B);
		}

		public Colour(Color color) {
			SetRGB(color.R, color.G, color.B);
		}

		public Colour(int red, int green, int blue) {
			SetRGB(red, green, blue);
		}

		public Colour(double hue, double saturation, double luminosity) {
			this.Hue = hue;
			this.Saturation = saturation;
			this.Luminosity = luminosity;
		}

		/// <summary>
		/// Takes an HTML RBG Hex String eg #CC0000 and applies HSL adjustments.
		/// example: 
		/// Colour.Adjust("#CC0000", 1, 1, 2) -- make it twice as bright
		/// Colour.Adjust("#CC0000", 1, 0.5, 1) -- make it half as saturated (ie half as red)
		/// Colour.Adjust("#CC0000", 1, 0.5, 2) -- make it half as saturated and twice as bright
		/// </summary>
		public static string Adjust(string hexColour, double hueMultiplier, double saturationMultiplier, double luminosityMultiplier) {
			var col = new Colour(hexColour);
			col.Hue *= hueMultiplier;
			col.Saturation *= saturationMultiplier;
			col.Luminosity *= luminosityMultiplier;
			return col.ToString();
		}

	}


}

