using System.Data;
using System.Data.SqlClient;
using CoffeeMachine.Services.DTOs;

namespace CoffeeMachine.Services.DataAccess
{
    public class DataAccess : IDataAccess
    {
        public DataAccess() { }

        private SqlDataReader ExecuteProcedure(String command)
        {
            IList<SqlParameter> parameters = new List<SqlParameter>();

            return ExecuteProcedure(command, parameters);
        }

        private SqlDataReader ExecuteProcedure(String command, IList<SqlParameter> parameters)
        {

            var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;DataBase=CoffeeMachine;Trusted_Connection=True");
            connection.Open();
            SqlCommand cmd = new SqlCommand(command, connection);
            foreach (SqlParameter parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }

            cmd.CommandType = CommandType.StoredProcedure;
            var output = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            return output;
        }
        private async Task<SqlDataReader> ExecuteProcedureAsync(String command)
        {
            IList<SqlParameter> parameters = new List<SqlParameter>();

            return await ExecuteProcedureAsync(command, parameters);
        }

        private async Task<SqlDataReader> ExecuteProcedureAsync(String command, IList<SqlParameter> parameters)
        {

            var connection = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;DataBase=CoffeeMachine;Trusted_Connection=True");
            await connection.OpenAsync();
            SqlCommand cmd = new SqlCommand(command, connection);
            foreach (SqlParameter parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }

            cmd.CommandType = CommandType.StoredProcedure;
            var output = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            return output;
        }

        public async Task ActionLog(CoffeeMachineActionDTO action)
        {
            var command = "dbo.CoffeeMachine_Action_Log";

            IList<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("ActionTypeId", action.ActionTypeId));
            parameters.Add(new SqlParameter("ActionOptions", String.IsNullOrEmpty(action.Options) ? "" : action.Options));
            parameters.Add(new SqlParameter("TimeStamp", action.TimeStamp));
            parameters.Add(new SqlParameter("IsSuccess", action.IsSuccess));

            await ExecuteProcedureAsync(command, parameters);

        }

        public async Task<IList<HourlySummaryRowDTO>> HourlySummaryGet()
        {
            SqlDataReader? result = null;

            var ret = new List<HourlySummaryRowDTO>();

            try
            {
                var command = "dbo.CoffeeMachine_Usage_Summary_Hourly_Get";

                IList<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter("ActionType", "MakeCoffee"));

                result = await ExecuteProcedureAsync(command, parameters);

                while (await result.ReadAsync())
                {
                    ret.Add(new HourlySummaryRowDTO(await result.GetFieldValueAsync<String>(0), await result.GetFieldValueAsync<int>(1)));
                }
            }
            finally
            {
                if (result != null)
                {
                    await result.CloseAsync();
                }
            }

            return ret;

        }

        public async Task<IList<DailySummaryRowDTO>> DailySummaryGet()
        {
            SqlDataReader? result = null;

            var ret = new List<DailySummaryRowDTO>();

            try
            {
                var command = "dbo.CoffeeMachine_Usage_Summary_Daily_Get";

                IList<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter("ActionType", "MakeCoffee"));

                result = await ExecuteProcedureAsync(command, parameters);

                while (await result.ReadAsync())
                {
                    ret.Add(new DailySummaryRowDTO(await result.GetFieldValueAsync<String>(0), await result.GetFieldValueAsync<String>(1), await result.GetFieldValueAsync<String>(2), await result.GetFieldValueAsync<int>(3)));
                }
            }
            finally
            {
                if (result != null)
                {
                    await result.CloseAsync();
                }
            }

            return ret;

        }

        public IList<ActionTypeDTO> ActionTypesGet()
        {
            SqlDataReader? result = null;

            var ret = new List<ActionTypeDTO>();

            try
            {
                var command = "dbo.CoffeeMachine_ActionType_Get";

                result = new DataAccess().ExecuteProcedure(command);

                while (result.Read())
                {
                    ret.Add(new ActionTypeDTO(result.GetFieldValue<String>(0), result.GetFieldValue<String>(1), result.GetFieldValue<String>(2), result.GetFieldValue<Int16>(3)));
                }
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }

            return ret;

        }
    }

}
