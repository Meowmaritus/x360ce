﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;

namespace x360ce.Engine.Data
{
	public partial class PadSetting
	{
		public Guid CleanAndGetCheckSum()
		{

			// Make sure to update checksums in database if you are changing this method.
			var list = new List<string>();
			AddValue(ref list, x => x.AxisToDPadDeadZone, "256");
			AddValue(ref list, x => x.AxisToDPadEnabled);
			AddValue(ref list, x => x.AxisToDPadOffset);
			AddValue(ref list, x => x.ButtonA);
			AddValue(ref list, x => x.ButtonB);
			AddValue(ref list, x => x.ButtonGuide);
			AddValue(ref list, x => x.ButtonBack);
			AddValue(ref list, x => x.ButtonStart);
			AddValue(ref list, x => x.ButtonX);
			AddValue(ref list, x => x.ButtonY);
			AddValue(ref list, x => x.DPad);
			AddValue(ref list, x => x.DPadDown);
			AddValue(ref list, x => x.DPadLeft);
			AddValue(ref list, x => x.DPadRight);
			AddValue(ref list, x => x.DPadUp);
			AddValue(ref list, x => x.ForceEnable);
			AddValue(ref list, x => x.ForceOverall, "100");
			AddValue(ref list, x => x.ForceSwapMotor);
			AddValue(ref list, x => x.ForceType);
			AddValue(ref list, x => x.GamePadType);
			AddValue(ref list, x => x.LeftMotorPeriod);
			AddValue(ref list, x => x.LeftShoulder);
			AddValue(ref list, x => x.LeftThumbAntiDeadZoneX);
			AddValue(ref list, x => x.LeftThumbAntiDeadZoneY);
			AddValue(ref list, x => x.LeftThumbAxisX);
			AddValue(ref list, x => x.LeftThumbAxisY);
			AddValue(ref list, x => x.LeftThumbButton);
			AddValue(ref list, x => x.LeftThumbDeadZoneX);
			AddValue(ref list, x => x.LeftThumbDeadZoneY);
			AddValue(ref list, x => x.LeftThumbDown);
			AddValue(ref list, x => x.LeftThumbLeft);
			AddValue(ref list, x => x.LeftThumbRight);
			AddValue(ref list, x => x.LeftThumbUp);
			AddValue(ref list, x => x.LeftTrigger);
			AddValue(ref list, x => x.LeftTriggerDeadZone);
			AddValue(ref list, x => x.PassThrough);
			AddValue(ref list, x => x.RightMotorPeriod);
			AddValue(ref list, x => x.RightShoulder);
			AddValue(ref list, x => x.RightThumbAntiDeadZoneX);
			AddValue(ref list, x => x.RightThumbAntiDeadZoneY);
			AddValue(ref list, x => x.RightThumbAxisX);
			AddValue(ref list, x => x.RightThumbAxisY);
			AddValue(ref list, x => x.RightThumbButton);
			AddValue(ref list, x => x.RightThumbDeadZoneX);
			AddValue(ref list, x => x.RightThumbDeadZoneY);
			AddValue(ref list, x => x.RightThumbDown);
			AddValue(ref list, x => x.RightThumbLeft);
			AddValue(ref list, x => x.RightThumbRight);
			AddValue(ref list, x => x.RightThumbUp);
			AddValue(ref list, x => x.RightTrigger);
			AddValue(ref list, x => x.RightTriggerDeadZone);
			// New
			AddValue(ref list, x => x.LeftThumbLinearX);
			AddValue(ref list, x => x.LeftThumbLinearY);
			AddValue(ref list, x => x.RightThumbLinearX);
			AddValue(ref list, x => x.RightThumbLinearY);
			AddValue(ref list, x => x.LeftMotorStrength, "100");
			AddValue(ref list, x => x.RightMotorStrength, "100");
			AddValue(ref list, x => x.LeftMotorDirection);
			AddValue(ref list, x => x.RightMotorDirection);
			// Axis to Button dead-zones.
			AddValue(ref list, x => x.ButtonADeadZone);
			AddValue(ref list, x => x.ButtonBDeadZone);
			AddValue(ref list, x => x.ButtonBackDeadZone);
			AddValue(ref list, x => x.ButtonStartDeadZone);
			AddValue(ref list, x => x.ButtonXDeadZone);
			AddValue(ref list, x => x.ButtonYDeadZone);
			AddValue(ref list, x => x.LeftThumbButtonDeadZone);
			AddValue(ref list, x => x.RightThumbButtonDeadZone);
			AddValue(ref list, x => x.LeftShoulderDeadZone);
			AddValue(ref list, x => x.RightShoulderDeadZone);
			AddValue(ref list, x => x.DPadDownDeadZone);
			AddValue(ref list, x => x.DPadLeftDeadZone);
			AddValue(ref list, x => x.DPadRightDeadZone);
			AddValue(ref list, x => x.DPadUpDeadZone);
			// If all values are empty or default then...
			if (list.Count == 0)
				return Guid.Empty;
			// Prepare list for checksum.
			var s = string.Join("\r\n", list.ToArray());
			var bytes = System.Text.Encoding.ASCII.GetBytes(s);
			var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			return new Guid(md5.ComputeHash(bytes));
		}

		void AddValue(ref List<string> list, Expression<Func<PadSetting, object>> setting, string defaultValue = "0")
		{
			var p = (PropertyInfo)((MemberExpression)setting.Body).Member;
			var value = (string)p.GetValue(this, null);
			// If value is not empty or default then...
			if (!isDefault(value, defaultValue))
			{
				list.Add(string.Format("{0}={1}", p.Name, value));
			}
			// If value is default but not empty then reset value.
			else if (value != "")
			{

				p.SetValue(this, "", null);
			}
		}

		#region Do not serialize default values

		public bool isDefault<T>(T value, T defaultValue = default(T))
		{
			// If value is default for the type then...
			if (Equals(value, default(T)))
				return true;
			// If value is default.
			if (Equals(value, defaultValue))
				return true;
			// If value is stirng and empty or set to "0" then...
			if (value is string && (Equals(value, "") || Equals(value, "0")))
				return true;
			return false;
		}

		public bool ShouldSerializePadSettingChecksum() { return !isDefault(PadSettingChecksum); }
		public bool ShouldSerializeAxisToDPadDeadZone() { return !isDefault(AxisToDPadDeadZone, "256"); }
		public bool ShouldSerializeAxisToDPadEnabled() { return !isDefault(AxisToDPadEnabled); }
		public bool ShouldSerializeAxisToDPadOffset() { return !isDefault(AxisToDPadOffset); }
		public bool ShouldSerializeButtonA() { return !isDefault(ButtonA); }
		public bool ShouldSerializeButtonB() { return !isDefault(ButtonB); }
		public bool ShouldSerializeButtonBack() { return !isDefault(ButtonBack); }
		public bool ShouldSerializeButtonGuide() { return !isDefault(ButtonGuide); }
		public bool ShouldSerializeButtonStart() { return !isDefault(ButtonStart); }
		public bool ShouldSerializeButtonX() { return !isDefault(ButtonX); }
		public bool ShouldSerializeButtonY() { return !isDefault(ButtonY); }
		public bool ShouldSerializeDPad() { return !isDefault(DPad); }
		public bool ShouldSerializeDPadDown() { return !isDefault(DPadDown); }
		public bool ShouldSerializeDPadLeft() { return !isDefault(DPadLeft); }
		public bool ShouldSerializeDPadRight() { return !isDefault(DPadRight); }
		public bool ShouldSerializeDPadUp() { return !isDefault(DPadUp); }
		public bool ShouldSerializeForceEnable() { return !isDefault(ForceEnable); }
		public bool ShouldSerializeForceOverall() { return !isDefault(ForceOverall, "100"); }
		public bool ShouldSerializeForceSwapMotor() { return !isDefault(ForceSwapMotor); }
		public bool ShouldSerializeForceType() { return !isDefault(ForceType); }
		public bool ShouldSerializeGamePadType() { return !isDefault(GamePadType); }
		public bool ShouldSerializeLeftMotorPeriod() { return !isDefault(LeftMotorPeriod); }
		public bool ShouldSerializeLeftShoulder() { return !isDefault(LeftShoulder); }
		public bool ShouldSerializeLeftThumbAntiDeadZoneX() { return !isDefault(LeftThumbAntiDeadZoneX); }
		public bool ShouldSerializeLeftThumbAntiDeadZoneY() { return !isDefault(LeftThumbAntiDeadZoneY); }
		public bool ShouldSerializeLeftThumbAxisX() { return !isDefault(LeftThumbAxisX); }
		public bool ShouldSerializeLeftThumbAxisY() { return !isDefault(LeftThumbAxisY); }
		public bool ShouldSerializeLeftThumbButton() { return !isDefault(LeftThumbButton); }
		public bool ShouldSerializeLeftThumbDeadZoneX() { return !isDefault(LeftThumbDeadZoneX); }
		public bool ShouldSerializeLeftThumbDeadZoneY() { return !isDefault(LeftThumbDeadZoneY); }
		public bool ShouldSerializeLeftThumbDown() { return !isDefault(LeftThumbDown); }
		public bool ShouldSerializeLeftThumbLeft() { return !isDefault(LeftThumbLeft); }
		public bool ShouldSerializeLeftThumbRight() { return !isDefault(LeftThumbRight); }
		public bool ShouldSerializeLeftThumbUp() { return !isDefault(LeftThumbUp); }
		public bool ShouldSerializeLeftTrigger() { return !isDefault(LeftTrigger); }
		public bool ShouldSerializeLeftTriggerDeadZone() { return !isDefault(LeftTriggerDeadZone); }
		public bool ShouldSerializePassThrough() { return !isDefault(PassThrough); }
		public bool ShouldSerializeRightMotorPeriod() { return !isDefault(RightMotorPeriod); }
		public bool ShouldSerializeRightShoulder() { return !isDefault(RightShoulder); }
		public bool ShouldSerializeRightThumbAntiDeadZoneX() { return !isDefault(RightThumbAntiDeadZoneX); }
		public bool ShouldSerializeRightThumbAntiDeadZoneY() { return !isDefault(RightThumbAntiDeadZoneY); }
		public bool ShouldSerializeRightThumbAxisX() { return !isDefault(RightThumbAxisX); }
		public bool ShouldSerializeRightThumbAxisY() { return !isDefault(RightThumbAxisY); }
		public bool ShouldSerializeRightThumbButton() { return !isDefault(RightThumbButton); }
		public bool ShouldSerializeRightThumbDeadZoneX() { return !isDefault(RightThumbDeadZoneX); }
		public bool ShouldSerializeRightThumbDeadZoneY() { return !isDefault(RightThumbDeadZoneY); }
		public bool ShouldSerializeRightThumbDown() { return !isDefault(RightThumbDown); }
		public bool ShouldSerializeRightThumbLeft() { return !isDefault(RightThumbLeft); }
		public bool ShouldSerializeRightThumbRight() { return !isDefault(RightThumbRight); }
		public bool ShouldSerializeRightThumbUp() { return !isDefault(RightThumbUp); }
		public bool ShouldSerializeRightTrigger() { return !isDefault(RightTrigger); }
		public bool ShouldSerializeRightTriggerDeadZone() { return !isDefault(RightTriggerDeadZone); }
		public bool ShouldSerializeLeftThumbLinearX() { return !isDefault(LeftThumbLinearX); }
		public bool ShouldSerializeLeftThumbLinearY() { return !isDefault(LeftThumbLinearY); }
		public bool ShouldSerializeRightThumbLinearX() { return !isDefault(RightThumbLinearX); }
		public bool ShouldSerializeRightThumbLinearY() { return !isDefault(RightThumbLinearY); }
		public bool ShouldSerializeLeftMotorStrength() { return !isDefault(LeftMotorStrength, "100"); }
		public bool ShouldSerializeRightMotorStrength() { return !isDefault(RightMotorStrength, "100"); }
		public bool ShouldSerializeLeftMotorDirection() { return !isDefault(LeftMotorDirection); }
		public bool ShouldSerializeRightMotorDirection() { return !isDefault(RightMotorDirection); }
		public bool ShouldSerializeButtonADeadZone() { return !isDefault(ButtonADeadZone); }
		public bool ShouldSerializeButtonBDeadZone() { return !isDefault(ButtonBDeadZone); }
		public bool ShouldSerializeButtonBackDeadZone() { return !isDefault(ButtonBackDeadZone); }
		public bool ShouldSerializeButtonStartDeadZone() { return !isDefault(ButtonStartDeadZone); }
		public bool ShouldSerializeButtonXDeadZone() { return !isDefault(ButtonXDeadZone); }
		public bool ShouldSerializeButtonYDeadZone() { return !isDefault(ButtonYDeadZone); }
		public bool ShouldSerializeLeftThumbButtonDeadZone() { return !isDefault(LeftThumbButtonDeadZone); }
		public bool ShouldSerializeRightThumbButtonDeadZone() { return !isDefault(RightThumbButtonDeadZone); }
		public bool ShouldSerializeLeftShoulderDeadZone() { return !isDefault(LeftShoulderDeadZone); }
		public bool ShouldSerializeRightShoulderDeadZone() { return !isDefault(RightShoulderDeadZone); }
		public bool ShouldSerializeDPadDownDeadZone() { return !isDefault(DPadDownDeadZone); }
		public bool ShouldSerializeDPadLeftDeadZone() { return !isDefault(DPadLeftDeadZone); }
		public bool ShouldSerializeDPadRightDeadZone() { return !isDefault(DPadRightDeadZone); }
		public bool ShouldSerializeDPadUpDeadZone() { return !isDefault(DPadUpDeadZone); }

		#endregion

	}
}