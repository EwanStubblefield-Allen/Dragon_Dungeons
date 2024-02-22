namespace Dragon_Dungeons.Services;

public class CommentsService(CommentsRepository commentsRepository)
{
  private readonly CommentsRepository _commentsRepository = commentsRepository;

  internal Comment GetCommentById(string commentId)
  {
    Comment comment = _commentsRepository.GetCommentById(commentId);
    return comment ?? throw new Exception($"[NO COMMENT MATCHES THE ID {commentId}]");
  }

  internal List<Comment> GetCommentsByCampaignId(string campaignId)
  {
    return _commentsRepository.GetCommentByCampaignId(campaignId);
  }

  internal Comment CreateCommentByCampaignId(Comment commentData)
  {
    _commentsRepository.CreateCommentByCampaignId(commentData);
    return GetCommentById(commentData.Id);
  }

  internal Comment UpdateCommentByCampaignId(Comment commentData)
  {
    Comment originalComment = GetCommentById(commentData.Id);
    originalComment.Description = commentData.Description ?? originalComment.Description;
    _commentsRepository.UpdateCommentByCampaignId(commentData);
    return originalComment;
  }

  internal Comment RemoveCommentByCampaignId(string commentId, string userId, string campaignCreatorId)
  {
    Comment commentToDelete = GetCommentById(commentId);
    if (commentToDelete.CreatorId != userId && campaignCreatorId != userId)
    {
      throw new Exception($"[YOU ARE NOT THE CREATOR OF THIS COMMENT]");
    }
    _commentsRepository.RemoveCommentByCampaignId(commentId);
    return commentToDelete;
  }
}