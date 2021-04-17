namespace KronosData.Model
{
    public class Account : DB_Access
    {
        /// <summary>
        /// Creates a new Account object
        /// </summary>
        /// <param name="number">The account number</param>
        public Account(string number)
        {
            Number = number;
            Title = string.Empty;
        }

        #region Overrides

        public override string ToString()
        {
            return string.Format("{0}: \"{1}\"", Number, Title);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The account number
        /// </summary>
        public string Number { get; }

        /// <summary>
        /// The title of the account
        /// </summary>
        public string Title { get; set; }

        #endregion
    }
}
