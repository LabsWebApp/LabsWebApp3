using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace LabsWebApp3.Helpers.Tests
{
    [TestClass()]
    public class ConfigTests
    {
        [TestMethod()]
        public void ConnectionStringGet_SQLSeverConnection_StateOpen()
        {
            //arrange
            DbConnection connection = new SqlConnection();
            string connStr = Config.ConnectionString;
            ConnectionState expected = ConnectionState.Open;
            //act
            ConnectionState actual = getState(connection, connStr);
            //assert
            Assert.AreEqual(expected, actual);
        }

        private ConnectionState getState(DbConnection connection, string connStr)
        {
            ConnectionState actual = ConnectionState.Closed;
            try
            {
                connection.ConnectionString = connStr;
                connection.Open();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{e.GetType()}: \n\t{e.Message}");
                if (connection != null)
                    Debug.WriteLine($"Connection state: {connection.State}");
            }
            finally
            {
                if (connection != null)
                {
                    actual = connection.State;
                    connection.Close();
                }
            }
            return actual;
        }
    }
}