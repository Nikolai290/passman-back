using SearchingLibrary.Models;
using SearchingLibrary.ServiceInterface;

namespace SearchingLibrary.Service {
    public class Searcher : ISearcher {

        public IEnumerable<SearchableEntity<TId>> SearchAnd<TId>(IEnumerable<SearchableEntity<TId>> source, string search) {
            if (string.IsNullOrEmpty(search)) { return source; }

            var words = search.ToLower().Split(' ');

            var result = source.Where(item => words.All(word =>item.SearchableProperty.ToLower().Contains(word.Trim())));

            return result;
        }

        public IEnumerable<SearchableEntity<TId>> SearchOr<TId>(IEnumerable<SearchableEntity<TId>> source, string search) {
            if (string.IsNullOrEmpty(search)) { return source; }

            var words = search.ToLower().Split(' ');

            var result = source.Where(item => words.Any(word =>item.SearchableProperty.ToLower().Contains(word.Trim())));

            return result;
        }
    }
}
