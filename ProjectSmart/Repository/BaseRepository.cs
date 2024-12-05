using Microsoft.Extensions.Configuration;
using SmartLicencia.Data;
using SmartLicencia.Entity;
using System.Data;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class BaseRepository<T> : AbstractConnection
    {
        protected readonly IConfiguration _configuration;

        public BaseRepository(IConfiguration configuration)
            :base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<T>> GetAllData<T>(string storedProcedure, Func<SqlDataReader, T> mapFunction)
        {
            ResponseEntity<T> response = new ResponseEntity<T>();
            string conexion = _configuration.GetConnectionString("Conexion");
            using (SqlConnection connection = new SqlConnection(conexion))
            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    await connection.OpenAsync();
                    List<T> result = new List<T>();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(mapFunction(reader));
                        }
                    }
                    response.success = true;
                    response.items = result;
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.message = ex.Message;
                }
            }
            return response;
        }

        public async Task<ResponseEntity<T>> GetAllQuery<T>(string query, Func<SqlDataReader, T> mapFunction)
        {
            ResponseEntity<T> response = new ResponseEntity<T>();
            string conexion = _configuration.GetConnectionString("Conexion");
            using (SqlConnection connection = new SqlConnection(conexion))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                try
                {
                    await connection.OpenAsync();
                    List<T> result = new List<T>();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                            result.Add(mapFunction(reader));
                    }
                    response.success = true;
                    response.items = result;
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.message = ex.Message;
                }
            }
            return response;
        }

        public async Task<ResponseEntity<T>> GetAllDataById<T>(string storedProcedure, Func<SqlDataReader, T> mapFunction, Dictionary<string, object> parameters = null)
        {
            ResponseEntity<T> response = new ResponseEntity<T>();
            string conexion = _configuration.GetConnectionString("Conexion");
            using (SqlConnection connection = new SqlConnection(conexion))
            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                }
                try
                {
                    await connection.OpenAsync();
                    List<T> result = new List<T>();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(mapFunction(reader));
                        }
                    }
                    response.success = true;
                    response.items = result;
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.message = ex.Message;
                }
            }
            return response;
        }

        public async Task<ResponseEntity<T>> GetAllDataPaginator<T>(string storedProcedure, Func<SqlDataReader, T> mapFunction, Dictionary<string, object> parameters = null)
        {
            ResponseEntity<T> response = new ResponseEntity<T>();
            string conexion = _configuration.GetConnectionString("Conexion");
            using (SqlConnection connection = new SqlConnection(conexion))
            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    command.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                }
                try
                {
                    await connection.OpenAsync();
                    List<T> result = new List<T>();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(mapFunction(reader));
                        }
                    }

                    int result2 = Convert.ToInt32(command.Parameters["Resultado"].Value);
                    response.success = true;
                    response.extra = result2;
                    response.items = result;
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.message = ex.Message;
                }
            }
            return response;
        }

        public async Task<ResponseEntity<T>> GetData<T>(string storedProcedure, Dictionary<string, object> parameters, Func<SqlDataReader, T> mapFunction)
        {
            ResponseEntity<T> response = new ResponseEntity<T>();
            string conexion = _configuration.GetConnectionString("Conexion");
            using (SqlConnection connection = new SqlConnection(conexion))
            using (SqlCommand command = new SqlCommand(storedProcedure, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                try
                {
                    await connection.OpenAsync();
                    T result = default(T);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = mapFunction(reader);
                            response.success = true;
                            response.item = result;
                        }
                        else
                        {
                            response.success = false;
                            response.message = "No se encontro en BD";
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.message = ex.Message;
                }
            }
            return response;
        }

        public async Task<ResponseEntity<T>> Add<T>(T entity, string storedProcedureName, Dictionary<string, object> parameters)
        {
            ResponseEntity<T> response = new ResponseEntity<T>();
            try
            {
                string connectionString = _configuration.GetConnectionString("Conexion");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (var parameter in parameters)
                            {
                                string paramName = parameter.Key.StartsWith("@") ? parameter.Key : $"@{parameter.Key}";
                                object? value = parameter.Value;

                                if (value != null)
                                {
                                    if (value.GetType() == typeof(DateTime) && (value == null || Convert.ToDateTime(value) == DateTime.MinValue))
                                        value = null;
                                }

                                if (value == null)
                                    value = DBNull.Value;

                                command.Parameters.AddWithValue(paramName, value);
                            }
                        }
                        command.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        int result = Convert.ToInt32(command.Parameters["Resultado"].Value);
                        response.success = true;
                        response.extra = result;
                        response.message = "Se ha registrado correctamente";
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseEntity<T>> AddAsync<T>(T entity, string storedProcedureName, Dictionary<string, object> parameters)
        {
            ResponseEntity<T> response = new ResponseEntity<T>();
            try
            {
                string connectionString = _configuration.GetConnectionString("Conexion");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (var parameter in parameters)
                            {
                                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                            }
                        }
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        response.success = true;
                        response.message = "Se ha registrado correctamente";
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }
        public async Task<ResponseEntity<T>> Update<T>(T entity, string storedProcedureName, Dictionary<string, object> parameters)
        {
            ResponseEntity<T> response = new ResponseEntity<T>();
            try
            {
                string connectionString = _configuration.GetConnectionString("Conexion");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (var parameter in parameters)
                            {
                                string paramName = parameter.Key.StartsWith("@") ? parameter.Key : $"@{parameter.Key}";
                                object? value = parameter.Value;

                                if(value != null)
                                {
                                    if (value.GetType() == typeof(DateTime) && (value == null || Convert.ToDateTime(value) == DateTime.MinValue))
                                        value = null;
                                }

                                if (value == null)
                                    value = DBNull.Value;

                                command.Parameters.AddWithValue(paramName, value);
                            }
                        }
                        command.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        int result = Convert.ToInt32(command.Parameters["Resultado"].Value);
                        response.success = true;
                        response.extra = result;
                        response.message = "Se ha actualizado correctamente";
                    }
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }


        public async Task<ResponseEntity<T>> DeleteEntity(string storedProcedureName, Dictionary<string, object> parameters)
        {
            ResponseEntity<T> response = new ResponseEntity<T>();
            try
            {
                string connectionString = _configuration.GetConnectionString("Conexion");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null && parameters.Count > 0)
                        {
                            foreach (var parameter in parameters)
                            {
                                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                            }
                        }
                        command.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                        response.extra = Convert.ToInt32(command.Parameters["Resultado"].Value);
                        response.message = "Se ha elimnado correctamente";
                        response.success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                response.extra = 0;
                response.success = false;
                response.message = $"Error: {ex.Message}";
            }
            return response;
        }

    }
}
