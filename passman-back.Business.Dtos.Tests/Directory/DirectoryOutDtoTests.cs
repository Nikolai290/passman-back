using FluentAssertions;
using System.Linq;
using Xunit;
using passman_back.Business.Dtos;
using System.Collections.Generic;
using passman_back.Domain.Core.DbEntities;
using passman_back.Domain.Core.Enums;

namespace passman_back.Business.Dtos.Tests.Directory {
    public class DirectoryOutDtoTests {

        public static readonly DirectoryOutDto directoryOutDto = new DirectoryOutDto() {
            Id = 1,
            Childrens = new List<DirectoryOutDto> {
                    new () {
                        Id = 2,
                        Childrens = new List<DirectoryOutDto> { }
                    },
                    new () {
                        Id = 3,
                        Childrens = new List<DirectoryOutDto>{
                            new () {
                                Id = 4,
                                Childrens = new List<DirectoryOutDto> { }
                            }
                        }
                    },
                    new  (){
                        Id = 5,
                        Childrens = new List<DirectoryOutDto> { }
                    }
                }
        };

        [Fact]
        public void ExcludeChildrenWhoHasNotInList_Testpass() {
            // Arrange
            DirectoryOutDto dto = new DirectoryOutDto() {
                Id = 1,
                Childrens = new List<DirectoryOutDto> {
                    new () {
                        Id = 2,
                        Childrens = new List<DirectoryOutDto> { }
                    },
                    new () {
                        Id=3,
                        Childrens = new List<DirectoryOutDto>{
                            new () {
                                Id = 4,
                                Childrens = new List<DirectoryOutDto> {
                                    new DirectoryOutDto() {
                                        Id = 6,
                                        Childrens = new List<DirectoryOutDto> { }
                                    }
                                }
                            }
                        }
                    },
                    new  (){
                        Id = 5,
                        Childrens = new List<DirectoryOutDto> { }
                    }
                }
            };
            long[] findIds = {1, 2, 3, 5, 6 };
            var expected = new DirectoryOutDto() {
                Id = 1,
                Childrens = new List<DirectoryOutDto> {
                    new () {
                        Id = 2,
                        Childrens = new List<DirectoryOutDto> { }
                    },
                    new () {
                        Id = 3,
                        Childrens = new List<DirectoryOutDto> { }
                    },
                    new  (){
                        Id = 5,
                        Childrens = new List<DirectoryOutDto> { }
                    }
                }
            };
            // Act
            dto.ExcludeChildrenWhoHasNotInList(findIds);
            // Assert
            dto.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void ContainsChildrenWithId_ReturnsTrue() {
            // Arrange
            long findId = 4;
            // Act
            var actual = directoryOutDto.ContainsChildrenWithId(findId);
            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void ContainsChildrenWithId_ReturnsFalse() {
            // Arrange
            long findId = 99;
            // Act
            var actual = directoryOutDto.ContainsChildrenWithId(findId);
            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void SetPermissionForAllChildren_test() {
            // Arrange
            var dto = new DirectoryOutDto() {
                Id = 1,
                Childrens = new List<DirectoryOutDto> {
                    new () {
                        Id = 2,
                        Childrens = new List<DirectoryOutDto> { }
                    },
                    new () {
                        Id = 3,
                        Childrens = new List<DirectoryOutDto>{
                            new () {
                                Id = 4,
                                Childrens = new List<DirectoryOutDto> { }
                            }
                        }
                    },
                    new  (){
                        Id = 5,
                        Childrens = new List<DirectoryOutDto> { }
                    }
                }
            };
            var relations = new List<UserGroupDirectoryRelation>(){
                new(){
                    Directory = new(){Id = 1},
                    Permission = Permission.FullAccess
                },
                new(){
                    Directory = new(){Id = 2},
                    Permission = Permission.ReadOnly
                },
                new(){
                    Directory = new(){Id = 3},
                    Permission = Permission.ReadOnly
                },
                new(){
                    Directory = new(){Id = 4},
                    Permission = Permission.FullAccess
                },
                new(){
                    Directory = new(){Id = 5},
                    Permission = Permission.None
                },
            };
            var expected = new DirectoryOutDto() {
                Id = 1,
                Permission = Permission.FullAccess,
                Childrens = new List<DirectoryOutDto> {
                    new () {
                        Id = 2,
                    Permission = Permission.ReadOnly,
                        Childrens = new List<DirectoryOutDto> { }
                    },
                    new () {
                        Id = 3,
                        Permission = Permission.ReadOnly,
                        Childrens = new List<DirectoryOutDto>{
                            new () {
                                Id = 4,
                                Permission = Permission.FullAccess,
                                Childrens = new List<DirectoryOutDto> { }
                            }
                        }
                    },
                    new  (){
                        Id = 5,
                        Permission = Permission.None,
                        Childrens = new List<DirectoryOutDto> { }
                    }
                }
            };
            // Act
            dto.SetPermissionForAllChildren(relations);
            // Assert
            dto.Should().BeEquivalentTo(expected);
        }
    }
}