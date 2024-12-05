using System.Data;
using System.Reflection;

namespace SmartLicencia.Data
{
    public abstract class AbstractConnection
    {
        protected string ConnectionString { get; set; }
        protected readonly IConfiguration Configuration;

        protected AbstractConnection(IConfiguration configuration)
        {
            Configuration = configuration;
            string? strConnection = configuration.GetConnectionString("Conexion");

            if (string.IsNullOrWhiteSpace(strConnection))
                throw new InvalidOperationException("No se ha encontrado cadena de conexión.");

            ConnectionString = strConnection;
        }

        protected IDbCommand CreateCommand<TCommand>() where TCommand : IDbCommand
        {
            return (IDbCommand)Convert.ChangeType(Activator.CreateInstance<TCommand>(), typeof(TCommand));
        }

        protected IDbCommand CreateCommand<TCommand>(string commandText, IDbConnection connection) where TCommand : IDbCommand
        {
            var cmd = CreateCommand<TCommand>();
            cmd.CommandText = commandText;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            return cmd;
        }

        protected IDbCommand CreateCommandProcedure<TCommand>(string name, IDbConnection connection) where TCommand : IDbCommand
        {
            var cmd = CreateCommand<TCommand>();
            cmd.CommandText = name;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = connection;
            return cmd;
        }

        protected IDbConnection CreateConnection<TConnection>() where TConnection : IDbConnection
        {
            var connection = Activator.CreateInstance<TConnection>();
            connection.ConnectionString = ConnectionString;
            return (IDbConnection)Convert.ChangeType(connection, typeof(TConnection));
        }

        protected IDataParameter CreateParameter<TParameter>(string name, object? value = null) where TParameter : IDataParameter
        {
            var parameter = Activator.CreateInstance<TParameter>();
            parameter.ParameterName = name;

            if(value?.GetType() == typeof(DateTime) && ((DateTime) value) == DateTime.MinValue)
                value = null;

            if (value != null)
                parameter.Value = value;
            else
                parameter.Value = DBNull.Value;

            return (IDataParameter)Convert.ChangeType(parameter, typeof(TParameter));
        }

        public IDataParameter CreateParameter<TParameter>(string name, DbType dbType, object? value, int? size = null) where TParameter : IDataParameter
        {
            var parameter = CreateParameter<TParameter>(name, value);
            parameter.DbType = dbType;

            if (size != null)
            {
                var property = parameter.GetType().GetProperty("Size", BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
                if (property != null && property.CanWrite)
                    property.SetValue(parameter, size, null);
            }

            return parameter;
        }

        public IDataParameter CreateParameter<TParameter>(string name, DbType dbType, ParameterDirection direction, int? size = null, object? value = null) where TParameter : IDataParameter
        {
            var parameter = CreateParameter<TParameter>(name, dbType, value, size);
            parameter.Direction = direction;
            return parameter;
        }

        public TResult ExecQuery<TConnection, TCommand, TResult>(string sqlQuery, Func<TCommand, TResult> callback) where TConnection : IDbConnection where TCommand : IDbCommand
        {
            IDbConnection? cn = null;
            TCommand? cmd = default;
            TResult result;
            try
            {
                cn = CreateConnection<TConnection>();
                cn.Open();
                cmd = (TCommand)CreateCommand<TCommand>(sqlQuery, cn);
                result = callback(cmd);
            }
            catch (Exception)
            {
                throw;
            } finally
            {
                if(cmd != null)
                    cmd.Dispose();

                if (cn != null)
                {
                    if(cn.State == ConnectionState.Open)
                        cn.Close();
                }
            }
            return result;
        }

        public TResult ExecProcedure<TConnection, TCommand, TResult>(string name, Func<TCommand, TResult> callback) where TConnection : IDbConnection where TCommand : IDbCommand
        {
            IDbConnection? cn = null;
            TCommand? cmd = default;
            TResult result;
            try
            {
                cn = CreateConnection<TConnection>();
                cn.Open();
                cmd = (TCommand)CreateCommandProcedure<TCommand>(name, cn);
                result = callback(cmd);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (cn != null)
                {
                    if (cn.State == ConnectionState.Open)
                        cn.Close();
                }
            }
            return result;
        }
    }
}
