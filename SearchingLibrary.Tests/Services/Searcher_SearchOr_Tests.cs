using FluentAssertions;
using SearchingLibrary.Models;
using SearchingLibrary.Service;
using System.Collections.Generic;
using Xunit;

namespace SearchingLibrary.Tests.Services {
    public class Searcher_SearchOr_Tests {

        private readonly static IEnumerable<SearchableEntity> Source = new List<SearchableEntity>(){
                new SearchableEntity(1,"папка"),
                new SearchableEntity(2,"папка другая"),
                new SearchableEntity(3,"папка чужая"),
                new SearchableEntity(4,"папка чужая другая"),
                new SearchableEntity(5,"непапка"),
                new SearchableEntity(6,"совсем иное имя"),
            };
        private static Searcher searcher = new Searcher();

        [Fact]
        public void SearchAnd_другая_чужая_returns_3_Items() {
            // Arrange
            var searchString = "другая чужая";
            var expected = new List<SearchableEntity> {
                new SearchableEntity(2,"папка другая"),
                new SearchableEntity(3,"папка чужая"),
                new SearchableEntity(4,"папка чужая другая"),
            };

            // Act
            var actual = searcher.SearchOr(Source, searchString);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SearchAnd_папка_другая_returns_6_Items() {
            // Arrange
            var searchString = "папка имя";
            var expected = new List<SearchableEntity> {
                new SearchableEntity(1,"папка"),
                new SearchableEntity(2,"папка другая"),
                new SearchableEntity(3,"папка чужая"),
                new SearchableEntity(4,"папка чужая другая"),
                new SearchableEntity(5,"непапка"),
                new SearchableEntity(6,"совсем иное имя"),
            };

            // Act
            var actual = searcher.SearchOr(Source, searchString);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SearchAnd_папкадругая_returns_empty() {
            // Arrange
            var searchString = "папкадругая";
            var expected = new List<SearchableEntity> {
            };

            // Act
            var actual = searcher.SearchOr(Source, searchString);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}