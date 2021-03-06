﻿namespace RPM.Services.Management.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Data.Models.Enums;
    using RPM.Services.Management.Models;

    using static RPM.Common.GlobalConstants;

    public class OwnerRequestService : IOwnerRequestService
    {
        private readonly ApplicationDbContext context;

        public OwnerRequestService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<OwnerIndexRequestsServiceModel>> GetRequestsAsync(string id)
        {
            var requestsFromDb = await this.context.Requests
                .Where(requests => requests.Home.OwnerId == id)
                .OrderByDescending(r => r.Date)
                .ThenByDescending(r => r.Date.TimeOfDay)
                .Select(r => new OwnerIndexRequestsServiceModel
                {
                    Id = r.Id,
                    Date = r.Date.ToString("dd/MM/yyyy h:mm tt"),
                    Type = r.Type.ToString(),
                    FullName = string.Format(DashboardRequestFullName, r.User.FirstName, r.User.LastName),
                    Status = r.Status.ToString(),
                    Location = string.Format(DashboardRequestLocation, r.Home.City.Name, r.Home.Address),
                })
                .ToListAsync();

            return requestsFromDb;
        }

        public async Task<IEnumerable<OwnerAllRequestsServiceModel>>
            GetAllRequestsWthDetailsAsync(string id)
        {
            var requests = await this.context.Requests
                .Where(r => r.Home.OwnerId == id)
                .Select(r => new OwnerAllRequestsServiceModel
                {
                    Id = r.Id,
                    Date = r.Date.ToString(DateFormatWithTime),
                    Type = r.Type.ToString(),
                    FullName = string.Format(DashboardRequestFullName, r.User.FirstName, r.User.LastName),
                    Username = r.User.UserName,
                    Email = r.User.Email,
                    Phone = r.User.PhoneNumber,
                    Status = r.Status.ToString(),
                    Location = string.Format(DashboardRequestLocation, r.Home.City.Name, r.Home.Address),
                })
                .ToListAsync();

            return requests;
        }

        public async Task<Request> ApproveRequestAsync(string id)
        {
            var request = await this.context.Requests
                            .Where(r => r.Id == id)
                            .FirstOrDefaultAsync();

            if (request == null)
            {
                return null;
            }

            request.Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), Approved);
            await this.context.SaveChangesAsync();

            return request;
        }

        public async Task<bool> RejectRequestAsync(string id)
        {
            var request = await this.context.Requests
                            .Where(r => r.Id == id)
                            .FirstOrDefaultAsync();

            if (request == null)
            {
                return false;
            }

            request.Status = (RequestStatus)Enum.Parse(typeof(RequestStatus), Rejected);
            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<byte[]> GetFileAsync(string requestId)
        {
            var file = await this.context.Requests
                .Where(r => r.Id == requestId)
                .Select(r => r.Document)
                .FirstOrDefaultAsync();

            return file;
        }

        public async Task<OwnerRequestInfoServiceModel> GetRequestInfoAsync(string requestId)
        {
            var request = await this.context.Requests
                .Where(r => r.Id == requestId)
                .Select(r => new OwnerRequestInfoServiceModel
                {
                    UserId = r.User.Id,
                    UserFirstName = r.User.FirstName,
                    UserLastName = r.User.LastName,
                    RequestType = r.Type.ToString(),
                    Document = r.Document,
                })
                .FirstOrDefaultAsync();

            return request;
        }

        public async Task<OwnerRequestDetailsServiceModel> GetRequestDetailsAsync(string requestId)
        {
            var home = await this.context.Requests
                .Include(h => h.Home.City)
                .Include(h => h.Home.City.Country)
                .Include(h => h.Home.Images)
                .Where(r => r.Id == requestId)
                .Select(r => r.Home)
                .FirstOrDefaultAsync();

            var homeInfo = new OwnerRequestListingDetailsServiceModel
            {
                Id = home.Id,
                Name = home.Name,
                City = home.City.Name,
                Address = home.Address,
                Country = home.City.Country.Name,
                Description = home.Description,
                Price = home.Price,
                Status = home.Status,
                Category = home.Category,
                Image = home.Images.Select(i => i.PictureUrl).FirstOrDefault(),
            };

            var request = await this.context.Requests
                .Where(r => r.Id == requestId)
                .Select(r => new OwnerRequestDetailsServiceModel
                {
                    Id = r.Id,
                    Date = r.Date.ToString("dd/MM/yyyy h:mm tt"),
                    UserFirstName = r.User.FirstName,
                    UserLastName = r.User.LastName,
                    Email = r.User.Email,
                    Phone = r.User.PhoneNumber,
                    RequestType = r.Type.ToString(),
                    Status = r.Status,
                    Message = r.Message,
                    About = r.User.About,
                    Document = r.Document,
                    HomeInfo = homeInfo,
                })
                .FirstOrDefaultAsync();

            return request;
        }
    }
}
