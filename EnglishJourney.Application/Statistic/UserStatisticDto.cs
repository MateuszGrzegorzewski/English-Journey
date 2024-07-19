namespace EnglishJourney.Application.Statistic
{
    public class UserStatisticDto
    {
        public DateTime Date { get; set;  }
        public int RegisteredAccounts { get; set; }
        public int Admins { get; set; }
        public int NationalitiesSet { get; set; }
    }
}