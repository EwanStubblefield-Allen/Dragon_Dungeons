namespace Dragon_Dungeons.Repositories;

public class CommentsRepository
{
  private readonly IDbConnection _db;

  public CommentsRepository(IDbConnection db)
  {
    _db = db;
  }

  internal Comment GetCommentById(string commentId)
  {
    string sql = @"
      SELECT
        c.*,
        a.*
      FROM
        comments c
        JOIN accounts a ON a.id = c.creatorId
      WHERE c.id = @commentId;";
    return _db.Query<Comment, Profile, Comment>(
      sql,
      (comment, profile) =>
      {
        comment.Creator = profile;
        return comment;
      },
      new { commentId }).FirstOrDefault();
  }

  internal List<Comment> GetCommentByCampaignId(string campaignId)
  {
    string sql = @"
      SELECT
        c.*,
        a.*
      FROM
        comments c
        JOIN accounts a ON a.id = c.creatorId
      WHERE c.campaignId = @campaignId;";
    return _db.Query<Comment, Profile, Comment>(
      sql,
      (comment, profile) =>
      {
        comment.Creator = profile;
        return comment;
      },
      new { campaignId }).ToList();
  }

  internal void CreateCommentByCampaignId(Comment commentData)
  {
    string sql = @"
      INSERT INTO comments(id, description, creatorId, campaignId)
      VALUES(@Id, @Description, @CreatorId, @CampaignId);";
    _db.Execute(sql, commentData);
  }

  internal void UpdateCommentByCampaignId(Comment commentData)
  {
    string sql = @"
      UPDATE comments SET
        description = @Description
      WHERE id = @Id LIMIT 1;";
    _db.Execute(sql, commentData);
  }

  internal void RemoveCommentByCampaignId(string commentId)
  {
    string sql = "DELETE FROM comments WHERE id = @commentId LIMIT 1;";
    _db.Execute(sql, new { commentId });
  }
}