namespace AchieveIt.Shared.Options
{
    public class JwtOptions
    {
        public const string JwtSectionName = "JWT";

        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int LifeTimeMinutes { get; set; }
    }
}