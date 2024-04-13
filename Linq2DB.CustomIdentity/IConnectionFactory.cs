
using LinqToDB.Data;
using LinqToDB;

namespace Linq2DB.CustomIdentity
{
    public interface IConnectionFactory
    {
        /// <summary>
		///     Gets new instance of <see cref="IDataContext" />
		/// </summary>
		/// <returns>
		///     <see cref="IDataContext" />
		/// </returns>
		IDataContext GetContext();

        /// <summary>
        ///     Gets new instance of <see cref="DataConnection" />
        /// </summary>
        /// <returns>
        ///     <see cref="DataConnection" />
        /// </returns>
        DataConnection GetConnection();
    }
}
