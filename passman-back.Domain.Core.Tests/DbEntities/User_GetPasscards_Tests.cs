using FluentAssertions;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace passman_back.Domain.Core.Tests.DbEntities {
    public class User_GetPasscards_Tests {
        [Fact]
        public void GetPasscards_HasFullAccess_ById_ReturnsOnePasscard() {
            // Arrange
            var user = new User(){
                Id = 1,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>{
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    Id=1,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                        Id=101,
                                        }
                                    },
                                }
                            },
                        }
                    }
                }
            };
            var expected = new List<Passcard>(){
                new Passcard(){
                    Id=101,
                }
            };

            // Act
            var actual = user.GetPasscardsBy(1);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetPasscards_HasFullAccess_ReturnsOnePasscard() {
            // Arrange
            var user = new User(){
                Id = 1,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>{
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    Id=1,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                        Id=101,
                                        }
                                    },
                                }
                            },
                        }
                    }
                }
            };
            var expected = new List<Passcard>(){
                new Passcard(){
                    Id=101,
                }
            };

            // Act
            var actual = user.GetPasscardsBy();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetPasscards_DirectoryIsDeleted_ReturnsEmpty() {
            // Arrange
            var user = new User(){
                Id = 1,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>{
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    IsDeleted=true,
                                    Id=1,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                        Id=101,
                                        }
                                    },
                                }
                            },
                        }
                    }
                }
            };
            var expected = new List<Passcard>();

            // Act
            var actual = user.GetPasscardsBy();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetPasscards_UserGroupIsDeleted_ReturnsEmpty() {
            // Arrange
            var user = new User(){
                Id = 1,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        IsDeleted=true,
                        Relations = new List<UserGroupDirectoryRelation>{
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    Id=1,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                        Id=101,
                                        }
                                    },
                                }
                            },
                        }
                    }
                }
            };
            var expected = new List<Passcard>();

            // Act
            var actual = user.GetPasscardsBy();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetPasscards_PasscardIsDeleted_ReturnsEmpty() {
            // Arrange
            var user = new User(){
                Id = 1,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>{
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    Id=1,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=101,
                                            IsDeleted=true,
                                        }
                                    },
                                }
                            },
                        }
                    }
                }
            };
            var expected = new List<Passcard>();

            // Act
            var actual = user.GetPasscardsBy();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetPasscards_HasThreeRelationsInTwoGroups_ReturnsTwoPasscards() {
            // Arrange
            var user = new User(){
                Id = 1,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>{
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    Id=1,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=101,
                                        }
                                    },
                                }
                            },
                        }
                    },
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>{
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.ReadOnly,
                                Directory = new Directory(){
                                    Id=2,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=102,
                                        }
                                    },
                                }
                            },
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.None,
                                Directory = new Directory(){
                                    Id=3,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=103,
                                        }
                                    },
                                }
                            },
                        }
                    }
                }
            };
            var expected = new List<Passcard>(){
                new Passcard{
                    Id=101,
                },
                new Passcard{
                    Id=102,
                },
            };

            // Act
            var actual = user.GetPasscardsBy();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetPasscards_HasTwoDirectoriesWithOneSamePasscard_ReturnsOnePasscard() {
            // Arrange
            var user = new User(){
                Id = 1,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>{
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.ReadOnly,
                                Directory = new Directory(){
                                    Id=2,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=102,
                                        }
                                    },
                                }
                            },
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    Id=3,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=102,
                                        }
                                    },
                                }
                            },
                        }
                    }
                }
            };
            var expected = new List<Passcard>(){
                new Passcard{
                    Id=102,
                },
            };

            // Act
            var actual = user.GetPasscardsBy();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
