using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Logic_Layer.DTO.ReviewDTO;
using Business_Logic_Layer.Exceptions;
using Business_Logic_Layer.Exceptions.UserExceptions;
using Business_Logic_Layer.Service.Interface;
using Data_Access_Layer.Models;
using Data_Access_Layer.Repository.Interface;
using Data_Access_Layer.Specifications.EventSpecifications;
using Microsoft.AspNetCore.Identity;

namespace Business_Logic_Layer.Service.Implementation
{
    public class ReviewService(IUnitOfWork _unitOfWork , IMapper _mapper , ICurrentUserService _currentUser , UserManager<ApplicationUser> _userManager) : IReviewService
    {
        public async Task<ServiceResponse<int>> CreateReview(CreateReviewDTO dto)
        {
            var userId= _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            var eventRepo = await _unitOfWork.GetRepository<Event, int>().GetById(dto.EventId);
            if (eventRepo == null)
            {
                throw new EventNotFoundException(dto.EventId);
            }
            if(eventRepo.IsDeleted)
            {
                throw new BadRequestException("Cannot review a deleted event.");
            }
            if (eventRepo.Status ==Data_Access_Layer.Enum.EventStatus.Canceled)
            {
                throw new BadRequestException("Cannot review a canceled event.");
            }
            var registrationSpec= new  UserEventRegistrationSpecification(userId, dto.EventId);
            var registration = await _unitOfWork.GetRepository<Registration, int>().CountAsync(registrationSpec);
            if (registration == 0)
            {
                throw new BadRequestException("User must be registered for the event to leave a review.");
            }
            var reviewSpec = new UserEventReviewSpecification(userId, dto.EventId);
            var existingReview = await _unitOfWork.GetRepository<Review, int>().CountAsync(reviewSpec);
            if (existingReview > 0)
            {
                throw new BadRequestException("User has already reviewed this event.");
            }
            if (eventRepo.Date > DateTime.UtcNow)
            {
                throw new BadRequestException("You can review the event after it has taken place.");
            }
            var review = _mapper.Map<Review>(dto);
            review.UserId = userId;
            review.CreatedBy = _currentUser.FullName;
            review.CreatedAt = DateTime.UtcNow;
            await _unitOfWork.GetRepository<Review, int>().Create(review);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<int>
            {
                Data = review.Id,
                Message = "Review created successfully.",
                Success = true
            };


        }

        public async Task<ServiceResponse<GetReviewDTO>> GetReviewById(int id)
        {
            var spec = new ReviewByIdSpecification(id);
            var review = await _unitOfWork.GetRepository<Review, int>().GetById(spec);
            if (review == null)
            {
                throw new NotFoundException($"Review With This Id = {id} is not Found");
            }
            var reviewDTO = _mapper.Map<GetReviewDTO>(review);
            return new ServiceResponse<GetReviewDTO>
            {
                Success = true,
                Message = "Review retrieved successfully.",
                Data = reviewDTO,
            };
        }


        public async Task<ServiceResponse<GetReviewDTO>> UpdateReview(int id, UpdateReviewDTO dto)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            var review = await _unitOfWork.GetRepository<Review,int>().GetById(id);
            if (review == null)
            {
                throw new NotFoundException($"Review With This Id = {id} is not Found");
            }
            if (review.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to update this review.");
            }
            if (dto.Rating.HasValue)
            {
                review.Rating = dto.Rating.Value;
            }
            if (dto.Comment != null)
            {
                review.Comment = dto.Comment;
            }
            var reviewDto = _mapper.Map<GetReviewDTO>(review);
             _unitOfWork.GetRepository<Review, int>().Update(review);
            review.UpdatedAt = DateTime.UtcNow;
            review.UpdatedBy = _currentUser.FullName;
            await _unitOfWork.SaveChangesAsync();

            return new ServiceResponse<GetReviewDTO>
            {
                Success = true,
                Message = "Review updated successfully.",
                Data = reviewDto
            };
        }
        public async Task<ServiceResponse<List<GetReviewDTO>>> MyReviews()
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var spec = new MyReviewsSpecification(userId);
            var reviews = await _unitOfWork.GetRepository<Review, int>().GetAll(spec);
            var reviewDTOs = _mapper.Map<List<GetReviewDTO>>(reviews);
            return new ServiceResponse<List<GetReviewDTO>>
            {
                Success = true,
                Message = "Reviews retrieved successfully.",
                Data = reviewDTOs
            };
        }

        public async Task<ServiceResponse<bool>> DeleteReview(int id)
        {
            var userId = _currentUser.UserId;
            if (userId == null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }
            var review = await _unitOfWork.GetRepository<Review, int>().GetById(id);
            if (review == null)
            {
                throw new NotFoundException($"Review With This Id = {id} is not Found");
            }
            if (review.UserId != userId)
            {
                throw new UnauthorizedException("You are not authorized to delete this review.");
            }
            review.DeletedBy = _currentUser.FullName;
            review.DeletedDate = DateTime.UtcNow;
            _unitOfWork.GetRepository<Review, int>().Delete(review);
            await _unitOfWork.SaveChangesAsync();
            return new ServiceResponse<bool>()
            {
                Success = true,
                Message = "Review deleted successfully.",
                Data = true
            };
        }

        public async Task<ServiceResponse<List<GetReviewDTO>>> GetAllReviewsByEventId(int eventId)
        {
            var eventEntity = await _unitOfWork.GetRepository<Event, int>().GetById(eventId);
            if (eventEntity == null)
            {
                throw new EventNotFoundException(eventId);
            }
            var spec = new ReviewsByEventIdSpecification(eventId);
            var reviews = await _unitOfWork.GetRepository<Review, int>().GetAll(spec);
            var reviewDTOs = _mapper.Map<List<GetReviewDTO>>(reviews);
            return new ServiceResponse<List<GetReviewDTO>>
            {
                Success = true,
                Message = "Reviews retrieved successfully.",
                Data = reviewDTOs
            };
        }
    }
}
