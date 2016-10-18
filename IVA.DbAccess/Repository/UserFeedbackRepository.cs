using IVA.DTO;
using IVA.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess.Repository
{
    public class UserFeedbackRepository : BaseRepository
    {
        public UserFeedbackRepository(AppDBContext context) : base(context)
        {
        }

        public long Add(IUserFeedback Feedback)
        {
            UserFeedback feedback = new UserFeedback
            {
                UserId = Feedback.Id,
                Description = Feedback.Description,
                Date = Feedback.Date,
                Rating = Feedback.Rating
            };
            context.UserFeedbacks.Add(feedback);
            context.SaveChanges();
            return feedback.Id;
        }
    }
}
