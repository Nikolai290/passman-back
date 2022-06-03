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
    public class User_HasFullAccessToPasscard_Tests {
        [Fact]
        public void HasFullAccessToPasscard_admin_returnsTrue() {
            // Arrange
            var user = new User(){
                Role = Role.Admin
            };

            // Act & Assert
            user.HasFullAccessToPasscard(999999).Should().BeTrue();
        }

        [Fact]
        public void HasFullAccessToPasscard_userHasFullAccess_returnsTrue() {
            // Arrange
            var user = new User(){
                Role = Role.User,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>(){
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=1
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act & Assert
            user.HasFullAccessToPasscard(1).Should().BeTrue();
        }

        [Fact]
        public void HasFullAccessToPasscard_userHasReadOnly_returnsFalse() {
            // Arrange
            var user = new User(){
                Role = Role.User,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>(){
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.ReadOnly,
                                Directory = new Directory(){
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=1
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act & Assert
            user.HasFullAccessToPasscard(1).Should().BeFalse();
        }

        [Fact]
        public void HasFullAccessToPasscard_userHasFullAccess_PasscardIsDeleted_returnsFalse() {
            // Arrange
            var user = new User(){
                Role = Role.User,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>(){
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=1,
                                            IsDeleted=true
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act & Assert
            user.HasFullAccessToPasscard(1).Should().BeFalse();
        }


        [Fact]
        public void HasFullAccessToPasscard_userHasFullAccess_DirectoryIsDeleted_returnsFalse() {
            // Arrange
            var user = new User(){
                Role = Role.User,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>(){
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    IsDeleted=true,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=1,
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act & Assert
            user.HasFullAccessToPasscard(1).Should().BeFalse();
        }

        [Fact]
        public void HasFullAccessToPasscard_userTwoGroups_returnsTrue() {
            // Arrange
            var user = new User(){
                Role = Role.User,
                UserGroups = new List<UserGroup>{
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>(){
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    IsDeleted=true,
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=1,
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new UserGroup(){
                        Relations = new List<UserGroupDirectoryRelation>(){
                            new UserGroupDirectoryRelation(){
                                Permission = Permission.FullAccess,
                                Directory = new Directory(){
                                    Passcards = new List<Passcard>(){
                                        new Passcard(){
                                            Id=1,
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act & Assert
            user.HasFullAccessToPasscard(1).Should().BeTrue();
        }


    }
}
