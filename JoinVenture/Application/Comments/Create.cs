using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Application.Core;
using Application.Interface;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
    public class Create
    {
        public class Command : IRequest<Result<CommentDto>>
        {
            public string Body { get; set; }
            public string ActivityId { get; set; }
        }

        // public class CommandValidator:AbstractValidator<Command>
        // {

        // }

        public class Handler : IRequestHandler<Command, Result<CommentDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            //wait to add IUserAccessor
            public Handler(DataContext context,IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;

            }

            public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
            {

                if (int.TryParse(request.ActivityId, out int activityId))
                {
                    var activity = await _context.Activities.FindAsync(activityId);
                    var user = await _context.Users.Include(u => u.Photos).SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                    Console.WriteLine("Found the user~~~~~~~~~~~~~~~~" + user);

                    var comment  = new Comment
                    {
                        Author = user,
                        Activity = activity,
                        Body = request.Body
                    };

                    activity.Comments.Add(comment);

                    var success = await _context.SaveChangesAsync() > 0;

                    if(success) return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));

                    return Result<CommentDto>.Failure("Failed to add comment");

                }
                
                else
                {
                    return null;
                }



            }
        }
    }
}