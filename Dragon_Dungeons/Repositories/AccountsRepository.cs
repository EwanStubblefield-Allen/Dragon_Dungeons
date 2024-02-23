namespace Dragon_Dungeons.Repositories;

public class AccountsRepository(IDbConnection db)
{
  private readonly IDbConnection _db = db;

  internal Account GetByEmail(string userEmail)
  {
    string sql = "SELECT * FROM accounts WHERE email = @userEmail";
    return _db.QueryFirstOrDefault<Account>(sql, new { userEmail });
  }

  internal Account GetById(string id)
  {
    string sql = "SELECT * FROM accounts WHERE id = @id";
    return _db.QueryFirstOrDefault<Account>(sql, new { id });
  }

  internal Account Create(Account newAccount)
  {
    string sql = @"
      INSERT INTO accounts
        (name, picture, email, id)
      VALUES
        (@Name, @Picture, @Email, @Id)";
    _db.Execute(sql, newAccount);
    newAccount.CoverImg = "https://cdn.pixabay.com/photo/2023/06/26/15/21/ai-generated-8090013_1280.png";
    return newAccount;
  }

  internal Account Edit(Account update)
  {
    string sql = @"
      UPDATE accounts
      SET 
        name = @Name,
        picture = @Picture,
        coverImg = @CoverImg
      WHERE id = @Id;";
    _db.Execute(sql, update);
    return update;
  }
}