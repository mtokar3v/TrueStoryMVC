using System.Collections.Generic;

namespace TrueStoryMVC.Models.ViewModels
{
    public class PageListModel<T, K>
    {
        public K Key { get; private set; }
        public IEnumerable<T> ModelsOnCurrentPage { get; private set; }
        public PageListScrollingModel PageListScrolling { get; private set; }

        public PageListModel(IEnumerable<T> modelsOnCurrentPage, PageListScrollingModel pageListScrolling, K key)
        {
            ModelsOnCurrentPage = modelsOnCurrentPage;
            PageListScrolling = pageListScrolling;
            Key = key;
        }
    }

    public class PageListModel<T>
    {
        public IEnumerable<T> ModelsOnCurrentPage { get; private set; }
        public PageListScrollingModel PageListScrolling { get; private set; }

        public PageListModel(IEnumerable<T> modelsOnCurrentPage, PageListScrollingModel pageListScrolling)
        {
            ModelsOnCurrentPage = modelsOnCurrentPage;
            PageListScrolling = pageListScrolling;
        }
    }
}
