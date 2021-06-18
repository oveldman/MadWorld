using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2;
using Datalayer.Database;
using Datalayer.Database.Queries;
using Datalayer.Database.Tables;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Tests.Setup;
using Xunit;

namespace Tests.Datalayer.Database.Queries
{
    public class LoggerQueriesTest
    {
        DbContextOptions<MadWorldContext> Options;

        [Fact]
        public void GetLog_QueryWithID_LogFound()
        {
            // Test data
            Guid logIDExpected = new("0b0079a4-8e3d-4b29-97c1-db0e7c3712ed");

            // Setup
            LoggerQueries queries = SetupLoggerQuery();

            // Act
            Log resultLog = queries.GetLog(logIDExpected);

            // Assert
            Assert.Equal(logIDExpected, resultLog.ID);

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetLog_QueryWithID_LogNotFound()
        {
            // Test data
            Guid logIDExpected = new("cde8addd-5abd-4ab5-af2b-5849a8007e69");

            // Setup
            LoggerQueries queries = SetupLoggerQuery();

            // Act
            Log resultLog = queries.GetLog(logIDExpected);

            // Assert
            Assert.Null(resultLog);

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetLog_QueryWithEmptyID_LogNotFound()
        {
            // Test data
            Guid logIDExpected = new();

            // Setup
            LoggerQueries queries = SetupLoggerQuery();

            // Act
            Log resultLog = queries.GetLog(logIDExpected);

            // Assert
            Assert.Null(resultLog);

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetLogs_QueryWithoutDates_ThreeLogs()
        {
            // No Test data

            // Setup
            LoggerQueries queries = SetupLoggerQuery();

            // Act
            List<Log> resultLogs = queries.GetLogs(null, null);

            // Assert
            Assert.True(resultLogs.Count() == 3, "Expected a list of three log items");

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetLogs_QueryOnlyStartDate_TwoLogs()
        {
            // Test data
            DateTime? startDate = new DateTime(2021, 6, 5);

            // Setup
            LoggerQueries queries = SetupLoggerQuery();

            // Act
            List<Log> resultLogs = queries.GetLogs(startDate, null);

            // Assert
            Assert.True(resultLogs.Count() == 2, "Expected a list of two log items");
            Assert.Equal(new DateTime(2021, 6, 6, 5, 0, 0), resultLogs[0].Created);
            Assert.Equal(new DateTime(2021, 6, 5, 15, 0, 0), resultLogs[1].Created);

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetLogs_QueryOnlyEndDate_TwoLogs()
        {
            // Test data
            DateTime? endDate = new DateTime(2021, 6, 5);

            // Setup
            LoggerQueries queries = SetupLoggerQuery();

            // Act
            List<Log> resultLogs = queries.GetLogs(null, endDate);

            // Assert
            Assert.True(resultLogs.Count() == 2, "Expected a list of two log items");
            Assert.Equal(new DateTime(2021, 6, 5, 15, 0, 0), resultLogs[0].Created);
            Assert.Equal(new DateTime(2021, 6, 4, 11, 0, 0), resultLogs[1].Created);

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetLogs_QueryTwoDates_OneLogs()
        {
            // Test data
            DateTime? startDate = new DateTime(2021, 6, 5);
            DateTime? endDate = new DateTime(2021, 6, 5);

            // Setup
            LoggerQueries queries = SetupLoggerQuery();

            // Act
            List<Log> resultLogs = queries.GetLogs(startDate, endDate);

            // Assert
            Assert.True(resultLogs.Count() == 1, "Expected a list of one log item");
            Assert.Equal(new DateTime(2021, 6, 5, 15, 0, 0), resultLogs[0].Created);

            // Teardown
            TearDown();
        }

        [Fact]
        public void GetLogs_QueryTwoDates_NoResult()
        {
            // Test data
            DateTime? startDate = new DateTime(2021, 7, 5);
            DateTime? endDate = new DateTime(2021, 7, 5);

            // Setup
            LoggerQueries queries = SetupLoggerQuery();

            // Act
            List<Log> resultLogs = queries.GetLogs(startDate, endDate);

            // Assert
            resultLogs.Should().BeEquivalentTo(new List<Log>());

            // Teardown
            TearDown();
        }

        private LoggerQueries SetupLoggerQuery()
        {
            return SetupLoggerQuery(GetDataSet());
        }

        private LoggerQueries SetupLoggerQuery(List<Log> testData)
        {
            Options = new DbContextOptionsBuilder<MadWorldContext>()
                        .UseInMemoryDatabase(databaseName: "LoggerDatabase", new InMemoryDatabaseRoot())
                        .Options;

            using (var context = new MadWorldContext(Options))
            {
                context.Logs.AddRange(testData);
                context.SaveChanges();
            }

            return new LoggerQueries(new MadWorldContext(Options));
        }

        private List<Log> GetDataSet()
        {
            return new List<Log>()
            {
                new Log
                {
                    ID = new Guid("53b620e8-1037-41ef-97af-905b2adfbb8b"),
                    Created = new DateTime(2021, 6, 4, 11, 0, 0)
                },
                new Log
                {
                    ID = new Guid("0b0079a4-8e3d-4b29-97c1-db0e7c3712ed"),
                    Created = new DateTime(2021, 6, 5, 15, 0, 0)
                },
                new Log
                {
                    ID = new Guid("715dcc97-fb4e-4944-b559-1f926c4875ec"),
                    Created = new DateTime(2021, 6, 6, 5, 0, 0)
                }
            };
        }

        private void TearDown()
        {
            using (var context = new MadWorldContext(Options))
            {
                context.Database.EnsureDeleted();
                context.SaveChanges();
            }
        }
    }
}
