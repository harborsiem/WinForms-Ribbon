using System.Globalization;
using Windows.Win32.UI.Ribbon;

namespace Windows.Win32.UI.Shell.PropertiesSystem
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// Additional implementations for the CsWin32 PROPERTYKEY.
    /// Maybe in future we can delete the first partial struct.
    /// </summary>
    /// <inheritdoc cref="PROPERTYKEY"/>
    partial struct PROPERTYKEY
    {
        public static bool operator ==(PROPERTYKEY left, PROPERTYKEY right) =>
             ((left.fmtid == right.fmtid) && (left.pid == right.pid));

        public static bool operator !=(PROPERTYKEY left, PROPERTYKEY right) => !(left == right);

        public bool Equals(PROPERTYKEY other) => this == other;

        public override bool Equals(object? obj) => obj is PROPERTYKEY other && this.Equals(other);

        public override int GetHashCode() => (fmtid.GetHashCode() ^ pid.GetHashCode());

        public override string ToString()
        {
            string? value = null;
            ToString(ref value);
            if (value != null)
            {
                return value;
            }
            return "PROPERTYKEY: " + fmtid.ToString() + " : " + pid.ToString(CultureInfo.InvariantCulture.NumberFormat);
        }

        partial void ToString(ref string? value);
    }

    /// <inheritdoc cref="PROPERTYKEY"/>
    partial struct PROPERTYKEY
    {
        partial void ToString(ref string? value)
        {
            value = RibbonProperties.KeyToString(this);
        }
    }
}
