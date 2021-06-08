using System;
using System.Collections.Generic;
using System.Linq;
using Business.Models.PlanningPoker;
using Business.PlanningPoker;
using FluentAssertions;
using Tests.Setup;
using Xunit;

namespace Tests.Business.PlanningPoker
{
    public class PokerManagerTest
    {
        [Fact]
        public void CreateOrAddToRoom_RoomNameConnectIdUserNameClientSession_SucceedNewRoom()
        {
            string roomName = "RandomRoom";
            string connectionID = "PartConnection";
            string username = "Jan";
            Guid clientSessionBrowser = new Guid("82f68fc7-859a-4190-9e5a-d037c4b34ffc");

            // Setup
            PokerManager manager = Setup(out PokerSession testSession);

            // Act
            bool succeed = manager.CreateOrAddToRoom(roomName, connectionID, username, clientSessionBrowser);

            // Assert
            Assert.True(succeed);
            Assert.True(testSession.Rooms.Count == 3, "Expected a list of three room items");
            Assert.True(testSession.Rooms[2].Name == roomName, $"Expected a room with the name {roomName}");

            // No Teardown
        }

        [Fact]
        public void CreateOrAddToRoom_RoomNameConnectIdUserNameClientSession_SucceedNewUserExistsRoom()
        {
            string roomName = "TestTwo";
            string connectionID = "PartConnection";
            string username = "Jan";
            Guid clientSessionBrowser = new Guid("82f68fc7-859a-4190-9e5a-d037c4b34ffc");

            // Setup
            PokerManager manager = Setup(out PokerSession testSession);

            // Act
            bool succeed = manager.CreateOrAddToRoom(roomName, connectionID, username, clientSessionBrowser);

            // Assert
            Assert.True(succeed);
            Assert.True(testSession.Rooms.Count == 2, "Expected a list of two room items");
            Assert.True(testSession.Rooms[1].Users.Count == 3, "Expected a list of three users items");
            Assert.True(testSession.Rooms[1].Users[2].Name == username, $"Expected a user with the name {username}");

            // No Teardown
        }

        [Fact]
        public void CreateOrAddToRoom_EmptyRoomName_Failed()
        {
            string roomName = "";
            string connectionID = "PartConnection";
            string username = "";
            Guid clientSessionBrowser = new Guid("82f68fc7-859a-4190-9e5a-d037c4b34ffc");

            // Setup
            PokerManager manager = Setup(out PokerSession testSession);

            // Act
            bool succeed = manager.CreateOrAddToRoom(roomName, connectionID, username, clientSessionBrowser);

            // Assert
            Assert.False(succeed);
            Assert.True(testSession.Rooms.Count == 2, "Expected a list of two room items");

            // No Teardown
        }

        [Fact]
        public void GetRoom_ConnectionID_Name()
        {
            // Test data
            string connectionID = "ConTop";

            // Setup
            PokerManager manager = Setup(out _);

            // Act
            string roomname = manager.GetRoomName(connectionID);

            // Assert
            Assert.Equal("TestTwo", roomname);

            // No Teardown
        }

        [Fact]
        public void GetRoom_ConnectionID_EmptyString()
        {
            // Test data
            string connectionID = "UnknownCon";

            // Setup
            PokerManager manager = Setup(out _);

            // Act
            string roomname = manager.GetRoomName(connectionID);

            // Assert
            Assert.Equal(string.Empty, roomname);

            // No Teardown
        }

        [Fact]
        public void GetUser_ConnectionID_User()
        {
            // Test data
            string connectionID = "TestConnection";
            PokerUser expectedUser = new PokerUser
            {
                Id = 2,
                ClientSession = new Guid("d4950e39-fe50-4fb1-a662-d378e29b451d"),
                ConnectionId = "TestConnection",
                Name = "Piet"
            };

            // Setup
            PokerManager manager = Setup(out _);

            // Act
            PokerUser user = manager.GetUser(connectionID);

            // Assert
            user.Should().BeEquivalentTo(expectedUser);

            // No Teardown
        }

        [Fact]
        public void GetUser_ConnectionID_EmptyUser()
        {
            // Test data
            string connectionID = "UnknownCon";

            // Setup
            PokerManager manager = Setup(out _);

            // Act
            PokerUser user = manager.GetUser(connectionID);

            // Assert
            user.Should().BeEquivalentTo(new PokerUser());

            // No Teardown
        }

        [Fact]
        public void GetUsersFromRoom_RoomName_Users()
        {
            // Test data
            string roomName = "TestTwo";
            List<PokerUser> expectedUsers = new List<PokerUser>
            {
                new PokerUser {
                    Id = 3,
                    ClientSession = new Guid("6f100f99-bb66-4fff-8f79-8bbb844f2355"),
                    ConnectionId = "ConTop",
                    Name = "Joost"
                },
                new PokerUser {
                    Id = 4,
                    ClientSession = new Guid("ed8f7ed8-1d67-4646-82c6-8c88a97d2c37"),
                    ConnectionId = "DownCon",
                    Name = "Bob"
                }
            };

            // Setup
            PokerManager manager = Setup(out _);

            // Act
            List<PokerUser> users = manager.GetUsersFromRoom(roomName);

            // Assert
            users.Should().BeEquivalentTo(expectedUsers);

            // No Teardown
        }

        [Fact]
        public void GetUsersFromRoom_RoomName_EmptyListOfUsers()
        {
            // Test data
            string roomName = "TestThree";

            // Setup
            PokerManager manager = Setup(out _);

            // Act
            List<PokerUser> users = manager.GetUsersFromRoom(roomName);

            // Assert
            users.Should().BeEquivalentTo(new List<PokerUser>());

            // No Teardown
        }

        [Fact]
        public void RemoveUserFromRoom_ConnectionID_EmptyRoomName()
        {
            // Test data
            string connectionID = "UnknownConnection";

            // Setup
            PokerManager manager = Setup(out PokerSession testSession);

            // Act
            string roomName = manager.RemoveUserFromRoom(connectionID);

            // Assert
            Assert.Equal(string.Empty, roomName);
            Assert.True(testSession.Rooms.Count == 2, "Expected a list of two room items");
            Assert.True(testSession.Rooms[0].Users.Count == 2, "Expected a list of two users items");
            Assert.True(testSession.Rooms[1].Users.Count == 2, "Expected a list of two users items");

            // No Teardown
        }

        [Fact]
        public void RemoveUserFromRoom_ConnectionID_RoomNameRemoveUser()
        {
            // Test data
            string connectionID = "RandomCon";

            // Setup
            PokerManager manager = Setup(out PokerSession testSession);

            // Act
            string roomName = manager.RemoveUserFromRoom(connectionID);

            // Assert
            Assert.Equal("TestOne", roomName);
            Assert.True(testSession.Rooms[0].Users.Count == 1, "Expected a list of one user item");

            // No Teardown
        }

        [Fact]
        public void RemoveUserFromRoom_ConnectionID_RoomNameRemoveRoom()
        {
            // Test data
            string connectionID = "RandomCon";

            // Setup
            PokerManager manager = SetupOneUser(out PokerSession testSession);

            // Act
            string roomName = manager.RemoveUserFromRoom(connectionID);

            // Assert
            Assert.Equal("TestOne", roomName);
            Assert.True(testSession.Rooms.Count == 1, "Expected a list of one room item");

            // No Teardown
        }

        private PokerManager Setup(out PokerSession testSession)
        {
            List<PokerUser> usersRoomOne = new List<PokerUser>
            {
                new PokerUser {
                    Id = 1,
                    ClientSession = new Guid("6e1af4d7-866d-42a9-98c4-76e9e9973046"),
                    ConnectionId = "RandomCon",
                    Name = "Kees"
                },
                new PokerUser {
                    Id = 2,
                    ClientSession = new Guid("d4950e39-fe50-4fb1-a662-d378e29b451d"),
                    ConnectionId = "TestConnection",
                    Name = "Piet"
                }
            };

            List<PokerUser> usersRoomtwo = new List<PokerUser>
            {
                new PokerUser {
                    Id = 3,
                    ClientSession = new Guid("6f100f99-bb66-4fff-8f79-8bbb844f2355"),
                    ConnectionId = "ConTop",
                    Name = "Joost"
                },
                new PokerUser {
                    Id = 4,
                    ClientSession = new Guid("ed8f7ed8-1d67-4646-82c6-8c88a97d2c37"),
                    ConnectionId = "DownCon",
                    Name = "Bob"
                }
            };


            List<PokerRoom> rooms = new List<PokerRoom>
            {
                new PokerRoom {
                    Name = "TestOne",
                    Users = usersRoomOne
                },
                new PokerRoom {
                    Name = "TestTwo",
                    Users = usersRoomtwo
                },
            };

            testSession = new PokerSession
            {
                Rooms = rooms,
            };

            return new PokerManager(testSession);
        }

        private PokerManager SetupOneUser(out PokerSession testSession)
        {
            List<PokerUser> usersRoomOne = new List<PokerUser>
            {
                new PokerUser {
                    Id = 1,
                    ClientSession = new Guid("6e1af4d7-866d-42a9-98c4-76e9e9973046"),
                    ConnectionId = "RandomCon",
                    Name = "Kees"
                }
            };

            List<PokerUser> usersRoomtwo = new List<PokerUser>
            {
                new PokerUser {
                    Id = 3,
                    ClientSession = new Guid("6f100f99-bb66-4fff-8f79-8bbb844f2355"),
                    ConnectionId = "ConTop",
                    Name = "Joost"
                }
            };


            List<PokerRoom> rooms = new List<PokerRoom>
            {
                new PokerRoom {
                    Name = "TestOne",
                    Users = usersRoomOne
                },
                new PokerRoom {
                    Name = "TestTwo",
                    Users = usersRoomtwo
                },
            };

            testSession = new PokerSession
            {
                Rooms = rooms,
            };

            return new PokerManager(testSession);
        }
    }
}
