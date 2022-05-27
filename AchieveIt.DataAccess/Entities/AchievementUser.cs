namespace AchieveIt.DataAccess.Entities
{
    public class AchievementUser : EntityBase<int>
    {
        public int AchievementId { get; set; }
        
        public int UserId { get; set; }
        
        public Achievement Achievement { get; set; }
        
        public User User { get; set; }
    }
}