using Commons;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.UserArea.Interface;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.ViewModels.Search;
using Models.Class.Search;

namespace Service.UserArea
{
    public  class SearchService : ISearchService
    {

        private readonly IGenericRepository<DataEntities.Model.Search> _searchRepo;
        private readonly IGenericRepository<DataEntities.Model.SearchResult> _searchResultRepo;

        public SearchService(IGenericRepository<DataEntities.Model.Search> searchRepo,
                IGenericRepository<DataEntities.Model.SearchResult> searchResultRepo)
        {
            _searchRepo = searchRepo;
            _searchResultRepo = searchResultRepo;
        }


        /// <summary>
        /// Create a search
        /// </summary>
        /// <param name="Pattern"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int CreateSearch(string Pattern, int? UserId,int ResultsNumber=0)
        {
            int result = -1;
            try
            {
                DataEntities.Model.Search search = new DataEntities.Model.Search();
                search.UserId = UserId;
                search.Pattern = Pattern;
                search.ResultsNumber = ResultsNumber;
                search.Date = DateTime.UtcNow;
                _searchRepo.Add(search);
                if(_searchRepo.Save())
                {
                    result = search.Id;
                } 
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = "+ Pattern+ " and UserId = "+UserId);
            }
            return result;
        }


        public Search GetSearchById(int SearchId)
        {
            Search result = null;
            try
            {
                if(SearchId > 0)
                {
                    result = _searchRepo.Get(SearchId);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchId = " + SearchId);
            }
            return result;
        }


        public DisplaySearchViewModel GetDisplaySearchViewModel(string Pattern, int StartAt, int PageSize)
        {
            DisplaySearchViewModel model = new DisplaySearchViewModel();
            try
            {

                model.Pattern = Pattern;
                model.PageSize = PageSize;
                model.StartAt = StartAt;
                if (Pattern == null)
                    Pattern = "";
                Pattern = Pattern.ToLower();
                var Users = new List<SearchItem>();


                var Pages = new List<SearchItem>();



                model.SearchList.AddRange(Users);
                model.SearchList.AddRange(Pages);

                model.Count = model.SearchList.Count;
                model.SearchList = model.SearchList.OrderByDescending(a => a.Name).Skip(StartAt).Take(PageSize).ToList();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
        }

    }
}
