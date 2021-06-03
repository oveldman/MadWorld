using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Xunit2;
using Database;
using Database.Queries;
using Database.Tables;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Setup;
using Xunit;

namespace Tests.Database.Queries
{
    public class LoggerQueriesTest
    {
        DbContextOptions<MadWorldContext> Options;

        [Theory]
        [AutoDomainData]
        public void GetLogs_QueryOnDates_ThreeLogs(
            List<Log> logs
            )
        {
            // No Test data

            // Setup
            LoggerQueries queries = SetupLoggerQuery(logs);

            // Act
            List<Log> resultLogs = queries.GetLogs(null, null);

            // Assert
            resultLogs.Should().BeEquivalentTo(logs);

            // Teardown
            TearDown();
        }

        private LoggerQueries SetupLoggerQuery(List<Log> testData)
        {
            Options = new DbContextOptionsBuilder<MadWorldContext>()
                        .UseInMemoryDatabase(databaseName: "LoggerDatabase")
                        .Options;

            using (var context = new MadWorldContext(Options))
            {
                context.Logs.AddRange(testData);
                context.SaveChanges();
            }

            return new LoggerQueries(new MadWorldContext(Options));
        }

        private void TearDown()
        {
            using (var context = new MadWorldContext(Options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
