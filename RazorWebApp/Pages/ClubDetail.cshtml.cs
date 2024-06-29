using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Services.IService;
using System.Net.WebSockets;

namespace WebAppRazor.Pages
{
    public class ClubDetailModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _service;
        public Club Club { get; set; }
        public List<Slot> SlotList { get; set; }
        public List<Review> ReviewList { get; set; }
        public List<Review> FilterReviewList { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public ClubDetailModel (IServiceManager service)
        {
            _service = service;
        }

        private void Paging (int page = 0)
        {
            const int PageSize = 10;  // Set the number of items per page

            //if (!string.IsNullOrWhiteSpace(searchString))
            //{
            //    FilterReviewList = searchProperty switch
            //    {
            //        "AccountId" => ReviewList.Where(e => e.UserId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
            //        "Username" => ReviewList.Where(e => e.Username.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
            //        "ClubId" => ReviewList.Where(e => e.ClubManageId.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
            //        "ClubName" => ReviewList.Where(e => e.ClubManage.ClubName.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
            //        _ => ReviewList,
            //    };
            //}

            //if (!string.IsNullOrWhiteSpace(sortProperty))
            //{
            //    FilterReviewList = sortProperty switch
            //    {
            //        "AccountId" => sortOrder == -1 ? FilterReviewList.OrderByDescending(e => e.UserId).ToList() : sortOrder == 1 ? FilterReviewList.OrderBy(e => e.UserId).ToList() : FilterReviewList,
            //        "Username" => sortOrder == -1 ? FilterReviewList.OrderByDescending(e => e.Username).ToList() : sortOrder == 1 ? FilterReviewList.OrderBy(e => e.Username).ToList() : FilterReviewList,
            //        "ClubId" => sortOrder == -1 ? FilterReviewList.OrderByDescending(e => e.ClubManageId).ToList() : sortOrder == 1 ? FilterReviewList.OrderBy(e => e.ClubManageId).ToList() : FilterReviewList,
            //        "ClubName" => sortOrder == -1 ? FilterReviewList.OrderByDescending(e => e.ClubManage.ClubName).ToList() : sortOrder == 1 ? FilterReviewList.OrderBy(e => e.ClubManage.ClubName).ToList() : FilterReviewList,
            //        _ => FilterReviewList,
            //    };
            //}

            // Pagination logic
            page = page == 0 ? 1 : page;
            CurrentPage = page;
            TotalPages = (int)Math.Ceiling(ReviewList.Count / (double)PageSize);
            FilterReviewList = ReviewList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }

        public IActionResult OnGet (int? id)
        {
            LoadAccountFromSession();
            if (id.HasValue)
            {
                InitialData(id.Value);
                int page = Convert.ToInt32(Request.Query["page"]);
                Paging(page);
            }

            return Page();
        }

        private void InitialData (int id)
        {
            Club = _service.ClubService.GetClubById(id);
            SlotList = _service.SlotService.GetAllByClubId(id);
            ReviewList = _service.ReviewService.GetAllByClubId(id);
        }
    }
}
