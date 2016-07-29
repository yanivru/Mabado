namespace Mabado.View.Domain
{
    public class InstallationInfo
    {
        public string Version { get; set; }

        public int UserSid { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((InstallationInfo) obj);
        }

        protected bool Equals(InstallationInfo other)
        {
            return string.Equals(Version, other.Version) && UserSid == other.UserSid;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Version != null ? Version.GetHashCode() : 0) * 397) ^ UserSid;
            }
        }
    }
}