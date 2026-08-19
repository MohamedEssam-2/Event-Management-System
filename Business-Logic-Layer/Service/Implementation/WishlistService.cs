using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.WishlistDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Data_Access_Layer.Specifications.EventSpecifications;

namespace Business_Logic_Layer.Service.Implementation
{
    public class WishlistService(IUnitOfWork _unitOfWork, ICurrentUserService _currentUser,IMapper _mapper) : IWishlistService
    {
        public async Task<ServiceResponse<int>> CreateWishlist(int eventId)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var eventResult = await _unitOfWork.GetRepository<Event, int>().GetById(eventId);
            if (eventResult == null)
            {
                throw new EventNotFoundException(eventId);
            }
            if (eventResult.IsDeleted)
            {
                throw new BadRequestException("Cannot add a deleted event to wishlist.");
            }
            var wishlistSpec = new UserEventWishlistSpecification(userId, eventId);
            var existingWishlist = await _unitOfWork.GetRepository<Wishlist, int>().CountAsync(wishlistSpec);
            if (existingWishlist > 0)
            {
                throw new BadRequestException("User has already added this event to wishlist.");
            }
            var wishlist = new Wishlist
            {
                UserId = userId,
                EventId = eventId,
                CreatedBy = _currentUser.FullName,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.GetRepository<Wishlist, int>().Create(wishlist);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<int>
            {
                Success = true,
                Message = "Event added to wishlist successfully.",
                Data = wishlist.Id,
            };


        }



        public async Task<ServiceResponse<List<ReadWishlistDTO>>> GetWishlist()
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var wishlistSpec = new UserWishlistSpecification(userId);
            var wishlist = await _unitOfWork.GetRepository<Wishlist, int>().GetAll(wishlistSpec);
            var wishlistDTOs = _mapper.Map<List<ReadWishlistDTO>>(wishlist);
            return new ServiceResponse<List<ReadWishlistDTO>>
            {
                Success = true,
                Message = "Wishlist retrieved successfully.",
                Data = wishlistDTOs
            };
        }

        //public async Task<ServiceResponse<List<WishlistByEventDTO>>> GetWishlistByEventId(int EventId)
        //{
        //    var userId = _currentUser.UserId;
        //    if (userId == null)
        //    {
        //        throw new UnauthorizedException("User is not authenticated.");
        //    }
        //    var eventResult= await _unitOfWork.GetRepository<Event, int>().GetById(EventId);
        //    if (eventResult == null)
        //    {
        //        throw new EventNotFoundException(EventId);
        //    }
        //    var wishlistSpec = new UserEventWishlistSpecification(userId, EventId);
        //    var wishlist = await _unitOfWork.GetRepository<Wishlist, int>().GetAll(wishlistSpec);
        //    var wishlistDTOs = _mapper.Map<List<WishlistByEventDTO>>(wishlist);
        //    return new ServiceResponse<List<WishlistByEventDTO>>
        //    {
        //        Success = true,
        //        Message = "Wishlist retrieved successfully.",
        //        Data = wishlistDTOs
        //    };
        //}
        public async Task<ServiceResponse<bool>> DeleteWishlist(int id)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var wishlist = await _unitOfWork.GetRepository<Wishlist, int>().GetById(id);
            if (wishlist == null)
            {
                throw new NotFoundException($"Wishlist With This Id = {id} is not Found");
            }
            if (wishlist.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to delete this wishlist.");
            }
            wishlist.DeletedBy = _currentUser.FullName;
            wishlist.DeletedDate = DateTime.UtcNow;
            _unitOfWork.GetRepository<Wishlist, int>().Delete(wishlist);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<bool>()
            {
                Success = true,
                Message = "Wishlist deleted successfully.",
                Data = true
            };
        }

    }
}
