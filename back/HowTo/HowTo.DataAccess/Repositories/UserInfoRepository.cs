using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATI.Services.Common.Behaviors;
using HowTo.Entities;
using HowTo.Entities.UserInfo;
using HowTo.Entities.ViewedEntity;
using Microsoft.EntityFrameworkCore;

namespace HowTo.DataAccess.Repositories;

public class UserInfoRepository
{
    private readonly IDbContextFactory<ApplicationContext> _dbContextFactory;
    public UserInfoRepository(IDbContextFactory<ApplicationContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<OperationResult<UserUniqueInfoDto>> SetLastReadCourseIdAsync(User user, int courseId)
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();
            var userInfoDto = await db.UserUniqueInfoContext.FirstOrDefaultAsync(v => v.Id == user.Id);
            if (userInfoDto == null)
            {
                userInfoDto = new UserUniqueInfoDto
                {
                    Id = user.Id,
                    LastReadCourseId = courseId
                };
                await db.UserUniqueInfoContext.AddAsync(userInfoDto);
            }
            else
            {
                userInfoDto.LastReadCourseId = courseId;
            }

            await db.SaveChangesAsync();
            return new(userInfoDto);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<OperationResult<UserUniqueInfoDto>> AddApprovedViewAsync(User user, AddApprovedViewRequest request)
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();
            var userInfoDto = await db.UserUniqueInfoContext
                .Include(d=>d.ApprovedViewArticleIds)
                .SingleOrDefaultAsync(v => v.Id == user.Id);
            
            if (userInfoDto == null)
            {
                userInfoDto = new UserUniqueInfoDto
                {
                    Id = user.Id,
                    ApprovedViewArticleIds = new List<ViewedEntity> { new(request.CourseId, request.ArticleId)}
                };
                await db.UserUniqueInfoContext.AddAsync(userInfoDto);
            }
            else
            {
                if (!userInfoDto.ApprovedViewArticleIds.Any(v => v.CourseId == request.CourseId && v.ArticleId == request.ArticleId))
                    userInfoDto.ApprovedViewArticleIds.Add(new(request.CourseId, request.ArticleId));
            }

            await db.SaveChangesAsync();
            return new(userInfoDto);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<OperationResult<UserUniqueInfoDto>> GetUserInfoAsync(User user)
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();
            var userInfoDto = await db.UserUniqueInfoContext
                .Include(d=>d.ApprovedViewArticleIds)
                .SingleOrDefaultAsync(u => u.Id == user.Id);
            if (userInfoDto == null)
                return new(ActionStatus.BadRequest, "user_not_found", $"user with id {user.Id} not found");
            

            return new(userInfoDto);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }
}