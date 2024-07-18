namespace EnglishJourney.Domain.Entities
{
    public class UserStatistic
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int RegisteredAccounts { get; set; }
        public int Admins { get; set; }
        public int NationalitiesSet { get; set; }
    }
}