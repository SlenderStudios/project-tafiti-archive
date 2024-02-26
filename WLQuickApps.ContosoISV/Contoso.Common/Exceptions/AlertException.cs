/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
using System;
namespace Contoso.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when an error occurs using Alerts
    /// </summary>
    public class AlertException: Exception
    {
		public AlertException()
		{
		}

		public AlertException(string message)
			: base(message)
		{
		}

        public AlertException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
    }
}
