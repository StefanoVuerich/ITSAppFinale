using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;
using System.Data.Common;


namespace Data
{
	internal static class SqlHelper
	{

		internal static IEnumerable<SqlDataReader> ForEach(this SqlDataReader reader)
		{
			while (reader.Read())
				yield return reader;

			yield break;
		}

		internal static bool IsNull(this DbDataReader reader, string name)
		{
			int index = reader.GetOrdinal(name);
			return reader.IsDBNull(index);
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader">The reader.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		internal static T GetValue<T>(this DbDataReader reader, string name)
		{
			try
			{
				int index = reader.GetOrdinal(name);

				if (reader.IsDBNull(index))
					return default(T);

				return (T)reader.GetValue(index);
			}
			catch (InvalidCastException)
			{
				throw new InvalidCastException(string.Format("Column '{0}' type is not compatible with type '{1}'", name, typeof(T)));
			}
		}

		/// <summary>
		/// Gets the enum value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="reader">The reader.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		/*internal static T GetEnumValue<T>(this DbDataReader reader, string name)
		{
			try
			{
				int index = reader.GetOrdinal(name);

				if (reader.IsDBNull(index))
					return default(T);

				return Conversion.GetEnumValue<T>(reader.GetValue(index));
			}
			catch (InvalidCastException)
			{
				throw new InvalidCastException(string.Format("Column '{0}' type is not compatible with type '{1}'", name, typeof(T)));
			}
	}*/

		/// <summary>
		/// Executes the non query.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <param name="parameters">The parameters.</param>
		internal static void ExecuteNonQuery(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			params SqlParameter[] parameters)
		{
			using (SqlCommand command = SqlHelper.CreateCommand(connection, commandText, commandType))
			{
				if (parameters.Length > 0)
					command.Parameters.AddRange(parameters);

				command.ExecuteNonQuery();
			}
		}

		internal static void ExecuteNonQuery(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			SqlTransaction transaction,
			params SqlParameter[] parameters)
		{
			using (SqlCommand command = SqlHelper.CreateCommand(connection, commandText, commandType))
			{
				command.Transaction = transaction;

				if (parameters.Length > 0)
					command.Parameters.AddRange(parameters);

				command.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Executes the scalar.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		internal static T ExecuteScalar<T>(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			params SqlParameter[] parameters)
		{
			using (SqlCommand command = SqlHelper.CreateCommand(connection, commandText, commandType))
			{
				if (parameters.Length > 0)
					command.Parameters.AddRange(parameters);

				return ConvertValue<T>(command.ExecuteScalar());
			}
		}

		/// <summary>
		/// Executes the scalar.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <param name="transaction">The transaction.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		internal static T ExecuteScalar<T>(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			SqlTransaction transaction,
			params SqlParameter[] parameters)
		{
			using (SqlCommand command = SqlHelper.CreateCommand(connection, commandText, commandType))
			{
				command.Transaction = transaction;

				if (parameters.Length > 0)
					command.Parameters.AddRange(parameters);

				return ConvertValue<T>(command.ExecuteScalar());
			}
		}

		/// <summary>
		/// Executes the reader.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		internal static SqlDataReader ExecuteReader(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			params SqlParameter[] parameters)
		{
			return SqlHelper.ExecuteReader(connection, commandText, commandType, CommandBehavior.Default, parameters);
		}

		/// <summary>
		/// Executes the reader.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <param name="transaction">The transaction.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		internal static SqlDataReader ExecuteReader(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			SqlTransaction transaction,
			params SqlParameter[] parameters)
		{
			return SqlHelper.ExecuteReader(connection, commandText, commandType, CommandBehavior.Default, transaction, parameters);
		}

		/// <summary>
		/// Executes the reader.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <param name="commandTimeout">The command timeout.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		internal static SqlDataReader ExecuteReader(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			int commandTimeout,
			params SqlParameter[] parameters)
		{
			return SqlHelper.ExecuteReader(connection, commandText, commandType, CommandBehavior.Default, commandTimeout, parameters);
		}

		/// <summary>
		/// Executes the reader.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <param name="commandTimeout">The command timeout.</param>
		/// <param name="transaction">The transaction.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		internal static SqlDataReader ExecuteReader(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			int commandTimeout,
			SqlTransaction transaction,
			params SqlParameter[] parameters)
		{
			return SqlHelper.ExecuteReader(connection, commandText, commandType, CommandBehavior.Default, commandTimeout, transaction, parameters);
		}

		/// <summary>
		/// Executes the reader.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <param name="commandBehavior">The command behavior.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		internal static SqlDataReader ExecuteReader(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			CommandBehavior commandBehavior,
			params SqlParameter[] parameters)
		{
			using (SqlCommand command = SqlHelper.CreateCommand(connection, commandText, commandType))
			{
				if (parameters.Length > 0)
					command.Parameters.AddRange(parameters);

				return command.ExecuteReader(commandBehavior);
			}
		}

		internal static SqlDataReader ExecuteReader(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			CommandBehavior commandBehavior,
			SqlTransaction transaction,
			params SqlParameter[] parameters)
		{
			using (SqlCommand command = SqlHelper.CreateCommand(connection, commandText, commandType))
			{
				command.Transaction = transaction;

				if (parameters.Length > 0)
					command.Parameters.AddRange(parameters);

				return command.ExecuteReader(commandBehavior);
			}
		}

		/// <summary>
		/// Executes the reader.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <param name="commandBehavior">The command behavior.</param>
		/// <param name="commandTimeout">The command timeout.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns></returns>
		internal static SqlDataReader ExecuteReader(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			CommandBehavior commandBehavior,
			int commandTimeout,
			params SqlParameter[] parameters)
		{
			using (SqlCommand command = SqlHelper.CreateCommand(connection, commandText, commandType))
			{
				command.CommandTimeout = commandTimeout;

				if (parameters.Length > 0)
					command.Parameters.AddRange(parameters);

				return command.ExecuteReader(commandBehavior);
			}
		}

		internal static SqlDataReader ExecuteReader(
			this SqlConnection connection,
			string commandText,
			CommandType commandType,
			CommandBehavior commandBehavior,
			int commandTimeout,
			SqlTransaction transaction,
			params SqlParameter[] parameters)
		{
			using (SqlCommand command = SqlHelper.CreateCommand(connection, commandText, commandType))
			{
				command.Transaction = transaction;
				command.CommandTimeout = commandTimeout;

				if (parameters.Length > 0)
					command.Parameters.AddRange(parameters);

				return command.ExecuteReader(commandBehavior);
			}
		}

		/// <summary>
		/// Converts the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ConvertValue<T>(object value)
		{
			if (value == null)
				throw new ArgumentNullException("Cannot convert null value", "value");

			if (value.GetType() == typeof(T))
				return (T)value;

			TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());

			return (T)converter.ConvertTo(value, typeof(T));
		}

		#region CreateCommand

		/// <summary>
		/// Creates the command.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <returns></returns>
		internal static SqlCommand CreateCommand(SqlConnection connection)
		{
			return SqlHelper.CreateCommand(connection, string.Empty, CommandType.Text, null);
		}

		/// <summary>
		/// Creates the command.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <returns></returns>
		internal static SqlCommand CreateCommand(SqlConnection connection, string commandText)
		{
			return SqlHelper.CreateCommand(connection, commandText, CommandType.Text, null);
		}

		/// <summary>
		/// Creates the command.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <returns></returns>
		internal static SqlCommand CreateCommand(SqlConnection connection, string commandText, CommandType commandType)
		{
			return SqlHelper.CreateCommand(connection, commandText, commandType, null);
		}

		/// <summary>
		/// Creates the command.
		/// </summary>
		/// <param name="connection">The connection.</param>
		/// <param name="commandText">The command text.</param>
		/// <param name="commandType">Type of the command.</param>
		/// <param name="transaction">The transaction.</param>
		/// <returns></returns>
		internal static SqlCommand CreateCommand(SqlConnection connection, string commandText, CommandType commandType, SqlTransaction transaction)
		{
			return new SqlCommand
			{
				CommandText = commandText,
				Connection = connection,
				Transaction = transaction,
				CommandType = commandType
			};
		}

		#endregion

		#region CreateParameter

		/// <summary>
		/// Creates the parameter.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		internal static SqlParameter CreateParameter(string parameterName, object value)
		{
			return new SqlParameter("@" + parameterName, value ?? (object)DBNull.Value);
		}

		/// <summary>
		/// Creates the parameter.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="value">The value.</param>
		/// <param name="direction">The direction.</param>
		/// <returns></returns>
		internal static SqlParameter CreateParameter(string parameterName, object value, SqlDbType dbType, ParameterDirection direction)
		{
			return new SqlParameter("@" + parameterName, value ?? (object)DBNull.Value)
			{
				SqlDbType = dbType,
				Direction = direction
			};
		}

		/// <summary>
		/// Creates the parameter.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="value">The value.</param>
		/// <param name="dbType">Type of the db.</param>
		/// <returns></returns>
		internal static SqlParameter CreateParameter(string parameterName, object value, SqlDbType dbType)
		{
			return new SqlParameter("@" + parameterName, value ?? (object)DBNull.Value)
			{
				SqlDbType = dbType
			};
		}

		/// <summary>
		/// Creates the parameter.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		internal static SqlParameter CreateParameter(string parameterName, string value)
		{
			return new SqlParameter("@" + parameterName, value ?? (object)DBNull.Value)
			{
				SqlDbType = SqlDbType.NVarChar,
				Size = value.Length
			};
		}

		/// <summary>
		/// Creates the parameter.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="value">The value.</param>
		/// <param name="dbType">Type of the db.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		internal static SqlParameter CreateParameter(string parameterName, object value, SqlDbType dbType, int size)
		{
			return new SqlParameter("@" + parameterName, value ?? (object)DBNull.Value)
			{
				SqlDbType = dbType,
				Size = size
			};
		}

		/// <summary>
		/// Determines whether the specified reader has field.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="name">The name.</param>
		/// <returns>
		/// 	<c>true</c> if the specified reader has field; otherwise, <c>false</c>.
		/// </returns>
		internal static bool HasField(this IDataReader reader, string name)
		{
			for (int i = 0; i < reader.FieldCount; i++)
				if (reader.GetName(i) == name) return true;

			return false;
		}

		/// <summary>
		/// Safes the convert2 datetime.
		/// </summary>
		/// <param name="oValue">The o value.</param>
		/// <returns></returns>
		internal static DateTime SafeConvert2Datetime(object oValue)
		{
			if (oValue != DBNull.Value)
			{
				int nValue = 0;
				if (oValue is double || oValue is decimal)
				{
					oValue = Convert.ToInt16(oValue);
				}

				if (oValue.ToString() == "9999")
				{
					//return (DateTime.MinValue);
					return (new DateTime(2000, 1, 1));
				}

				if (oValue is short)
				{
					if (oValue.ToString().Length == 4)
					{
						int nV1 = Convert.ToInt16(oValue.ToString().Substring(0, 2));
						if (nV1 >= 24)
						{
							//if (Convert.ToInt32(oValue.ToString()) == 2400)
							//{
							//    int a = 0;
							//}
							nValue = Convert.ToInt32(oValue.ToString()) - 2400;
							return (new DateTime(2001, 1, 1, (int)(nValue / 100), nValue >= 100 ? (int)(nValue % 100) : nValue, 0).AddDays(1));
						}
					}
					nValue = Convert.ToInt32(oValue);

					return (new DateTime(2001, 1, 1, (int)(nValue / 100), nValue >= 100 ? (int)(nValue % 100) : nValue, 0));

				}
				else
					return (Convert.ToDateTime(oValue));
			}
			else
				return (new DateTime(2000, 1, 1));

		}

		#endregion
	}
}