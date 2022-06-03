using FluentAssertions;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Core.Enums;
using System.Collections.Generic;
using Xunit;

namespace passman_back.Domain.Core.Tests.DbEntities {
    public class UserTests {
        public static UserGroup alfa = new UserGroup(){
            Id = 1,
            Name = "Альфачи",
            Relations = new List<UserGroupDirectoryRelation>{
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 1},
                    Permission = Permission.FullAccess
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 2},
                    Permission = Permission.FullAccess
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 4},
                    Permission = Permission.FullAccess
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 5},
                    Permission = Permission.ReadOnly
                },
            }
        };

        public static UserGroup beta = new UserGroup(){
            Id = 2,
            Name = "Чмошники",
            Relations = new List<UserGroupDirectoryRelation>{
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 1},
                    Permission = Permission.ReadOnly
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 3},
                    Permission = Permission.ReadOnly
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 4},
                    Permission = Permission.None
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 5},
                    Permission = Permission.None
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 6},
                    Permission = Permission.None
                },

            }
        };

        public static User currentUser = new User(){
            Id = 1,
            UserGroups = new List<UserGroup>{
                alfa, beta
            }
        };


        [Fact]
        public void GetHashSetRelations_pass() {
            // Arrange
            var expected = new HashSet<UserGroupDirectoryRelation>(){
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 1},
                    Permission = Permission.FullAccess
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 2},
                    Permission = Permission.FullAccess
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 3},
                    Permission = Permission.ReadOnly
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 4},
                    Permission = Permission.FullAccess
                },
                new UserGroupDirectoryRelation(){
                    Directory = new(){Id = 5},
                    Permission = Permission.ReadOnly
                },
            };
            // Act
            var actual = currentUser.GetHashSetRelations();

            // Assert

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetPermissionForDirectory_ReturnsFullAccess() {
            // Act & Assert
            currentUser.GetPermissionForDirectory(1).Should().Be(Permission.FullAccess);
        }

        [Fact]
        public void GetPermissionForDirectory_ReturnsReadonly() {
            // Act & Assert
            currentUser.GetPermissionForDirectory(5).Should().Be(Permission.ReadOnly);
        }

        [Fact]
        public void GetPermissionForDirectory_ReturnsNone() {
            // Act & Assert
            currentUser.GetPermissionForDirectory(6).Should().Be(Permission.None);
        }

        [Fact]
        public void GetPermissionForDirectory_RelationNotExist_ReturnsNone() {
            // Act & Assert
            currentUser.GetPermissionForDirectory(9999).Should().Be(Permission.None);
        }

    }
}