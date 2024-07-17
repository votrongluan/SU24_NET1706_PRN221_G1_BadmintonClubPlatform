using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BusinessObjects.Dtos.Club;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using DataAccessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.IService;
using WebAppRazor.Constants;

namespace WebAppRazor.Pages.Staff
{
    public class ClubManageModel : AuthorPageServiceModel
    {
        private readonly IServiceManager _serviceManager;

        public ClubManageModel(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [BindProperty]
        public ClubDto ClubDto { get; set; }

        public int c;

        [BindProperty]
        public int CityId { get; set; }

        [BindProperty]
        public int DistrictId { get; set; }

        public SelectList Cities { get; set; }
        public SelectList Districts { get; set; }

        public string Message { get; set; }

        [BindProperty]
        public List<int> SelectedBookingTypes { get; set; }

        public List<SelectListItem> BookingTypes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Authorize
            LoadAccountFromSession();
            var navigatePage = GetNavigatePageByAllowedRole(AccountRoleEnum.Staff.ToString());

            if (!string.IsNullOrWhiteSpace(navigatePage))
            {
                return RedirectToPage(navigatePage);
            }

            // Set and clear the message
            if (!string.IsNullOrWhiteSpace(Message))
            {
                Message = string.Empty;
            }

            if (TempData.ContainsKey("Message"))
            {
                Message = TempData["Message"].ToString();
            }

            var bookingTypes = _serviceManager.BookingTypeService.GetAllBookingTypes().ToList();
            BookingTypes = bookingTypes.Select(bt => new SelectListItem
            {
                Value = bt.BookingTypeId.ToString(),
                Text = bt.Description
            }).Where(e => e.Value != ((int)BookingTypeEnum.LichThiDau).ToString()).ToList();
            

            if (LoginedAccount.ClubManageId != null)
            {
                int id = (int)LoginedAccount.ClubManageId;
                var Club = _serviceManager.ClubService.GetClubByIdReal(id);
                if (Club.Status == false)
                {
                    TempData["Message"] = $"{MessagePrefix.INFO}Đơn đăng ký Club của bạn đang được xử lý";

                    // Set and clear the message
                    if (!string.IsNullOrWhiteSpace(Message))
                    {
                        Message = string.Empty;
                    }

                    if (TempData.ContainsKey("Message"))
                    {
                        Message = TempData["Message"].ToString();
                    }
                    c = 1;
                    return Page();
                }

                ClubDto = _serviceManager.ClubService.ToDto(Club);
                var cityService = _serviceManager.CityService.GetAllCities().ToList();
                Cities = new SelectList(cityService, "CityId", "CityName");
                CityId = Club.District.CityId; // Set the initial CityId
                var districtService = _serviceManager.DistrictService.GetAllDistricts().ToList();
                Districts = new SelectList(districtService.Where(d => d.CityId == CityId), "DistrictId", "DistrictName");
                DistrictId = ClubDto.DistrictId; // Set the initial DistrictId

                SelectedBookingTypes = _serviceManager.AvailableBookingTypeService.GetAvailableBookingTypes()
                    .Where(abt => abt.ClubId == ClubDto.ClubId)
                    .Select(abt => abt.BookingTypeId)
                    .ToList();
            }
            else
            {
                var cityService = _serviceManager.CityService.GetAllCities().ToList();
                Cities = new SelectList(cityService, "CityId", "CityName");

                var districtService = _serviceManager.DistrictService.GetAllDistricts().ToList();
                Districts = new SelectList(districtService.Where(d => d.CityId == CityId), "DistrictId", "DistrictName");

                // Fetch selected booking types for the club
                SelectedBookingTypes = _serviceManager.AvailableBookingTypeService.GetAvailableBookingTypes()
                    .Select(abt => abt.BookingTypeId)
                    .ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSaveClubAsync()
        {
            LoadAccountFromSession();
            int id = (int)LoginedAccount.ClubManageId;

            // Update club properties with the values from the form
            ClubDto.ClubId = id;

            try
            {
                var existingClub = _serviceManager.ClubService.GetClubById(id);

                ClubDto.TotalStar = existingClub.TotalStar;
                ClubDto.TotalReview = existingClub.TotalReview;
                ClubDto.DefaultPricePerHour = existingClub.DefaultPricePerHour;

                
                var club = _serviceManager.ClubService.ToEntity(ClubDto);

                _serviceManager.ClubService.UpdateClub(club);

                // Delete existing booking types for the club
                List<AvailableBookingType> availableBookingTypes = _serviceManager.AvailableBookingTypeService.GetAvailableBookingTypes()
                    .Where(abt => abt.ClubId == id)
                    .ToList();

                foreach (var e in availableBookingTypes)
                {
                    _serviceManager.AvailableBookingTypeService.DeleteAvailableBookingType(e.AvailableBookingTypeId);
                }

                // Add new booking types based on the selected booking types
                foreach (var bookingTypeId in SelectedBookingTypes)
                {
                    var newAvailableBookingType = new AvailableBookingType
                    {
                        ClubId = id,
                        BookingTypeId = bookingTypeId
                    };
                    _serviceManager.AvailableBookingTypeService.AddAvailableBookingType(newAvailableBookingType);
                }

                TempData["Message"] = $"{MessagePrefix.SUCCESS}Câu lạc bộ đã được cập nhật thành công.";
                return RedirectToPage("ClubManage");
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Có lỗi xảy ra khi cập nhật câu lạc bộ: {ex.Message}";
                return Page();
            }
        }

        public async Task<JsonResult> OnGetGetDistricts(int cityId)
        {
            var districtService = _serviceManager.DistrictService.GetAllDistricts().ToList();
            var districts = districtService.Where(d => d.CityId == cityId).Select(d => new SelectListItem
            {
                Value = d.DistrictId.ToString(),
                Text = d.DistrictName
            });

            return new JsonResult(districts);
        }

        private bool ClubExists(int id)
        {
            return _serviceManager.ClubService.GetAllClubs().Any(e => e.ClubId == id);
        }

        public async Task<IActionResult> OnPostAddClubAsync()
        {
            LoadAccountFromSession();

            try
            {
                ClubDto.Status = false;
                ClubDto.TotalReview = 0;
                ClubDto.TotalStar = 0;

                var club = _serviceManager.ClubService.ToEntity(ClubDto);
                _serviceManager.ClubService.AddClub(club);

                Account owner = LoginedAccount;
                owner.ClubManageId = club.ClubId;
                _serviceManager.AccountService.UpdateStaffAccount(owner);

                UpdateAccountSession(owner);

                // Add booking types for the new club
                foreach (var bookingTypeId in SelectedBookingTypes)
                {
                    var newAvailableBookingType = new AvailableBookingType
                    {
                        ClubId = ClubDto.ClubId,
                        BookingTypeId = bookingTypeId
                    };
                    _serviceManager.AvailableBookingTypeService.AddAvailableBookingType(newAvailableBookingType);
                }

                return RedirectToPage("ClubManage");
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"{MessagePrefix.ERROR}Có lỗi xảy ra khi đăng ký câu lạc bộ: {ex.Message}";
                return Page();
            }
        }
    }
}
