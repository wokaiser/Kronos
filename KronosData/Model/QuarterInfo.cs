namespace KronosData.Model
{
    public class QuarterInfo
    {
        public QuarterInfo(int twd, int sd, int fd, int md) 
        {
            TotalWorkDays = twd;
            SickDays = sd;
            FreeDays = fd;
            MobileDays = md;
        }

        #region Properties

        public static QuarterInfo Empty
        {
            get
            {
                return new QuarterInfo(0, 0, 0, 0);
            }
        }

        public int TotalWorkDays { get; }

        public int SickDays { get; }

        public int FreeDays { get; }

        public int MobileDays { get; }

        #endregion
    }
}
