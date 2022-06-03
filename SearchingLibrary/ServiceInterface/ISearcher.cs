using SearchingLibrary.Models;

namespace SearchingLibrary.ServiceInterface {
    public interface ISearcher {
        IEnumerable<SearchableEntity<TId>> SearchAnd<TId>(IEnumerable<SearchableEntity<TId>> source, string search);
        IEnumerable<SearchableEntity<TId>> SearchOr<TId>(IEnumerable<SearchableEntity<TId>> source, string search);
    }
}
