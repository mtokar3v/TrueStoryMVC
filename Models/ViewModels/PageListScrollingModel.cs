using System;

namespace TrueStoryMVC.Models.ViewModels
{
    public class PageListScrollingModel
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public PageListScrollingModel(int currentPage, int totalModelsCount, int maxModelsOnPage)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling((double) totalModelsCount / maxModelsOnPage);
        }

        public bool canScrollNext() => CurrentPage < TotalPages;
        public bool canScrollPrevious() => CurrentPage > 1;
    }
}
