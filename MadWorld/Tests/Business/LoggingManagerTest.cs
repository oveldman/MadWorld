using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using Business;
using Datalayer.Database.Queries.Interfaces;
using Datalayer.Database.Tables;
using FluentAssertions;
using Moq;
using Tests.Setup;
using Website.Shared.Models.BackendInfo;
using Xunit;

namespace Tests.Business
{
    public class LoggingManagerTest
    {
        [Theory]
        [AutoDomainData]
        public void GetLogging_ValidDatimes_LogsNotFound(
            [Frozen] Mock<ILoggerQueries> queries,
            LoggingManager manager
            )
        {
            // Test data
            DateTime? startDate = DateTime.Parse("05/24/2021 10:00:00");
            DateTime? endDate = DateTime.Parse("05/26/2021 10:00:00");

            // Setup
            queries.Setup(q => q.GetLogs(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns<List<Log>>(null);

            // Act
            var logs = manager.GetLogging(startDate, endDate);

            // Assert
            logs.Should().BeEquivalentTo(new List<LogItem>());

            // No Teardown
        }

        [Theory]
        [AutoDomainInlineData(2, "Information")]
        [AutoDomainInlineData(5, "Critical")]
        public void GetLogging_ValidDatimes_LogsFoundConvertLevelCorrect(
            int levelValue,
            string expectedTranslation,
            [Frozen] Mock<ILoggerQueries> queries,
            List<Log> dbMockLogs,
            LoggingManager manager
            )
        {
            // Test data
            DateTime? startDate = DateTime.Parse("05/24/2021 10:00:00");
            DateTime? endDate = DateTime.Parse("05/26/2021 10:00:00");

            dbMockLogs.ForEach(l => l.Level = levelValue);

            // Setup
            queries.Setup(q => q.GetLogs(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(dbMockLogs);

            // Act
            var logs = manager.GetLogging(startDate, endDate);

            // Assert
            Assert.Equal(expectedTranslation, logs[0].Level);

            // No Teardown
        }
    }
}
