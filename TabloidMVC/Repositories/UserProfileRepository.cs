using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;
using System.Collections.Generic;
using System;
namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.UserActive,
                              uT.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType uT ON u.UserTypeId = uT.id
                         ORDER BY LastName
                       ";

                    var reader = cmd.ExecuteReader();

                    var categories = new List<UserProfile>();

                    while (reader.Read())
                    {
                        categories.Add(new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                            UserActive = reader.GetInt32(reader.GetOrdinal("UserActive"))
                        });
                    }

                    reader.Close();

                    return categories;
                }
            }
        }

        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.UserActive,
                              uT.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType uT ON u.UserTypeId = uT.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                            UserActive = reader.GetInt32(reader.GetOrdinal("UserActive"))
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }

        public UserProfile GetUserProfileById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId, u.UserActive,
                              uT.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType uT ON u.UserTypeId = uT.id
                       WHERE u.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        UserProfile userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                            UserActive = reader.GetInt32(reader.GetOrdinal("UserActive"))
                        };

                        reader.Close();
                        return userProfile;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }

                }
            }
        }

        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE UserProfile
                            SET 
                                Email = @Email, 
                                FirstName = @firstName, 
                                LastName = @lastName, 
                                DisplayName = @displayName,
                                ImageLocation = @imageLocation,
                                UserTypeId = @userTypeId,
                                UserActive = @userActive,
                            WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", userProfile.Id);
                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@firstName", userProfile.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@displayName", userProfile.DisplayName);
                    cmd.Parameters.AddWithValue("@imageLocation", userProfile.ImageLocation);
                    
                    

                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        public void UpdateStatus(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                
            
                {
                    cmd.CommandText = @"
                    UPDATE UserProfile      
                            SET 
                                UserActive = 2
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", userProfile.Id);
                    cmd.Parameters.AddWithValue("@userActive", userProfile.UserActive);



                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActivateStatus(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())


                {
                    cmd.CommandText = @"
                    UPDATE UserProfile      
                            SET 
                                UserActive = 1
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", userProfile.Id);
                    cmd.Parameters.AddWithValue("@userActive", userProfile.UserActive);



                    cmd.ExecuteNonQuery();
                }
            }
        }



        public void Delete(int userProfileId)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM UserProfile
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", userProfileId);
                    cmd.ExecuteNonQuery();
                }
            }
        }



        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile (DisplayName, FirstName, LastName, Email, CreateDateTime, ImageLocation, UserTypeId, UserActive)
                        OUTPUT INSERTED.ID
                        VALUES (@DisplayName, @FirstName, @LastName, @Email, GETDATE(), @ImageLocation, 2, 1)";

                    cmd.Parameters.AddWithValue("@DisplayName", userProfile.DisplayName);
                    cmd.Parameters.AddWithValue("@FirstName", userProfile.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@Email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@ImageLocation", DbUtils.ValueOrDBNull(userProfile.ImageLocation));
                    cmd.Parameters.AddWithValue("@UserTypeId", userProfile.UserTypeId);
                    cmd.Parameters.AddWithValue("@UserActive", userProfile.UserActive);
                    ;

                    int id = (int)cmd.ExecuteScalar();
                    userProfile.Id = id;
                }
            }
        }
        public void UpdateUserTypeAdmin(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    UPDATE UserProfile
                            SET
                                UserTypeId = 1
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@userTypeId", userProfile.UserTypeId);
                    cmd.Parameters.AddWithValue("@id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<UserProfile> NumberOfAdmins()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      SELECT COUNT(UserTypeId) AS NumberOfAdmins
                         FROM UserProfile 
                              LEFT JOIN UserType uT ON UserProfile.UserTypeId = uT.id
                         WHERE UserTypeId = 1
                       ";

                    var reader = cmd.ExecuteReader();

                    var categories = new List<UserProfile>();

                    while (reader.Read())
                    {
                        categories.Add(new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                            UserActive = reader.GetInt32(reader.GetOrdinal("UserActive")),
                            NumberOfAdmins = reader.GetInt32(reader.GetOrdinal("UserTypeId"))
                        });
                    }

                    reader.Close();

                    return categories;
                }
            }
        }
    }
}