namespace SearchingLibrary.Models {
    public class SearchableEntity<TId> {
        public TId Id { get; set; }
        public string SearchableProperty { get; set; }
    }

    public class SearchableEntity : SearchableEntity<int> {
        public static int Counter { get; private set; } = 1;

        public SearchableEntity(string searchableProperty) {
            this.Id = Counter++;
            this.SearchableProperty = searchableProperty;
        }

        public SearchableEntity(int id, string searchableProperty) {
            this.Id = id;
            this.SearchableProperty = searchableProperty;
        }
    }

    public class AbstractModel<TId> {
        public TId Id { get; set; }
    }
    public class AbstractModel : AbstractModel<int> { }
}
