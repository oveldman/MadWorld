using System;
using Business;
using Common;
using Database.Queries.Interfaces;
using Database.Tables.Identity;
using Moq;
using Xunit;

namespace Tests.Business
{
    public class UserExtremeManagerTest
    {
        [Fact]
        public void FindUserByTwoFactorSession_Session_UserFoundSessionNotValid()
        {
            // Test data
            Guid? session = Guid.Parse("6b976751-e0c9-43c6-9aef-57bfba4ecfa9");

            // Setup
            var mock = new Mock<IAccountQueries>();
            mock.Setup(queries => queries.FindUserBySession(session)).Returns(UserFoundData());
            UserExtremeManager userManager = new UserExtremeManager(mock.Object);

            SystemTime.SetDateTime(DateTime.Parse("05/24/2021 10:30:00"));

            // Act
            User user = userManager.FindUserByTwoFactorSession(session);

            // Assert
            Assert.Null(user);

            // Teardown
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void FindUserByTwoFactorSession_Session_UserFoundSessionValid()
        {
            // Test data
            Guid? session = Guid.Parse("6b976751-e0c9-43c6-9aef-57bfba4ecfa9");

            // Setup
            var mock = new Mock<IAccountQueries>();
            mock.Setup(queries => queries.FindUserBySession(session)).Returns(UserFoundData());
            UserExtremeManager userManager = new UserExtremeManager(mock.Object);
            SystemTime.SetDateTime(DateTime.Parse("05/24/2021 09:30:00"));

            // Act
            User user = userManager.FindUserByTwoFactorSession(session);

            // Assert
            Assert.Equal(user.TwoFactorSession, session);

            // Teardown
            SystemTime.ResetDateTime();
        }

        [Fact]
        public void FindUserByTwoFactorSession_Session_UserNotFound()
        {
            // Test data
            Guid? session = Guid.Parse("6b976751-e0c9-43c6-9aef-57bfba4ecfa9");

            // Setup
            var mock = new Mock<IAccountQueries>();
            mock.Setup(queries => queries.FindUserBySession(session)).Returns<User>(null);
            UserExtremeManager userManager = new UserExtremeManager(mock.Object);
            SystemTime.SetDateTime(DateTime.Parse("05/24/2021 10:30:00"));

            // Act
            User user = userManager.FindUserByTwoFactorSession(session);

            // Assert
            Assert.Null(user);

            // Teardown
            SystemTime.ResetDateTime();
        }

        private User UserFoundData()
        {
            return new User
            {
                UserName = "test@test.nl",
                TwoFactorEnabled = true,
                TwoFactorSessionExpire = DateTime.Parse("05/24/2021 10:00:00"),
                TwoFactorSession = Guid.Parse("6b976751-e0c9-43c6-9aef-57bfba4ecfa9"),
                TwoFactorOn = true,
            };
        }
    }
}
